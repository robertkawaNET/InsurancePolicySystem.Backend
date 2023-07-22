namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.PriceCOnfiguration;

public interface IPriceConfigurationService
{
    PriceConfigurationDto Get();
    void Update(PriceConfigurationDto priceConfiguration);
}