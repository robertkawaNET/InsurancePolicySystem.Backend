using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Infrastructure;

internal class InMemoryIndividualTravelInsuranceRepository : IIndividualTravelInsuranceRepository
{
    private readonly Dictionary<IndividualTravelInsurancePolicyId, IndividualTravelInsurancePolicy> _policies = new();

    private readonly ISearchPolicyStorage _searchPolicyStorage;

    public InMemoryIndividualTravelInsuranceRepository(ISearchPolicyStorage searchPolicyStorage)
    {
        _searchPolicyStorage = searchPolicyStorage;
    }

    public Task<IndividualTravelInsurancePolicy?> GetByIdAsync(IndividualTravelInsurancePolicyId policyPolicyId)
        =>  Task.FromResult(_policies.GetValueOrDefault(policyPolicyId));

    public async Task AddAsync(IndividualTravelInsurancePolicy policy)
    {
        _policies.Add(policy.PolicyPolicyId, policy);
        
        await _searchPolicyStorage.AddAsync(new SearchPolicy
        {
            PolicyId = new PolicyId(policy.PolicyPolicyId.Value),
            PolicyNumber = policy.PolicyNumber,
            Price = policy.Variant.TotalPrice,
            Package = Package.Travel,
            CreateDate = policy.CreateDate,
            Status = policy.Status,
            DateFrom = policy.Variant.DateFrom,
            DateTo = policy.Variant.DateTo
        });
    }
    
    public async Task SaveAsync(IndividualTravelInsurancePolicy policy)
    {
        _policies[policy.PolicyPolicyId] = policy;
        
        await _searchPolicyStorage.SaveAsync(new SearchPolicy
        {
            PolicyId = new PolicyId(policy.PolicyPolicyId.Value),
            PolicyNumber = policy.PolicyNumber,
            Price = policy.Variant.TotalPrice,
            Package = Package.Work,
            CreateDate = policy.CreateDate,
            Status = policy.Status
        });
    }
}