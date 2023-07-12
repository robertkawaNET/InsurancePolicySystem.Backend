using System.Collections.Immutable;
using InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;

public interface ISearchPolicyStorage
{
    Task<SearchPolicy?> GetByPolicyNumberAsync(PolicyNumber policyNumber);
    Task<SearchPolicy?> GetByPolicyIdAsync(PolicyId policyId);
    Task<List<SearchPolicy>> GetAllAsync();
    Task AddAsync(SearchPolicy searchPolicy);
}

public class InMemorySearchPolicyStorage : ISearchPolicyStorage
{
    private readonly List<SearchPolicy> _searchPolicies = new();

    public Task<SearchPolicy?> GetByPolicyNumberAsync(PolicyNumber policyNumber)
        => Task.FromResult(_searchPolicies.SingleOrDefault(x => x.PolicyNumber == policyNumber));
    
    public Task<SearchPolicy?> GetByPolicyIdAsync(PolicyId policyId)
        => Task.FromResult(_searchPolicies.SingleOrDefault(x => x.PolicyId == policyId));

    public Task<List<SearchPolicy>> GetAllAsync()
        => Task.FromResult(_searchPolicies.ToList());
    
    public Task AddAsync(SearchPolicy searchPolicy)
    {
        _searchPolicies.Add(searchPolicy);
        return Task.CompletedTask;
    }
}

public class SearchPolicy
{
    public PolicyId PolicyId { get; set; }
    public PolicyNumber PolicyNumber { get; set; }
    public Price Price { get; set; }
    public Package Package { get; set; }
    public DateTime CreateDate { get; set; }
}

public class SearchPolicyDto
{
    public Guid PolicyId { get; set; }
    public string PolicyNumber { get; set; }
    public decimal Price { get; set; }
    public string Package { get; set; }
    public DateTime CreateDate { get; set; }
}

internal static class SearchPolicyDtoMapper
{
    internal static SearchPolicyDto MapToDto(this SearchPolicy searchPolicy)
        => new()
        {
            PolicyId = searchPolicy.PolicyId.Value,
            PolicyNumber = searchPolicy.PolicyNumber.Value,
            Price = searchPolicy.Price.Value,
            Package = searchPolicy.Package.Value,
            CreateDate = searchPolicy.CreateDate
        };
}



public record PolicyId(Guid Value); 

public record Price(decimal Value);

public readonly record struct Package
{
    public string Value { get; }

    private Package(string value)
    {
        Value = value;
    }

    public static readonly Package Work = new("Work");
    public static readonly Package Individual = new("Individual");
}