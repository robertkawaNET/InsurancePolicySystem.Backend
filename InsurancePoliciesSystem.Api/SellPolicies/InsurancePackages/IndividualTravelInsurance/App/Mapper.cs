using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.PriceCOnfiguration;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using InsurancePoliciesSystem.Api.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App;

internal static class Mapper
{
    private static int _policyCount;
    
    public static IndividualTravelInsurancePolicy Map(CreatePolicyDto createPolicyDto, PriceConfigurationDto priceConfigurationDto)
    {
        IndividualTravelInsurancePolicy workInsurancePolicy = new IndividualTravelInsurancePolicy
        {
            PolicyPolicyId = new IndividualTravelInsurancePolicyId(Guid.NewGuid()),
            Policyholder = MapPolicyholderDtoToPolicyholder(createPolicyDto.Policyholder),
            Variant = MapVariantConfigurationDtoToVariant(createPolicyDto.Variant, priceConfigurationDto),
            AgreementsIds = createPolicyDto.AgreementsIds.Select(x => new AgreementId(x)).ToList(),
            PolicyNumber = new PolicyNumber($"250-30-{_policyCount++}"),
            CreateDate = DateTime.Now
        };

        return workInsurancePolicy;
    }

    private static Policyholder MapPolicyholderDtoToPolicyholder(PolicyholderDto policyholderDto)
    {
        return new Policyholder
        {
            FirstName = policyholderDto.FirstName,
            LastName = policyholderDto.LastName,
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
        var pricePerDay = variantConfigurationDto.SelectedPackage switch
        {
            PackageType.Essential => priceConfigurationForInsuranceSum.Essential,
            PackageType.Adventure => priceConfigurationForInsuranceSum.Adventure,
            PackageType.Relax => priceConfigurationForInsuranceSum.Relax,
            _ => throw new InvalidOperationException()
        };

        var numberOfDays = variantConfigurationDto.DateFrom.CalculateDaysDifference(variantConfigurationDto.DateTo);
        
        return new Variant
        {
            InsuranceSum = variantConfigurationDto.InsuranceSum,
            SelectedPackage = variantConfigurationDto.SelectedPackage,
            DateFrom = variantConfigurationDto.DateFrom,
            DateTo = variantConfigurationDto.DateTo,
            Country = new Country(variantConfigurationDto.Country,
                PricePerDay = new Price(pricePerDay),
                TotalPrice = new Price(pricePerDay * numberOfDays)
        };
    }

}