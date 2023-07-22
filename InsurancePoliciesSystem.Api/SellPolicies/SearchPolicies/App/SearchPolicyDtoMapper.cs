using InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.Domain;
using InsurancePoliciesSystem.Api.SellPolicies.Shared;

namespace InsurancePoliciesSystem.Api.SellPolicies.SearchPolicies.App;

internal static class SearchPolicyDtoMapper
{
    internal static SearchPolicyDto MapToDto(this SearchPolicy searchPolicy, DateTime now)
        => new()
        {
            PolicyId = searchPolicy.PolicyId.Value,
            PolicyNumber = searchPolicy.PolicyNumber.Value,
            Price = searchPolicy.Price.Value,
            Package = searchPolicy.Package.Value,
            CreateDate = searchPolicy.CreateDate,
            DateFrom = searchPolicy.DateFrom,
            DateTo = searchPolicy.DateTo,
            Status = searchPolicy.Status,
            CanCancel = now < searchPolicy.DateFrom && searchPolicy.Status == Status.Active
        };
}