using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;

public interface ISearchPolicyStorage
{
    Task<SearchPolicy?> GetByPolicyNumberAsync(PolicyNumber policyNumber);
    Task<SearchPolicy?> GetByPolicyIdAsync(PolicyId policyId);
    Task<List<SearchPolicy>> GetAllAsync();
    Task AddAsync(SearchPolicy searchPolicy);
    Task SaveAsync(SearchPolicy searchPolicy);
}