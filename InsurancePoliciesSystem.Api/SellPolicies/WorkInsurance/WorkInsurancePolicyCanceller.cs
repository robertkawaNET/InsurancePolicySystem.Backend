using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Services;
using InsurancePoliciesSystem.Api.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;

public class WorkInsurancePolicyCanceller : PolicyCanceller
{
    public override Package Package => Package.Work;

    private readonly IWorkInsuranceRepository _repository;
    private readonly IClock _clock;

    public WorkInsurancePolicyCanceller(IWorkInsuranceRepository repository, IClock clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public override async Task CancelAsync(PolicyId policyId)
    {
        var policy = await _repository.GetByIdAsync(new WorkInsurancePolicyId(policyId.Value));
        policy.Cancel(_clock.UtcNow);
        await _repository.SaveAsync(policy);
    }
}