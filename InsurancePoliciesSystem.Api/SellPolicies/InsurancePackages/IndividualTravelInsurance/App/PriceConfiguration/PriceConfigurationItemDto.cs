namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.PriceCOnfiguration;

public class PriceConfigurationItemDto
{
    public int InsuranceSum { get; set; }
    public decimal Essential { get; set; }
    public decimal Adventure { get; set; }
    public decimal Relax { get; set; }
}