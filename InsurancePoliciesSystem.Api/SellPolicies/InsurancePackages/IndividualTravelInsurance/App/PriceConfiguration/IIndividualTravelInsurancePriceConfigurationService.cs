namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.PriceCOnfiguration;

public interface IIndividualTravelInsurancePriceConfigurationService
{
    Task<PriceConfigurationDto> GetAsync();
    Task UpdateAsync(PriceConfigurationDto priceConfiguration);
}