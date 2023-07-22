namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;

public interface IIndividualTravelInsuranceRepository
{
    Task<IndividualTravelInsurancePolicy?> GetByIdAsync(IndividualTravelInsurancePolicyId policyPolicyId);
    Task AddAsync(IndividualTravelInsurancePolicy policy);
    Task SaveAsync(IndividualTravelInsurancePolicy policy);
}