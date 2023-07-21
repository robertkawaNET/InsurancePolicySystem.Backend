using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Services;
using InsurancePoliciesSystem.Api.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.IndividualTravelInsurance;

public class IndividualTravelInsuranceInsurancePolicyCanceller : PolicyCanceller
{
    public override Package Package => Package;

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