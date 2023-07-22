using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App;

public class VariantConfigurationDto
{
    public int NumberOfPeople { get; set; }
    public int InsuranceSum { get; set; }
    public PackageType SelectedPackage { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}