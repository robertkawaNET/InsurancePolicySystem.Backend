namespace InsurancePoliciesSystem.Api.SellPolicies.Shared;

public readonly record struct Package(string Value)
{
    public static readonly Package Work = new("Work");
    public static readonly Package Travel = new("Travel");
}