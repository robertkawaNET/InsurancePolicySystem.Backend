using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.App.CancelPolicy;

public abstract class PolicyCanceller
{
    protected abstract Package Package { get; }
    
    internal bool IsResponsible(Package package) => package.Equals(Package);

    public abstract Task CancelAsync(PolicyId policyId);
}