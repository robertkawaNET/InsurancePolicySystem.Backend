using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsurancePoliciesSystem.Api.Database;

public class SearchPolicyConfiguration : IEntityTypeConfiguration<SearchPolicy>
{
    public void Configure(EntityTypeBuilder<SearchPolicy> builder)
    {
        builder.ToTable("SearchPolicy");

        builder.HasKey(x => x.PolicyId);
        
        builder.Property(x => x.PolicyId)
            .IsRequired()
            .HasColumnName("PolicyId")
            .HasConversion(
                id => id.Value,
                value => new PolicyId(value)
            );
        
        builder.Property(x => x.PolicyNumber)
            .IsRequired()
            .HasColumnName("PolicyNumber")
            .HasConversion(
                id => id.Value,
                value => new PolicyNumber(value)
            );
        
        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnName("Price")
            .HasConversion(
                id => id.Value,
                value => new Price(value)
            );
        
        builder.Property(x => x.Package)
            .IsRequired()
            .HasColumnName("Package")
            .HasConversion(
                id => id.Value,
                value => new Package(value)
            );
        
        builder.Property(x => x.CreateDate)
            .IsRequired()
            .HasColumnName("CreateDate");
        
        builder.Property(x => x.DateFrom)
            .IsRequired()
            .HasColumnName("DateFrom");
        
        builder.Property(x => x.DateTo)
            .IsRequired()
            .HasColumnName("DateTo");
        
        builder.Property(x => x.Status)
            .IsRequired()
            .HasColumnName("Status");
    }
}
