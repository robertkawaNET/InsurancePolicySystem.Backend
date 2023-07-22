namespace InsurancePoliciesSystem.Api.BackOffice.Agreements.App;

public class AgreementDto
{
    public Guid AgreementId { get; set; }
    public string AgreementText { get; set; }
    public string Package { get; set; }
    public bool IsRequired { get; set; }
}