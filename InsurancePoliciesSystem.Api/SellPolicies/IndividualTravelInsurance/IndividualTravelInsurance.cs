using InsurancePoliciesSystem.Api.BackOffice.Agreements;
using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies;
using InsurancePoliciesSystem.Api.SellPolicies.WorkInsurance;
using InsurancePoliciesSystem.Api.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.SellPolicies.IndividualTravelInsurance;

[ApiController]
[Route("api/sell-policies/travel-insurance")]
[Authorize(Roles = "Agent,BackOffice")]
public class IndividualTravelInsuranceController : ControllerBase
{
    private readonly IPriceConfigurationService _priceConfigurationService;
    private readonly IAgreementsRepository _agreementsRepository;
    private readonly ICountriesProvider _countriesProvider;
    private readonly IIndividualTravelInsuranceRepository _repository;
    private readonly IndividualTravelInsurancePdfGenerator _pdfGenerator;

    public IndividualTravelInsuranceController(
        IPriceConfigurationService priceConfigurationService,
        IAgreementsRepository agreementsRepository,
        ICountriesProvider countriesProvider,
        IIndividualTravelInsuranceRepository repository,
        IndividualTravelInsurancePdfGenerator pdfGenerator)
    {
        _priceConfigurationService = priceConfigurationService;
        _agreementsRepository = agreementsRepository;
        _countriesProvider = countriesProvider;
        _repository = repository;
        _pdfGenerator = pdfGenerator;
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

    [HttpGet, Route("countries")]
    public async Task<IActionResult> GetCountries() => Ok(await _countriesProvider.GetAsync());
    
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
        var policy = Mapper.Map(request, _priceConfigurationService.Get());
        await _repository.AddAsync(policy);
        return Ok(await Task.FromResult(new
        {
            policyNumber = policy.PolicyNumber.Value,
            policyId = policy.PolicyPolicyId.Value
        }));
    }
    
    [HttpGet, Route("pdf/{policyId}")]
    public async Task<IActionResult> GetPdf(Guid policyId)
    {
        var policyPdf = await _pdfGenerator.GenerateAsync(new PolicyId(policyId));
        return File(policyPdf.FileData, "application/pdf", policyPdf.FileName);
    }
}


public class PriceConfigurationItemDto
{
    public int InsuranceSum { get; set; }
    public decimal Essential { get; set; }
    public decimal Adventure { get; set; }
    public decimal Relax { get; set; }
}

public interface IPriceConfigurationService
{
    PriceConfigurationDto Get();
    void Update(PriceConfigurationDto priceConfiguration);
}


public class PriceConfigurationDto
{
    public List<PriceConfigurationItemDto> PriceConfigurationItems { get; set; }
}

public class InMemoryPriceConfigurationService : IPriceConfigurationService
{
    private PriceConfigurationDto _priceConfiguration = new PriceConfigurationDto
    {
        PriceConfigurationItems = new List<PriceConfigurationItemDto>
        {
            new() { InsuranceSum = 10000, Essential = 51m, Adventure = 64m, Relax = 79m },
            new() { InsuranceSum = 15000, Essential = 62m, Adventure = 75m, Relax = 80m },
            new() { InsuranceSum = 20000, Essential = 73m, Adventure = 86m, Relax = 91 }
        }
    };

    public PriceConfigurationDto Get() => _priceConfiguration;

    public void Update(PriceConfigurationDto priceConfiguration)
    {
        _priceConfiguration = priceConfiguration;
    }
}

public class CountryDto
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public interface ICountriesProvider
{
    Task<List<CountryDto>> GetAsync();
}

public class InMemoryCountriesDataProvider : ICountriesProvider
{
    private static readonly List<CountryDto> Countries = new()
    {
        new() { Code = "US", Name = "United States" },
        new() { Code = "CA", Name = "Canada" },
        new() { Code = "GB", Name = "United Kingdom" },
        new() { Code = "DE", Name = "Germany" },
        new() { Code = "FR", Name = "France" },
        new() { Code = "JP", Name = "Japan" },
        new() { Code = "AU", Name = "Australia" },
        new() { Code = "BR", Name = "Brazil" },
        new() { Code = "CN", Name = "China" },
        new() { Code = "IN", Name = "India" },
        new() { Code = "RU", Name = "Russia" },
        new() { Code = "SA", Name = "Saudi Arabia" },
        new() { Code = "ZA", Name = "South Africa" },
        new() { Code = "KR", Name = "South Korea" },
        new() { Code = "MX", Name = "Mexico" }
    };
    
    public Task<List<CountryDto>> GetAsync()
    {
        return Task.FromResult(Countries);
    }
}




public class IndividualTravelInsurancePolicy
{
    public IndividualTravelInsurancePolicyId PolicyPolicyId { get; set; }
    public Policyholder Policyholder { get; set; }
    public Variant Variant { get; set; }
    public List<AgreementId> AgreementsIds { get; set; }
    public PolicyNumber PolicyNumber { get; set; }
    public DateTime CreateDate { get; set; }
    public Status Status = Status.Active;

    public void Cancel(DateTime now)
    {
        if (now >= Variant.DateFrom)
        {
            throw new InvalidOperationException("The policy cannot be canceled after its start date");
        }

        Status = Status.Cancelled;
    }
}

public record IndividualTravelInsurancePolicyId(Guid Value);



public class Policyholder
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string FlatNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}


internal static class Mapper
{
    private static int _policyCount;
    
