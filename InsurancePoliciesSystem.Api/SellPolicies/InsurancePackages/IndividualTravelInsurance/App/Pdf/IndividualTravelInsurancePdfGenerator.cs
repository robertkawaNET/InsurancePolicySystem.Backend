using DinkToPdf;
using DinkToPdf.Contracts;
using Fluid;
using InsurancePoliciesSystem.Api.SellPolicies.IndividualTravelInsurance;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.App.Pdf;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using Newtonsoft.Json;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.Pdf;

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