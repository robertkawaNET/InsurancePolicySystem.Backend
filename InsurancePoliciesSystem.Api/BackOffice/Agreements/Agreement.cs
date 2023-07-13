using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;

namespace InsurancePoliciesSystem.Api.BackOffice.Agreements;

public class Agreement
{
    public AgreementId AgreementId { get; set; }
    public AgreementText AgreementText { get; set; }
    public Package Package { get; set; }
    public bool IsRequired { get; set; }
    public bool IsDeleted { get; set; }

    public void MarkAsDeleted() => IsDeleted = true;
}

public record AgreementId(Guid Value);
public record AgreementText(string Value);

public interface IAgreementsRepository
{
    Task<List<Agreement>> GetAllAsync();
    Task<Agreement?> GetByIdAsync(AgreementId agreementId);
    Task AddAsync(Agreement agreement);
    Task SaveAsync(Agreement agreement);
}

public class InMemoryAgreementsRepository : IAgreementsRepository
{
    private readonly Dictionary<AgreementId, Agreement> _agreements = new();
    
    public Task<List<Agreement>>  GetAllAsync()
        => Task.FromResult(_agreements.Values.ToList());
    
    public Task<Agreement?> GetByIdAsync(AgreementId agreementId)
        => Task.FromResult(_agreements.GetValueOrDefault(agreementId));

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
}


public class AgreementDto
{
    public Guid AgreementId { get; set; }
    public string AgreementText { get; set; }
    public string Package { get; set; }
    public bool IsRequired { get; set; }
}

internal static class AgreementDtoMapper
{
    internal static AgreementDto MapToDto(this Agreement agreement)
        => new()
        {
            AgreementId = agreement.AgreementId.Value,
            AgreementText = agreement.AgreementText.Value,
            Package = agreement.Package.Value,
            IsRequired = agreement.IsRequired
        };
    
    internal static Agreement MapToDomain(this AgreementDto agreement)
        => new()
        {
            AgreementId = new AgreementId(agreement.AgreementId),
            AgreementText = new AgreementText(agreement.AgreementText),
            Package = new Package(agreement.Package),
            IsRequired = agreement.IsRequired
        };
}


