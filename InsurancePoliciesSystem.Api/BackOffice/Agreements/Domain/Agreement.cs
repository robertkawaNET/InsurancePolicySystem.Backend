using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;

public class Agreement
{
    public AgreementId AgreementId { get; set; }
    public AgreementText AgreementText { get; set; }
    public Package Package { get; set; }
    public bool IsRequired { get; set; }
    public bool IsDeleted { get; set; }

    public void MarkAsDeleted() => IsDeleted = true;
}