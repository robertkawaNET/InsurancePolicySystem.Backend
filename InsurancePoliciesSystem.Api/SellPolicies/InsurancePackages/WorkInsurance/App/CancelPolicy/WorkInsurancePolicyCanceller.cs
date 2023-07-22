using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.App.CancelPolicy;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using InsurancePoliciesSystem.Api.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.CancelPolicy;

public class WorkInsurancePolicyCanceller : PolicyCanceller
{
    protected override Package Package => Package.Work;

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