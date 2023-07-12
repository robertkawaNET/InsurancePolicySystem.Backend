using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Services;
using InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;

[Authorize]
[ApiController]
[Route("api/search-policies")]
public class SearchPoliciesController : ControllerBase
{
    private readonly ISearchPolicyStorage _searchPolicyStorage;
    private readonly PdfProvider _pdfProvider;

    public SearchPoliciesController(ISearchPolicyStorage searchPolicyStorage, PdfProvider pdfProvider)
    {
        _searchPolicyStorage = searchPolicyStorage;
        _pdfProvider = pdfProvider;
    }

    [HttpGet, Route("{policyNumber}")]
    public async Task<IActionResult> GetByPolicyNumber(string policyNumber)
    {
        var policy = await _searchPolicyStorage.GetByPolicyNumberAsync(new PolicyNumber(policyNumber));
        if (policy is null)
        {
            return NotFound();
        }

        return Ok(policy.MapToDto());
    } 
    
    [HttpGet, Route("pdf/{policyId}")]
    public async Task<IActionResult> GetPdf(Guid policyId)
    {
        var policyPdf = await _pdfProvider.GenerateAsync(new PolicyId(policyId));
        return File(policyPdf.FileData, "application/pdf", policyPdf.FileName);
    }
    
    [HttpGet, Route("")]
    public async Task<IActionResult> GetAll()
    {
        var policies = (await _searchPolicyStorage.GetAllAsync())
            .Select(x => x.MapToDto())
            .ToList();
        
        return Ok(policies);
    } 
}