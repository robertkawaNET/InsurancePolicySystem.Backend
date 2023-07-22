using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;

public class Variant
{
    public int NumberOfPeople { get; set; }
    public int InsuranceSum { get; set; }
    public PackageType SelectedPackage { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public Price PricePerPerson { get; set; }
    public Price TotalPrice { get; set; }
}