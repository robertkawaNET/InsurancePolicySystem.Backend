using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;

public class WorkInsurancePolicy
{
    public WorkInsurancePolicyId PolicyId { get; set; }
    public Policyholder Policyholder { get; set; }
    public Variant Variant { get; set; }
    public List<AgreementId> AgreementsIds { get; set; }
    public PolicyNumber PolicyNumber { get; set; }
    public List<Person> Persons { get; set; } = new();
    public DateTime CreateDate { get; set; }
    public Status Status = Status.Active;

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
}