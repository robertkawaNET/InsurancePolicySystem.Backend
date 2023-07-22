using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.Pdf;

public class WorkInsurancePolicyPdfModel
{
    public string PolicyNumber { get; set; }
    public Policyholder Policyholder { get; set; }
    public List<PersonPdfModel> Persons { get; set; }
    public VariantPdfModel Variant { get; set; }
    public List<string> Agreements { get; set; }
}