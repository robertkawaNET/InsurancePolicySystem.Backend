namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Services;

public class CancelPolicyService
{
    private readonly IEnumerable<PolicyCanceller> _policyCancellers;
    private readonly ISearchPolicyStorage _searchPolicyStorage;

    public CancelPolicyService(IEnumerable<PolicyCanceller> policyCancellers, ISearchPolicyStorage searchPolicyStorage)
    {
        _policyCancellers = policyCancellers;
        _searchPolicyStorage = searchPolicyStorage;
    }

    public async Task Cancel(PolicyId policyId)
    {
        var policy = await _searchPolicyStorage.GetByPolicyIdAsync(policyId);

        var policyCanceller = _policyCancellers.Single(x => x.IsResponsible(policy.Package));

        await policyCanceller.CancelAsync(policyId);
    }
}


public abstract class PolicyCanceller
{
    public abstract Package Package { get; }
    
    internal bool IsResponsible(Package package) => package.Equals(Package);

    public abstract Task CancelAsync(PolicyId policyId);
}