using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;

public class IndividualTravelInsurancePolicy
{
    public IndividualTravelInsurancePolicyId PolicyId { get; set; }
    public Policyholder Policyholder { get; set; }
    public Variant Variant { get; set; }
    public List<AgreementId> AgreementsIds { get; set; }
    public PolicyNumber PolicyNumber { get; set; }
    public DateTime CreateDate { get; set; }
    public Status Status = Status.Active;

    public void Cancel(DateTime now)
    {
        if (now >= Variant.DateFrom)
        {
            throw new InvalidOperationException("The policy cannot be canceled after its start date");
        }

        Status = Status.Cancelled;
    }
}