using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using InsurancePoliciesSystem.Api.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace InsurancePoliciesSystem.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WorkInsurancePolicyConfiguration : IEntityTypeConfiguration<WorkInsurancePolicy>
{
    public void Configure(EntityTypeBuilder<WorkInsurancePolicy> builder)
    {
        builder.ToTable("Policy", "WorkInsurance");

        builder.HasKey(x => x.PolicyId);

        builder.Property(x => x.PolicyId)
            .IsRequired()
            .HasColumnName("PolicyId")
            .HasConversion(
                id => id.Value,
                value => new WorkInsurancePolicyId(value)
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
            policyholderBuilder.Property(x => x.CompanyName)
                .IsRequired()
                .HasColumnName("CompanyName");

            policyholderBuilder.Property(x => x.Nip)
                .IsRequired()
                .HasColumnName("Nip");

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
            variantBuilder.Property(x => x.NumberOfPeople)
                .IsRequired()
                .HasColumnName("NumberOfPeople");

            variantBuilder.Property(x => x.InsuranceSum)
                .IsRequired()
                .HasColumnName("InsuranceSum");

            variantBuilder.Property(x => x.SelectedPackage)
                .IsRequired()
                .HasColumnName("SelectedPackage");

            variantBuilder.Property(x => x.DateFrom)
                .IsRequired()
                .HasColumnName("DateFrom");

            variantBuilder.Property(x => x.DateTo)
                .IsRequired()
                .HasColumnName("DateTo");

            variantBuilder.Property(x => x.PricePerPerson)
                .IsRequired()
                .HasColumnName("PricePerPerson")
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
        
        builder.HasMany(x => x.Persons)
            .WithOne() // Zakładając, że relacja jest jeden-do-wielu
            .HasForeignKey("WorkInsurancePolicyId") // Nazwa kolumny Foreign Key (musisz ją dodać do tabeli Persons)
            .OnDelete(DeleteBehavior.Cascade); // Zachowanie podczas usuwania
    }
}

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons", "WorkInsurance");

        builder.HasKey(x => x.PersonId);

        builder.Property(x => x.PersonId)
            .IsRequired()
            .HasColumnName("PersonId")
            .HasConversion(
                id => id.Value,
                value => new PersonId(value)
            );

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasColumnName("FirstName")
            .HasConversion(
                id => id.Value,
                value => new FirstName(value)
            );

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasColumnName("LastName")
            .HasConversion(
                id => id.Value,
                value => new LastName(value)
            );

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasColumnName("IsDeleted");
    }
}

public class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
{
    public void Configure(EntityTypeBuilder<Agreement> builder)
    {
        builder.ToTable("Agreement", "Agreements");

        builder.HasKey(x => x.AgreementId);

        builder.Property(x => x.AgreementId)
            .IsRequired()
            .HasColumnName("AgreementId")
            .HasConversion(
                id => id.Value,
                value => new AgreementId(value)
            );

        builder.Property(x => x.AgreementText)
            .IsRequired()
            .HasColumnName("AgreementText")
            .HasConversion(
                id => id.Value,
                value => new AgreementText(value)
            );

        builder.Property(x => x.Package)
            .IsRequired()
            .HasColumnName("Package")
            .HasConversion(
                id => id.Value,
                value => new Package(value)
            );

        builder.Property(x => x.IsRequired)
            .IsRequired()
            .HasColumnName("IsRequired");

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasColumnName("IsDeleted");
    }
}
