using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App;

public class VariantConfigurationDto
{
    public int InsuranceSum { get; set; }
    public PackageType SelectedPackage { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string Country { get; set; }
}