    public static IndividualTravelInsurancePolicy Map(CreatePolicyDto createPolicyDto, PriceConfigurationDto priceConfigurationDto)
    {
        IndividualTravelInsurancePolicy workInsurancePolicy = new IndividualTravelInsurancePolicy
        {
            PolicyPolicyId = new IndividualTravelInsurancePolicyId(Guid.NewGuid()),
            Policyholder = MapPolicyholderDtoToPolicyholder(createPolicyDto.Policyholder),
            Variant = MapVariantConfigurationDtoToVariant(createPolicyDto.Variant, priceConfigurationDto),
            AgreementsIds = createPolicyDto.AgreementsIds.Select(x => new AgreementId(x)).ToList(),
            PolicyNumber = new PolicyNumber($"250-30-{_policyCount++}"),
            CreateDate = DateTime.Now
        };

        return workInsurancePolicy;
    }

    private static Policyholder MapPolicyholderDtoToPolicyholder(PolicyholderDto policyholderDto)
    {
        return new Policyholder
        {
            FirstName = policyholderDto.FirstName,
            LastName = policyholderDto.LastName,
            Street = policyholderDto.Street,
            HouseNumber = policyholderDto.HouseNumber,
            FlatNumber = policyholderDto.FlatNumber,
            PostalCode = policyholderDto.PostalCode,
            City = policyholderDto.City,
            Email = policyholderDto.Email,
            PhoneNumber = policyholderDto.PhoneNumber
        };
    }

    private static Variant MapVariantConfigurationDtoToVariant(VariantConfigurationDto variantConfigurationDto, PriceConfigurationDto priceConfiguration)
    {
        var priceConfigurationForInsuranceSum = priceConfiguration.PriceConfigurationItems.Single(x => x.InsuranceSum == variantConfigurationDto.InsuranceSum);
        var pricePerDay = variantConfigurationDto.SelectedPackage switch
        {
            PackageType.Essential => priceConfigurationForInsuranceSum.Essential,
            PackageType.Adventure => priceConfigurationForInsuranceSum.Adventure,
            PackageType.Relax => priceConfigurationForInsuranceSum.Relax,
            _ => throw new InvalidOperationException()
        };

        var numberOfDays = variantConfigurationDto.DateFrom.CalculateDaysDifference(variantConfigurationDto.DateTo);
        
        return new Variant
        {
            InsuranceSum = variantConfigurationDto.InsuranceSum,
            SelectedPackage = variantConfigurationDto.SelectedPackage,
            DateFrom = variantConfigurationDto.DateFrom,
            DateTo = variantConfigurationDto.DateTo,
            Country = variantConfigurationDto.Country,
            PricePerDay = new Price(pricePerDay),
            TotalPrice = new Price(pricePerDay * numberOfDays)
        };
    }

}

public class CreatePolicyDto
{
    public PolicyholderDto Policyholder { get; set; }
    public VariantConfigurationDto Variant { get; set; }
    public List<Guid> AgreementsIds { get; set; }
}


public class PolicyholderDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string FlatNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}

public class VariantConfigurationDto
{
    public int InsuranceSum { get; set; }
    public PackageType SelectedPackage { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string Country { get; set; }
}


public enum PackageType
{
    Essential,
    Adventure,
    Relax
}

public class Variant
{
    public int InsuranceSum { get; set; }
    public PackageType SelectedPackage { get; set; }
    public string Country { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public Price PricePerDay { get; set; }
    public Price TotalPrice { get; set; }
}


public interface IIndividualTravelInsuranceRepository
{
    Task<IndividualTravelInsurancePolicy?> GetByIdAsync(IndividualTravelInsurancePolicyId policyPolicyId);
    Task AddAsync(IndividualTravelInsurancePolicy policy);
    Task SaveAsync(IndividualTravelInsurancePolicy policy);
}

internal class InMemoryIndividualTravelInsuranceRepository : IIndividualTravelInsuranceRepository
{
    private readonly Dictionary<IndividualTravelInsurancePolicyId, IndividualTravelInsurancePolicy> _policies = new();

    private readonly ISearchPolicyStorage _searchPolicyStorage;

    public InMemoryIndividualTravelInsuranceRepository(ISearchPolicyStorage searchPolicyStorage)
    {
        _searchPolicyStorage = searchPolicyStorage;
    }

    public Task<IndividualTravelInsurancePolicy?> GetByIdAsync(IndividualTravelInsurancePolicyId policyPolicyId)
        =>  Task.FromResult(_policies.GetValueOrDefault(policyPolicyId));

    public async Task AddAsync(IndividualTravelInsurancePolicy policy)
    {
        _policies.Add(policy.PolicyPolicyId, policy);
        
        await _searchPolicyStorage.AddAsync(new SearchPolicy
        {
            PolicyId = new PolicyId(policy.PolicyPolicyId.Value),
            PolicyNumber = policy.PolicyNumber,
            Price = policy.Variant.TotalPrice,
            Package = Package.Travel,
            CreateDate = policy.CreateDate,
            Status = policy.Status,
            DateFrom = policy.Variant.DateFrom,
            DateTo = policy.Variant.DateTo
        });
    }
    
    public async Task SaveAsync(IndividualTravelInsurancePolicy policy)
    {
        _policies[policy.PolicyPolicyId] = policy;
        
        await _searchPolicyStorage.SaveAsync(new SearchPolicy
        {
            PolicyId = new PolicyId(policy.PolicyPolicyId.Value),
            PolicyNumber = policy.PolicyNumber,
            Price = policy.Variant.TotalPrice,
            Package = Package.Work,
            CreateDate = policy.CreateDate,
            Status = policy.Status
        });
    }
}


