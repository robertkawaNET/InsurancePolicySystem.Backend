namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;

public interface IWorkInsuranceRepository
{
    Task<WorkInsurancePolicy?> GetByIdAsync(WorkInsurancePolicyId policyId);
    Task AddAsync(WorkInsurancePolicy policy);
    Task SaveAsync(WorkInsurancePolicy policy);
}