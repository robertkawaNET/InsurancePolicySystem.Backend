using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.App.CancelPolicy;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using InsurancePoliciesSystem.Api.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.CancelPolicy;

public class IndividualTravelInsuranceInsurancePolicyCanceller : PolicyCanceller
{
    protected override Package Package => Package;

    private readonly IIndividualTravelInsuranceRepository _repository;
    private readonly IClock _clock;

    public IndividualTravelInsuranceInsurancePolicyCanceller(IIndividualTravelInsuranceRepository repository, IClock clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public override async Task CancelAsync(PolicyId policyId)
    {
        var policy = await _repository.GetByIdAsync(new IndividualTravelInsurancePolicyId(policyId.Value));
        policy.Cancel(_clock.UtcNow);
        await _repository.SaveAsync(policy);
    }
}