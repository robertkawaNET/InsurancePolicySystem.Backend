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
    [HttpGet]
    public IActionResult Test() => Ok("test");
    
    [HttpGet, Route("config")]
    public IActionResult GetPriceConfig() => Ok(new PriceConfigDto());

    [HttpPost, Route("create")]
    public async Task<IActionResult> Create([FromBody] CreatePolicyDto request)
    {
        return Ok(await Task.FromResult(new {policyNumber = "123-324-234"}));
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
    public List<int> AgreementsIds { get; set; }
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


