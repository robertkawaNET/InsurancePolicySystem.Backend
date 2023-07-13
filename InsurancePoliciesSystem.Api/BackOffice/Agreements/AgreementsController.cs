using Microsoft.AspNetCore.Mvc;

namespace InsurancePoliciesSystem.Api.BackOffice.Agreements;

[ApiController]
[Route("api/back-office/agreements")]
public class AgreementsController : ControllerBase
{
    private readonly IAgreementsRepository _agreementsRepository;

    public AgreementsController(IAgreementsRepository agreementsRepository)
    {
        _agreementsRepository = agreementsRepository;
    }

    [HttpGet, Route("")]
    public async Task<IActionResult> GetAgreements()
    {
        var agreements = (await _agreementsRepository.GetAllAsync())
            .Select(x => x.MapToDto())
            .ToList();

        return Ok(agreements);
    } 
    
    [HttpPost, Route("")]
    public async Task<IActionResult> AddAgreement([FromBody] AgreementDto agreementDto)
    {
        var agreement = agreementDto.MapToDomain();
        await _agreementsRepository.AddAsync(agreement);

        return Ok(agreementDto);
    } 
    
    [HttpPut, Route("")]
    public async Task<IActionResult> UpdateAgreement([FromBody] AgreementDto agreementDto)
    {
        var agreement = agreementDto.MapToDomain();
        await _agreementsRepository.SaveAsync(agreement);

        return Ok();
    } 
    
    [HttpDelete, Route("{agreementId}")]
    public async Task<IActionResult> DeleteAgreement(Guid agreementId)
    {
        var agreement = await _agreementsRepository.GetByIdAsync(new AgreementId(agreementId));
        agreement.MarkAsDeleted();
        await _agreementsRepository.SaveAsync(agreement);

        return Ok();
    } 
}