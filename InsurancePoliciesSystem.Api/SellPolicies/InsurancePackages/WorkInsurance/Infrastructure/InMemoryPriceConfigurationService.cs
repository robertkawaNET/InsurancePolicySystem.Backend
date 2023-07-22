using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.PriceConfiguration;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Infrastructure;

public class InMemoryPriceConfigurationService : IPriceConfigurationService
{
    private PriceConfigurationDto _priceConfiguration = new()
    {
        PriceConfigurationItems = new List<PriceConfigurationItemDto>
        {
            new() { InsuranceSum = 10000, Basic = 0.5m, Plus = 1m, Max = 1.5m },
            new() { InsuranceSum = 15000, Basic = 0.75m, Plus = 1.25m, Max = 1.75m },
            new() { InsuranceSum = 20000, Basic = 1m, Plus = 1.5m, Max = 2m }
        }
    };

    public PriceConfigurationDto Get() => _priceConfiguration;

    public void Update(PriceConfigurationDto priceConfiguration)
    {
        _priceConfiguration = priceConfiguration;
    }
}