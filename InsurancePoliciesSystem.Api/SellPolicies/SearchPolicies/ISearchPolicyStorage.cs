using System.Collections.Immutable;
using InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;

public interface ISearchPolicyStorage
{
    Task<SearchPolicy?> GetByPolicyNumberAsync(PolicyNumber policyNumber);
    Task<SearchPolicy?> GetByPolicyIdAsync(PolicyId policyId);
    Task<List<SearchPolicy>> GetAllAsync();
    Task AddAsync(SearchPolicy searchPolicy);
    Task SaveAsync(SearchPolicy searchPolicy);
}

public class InMemorySearchPolicyStorage : ISearchPolicyStorage
{
    private readonly Dictionary<PolicyId, SearchPolicy> _searchPolicies = new();

    public Task<SearchPolicy?> GetByPolicyNumberAsync(PolicyNumber policyNumber)
        => Task.FromResult(_searchPolicies.Values.SingleOrDefault(x => x.PolicyNumber == policyNumber));
    
    public Task<SearchPolicy?> GetByPolicyIdAsync(PolicyId policyId)
        => Task.FromResult(_searchPolicies.GetValueOrDefault(policyId));

    public Task<List<SearchPolicy>> GetAllAsync()
        => Task.FromResult(_searchPolicies.Values.ToList());
    
    public Task AddAsync(SearchPolicy searchPolicy)
    {
        _searchPolicies.Add(searchPolicy.PolicyId, searchPolicy);
        return Task.CompletedTask;
    }
    
    public Task SaveAsync(SearchPolicy searchPolicy)
    {
        _searchPolicies[searchPolicy.PolicyId] = searchPolicy;
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
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public Status Status { get; set; }
}

public class SearchPolicyDto
{
    public Guid PolicyId { get; set; }
    public string PolicyNumber { get; set; }
    public decimal Price { get; set; }
    public string Package { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public bool CanCancel { get; set; }
}

internal static class SearchPolicyDtoMapper
{
    internal static SearchPolicyDto MapToDto(this SearchPolicy searchPolicy, DateTime now)
        => new()
        {
            PolicyId = searchPolicy.PolicyId.Value,
            PolicyNumber = searchPolicy.PolicyNumber.Value,
            Price = searchPolicy.Price.Value,
            Package = searchPolicy.Package.Value,
            CreateDate = searchPolicy.CreateDate,
            DateFrom = searchPolicy.DateFrom,
            DateTo = searchPolicy.DateTo,
            CanCancel = now < searchPolicy.DateFrom && searchPolicy.Status == Status.Active
        };
}



public record PolicyId(Guid Value); 

public record Price(decimal Value);

public readonly record struct Package(string Value)
{
    public static readonly Package Work = new("Work");
    public static readonly Package Individual = new("Individual");
}