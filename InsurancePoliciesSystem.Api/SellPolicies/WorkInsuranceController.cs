using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.SellPolicies;

[ApiController]
[Route("api/sell-policies/work-insurance")]
public class WorkInsuranceController : ControllerBase
{
    [HttpGet]
    public IActionResult Test() => Ok("test");
}