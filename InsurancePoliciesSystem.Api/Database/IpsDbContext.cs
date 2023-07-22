using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.PriceConfiguration;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using Microsoft.EntityFrameworkCore;
using IndividualTravelInsurance = InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance;

namespace InsurancePoliciesSystem.Api.Database;

public class IpsDbContext : DbContext
{
    public IpsDbContext(DbContextOptions<IpsDbContext> options) : base(options)
    {
    }
    
    
    public DbSet<PriceConfigurationItemDto> WorkInsurancePriceConfiguration { get; set; }
    public DbSet<IndividualTravelInsurance.App.PriceCOnfiguration.PriceConfigurationItemDto> IndividualTravelInsurancePriceConfiguration { get; set; }
    public DbSet<WorkInsurancePolicy> WorkInsurancePolicies { get; set; }
    public DbSet<IndividualTravelInsurancePolicy> IndividualTravelInsurancePolicies { get; set; }
    public DbSet<SearchPolicy> SearchPolicies { get; set; }
    public DbSet<Agreement> Agreements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WorkInsurancePriceConfigurationItemConfiguration());
        modelBuilder.ApplyConfiguration(new WorkInsurancePolicyConfiguration());
        modelBuilder.ApplyConfiguration(new IndividualTravelInsurancePolicyConfiguration());
        modelBuilder.ApplyConfiguration(new IndividualTravelInsurancePriceConfigurationItemConfiguration());
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new AgreementConfiguration());
        modelBuilder.ApplyConfiguration(new SearchPolicyConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}