using DinkToPdf;
using DinkToPdf.Contracts;
using Fluid;
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
    private readonly IWorkInsuranceRepository _repository;

    public WorkInsurancePdfGenerator(IConverter converter, IWorkInsuranceRepository repository)
    {
        _converter = converter;
        _repository = repository;
    }

    public override async Task<PolicyPdf> GenerateAsync(PolicyId policyId)
    {
        var parser = new FluidParser();

        if (!parser.TryParse(WorkInsuranceResources.PdfHtml, out var template, out var error))
        {
            throw new InvalidOperationException(error);
        }
        
        var policy = await _repository.GetByIdAsync(new WorkInsurancePolicyId(policyId.Value));
        var model = JsonSerializer.Serialize(policy);
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
        return new PolicyPdf($"{policy!.PolicyNumber}.pdf", bytes);
    }
}

public record PolicyPdf(string FileName, byte[] FileData);