using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.Pdf;

public class WorkInsurancePolicyPdfModelProvider
{
    private readonly IWorkInsuranceRepository _workInsuranceRepository;
    private readonly IAgreementsRepository _agreementsRepository;

    public WorkInsurancePolicyPdfModelProvider(IWorkInsuranceRepository workInsuranceRepository, IAgreementsRepository agreementsRepository)
    {
        _workInsuranceRepository = workInsuranceRepository;
        _agreementsRepository = agreementsRepository;
    }

    internal async Task<WorkInsurancePolicyPdfModel> GetAsync(WorkInsurancePolicyId policyId)
    {
        var policy = await _workInsuranceRepository.GetByIdAsync(policyId);
        var agreements = await _agreementsRepository.GetByIdsAsync(policy.AgreementsIds);

        return new WorkInsurancePolicyPdfModel
        {
            PolicyNumber = policy.PolicyNumber.Value,
            Policyholder = policy.Policyholder,
            Persons = policy.Persons.Select(x => new PersonPdfModel
            {
                FirstName = x.FirstName.Value,
                LastName = x.LastName.Value
            }).ToList(),
            Variant = new VariantPdfModel
            {
                NumberOfPeople = policy.Variant.NumberOfPeople,
                InsuranceSum = policy.Variant.InsuranceSum,
                SelectedPackage = policy.Variant.SelectedPackage.ToString().ToUpper(),
                DateFrom = policy.Variant.DateFrom,
                DateTo = policy.Variant.DateTo,
                PricePerPerson = policy.Variant.PricePerPerson.Value,
                TotalPrice = policy.Variant.TotalPrice.Value
            },
            Agreements = agreements.Select(x => x.AgreementText.Value).ToList(),
            
        };
    }
}