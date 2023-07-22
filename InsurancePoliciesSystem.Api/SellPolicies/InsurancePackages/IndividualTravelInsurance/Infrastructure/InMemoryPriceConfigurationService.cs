using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.PriceCOnfiguration;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Infrastructure;

public class InMemoryPriceConfigurationService : IPriceConfigurationService
{
    private PriceConfigurationDto _priceConfiguration = new PriceConfigurationDto
    {
        PriceConfigurationItems = new List<PriceConfigurationItemDto>
        {
            new() { InsuranceSum = 10000, Essential = 51m, Adventure = 64m, Relax = 79m },
            new() { InsuranceSum = 15000, Essential = 62m, Adventure = 75m, Relax = 80m },
            new() { InsuranceSum = 20000, Essential = 73m, Adventure = 86m, Relax = 91 }
        }
    };

    public PriceConfigurationDto Get() => _priceConfiguration;

    public void Update(PriceConfigurationDto priceConfiguration)
    {
        _priceConfiguration = priceConfiguration;
    }
}