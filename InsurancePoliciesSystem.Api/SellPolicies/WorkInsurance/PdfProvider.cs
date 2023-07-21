using DinkToPdf;
using DinkToPdf.Contracts;
using Fluid;
using InsurancePoliciesSystem.Api.BackOffice.Agreements;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Services;
using Newtonsoft.Json;

namespace InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;

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
        var model = JsonConvert.SerializeObject(policyModel);
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
    public List<PersonPdfModel> Persons { get; set; }
    public VariantPdfModel Variant { get; set; }
    public List<string> Agreements { get; set; }
}

public class PersonPdfModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class VariantPdfModel
{
    public int NumberOfPeople { get; set; }
    public int InsuranceSum { get; set; }
    public string SelectedPackage { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public decimal PricePerPerson { get; set; }
    public decimal TotalPrice { get; set; }
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
            Persons = policy.Persons.Select(x => new PersonPdfModel
            {
                FirstName = x.FirstName.Value,
                LastName = x.LastName.Value
            }).ToList(),
            Variant = new VariantPdfModel
            {
                NumberOfPeople = policy.Variant.NumberOfPeople,
                InsuranceSum = policy.Variant.InsuranceSum,
                SelectedPackage = policy.Variant.SelectedPackage.ToString().ToUpper(),
                DateFrom = policy.Variant.DateFrom,
                DateTo = policy.Variant.DateTo,
                PricePerPerson = policy.Variant.PricePerPerson.Value,
                TotalPrice = policy.Variant.TotalPrice.Value
            },
            Agreements = agreements.Select(x => x.AgreementText.Value).ToList(),
            
        };
    }
}