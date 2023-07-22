namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App;

public class CreatePolicyDto
{
    public PolicyholderDto Policyholder { get; set; }
    public VariantConfigurationDto Variant { get; set; }
    public List<Guid> AgreementsIds { get; set; }
}