using InsurancePoliciesSystem.Api.Users.Domain;

namespace InsurancePoliciesSystem.Api.Users.Infrastructure;

internal class InMemoryUserRepository : IUserRepository
{
    private static readonly List<User> Users = new()
    {
        new User(
            new UserId(Guid.Parse("7a3e3302-4619-4c0f-8ecd-c303b790459b")),
            new FirstName("Robert"),
            new LastName("Kawa"),
            new Login("robertkawa"),
            new Password("robertkawa123"),
            Role.BackOffice
        ),
        new User(
            new UserId(Guid.Parse("c182f578-50b8-447a-85e4-0ed0fe17b08c")),
            new FirstName("Emilly"),
            new LastName("Williams"),
            new Login("emillywilliams"),
            new Password("emillywilliams123"),
            Role.BackOffice
        ),
        new User(
            new UserId(Guid.Parse("c0a05dab-bb68-4846-b507-e878b8f5d394")),
            new FirstName("John"),
            new LastName("Doe"),
            new Login("johndoe"),
            new Password("johndoe123"),
            Role.Agent
        ),
        new User(
            new UserId(Guid.Parse("c825c37d-3703-4f62-afac-6b43799c8747")),
            new FirstName("Jane"),
            new LastName("Smith"),
            new Login("janesmith"),
            new Password("janesmith123"),
            Role.Agent
        ),
        new User(
            new UserId(Guid.Parse("79333407-1221-4c86-93c9-face720bf84c")),
            new FirstName("James"),
            new LastName("Johnson"),
            new Login("jamesjohnson"),
            new Password("jamesjohnson123"),
            Role.Agent
        )
    };

    public Task<User?> GetByLoginAsync(Login login)
        => Task.FromResult(Users.SingleOrDefault(x => x.Login == login));
}
