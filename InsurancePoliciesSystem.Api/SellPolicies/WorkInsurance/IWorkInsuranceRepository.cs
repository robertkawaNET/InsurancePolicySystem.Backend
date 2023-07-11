using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;
using InsurancePoliciesSystem.Api.Users;

namespace InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;

public interface IWorkInsuranceRepository
{
    Task<WorkInsurancePolicy?> GetByIdAsync(WorkInsurancePolicyId policyId);
    Task AddAsync(WorkInsurancePolicy policy);
    Task SaveAsync(WorkInsurancePolicy policy);
}

internal class InMemoryWorkInsuranceRepository : IWorkInsuranceRepository
{
    private static readonly List<WorkInsurancePolicy> Policies = new();

    private readonly ISearchPolicyStorage _searchPolicyStorage;

    public InMemoryWorkInsuranceRepository(ISearchPolicyStorage searchPolicyStorage)
    {
        _searchPolicyStorage = searchPolicyStorage;
    }

    public Task<WorkInsurancePolicy?> GetByIdAsync(WorkInsurancePolicyId policyId)
        =>  Task.FromResult(Policies.SingleOrDefault(x => x.PolicyId == policyId));

    public async Task AddAsync(WorkInsurancePolicy policy)
    {
        Policies.Add(policy);
        
        await _searchPolicyStorage.AddAsync(new SearchPolicy
        {
            PolicyId = new PolicyId(policy.PolicyId.Value),
            PolicyNumber = policy.PolicyNumber,
            Price = policy.Variant.Price,
            Package = Package.Work
        });
    }
    
    public Task SaveAsync(WorkInsurancePolicy policy)
    {
        return Task.CompletedTask;
    }
}


public class Policyholder
{
    public string CompanyName { get; set; }
    public string Nip { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string FlatNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}


public class WorkInsurancePolicy
{
    public WorkInsurancePolicyId PolicyId { get; set; }
    public Policyholder Policyholder { get; set; }
    public Variant Variant { get; set; }
    public List<int> AgreementsIds { get; set; }
    public PolicyNumber PolicyNumber { get; set; }
    public List<Person> Persons { get; set; } = new();

    public void AddPerson(Person person) => Persons.Add(person);
    
    public void DeletePerson(PersonId personId) => Persons.Single(x => x.PersonId == personId).MarkAsDeleted();
}

public class Person
{
    public PersonId PersonId { get; set; }
    public FirstName FirstName { get; set; }
    public LastName LastName { get; set; }
    public bool IsDeleted { get; set; }

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

public class PersonDto
{
    public Guid PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public record PersonId(Guid Value);

public record WorkInsurancePolicyId(Guid Value);

public record PolicyNumber(string Value);

public class Variant
{
    public int NumberOfPeople { get; set; }
    public int InsuranceSum { get; set; }
    public PackageType SelectedPackage { get; set; }
    public PolicyType PolicyType { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public Price Price { get; set; }
}

internal static class Mapper
{
    private static int _policyCount;
    
    public static WorkInsurancePolicy Map(CreatePolicyDto createPolicyDto)
    {
        WorkInsurancePolicy workInsurancePolicy = new WorkInsurancePolicy
        {
            PolicyId = new WorkInsurancePolicyId(Guid.NewGuid()),
            Policyholder = MapPolicyholderDtoToPolicyholder(createPolicyDto.Policyholder),
            Variant = MapVariantConfigurationDtoToVariant(createPolicyDto.Variant),
            AgreementsIds = createPolicyDto.AgreementsIds,
            PolicyNumber = new PolicyNumber($"120-20-{_policyCount++}")
        };

        return workInsurancePolicy;
    }

    private static Policyholder MapPolicyholderDtoToPolicyholder(PolicyholderDto policyholderDto)
    {
        return new Policyholder
        {
            CompanyName = policyholderDto.CompanyName,
            Nip = policyholderDto.Nip,
            Street = policyholderDto.Street,
            HouseNumber = policyholderDto.HouseNumber,
            FlatNumber = policyholderDto.FlatNumber,
            PostalCode = policyholderDto.PostalCode,
            City = policyholderDto.City,
            Email = policyholderDto.Email,
            PhoneNumber = policyholderDto.PhoneNumber
        };
    }

    private static Variant MapVariantConfigurationDtoToVariant(VariantConfigurationDto variantConfigurationDto)
    {
        return new Variant
        {
            NumberOfPeople = variantConfigurationDto.NumberOfPeople,
            InsuranceSum = variantConfigurationDto.InsuranceSum,
            SelectedPackage = variantConfigurationDto.SelectedPackage,
            PolicyType = variantConfigurationDto.PolicyType,
            DateFrom = variantConfigurationDto.DateFrom,
            DateTo = variantConfigurationDto.DateTo
        };
    }

}


