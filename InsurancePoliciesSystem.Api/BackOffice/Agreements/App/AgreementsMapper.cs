using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.BackOffice.Agreements.App;

internal static class AgreementsMapper
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