using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.BackOffice.Agreements.Infrastructure;

public class InMemoryAgreementsRepository : IAgreementsRepository
{
    private readonly Dictionary<AgreementId, Agreement> _agreements = TestAgreementsProvider
        .CreateForPackages(Package.Work, Package.Travel)
        .ToDictionary(x => x.AgreementId, x => x);
    
    public Task<List<Agreement>>  GetAllAsync()
        => Task.FromResult(_agreements.Values.ToList());
    
    public Task<Agreement?> GetByIdAsync(AgreementId agreementId)
        => Task.FromResult(_agreements.GetValueOrDefault(agreementId));

    public Task<List<Agreement>> GetByIdsAsync(IEnumerable<AgreementId> agreementIds)
        => Task.FromResult(_agreements
            .Where(x => agreementIds.Contains(x.Key))
            .Select(x => x.Value)
            .ToList());

    public Task<List<Agreement>> GetAllForPackageAsync(Package package)
        => Task.FromResult(_agreements.Values.Where(x => x.Package == package).ToList());

    public Task AddAsync(Agreement agreement)
    {
        _agreements.Add(agreement.AgreementId, agreement);
        return Task.CompletedTask;
    }

    public Task SaveAsync(Agreement agreement)
    {
        _agreements[agreement.AgreementId] = agreement;
        return Task.CompletedTask;
    }

    private static class TestAgreementsProvider
    {
        public static List<Agreement> CreateForPackages(params Package[] packages)
            => packages.SelectMany(package => new List<Agreement>
            {
                new()
                {
                    AgreementId = new AgreementId(Guid.NewGuid()),
                    AgreementText =
                        new AgreementText(
                            "I hereby consent to the processing of my personal data by Robert Kawa Insurance Company for the purpose of obtaining and maintaining an insurance policy."),
                    Package = package,
                    IsDeleted = false,
                    IsRequired = true
                },
                new()
                {
                    AgreementId = new AgreementId(Guid.NewGuid()),
                    AgreementText = new AgreementText(
                        "I give my consent to Robert Kawa Insurance Company to disclose necessary information regarding my insurance policy to authorized entities, including repair shops, medical service providers, and legal representatives, for the purpose of processing and settling claims."),
                    Package = package,
                    IsDeleted = false,
                    IsRequired = true
                },
                new()
                {
                    AgreementId = new AgreementId(Guid.NewGuid()),
                    AgreementText =
                        new AgreementText(
                            "I consent to receiving marketing communications from Robert Kawa Insurance Company regarding their products, services, and promotional offers."),
                    Package = package,
                    IsDeleted = false,
                    IsRequired = false
                }
            }).ToList();
    }
}