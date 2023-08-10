using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;

public class WorkInsurancePolicy
{
    public WorkInsurancePolicyId PolicyId { get; private set; }
    public Policyholder Policyholder { get; private set; }
    public Variant Variant { get; }
    public List<AgreementId> AgreementsIds { get; private set; }
    public PolicyNumber PolicyNumber { get; private set; }
    public List<Person> Persons { get; } = new();
    public DateTime CreateDate { get; private set; }
    public Status Status { get; private set; } = Status.Active;

    public void AddPerson(Person person)
    {
        Persons.Add(person);
        Recalculate();
    }
    
    public void DeletePerson(PersonId personId)
    {
        Persons.Single(x => x.PersonId == personId).MarkAsDeleted();
        Recalculate();
    }

    private void Recalculate()
    {
        Variant.NumberOfPeople = Persons.Count(x => !x.IsDeleted);
        Variant.TotalPrice = new Price(Persons.Count * Variant.PricePerPerson.Value);
    }

    public void Cancel(DateTime now)
    {
        if (now >= Variant.DateFrom)
        {
            throw new InvalidOperationException("The policy cannot be canceled after its start date");
        }

        Status = Status.Cancelled;
    }

    protected WorkInsurancePolicy()
    {
    }
    
    public WorkInsurancePolicy(
        WorkInsurancePolicyId policyId,
        Policyholder policyholder,
        Variant variant,
        List<AgreementId> agreementsIds,
        PolicyNumber policyNumber,
        DateTime createDate)
    {
        PolicyId = policyId;
        Policyholder = policyholder;
        Variant = variant;
        AgreementsIds = agreementsIds;
        PolicyNumber = policyNumber;
        CreateDate = createDate;
    }
}