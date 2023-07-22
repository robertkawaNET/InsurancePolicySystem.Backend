namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.PriceConfiguration;

public class PriceConfigurationItemDto
{
    public int InsuranceSum { get; set; }
    public decimal Basic { get; set; }
    public decimal Plus { get; set; }
    public decimal Max { get; set; }
}