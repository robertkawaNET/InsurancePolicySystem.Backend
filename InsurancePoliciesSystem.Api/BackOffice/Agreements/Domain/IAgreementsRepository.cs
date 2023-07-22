using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;

public interface IAgreementsRepository
{
    Task<List<Agreement>> GetAllAsync();
    Task<Agreement?> GetByIdAsync(AgreementId agreementId);
    Task<List<Agreement>> GetByIdsAsync(IEnumerable<AgreementId> agreementIds);
    Task<List<Agreement>> GetAllForPackageAsync(Package package);
    Task AddAsync(Agreement agreement);
    Task SaveAsync(Agreement agreement);
}