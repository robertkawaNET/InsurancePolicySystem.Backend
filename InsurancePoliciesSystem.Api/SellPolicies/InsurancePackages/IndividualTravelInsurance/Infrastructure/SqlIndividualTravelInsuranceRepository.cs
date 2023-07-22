using InsurancePoliciesSystem.Api.Database;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using Microsoft.EntityFrameworkCore;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Infrastructure;

internal class SqlIndividualTravelInsuranceRepository : IIndividualTravelInsuranceRepository
{
    private readonly IpsDbContext _context;
    private readonly ISearchPolicyStorage _searchPolicyStorage;

    public SqlIndividualTravelInsuranceRepository(IpsDbContext context, ISearchPolicyStorage searchPolicyStorage)
    {
        _context = context;
        _searchPolicyStorage = searchPolicyStorage;
    }

    public async Task<IndividualTravelInsurancePolicy?> GetByIdAsync(IndividualTravelInsurancePolicyId policyId)
        => await _context.IndividualTravelInsurancePolicies.SingleOrDefaultAsync(x => x.PolicyId == policyId);

    public async Task AddAsync(IndividualTravelInsurancePolicy policy)
    {
        _context.IndividualTravelInsurancePolicies.Add(policy);
        await _context.SaveChangesAsync();
        
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

    public async Task SaveAsync(IndividualTravelInsurancePolicy policy)
    {
        _context.Set<IndividualTravelInsurancePolicy>().Update(policy);
        await _context.SaveChangesAsync();
        
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