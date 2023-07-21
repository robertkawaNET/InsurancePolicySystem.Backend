namespace InsurancePoliciesSystem.Api.Shared;

public static class DateTimeExtensions
{
    public static int CalculateDaysDifference(this DateTime startDate, DateTime endDate)
    {
        var timeSpan = endDate - startDate;
        var daysDifference = timeSpan.Days;
        return Math.Abs(daysDifference);
    }
}