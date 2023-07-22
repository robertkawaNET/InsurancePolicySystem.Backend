using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.Pdf;

public class IndividualTravelInsurancePolicyPdfModelProvider
{
    private readonly IIndividualTravelInsuranceRepository _IndividualTravelInsuranceRepository;
    private readonly IAgreementsRepository _agreementsRepository;

    public IndividualTravelInsurancePolicyPdfModelProvider(IIndividualTravelInsuranceRepository IndividualTravelInsuranceRepository, IAgreementsRepository agreementsRepository)
    {
        _IndividualTravelInsuranceRepository = IndividualTravelInsuranceRepository;
        _agreementsRepository = agreementsRepository;
    }

    internal async Task<IndividualTravelInsurancePolicyPdfModel> GetAsync(IndividualTravelInsurancePolicyId policyPolicyId)
    {
        var policy = await _IndividualTravelInsuranceRepository.GetByIdAsync(policyPolicyId);
        var agreements = await _agreementsRepository.GetByIdsAsync(policy.AgreementsIds);

        return new IndividualTravelInsurancePolicyPdfModel
        {
            PolicyNumber = policy.PolicyNumber.Value,
            Policyholder = policy.Policyholder,
            Variant = new VariantPdfModel
            {
                Country = policy.Variant.Country.Name,
                InsuranceSum = policy.Variant.InsuranceSum,
                SelectedPackage = policy.Variant.SelectedPackage.ToString().ToUpper(),
                DateFrom = policy.Variant.DateFrom,
                DateTo = policy.Variant.DateTo,
                PricePerDay = policy.Variant.PricePerDay.Value,
                TotalPrice = policy.Variant.TotalPrice.Value
            },
            Agreements = agreements.Select(x => x.AgreementText.Value).ToList(),
        };
    }
}