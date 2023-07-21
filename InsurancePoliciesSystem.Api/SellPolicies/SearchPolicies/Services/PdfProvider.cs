namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Services;

public class PdfProvider
{
    private readonly IEnumerable<PolicyPdfGenerator> _pdfGenerators;
    private readonly ISearchPolicyStorage _searchPolicyStorage;

    public PdfProvider(IEnumerable<PolicyPdfGenerator> pdfGenerators, ISearchPolicyStorage searchPolicyStorage)
    {
        _pdfGenerators = pdfGenerators;
        _searchPolicyStorage = searchPolicyStorage;
    }
    
    public async Task<PolicyPdf> GenerateAsync(PolicyId policyId)
    {
        var policy = await _searchPolicyStorage.GetByPolicyIdAsync(policyId);

        var pdfGenerator = _pdfGenerators.Single(x => x.IsResponsible(policy.Package));

        return await pdfGenerator.GenerateAsync(policyId);
    }
}

public abstract class PolicyPdfGenerator
{
    public abstract Package Package { get; }
    
    internal bool IsResponsible(Package package) => package.Equals(Package);

    public abstract Task<PolicyPdf> GenerateAsync(PolicyId policyId);
}

public record PolicyPdf(string FileName, byte[] FileData);