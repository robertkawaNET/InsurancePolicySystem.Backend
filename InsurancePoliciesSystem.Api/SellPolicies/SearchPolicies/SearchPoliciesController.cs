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

    public SearchPoliciesController(ISearchPolicyStorage searchPolicyStorage)
    {
        _searchPolicyStorage = searchPolicyStorage;
    }

    [HttpGet("{policyNumber}")]
    public async Task<IActionResult> GetByPolicyNumber(string policyNumber)
    {
        var policy = await _searchPolicyStorage.GetByPolicyNumberAsync(new PolicyNumber(policyNumber));
        if (policy is null)
        {
            return NotFound();
        }

        return Ok(policy);
    } 
}