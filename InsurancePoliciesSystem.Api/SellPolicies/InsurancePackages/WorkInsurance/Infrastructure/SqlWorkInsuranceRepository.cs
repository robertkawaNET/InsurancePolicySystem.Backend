using InsurancePoliciesSystem.Api.Database;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using Microsoft.EntityFrameworkCore;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Infrastructure;

internal class SqlWorkInsuranceRepository : IWorkInsuranceRepository
{
    private readonly IpsDbContext _context;
    private readonly ISearchPolicyStorage _searchPolicyStorage;

    public SqlWorkInsuranceRepository(IpsDbContext context, ISearchPolicyStorage searchPolicyStorage)
    {
        _context = context;
        _searchPolicyStorage = searchPolicyStorage;
    }

    public async Task<WorkInsurancePolicy?> GetByIdAsync(WorkInsurancePolicyId policyId)
        => await _context.WorkInsurancePolicies
            .Include(x => x.Persons)
            .SingleOrDefaultAsync(x => x.PolicyId == policyId);

    public async Task AddAsync(WorkInsurancePolicy policy)
    {
        _context.WorkInsurancePolicies.Add(policy);
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

    public async Task SaveAsync(WorkInsurancePolicy policy)
    {
        _context.Set<WorkInsurancePolicy>().Update(policy);
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