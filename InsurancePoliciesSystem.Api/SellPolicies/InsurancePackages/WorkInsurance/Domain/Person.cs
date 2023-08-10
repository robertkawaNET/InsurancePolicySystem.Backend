using InsurancePoliciesSystem.Api.Users;
using InsurancePoliciesSystem.Api.Users.Domain;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;

public class Person
{
    public PersonId PersonId { get; private set; }
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public bool IsDeleted { get; private set; }

    public static Person Create(PersonId personId, FirstName firstName, LastName lastName)
    {
        return new Person
        {
            PersonId = personId,
            FirstName = firstName,
            LastName = lastName
        };
    }

    public void MarkAsDeleted() => IsDeleted = true;
}