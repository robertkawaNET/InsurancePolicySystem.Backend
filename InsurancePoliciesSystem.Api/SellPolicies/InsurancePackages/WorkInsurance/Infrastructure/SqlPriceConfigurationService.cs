using InsurancePoliciesSystem.Api.Database;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.PriceConfiguration;
using Microsoft.EntityFrameworkCore;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Infrastructure;

public class SqlPriceConfigurationService : IPriceConfigurationService
{
    private readonly IpsDbContext _context;
    private PriceConfigurationDto _priceConfiguration = new()
    {
        PriceConfigurationItems = new List<PriceConfigurationItemDto>
        {
            new() { InsuranceSum = 10000, Basic = 0.5m, Plus = 1m, Max = 1.5m },
            new() { InsuranceSum = 15000, Basic = 0.75m, Plus = 1.25m, Max = 1.75m },
            new() { InsuranceSum = 20000, Basic = 1m, Plus = 1.5m, Max = 2m }
        }
    };

    public SqlPriceConfigurationService(IpsDbContext context)
    {
        _context = context;
    }

    public async Task<PriceConfigurationDto> GetAsync()
    {
        var priceConfigurationItems = await _context.WorkInsurancePriceConfiguration.ToListAsync();
        return new PriceConfigurationDto { PriceConfigurationItems = priceConfigurationItems };
    }

    public async Task UpdateAsync(PriceConfigurationDto priceConfiguration)
    {
        _context.WorkInsurancePriceConfiguration.RemoveRange();
        _context.WorkInsurancePriceConfiguration.AddRange(priceConfiguration.PriceConfigurationItems);
        await _context.SaveChangesAsync();
    }
}