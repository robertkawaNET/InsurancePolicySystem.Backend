using InsurancePoliciesSystem.Api.Database;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.PriceCOnfiguration;
using Microsoft.EntityFrameworkCore;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Infrastructure;

public class SqlIndividualTravelInsurancePriceConfigurationService : IIndividualTravelInsurancePriceConfigurationService
{
    private readonly IpsDbContext _context;

    public SqlIndividualTravelInsurancePriceConfigurationService(IpsDbContext context)
    {
        _context = context;
    }

    public async Task<PriceConfigurationDto> GetAsync()
    {
        var priceConfigurationItems = await _context.IndividualTravelInsurancePriceConfiguration.ToListAsync();
        return new PriceConfigurationDto { PriceConfigurationItems = priceConfigurationItems };
    }

    public async Task UpdateAsync(PriceConfigurationDto priceConfiguration)
    {
        _context.IndividualTravelInsurancePriceConfiguration.RemoveRange();
        _context.IndividualTravelInsurancePriceConfiguration.AddRange(priceConfiguration.PriceConfigurationItems);
        await _context.SaveChangesAsync();
    }
}