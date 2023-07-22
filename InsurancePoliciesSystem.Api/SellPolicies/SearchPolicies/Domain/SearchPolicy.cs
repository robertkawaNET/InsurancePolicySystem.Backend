using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;

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