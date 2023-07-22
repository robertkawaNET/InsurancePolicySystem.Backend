namespace InsurancePoliciesSystem.Api.SellPolicies.InsurancePackages.IndividualTravelInsurance.Domain;

public readonly record struct Country
{
    public string Code { get; }
    public string Name { get; }

    private Country(string code, string name)
    {
        Code = code;
        Name = name;
    }

    private static readonly Dictionary<string, Country> CountryByCode = new()
    {
        { "US", new Country("US", "United States") },
        { "CA", new Country("CA", "Canada") },
        { "GB", new Country("GB", "United Kingdom") },
        { "DE", new Country("DE", "Germany") },
        { "FR", new Country("FR", "France") },
        { "JP", new Country("JP", "Japan") },
        { "AU", new Country("AU", "Australia") },
        { "BR", new Country("BR", "Brazil") },
        { "CN", new Country("CN", "China") },
        { "IN", new Country("IN", "India") },
        { "RU", new Country("RU", "Russia") },
        { "SA", new Country("SA", "Saudi Arabia") },
        { "ZA", new Country("ZA", "South Africa") },
        { "KR", new Country("KR", "South Korea") },
        { "MX", new Country("MX", "Mexico") }
    };

    public static readonly Country UnitedStates = CountryByCode["US"];
    public static readonly Country Canada = CountryByCode["CA"];
    public static readonly Country UnitedKingdom = CountryByCode["GB"];
    public static readonly Country Germany = CountryByCode["DE"];
    public static readonly Country France = CountryByCode["FR"];
    public static readonly Country Japan = CountryByCode["JP"];
    public static readonly Country Australia = CountryByCode["AU"];
    public static readonly Country Brazil = CountryByCode["BR"];
    public static readonly Country China = CountryByCode["CN"];
    public static readonly Country India = CountryByCode["IN"];
    public static readonly Country Russia = CountryByCode["RU"];
    public static readonly Country SaudiArabia = CountryByCode["SA"];
    public static readonly Country SouthAfrica = CountryByCode["ZA"];
    public static readonly Country SouthKorea = CountryByCode["KR"];
    public static readonly Country Mexico = CountryByCode["MX"];

    public static IReadOnlyCollection<Country> GetAll() => CountryByCode.Values.ToList();

    public static Country From(string code)
    {
        if (CountryByCode.TryGetValue(code, out Country country))
        {
            return country;
        }

        throw new ArgumentException("Invalid country code.", nameof(code));
    }
}