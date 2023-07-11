using DinkToPdf;
using DinkToPdf.Contracts;
using Fluid;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance.Pdf;

public class PdfCreator
{
    private readonly IConverter _converter;
    private readonly IWorkInsuranceRepository _repository;

    public PdfCreator(IConverter converter, IWorkInsuranceRepository repository)
    {
        _converter = converter;
        _repository = repository;
    }

    public async Task<byte[]> GeneratePolicyPdf(WorkInsurancePolicyId policyId)
    {
        var parser = new FluidParser();

        if (parser.TryParse(WorkInsuranceResources.PdfHtml, out var template, out var error))
        {
            var policy = await _repository.GetByIdAsync(policyId);
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
            
            return _converter.Convert(doc);
        }

        throw new InvalidOperationException(error);
    }
}