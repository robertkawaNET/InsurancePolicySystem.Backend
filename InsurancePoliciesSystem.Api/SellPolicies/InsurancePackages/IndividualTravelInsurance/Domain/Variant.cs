using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;

public class Variant
{
    public int InsuranceSum { get; set; }
    public PackageType SelectedPackage { get; set; }
    public Country Country { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public Price PricePerDay { get; set; }
    public Price TotalPrice { get; set; }
}