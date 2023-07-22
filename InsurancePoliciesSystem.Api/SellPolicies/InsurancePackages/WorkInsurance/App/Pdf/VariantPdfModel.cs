namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.Pdf;

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