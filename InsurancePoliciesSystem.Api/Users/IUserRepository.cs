namespace InsurancePoliciesSystem.Api.Users;

public interface IUserRepository
{
    Task<User?> GetByLoginAsync(Login login);
}

internal class InMemoryUserRepository : IUserRepository
{
    private static readonly List<User> Users = new()
    {
        new User(
            new UserId(Guid.Parse("7a3e3302-4619-4c0f-8ecd-c303b790459b")),
            new FirstName("Robert"),
            new LastName("Kawa"),
            new Login("robertkawa1992"),
            new Password("testpass")
        ),
        new User(
            new UserId(Guid.Parse("c0a05dab-bb68-4846-b507-e878b8f5d394")),
            new FirstName("John"),
            new LastName("Doe"),
            new Login("johndoe2000"),
            new Password("testpass123")
        )
    };

    public Task<User?> GetByLoginAsync(Login login)
        => Task.FromResult(Users.SingleOrDefault(x => x.Login == login));
}
