using InsurancePoliciesSystem.Api.BackOffice.Agreements.App;
using InsurancePoliciesSystem.Api.BackOffice.Agreements.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.Pdf;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.App.PriceCOnfiguration;
using InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance;

[ApiController]
[Route("api/sell-policies/travel-insurance")]
[Authorize(Roles = "Agent,BackOffice")]
public class IndividualTravelInsuranceController : ControllerBase
{
    private readonly IIndividualTravelInsurancePriceConfigurationService _individualTravelInsurancePriceConfigurationService;
    private readonly IAgreementsRepository _agreementsRepository;
    private readonly IIndividualTravelInsuranceRepository _repository;
    private readonly IndividualTravelInsurancePdfGenerator _pdfGenerator;

    public IndividualTravelInsuranceController(
        IIndividualTravelInsurancePriceConfigurationService individualTravelInsurancePriceConfigurationService,
        IAgreementsRepository agreementsRepository,
        IIndividualTravelInsuranceRepository repository,
        IndividualTravelInsurancePdfGenerator pdfGenerator)
    {
        _individualTravelInsurancePriceConfigurationService = individualTravelInsurancePriceConfigurationService;
        _agreementsRepository = agreementsRepository;
        _repository = repository;
        _pdfGenerator = pdfGenerator;
    }

    [HttpGet, Route("config")]
    public async Task<IActionResult> GetPriceConfig() => Ok(await _individualTravelInsurancePriceConfigurationService.GetAsync());
    
    [Authorize(Roles = "BackOffice")]
    [HttpPut, Route("config")]
    public async Task<IActionResult> UpdatePriceConfig([FromBody] PriceConfigurationDto priceConfigItem)
    {
        await _individualTravelInsurancePriceConfigurationService.UpdateAsync(priceConfigItem);
        return Ok();
    }

    [HttpGet, Route("countries")]
    public IActionResult GetCountries() 
        => Ok(Country
            .GetAll()
            .Select(x => new CountryDto{Code = x.Code, Name = x.Name})
            .ToList());
    
    [HttpGet, Route("agreements")]
    public async Task<IActionResult> GetAgreements()
    {
        var agreements = (await _agreementsRepository.GetAllForPackageAsync(Package.Travel))
            .Where(x => !x.IsDeleted)
            .Select(x => x.MapToDto())
            .ToList();

        return Ok(agreements);
    }
    
    [HttpPost, Route("create")]
    public async Task<IActionResult> Create([FromBody] CreatePolicyDto request)
    {
        var policy = Mapper.Map(request, await _individualTravelInsurancePriceConfigurationService.GetAsync());
        await _repository.AddAsync(policy);
        return Ok(await Task.FromResult(new
        {
            policyNumber = policy.PolicyNumber.Value,
            policyId = policy.PolicyId.Value
        }));
    }
    
    [HttpGet, Route("pdf/{policyId}")]
    public async Task<IActionResult> GetPdf(Guid policyId)
    {
        var policyPdf = await _pdfGenerator.GenerateAsync(new PolicyId(policyId));
        return File(policyPdf.FileData, "application/pdf", policyPdf.FileName);
    }
}