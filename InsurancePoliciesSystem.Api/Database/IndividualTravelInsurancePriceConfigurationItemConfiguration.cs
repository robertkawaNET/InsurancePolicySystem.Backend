using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.PriceCOnfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsurancePoliciesSystem.Api.Database;

public class IndividualTravelInsurancePriceConfigurationItemConfiguration : IEntityTypeConfiguration<PriceConfigurationItemDto>
{
    public void Configure(EntityTypeBuilder<PriceConfigurationItemDto> builder)
    {
        builder.ToTable("PriceConfiguration", "IndividualTravelInsurance");

        builder.HasKey(x => x.InsuranceSum);
        builder.Property(x => x.InsuranceSum).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.Essential).IsRequired();
        builder.Property(x => x.Adventure).IsRequired();
        builder.Property(x => x.Relax).IsRequired();
    }
}