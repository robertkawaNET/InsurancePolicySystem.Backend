using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.PriceConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsurancePoliciesSystem.Api.Database;

public class WorkInsurancePriceConfigurationItemConfiguration : IEntityTypeConfiguration<PriceConfigurationItemDto>
{
    public void Configure(EntityTypeBuilder<PriceConfigurationItemDto> builder)
    {
        builder.ToTable("PriceConfiguration", "WorkInsurance");

        builder.HasKey(x => x.InsuranceSum);
        builder.Property(x => x.InsuranceSum).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.Basic).IsRequired();
        builder.Property(x => x.Plus).IsRequired();
        builder.Property(x => x.Max).IsRequired();
    }
}