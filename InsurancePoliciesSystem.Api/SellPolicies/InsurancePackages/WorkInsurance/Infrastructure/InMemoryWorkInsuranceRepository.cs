using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Infrastructure;

internal class InMemoryWorkInsuranceRepository : IWorkInsuranceRepository
{
    private readonly Dictionary<WorkInsurancePolicyId, WorkInsurancePolicy> _policies = new();

    private readonly ISearchPolicyStorage _searchPolicyStorage;

    public InMemoryWorkInsuranceRepository(ISearchPolicyStorage searchPolicyStorage)
    {
        _searchPolicyStorage = searchPolicyStorage;
    }

    public Task<WorkInsurancePolicy?> GetByIdAsync(WorkInsurancePolicyId policyId)
        =>  Task.FromResult(_policies.GetValueOrDefault(policyId));

    public async Task AddAsync(WorkInsurancePolicy policy)
    {
        _policies.Add(policy.PolicyId, policy);
        
        await _searchPolicyStorage.AddAsync(new SearchPolicy
        {
            PolicyId = new PolicyId(policy.PolicyId.Value),
            PolicyNumber = policy.PolicyNumber,
            Price = policy.Variant.TotalPrice,
            Package = Package.Work,
            CreateDate = policy.CreateDate,
            Status = policy.Status,
            DateFrom = policy.Variant.DateFrom,
            DateTo = policy.Variant.DateTo
        });
    }
    
    public async Task SaveAsync(WorkInsurancePolicy policy)
    {
        _policies[policy.PolicyId] = policy;
        
        await _searchPolicyStorage.SaveAsync(new SearchPolicy
        {
            PolicyId = new PolicyId(policy.PolicyId.Value),
            PolicyNumber = policy.PolicyNumber,
            Price = policy.Variant.TotalPrice,
            Package = Package.Work,
            CreateDate = policy.CreateDate,
            Status = policy.Status
        });
    }
}