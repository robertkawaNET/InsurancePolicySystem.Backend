using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Infrastructure;

public class InMemorySearchPolicyStorage : ISearchPolicyStorage
{
    private readonly Dictionary<PolicyId, SearchPolicy> _searchPolicies = new();

    public Task<SearchPolicy?> GetByPolicyNumberAsync(PolicyNumber policyNumber)
        => Task.FromResult(_searchPolicies.Values.SingleOrDefault(x => x.PolicyNumber == policyNumber));

    public Task<SearchPolicy?> GetByPolicyIdAsync(PolicyId policyId)
        => Task.FromResult(_searchPolicies.GetValueOrDefault(policyId));

    public Task<List<SearchPolicy>> GetAllAsync()
        => Task.FromResult(_searchPolicies.Values.ToList());

    public Task AddAsync(SearchPolicy searchPolicy)
    {
        _searchPolicies.Add(searchPolicy.PolicyId, searchPolicy);
        return Task.CompletedTask;
    }

    public Task SaveAsync(SearchPolicy searchPolicy)
    {
        _searchPolicies[searchPolicy.PolicyId] = searchPolicy;
        return Task.CompletedTask;
    }
}