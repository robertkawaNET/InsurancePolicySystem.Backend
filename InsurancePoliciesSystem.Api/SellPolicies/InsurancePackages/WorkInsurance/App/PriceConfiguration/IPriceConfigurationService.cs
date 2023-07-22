namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.PriceConfiguration;

public interface IPriceConfigurationService
{
    PriceConfigurationDto Get();
    void Update(PriceConfigurationDto priceConfiguration);
}