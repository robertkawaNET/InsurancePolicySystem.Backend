using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Services;
using InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;
using InsurancePoliciesSystem.Api.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;

[Authorize(Roles = "Agent,BackOffice")]
[ApiController]
[Route("api/search-policies")]
public class SearchPoliciesController : ControllerBase
{
    private readonly ISearchPolicyStorage _searchPolicyStorage;
    private readonly PdfProvider _pdfProvider;
    private readonly CancelPolicyService _cancelPolicyService;
    private readonly IClock _clock;

    public SearchPoliciesController(ISearchPolicyStorage searchPolicyStorage, PdfProvider pdfProvider, CancelPolicyService cancelPolicyService, IClock clock)
    {
        _searchPolicyStorage = searchPolicyStorage;
        _pdfProvider = pdfProvider;
        _cancelPolicyService = cancelPolicyService;
        _clock = clock;
    }

    [HttpGet, Route("{policyNumber}")]
    public async Task<IActionResult> GetByPolicyNumber(string policyNumber)
    {
        var policy = await _searchPolicyStorage.GetByPolicyNumberAsync(new PolicyNumber(policyNumber));
        if (policy is null)
        {
            return NotFound();
        }

        return Ok(policy.MapToDto(_clock.UtcNow));
    } 
    
    [HttpGet, Route("pdf/{policyId}")]
    public async Task<IActionResult> GetPdf(Guid policyId)
    {
        var policyPdf = await _pdfProvider.GenerateAsync(new PolicyId(policyId));
        return File(policyPdf.FileData, "application/pdf", policyPdf.FileName);
    }
    
    [HttpPut, Route("action/cancel/{policyId}")]
    public async Task<IActionResult> CancelPolicy(Guid policyId)
    {
        await _cancelPolicyService.Cancel(new PolicyId(policyId));
        return Ok();
    }
    
    [HttpGet, Route("")]
    public async Task<IActionResult> GetAll()
    {
        var policies = (await _searchPolicyStorage.GetAllAsync())
            .Select(x => x.MapToDto(_clock.UtcNow))
            .ToList();
        
        return Ok(policies);
    } 
}