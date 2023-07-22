using InsurancePoliciesSystem.Api.Database;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using Microsoft.EntityFrameworkCore;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Infrastructure;

public class SqlSearchPolicyStorage : ISearchPolicyStorage
{
    private readonly IpsDbContext _context;

    public SqlSearchPolicyStorage(IpsDbContext context)
    {
        _context = context;
    }

    public async Task<SearchPolicy?> GetByPolicyNumberAsync(PolicyNumber policyNumber)
        => await _context.SearchPolicies.SingleOrDefaultAsync(x => x.PolicyNumber == policyNumber);

    public async Task<SearchPolicy?> GetByPolicyIdAsync(PolicyId policyId)
        => await _context.SearchPolicies.SingleOrDefaultAsync(x => x.PolicyId == policyId);

    public async Task<List<SearchPolicy>> GetAllAsync()
        => await _context.SearchPolicies.ToListAsync();

    public async Task AddAsync(SearchPolicy searchPolicy)
    {
        _context.SearchPolicies.Add(searchPolicy);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(SearchPolicy searchPolicy)
    {
        _context.Set<SearchPolicy>().Update(searchPolicy);
        await _context.SaveChangesAsync();
    }
}