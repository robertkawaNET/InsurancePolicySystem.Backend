namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.Pdf;

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