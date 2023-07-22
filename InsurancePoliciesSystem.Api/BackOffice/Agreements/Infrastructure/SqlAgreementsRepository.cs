using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.Database;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using Microsoft.EntityFrameworkCore;

namespace InsurancePoliciesSystem.Api.BackOffice.Agreements.Infrastructure;

public class SqlAgreementsRepository : IAgreementsRepository
{
    private readonly IpsDbContext _context;

    public SqlAgreementsRepository(IpsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Agreement>> GetAllAsync()
        => await _context.Agreements.ToListAsync();

    public async Task<Agreement?> GetByIdAsync(AgreementId agreementId)
        => await _context.Agreements.SingleOrDefaultAsync(x => x.AgreementId == agreementId);

    public async Task<List<Agreement>> GetByIdsAsync(IEnumerable<AgreementId> agreementIds)
        => await _context.Agreements.Where(x => agreementIds.Contains(x.AgreementId)).ToListAsync();
    
    public async Task<List<Agreement>> GetAllForPackageAsync(Package package)
        => await _context.Agreements.Where(x => x.Package == package).ToListAsync();

    public async Task AddAsync(Agreement agreement)
    {
        _context.Agreements.Add(agreement);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(Agreement agreement)
    {
        _context.Set<Agreement>().Update(agreement);
        await _context.SaveChangesAsync();
    }
}