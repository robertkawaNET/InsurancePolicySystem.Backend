using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace InsurancePoliciesSystem.Api.Database;

public class IndividualTravelInsurancePolicyConfiguration : IEntityTypeConfiguration<IndividualTravelInsurancePolicy>
{
    public void Configure(EntityTypeBuilder<IndividualTravelInsurancePolicy> builder)
    {
        builder.ToTable("Policy", "IndividualTravelInsurance");

        builder.HasKey(x => x.PolicyId);

        builder.Property(x => x.PolicyId)
            .IsRequired()
            .HasColumnName("PolicyId")
            .HasConversion(
                id => id.Value,
                value => new IndividualTravelInsurancePolicyId(value)
            );

        builder.Property(x => x.PolicyNumber)
            .IsRequired()
            .HasColumnName("PolicyNumber")
            .HasConversion(
                id => id.Value,
                value => new PolicyNumber(value)
            );

        builder.Property(x => x.CreateDate)
            .IsRequired()
            .HasColumnName("CreateDate");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasColumnName("Status");

        builder.OwnsOne(x => x.Policyholder, policyholderBuilder =>
        {
            policyholderBuilder.Property(x => x.FirstName)
                .IsRequired()
                .HasColumnName("FirstName");

            policyholderBuilder.Property(x => x.LastName)
                .IsRequired()
                .HasColumnName("LastName");

            policyholderBuilder.Property(x => x.Street)
                .IsRequired()
                .HasColumnName("Street");

            policyholderBuilder.Property(x => x.HouseNumber)
                .IsRequired()
                .HasColumnName("HouseNumber");

            policyholderBuilder.Property(x => x.FlatNumber)
                .HasColumnName("FlatNumber");

            policyholderBuilder.Property(x => x.PostalCode)
                .IsRequired()
                .HasColumnName("PostalCode");

            policyholderBuilder.Property(x => x.City)
                .IsRequired()
                .HasColumnName("City");

            policyholderBuilder.Property(x => x.Email)
                .IsRequired()
                .HasColumnName("Email");

            policyholderBuilder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasColumnName("PhoneNumber");
        });


        builder.OwnsOne(x => x.Variant, variantBuilder =>
        {
            variantBuilder.Property(x => x.Country)
                .IsRequired()
                .HasColumnName("Country")
                .HasConversion(
                    x => x.Code,
                    value => Country.From(value)
                );
            
            variantBuilder.Property(x => x.DateFrom)
                .IsRequired()
                .HasColumnName("DateFrom");
            
            variantBuilder.Property(x => x.DateTo)
                .IsRequired()
                .HasColumnName("DateTo");

            variantBuilder.Property(x => x.InsuranceSum)
                .IsRequired()
                .HasColumnName("InsuranceSum");

            variantBuilder.Property(x => x.SelectedPackage)
                .IsRequired()
                .HasColumnName("SelectedPackage");


            variantBuilder.Property(x => x.PricePerDay)
                .IsRequired()
                .HasColumnName("PricePerDay")
                .HasConversion(
                    id => id.Value,
                    value => new Price(value)
                );

            variantBuilder.Property(x => x.TotalPrice)
                .IsRequired()
                .HasColumnName("TotalPrice")
                .HasConversion(
                    id => id.Value,
                    value => new Price(value)
                );
        });
        
        builder.Property(x => x.AgreementsIds)
            .HasColumnName("AgreementsIds")
            .HasConversion(
                v => JsonConvert.SerializeObject(v.Select(id => id.Value).ToList()),
                v => JsonConvert.DeserializeObject<List<Guid>>(v)!.Select(id => new AgreementId(id)).ToList()
            );
    }
}