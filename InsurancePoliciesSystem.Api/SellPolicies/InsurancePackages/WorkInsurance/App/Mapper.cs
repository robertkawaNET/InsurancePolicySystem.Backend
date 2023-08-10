using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.PriceConfiguration;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App;

internal static class Mapper
{
    private static int _policyCount;
    
    public static WorkInsurancePolicy Map(CreatePolicyDto createPolicyDto, PriceConfigurationDto priceConfigurationDto)
    {
        var workInsurancePolicy = new WorkInsurancePolicy
        (
            new WorkInsurancePolicyId(Guid.NewGuid()),
            MapPolicyholderDtoToPolicyholder(createPolicyDto.Policyholder),
            MapVariantConfigurationDtoToVariant(createPolicyDto.Variant, priceConfigurationDto),
            createPolicyDto.AgreementsIds.Select(x => new AgreementId(x)).ToList(),
            new PolicyNumber($"120-20-{_policyCount++}"),
            DateTime.Now
        );

        return workInsurancePolicy;
    }

    private static Policyholder MapPolicyholderDtoToPolicyholder(PolicyholderDto policyholderDto)
    {
        return new Policyholder
        {
            CompanyName = policyholderDto.CompanyName,
            Nip = policyholderDto.Nip,
            Street = policyholderDto.Street,
            HouseNumber = policyholderDto.HouseNumber,
            FlatNumber = policyholderDto.FlatNumber,
            PostalCode = policyholderDto.PostalCode,
            City = policyholderDto.City,
            Email = policyholderDto.Email,
            PhoneNumber = policyholderDto.PhoneNumber
        };
    }

    private static Variant MapVariantConfigurationDtoToVariant(VariantConfigurationDto variantConfigurationDto, PriceConfigurationDto priceConfiguration)
    {
        var priceConfigurationForInsuranceSum = priceConfiguration.PriceConfigurationItems.Single(x => x.InsuranceSum == variantConfigurationDto.InsuranceSum);
        var pricePerPerson = variantConfigurationDto.SelectedPackage switch
        {
            PackageType.Basic => priceConfigurationForInsuranceSum.Basic,
            PackageType.Plus => priceConfigurationForInsuranceSum.Plus,
            PackageType.Max => priceConfigurationForInsuranceSum.Max,
            _ => throw new InvalidOperationException()
        };

        return new Variant
        {
            NumberOfPeople = variantConfigurationDto.NumberOfPeople,
            InsuranceSum = variantConfigurationDto.InsuranceSum,
            SelectedPackage = variantConfigurationDto.SelectedPackage,
            DateFrom = variantConfigurationDto.DateFrom,
            DateTo = variantConfigurationDto.DateTo,
            PricePerPerson = new Price(pricePerPerson),
            TotalPrice = new Price(pricePerPerson * variantConfigurationDto.NumberOfPeople)
        };
    }

}


