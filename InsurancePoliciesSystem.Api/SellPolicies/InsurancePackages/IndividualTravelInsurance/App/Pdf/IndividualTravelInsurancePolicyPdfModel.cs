using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.Pdf;

public class IndividualTravelInsurancePolicyPdfModel
{
    public string PolicyNumber { get; set; }
    public Policyholder Policyholder { get; set; }
    public List<PersonPdfModel> Persons { get; set; }
    public VariantPdfModel Variant { get; set; }
    public List<string> Agreements { get; set; }
}