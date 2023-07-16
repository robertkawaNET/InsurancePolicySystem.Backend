using DinkToPdf;
using DinkToPdf.Contracts;
using Fluid;
using InsurancePoliciesSystem.Api.BackOffice.Agreements;
using InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Services;

public class PdfProvider
{
    private readonly IEnumerable<PolicyPdfGenerator> _pdfGenerators;
    private readonly ISearchPolicyStorage _searchPolicyStorage;

    public PdfProvider(IEnumerable<PolicyPdfGenerator> pdfGenerators, ISearchPolicyStorage searchPolicyStorage)
    {
        _pdfGenerators = pdfGenerators;
        _searchPolicyStorage = searchPolicyStorage;
    }
    
    public async Task<PolicyPdf> GenerateAsync(PolicyId policyId)
    {
        var policy = await _searchPolicyStorage.GetByPolicyIdAsync(policyId);

        var pdfGenerator = _pdfGenerators.Single(x => x.IsResponsible(policy.Package));

        return await pdfGenerator.GenerateAsync(policyId);
    }
}

public abstract class PolicyPdfGenerator
{
    public abstract Package Package { get; }
    
    internal bool IsResponsible(Package package) => package.Equals(Package);

    public abstract Task<PolicyPdf> GenerateAsync(PolicyId policyId);
}

internal class Test : PolicyPdfGenerator
{
    public override Package Package => Package.Individual;

    public override Task<PolicyPdf> GenerateAsync(PolicyId policyId)
    {
        throw new NotImplementedException();
    }
}

public class WorkInsurancePdfGenerator : PolicyPdfGenerator
{
    public override Package Package => Package.Work;

    private readonly IConverter _converter;
    private readonly WorkInsurancePolicyPdfModelProvider _modelProvider;

    public WorkInsurancePdfGenerator(IConverter converter, WorkInsurancePolicyPdfModelProvider modelProvider)
    {
        _converter = converter;
        _modelProvider = modelProvider;
    }

    public override async Task<PolicyPdf> GenerateAsync(PolicyId policyId)
    {
        var parser = new FluidParser();

        if (!parser.TryParse(WorkInsuranceResources.PdfHtml, out var template, out var error))
        {
            throw new InvalidOperationException(error);
        }

        var policyModel = await _modelProvider.GetAsync(new WorkInsurancePolicyId(policyId.Value));
        var model = JsonSerializer.Serialize(policyModel);
        var jsonString = JsonConvert.DeserializeObject<dynamic>(model);
        var context = new TemplateContext(jsonString);
        var html = await template.RenderAsync(context);
        
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
            Objects = {
                new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = html,
                    WebSettings = { DefaultEncoding = "utf-8" },
                }
            }
        };
        
        var bytes = _converter.Convert(doc);
        return new PolicyPdf($"{policyModel!.PolicyNumber}.pdf", bytes);
    }
}

public class WorkInsurancePolicyPdfModel
{
    public string PolicyNumber { get; set; }
    public Policyholder Policyholder { get; set; }
    public List<string> Agreements { get; set; }
}

public class WorkInsurancePolicyPdfModelProvider
{
    private readonly IWorkInsuranceRepository _workInsuranceRepository;
    private readonly IAgreementsRepository _agreementsRepository;

    public WorkInsurancePolicyPdfModelProvider(IWorkInsuranceRepository workInsuranceRepository, IAgreementsRepository agreementsRepository)
    {
        _workInsuranceRepository = workInsuranceRepository;
        _agreementsRepository = agreementsRepository;
    }

    internal async Task<WorkInsurancePolicyPdfModel> GetAsync(WorkInsurancePolicyId policyId)
    {
        var policy = await _workInsuranceRepository.GetByIdAsync(policyId);
        var agreements = await _agreementsRepository.GetByIdsAsync(policy.AgreementsIds);

        return new WorkInsurancePolicyPdfModel
        {
            PolicyNumber = policy.PolicyNumber.Value,
            Policyholder = policy.Policyholder,
            Agreements = agreements.Select(x => x.AgreementText.Value).ToList()
        };
    }
}



public record PolicyPdf(string FileName, byte[] FileData);