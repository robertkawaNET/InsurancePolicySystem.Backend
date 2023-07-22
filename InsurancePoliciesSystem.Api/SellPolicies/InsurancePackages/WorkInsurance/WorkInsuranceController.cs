using InsurancePoliciesSystem.Api.BackOffice.Agreements.App;
using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.Pdf;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.App.PriceConfiguration;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using InsurancePoliciesSystem.Api.Users;
using InsurancePoliciesSystem.Api.Users.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.WorkInsurance;

[ApiController]
[Route("api/sell-policies/work-insurance")]
[Authorize(Roles = "Agent,BackOffice")]
public class WorkInsuranceController : ControllerBase
{
    private readonly WorkInsurancePdfGenerator _pdfGenerator;
    private readonly IWorkInsuranceRepository _repository;
    private readonly IAgreementsRepository _agreementsRepository;
    private readonly IPriceConfigurationService _priceConfigurationService;

    public WorkInsuranceController(
        WorkInsurancePdfGenerator pdfGenerator,
        IWorkInsuranceRepository repository,
        IAgreementsRepository agreementsRepository,
        IPriceConfigurationService priceConfigurationService)
    {
        _pdfGenerator = pdfGenerator;
        _repository = repository;
        _agreementsRepository = agreementsRepository;
        _priceConfigurationService = priceConfigurationService;
    }

    [HttpGet, Route("config")]
    public IActionResult GetPriceConfig() => Ok(_priceConfigurationService.Get());

    [Authorize(Roles = "BackOffice")]
    [HttpPut, Route("config")]
    public IActionResult UpdatePriceConfig([FromBody] PriceConfigurationDto priceConfigItem)
    {
        _priceConfigurationService.Update(priceConfigItem);
        return Ok();
    }


    [HttpGet, Route("agreements")]
    public async Task<IActionResult> GetAgreements()
    {
        var agreements = (await _agreementsRepository.GetAllForPackageAsync(Package.Work))
            .Where(x => !x.IsDeleted)
            .Select(x => x.MapToDto())
            .ToList();

        return Ok(agreements);
    }

    [HttpPost, Route("create")]
    public async Task<IActionResult> Create([FromBody] CreatePolicyDto request)
    {
        var policy = Mapper.Map(request, _priceConfigurationService.Get());
        await _repository.AddAsync(policy);
        return Ok(await Task.FromResult(new
        {
            policyNumber = policy.PolicyNumber.Value,
            policyId = policy.PolicyId.Value
        }));
    }

    [HttpGet, Route("{policyId}/persons")]
    public async Task<IActionResult> GetPersons(Guid policyId)
    {
        var policy = await _repository.GetByIdAsync(new WorkInsurancePolicyId(policyId));

        var persons = policy.Persons.Select(x => new PersonDto
        {
            PersonId = x.PersonId.Value,
            FirstName = x.FirstName.Value,
            LastName = x.LastName.Value
        }).ToList();

        return Ok(persons);
    }
    
    [HttpPost, Route("{policyId}/persons")]
    public async Task<IActionResult> AddPerson(Guid policyId, [FromBody] PersonDto request)
    {
        var policy = await _repository.GetByIdAsync(new WorkInsurancePolicyId(policyId));
        policy.AddPerson(Person.Create(
            new PersonId(request.PersonId),
            new FirstName(request.FirstName),
             new LastName(request.LastName)));
        
        await _repository.SaveAsync(policy);
        
        return Ok();
    }
    
    [HttpDelete, Route("{policyId}/persons/{personId}")]
    public async Task<IActionResult> DeletePerson(Guid policyId, Guid personId)
    {
        var policy = await _repository.GetByIdAsync(new WorkInsurancePolicyId(policyId));
        policy.DeletePerson(new PersonId(personId));

        await _repository.SaveAsync(policy);
        
        return Ok();
    }
    
    [HttpGet, Route("pdf/{policyId}")]
    public async Task<IActionResult> GetPdf(Guid policyId)
    {
        var policyPdf = await _pdfGenerator.GenerateAsync(new PolicyId(policyId));
        return File(policyPdf.FileData, "application/pdf", policyPdf.FileName);
    }
}