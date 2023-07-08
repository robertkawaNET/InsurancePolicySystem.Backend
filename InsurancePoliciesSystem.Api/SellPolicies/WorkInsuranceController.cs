using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.SellPolicies;

[Authorize]
[ApiController]
[Route("api/sell-policies/work-insurance")]
public class WorkInsuranceController : ControllerBase
{
    [HttpGet]
    public IActionResult Test() => Ok("test");
}