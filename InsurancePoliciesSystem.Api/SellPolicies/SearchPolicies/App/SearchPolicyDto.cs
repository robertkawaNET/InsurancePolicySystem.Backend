namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.App;

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