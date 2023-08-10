namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;

public class Policyholder
{
    public string? CompanyName { get; set; }
    public string Nip { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string FlatNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}