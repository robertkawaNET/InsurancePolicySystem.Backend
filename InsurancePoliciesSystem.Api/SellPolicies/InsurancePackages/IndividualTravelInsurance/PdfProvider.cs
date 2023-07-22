using DinkToPdf;
using DinkToPdf.Contracts;
using Fluid;
using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.App.Pdf;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using Newtonsoft.Json;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance;

public class IndividualTravelInsurancePdfGenerator : PolicyPdfGenerator
{
    public override Package Package => Package.Travel;

    private readonly IConverter _converter;
    private readonly IndividualTravelInsurancePolicyPdfModelProvider _modelProvider;

    public IndividualTravelInsurancePdfGenerator(IConverter converter, IndividualTravelInsurancePolicyPdfModelProvider modelProvider)
    {
        _converter = converter;
        _modelProvider = modelProvider;
    }

    public override async Task<PolicyPdf> GenerateAsync(PolicyId policyId)
    {
        var parser = new FluidParser();

        if (!parser.TryParse(IndividualTravelInsuranceResources.PdfHtml, out var template, out var error))
        {
            throw new InvalidOperationException(error);
        }

        var policyModel = await _modelProvider.GetAsync(new IndividualTravelInsurancePolicyId(policyId.Value));
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

public class IndividualTravelInsurancePolicyPdfModel
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
    public string Country { get; set; }
    public int InsuranceSum { get; set; }
    public string SelectedPackage { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal TotalPrice { get; set; }
}

public class IndividualTravelInsurancePolicyPdfModelProvider
{
    private readonly IIndividualTravelInsuranceRepository _IndividualTravelInsuranceRepository;
    private readonly IAgreementsRepository _agreementsRepository;

    public IndividualTravelInsurancePolicyPdfModelProvider(IIndividualTravelInsuranceRepository IndividualTravelInsuranceRepository, IAgreementsRepository agreementsRepository)
    {
        _IndividualTravelInsuranceRepository = IndividualTravelInsuranceRepository;
        _agreementsRepository = agreementsRepository;
    }

    internal async Task<IndividualTravelInsurancePolicyPdfModel> GetAsync(IndividualTravelInsurancePolicyId policyPolicyId)
    {
        var policy = await _IndividualTravelInsuranceRepository.GetByIdAsync(policyPolicyId);
        var agreements = await _agreementsRepository.GetByIdsAsync(policy.AgreementsIds);

        return new IndividualTravelInsurancePolicyPdfModel
        {
            PolicyNumber = policy.PolicyNumber.Value,
            Policyholder = policy.Policyholder,
            Variant = new VariantPdfModel
            {
                Country = policy.Variant.Country.Name,
                InsuranceSum = policy.Variant.InsuranceSum,
                SelectedPackage = policy.Variant.SelectedPackage.ToString().ToUpper(),
                DateFrom = policy.Variant.DateFrom,
                DateTo = policy.Variant.DateTo,
                PricePerDay = policy.Variant.PricePerDay.Value,
                TotalPrice = policy.Variant.TotalPrice.Value
            },
            Agreements = agreements.Select(x => x.AgreementText.Value).ToList(),
        };
    }
}