using InsurancePoliciesSystem.Api.BackOffice.Agreements;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Services;
using InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;
using InsurancePoliciesSystem.Api.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.SellPolicies;

[Authorize]
[ApiController]
[Route("api/sell-policies/work-insurance")]
public class WorkInsuranceController : ControllerBase
{
    private readonly WorkInsurancePdfGenerator _pdfGenerator;
    private readonly IWorkInsuranceRepository _repository;
    private readonly ISearchPolicyStorage _searchPolicyStorage;
    private readonly IAgreementsRepository _agreementsRepository;


    public WorkInsuranceController(WorkInsurancePdfGenerator pdfGenerator, IWorkInsuranceRepository repository, ISearchPolicyStorage searchPolicyStorage, IAgreementsRepository agreementsRepository)
    {
        _pdfGenerator = pdfGenerator;
        _repository = repository;
        _searchPolicyStorage = searchPolicyStorage;
        _agreementsRepository = agreementsRepository;
    }

    [HttpGet]
    public IActionResult Test() => Ok("test");
    
    [HttpGet, Route("config")]
    public IActionResult GetPriceConfig() => Ok(new PriceConfigDto());

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
        var policy = Mapper.Map(request);
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

public class PriceConfigDto
{
    public decimal Basic { get; set; } = 0.2m;
    public decimal Plus { get; set; } = 0.5m;
    public decimal Max { get; set; } = 0.7m;
    public decimal TickBite { get; set; } = 0.35m;
    public decimal Hospitalization { get; set; } = 0.6m;
}

public class PolicyholderDto
{
    public string CompanyName { get; set; }
    public string Nip { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string FlatNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}


public class CreatePolicyDto
{
    public PolicyholderDto Policyholder { get; set; }
    public VariantConfigurationDto Variant { get; set; }
    public List<Guid> AgreementsIds { get; set; }
}

public class VariantConfigurationDto
{
    public int NumberOfPeople { get; set; }
    public int InsuranceSum { get; set; }
    public PackageType SelectedPackage { get; set; }
    public PolicyType PolicyType { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}

public enum PolicyType
{
    Personal,
    NonPersonal
}

public enum PackageType
{
    Basic,
    Plus,
    Max
}


