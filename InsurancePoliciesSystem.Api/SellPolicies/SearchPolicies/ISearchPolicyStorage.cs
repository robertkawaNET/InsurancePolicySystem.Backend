using InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;

public interface ISearchPolicyStorage
{
    Task<SearchPolicy?> GetByPolicyNumberAsync(PolicyNumber policyNumber);
    Task AddAsync(SearchPolicy searchPolicy);
}

public class InMemorySearchPolicyStorage : ISearchPolicyStorage
{
    private readonly Dictionary<PolicyNumber, SearchPolicy> _searchPolicies = new();

    public Task<SearchPolicy?> GetByPolicyNumberAsync(PolicyNumber policyNumber)
        => Task.FromResult(_searchPolicies.GetValueOrDefault(policyNumber));

    public Task AddAsync(SearchPolicy searchPolicy)
    {
        _searchPolicies.Add(searchPolicy.PolicyNumber, searchPolicy);
        return Task.CompletedTask;
    }
}

public class SearchPolicy
{
    public PolicyId PolicyId { get; set; }
    public PolicyNumber PolicyNumber { get; set; }
    public Price Price { get; set; }
    public Package Package { get; set; }
}

public record PolicyId(Guid Value); 

public record Price(decimal Value);

public readonly record struct Package
{
    private readonly string _value;

    private string Value => _value;

    private Package(string value)
    {
        _value = value;
    }

    public static readonly Package Work = new("Work");
    public static readonly Package Individual = new("Individual");
}