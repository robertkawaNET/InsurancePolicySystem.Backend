namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.PriceConfiguration;

public interface IPriceConfigurationService
{
    Task<PriceConfigurationDto> GetAsync();
    Task UpdateAsync(PriceConfigurationDto priceConfiguration);
}