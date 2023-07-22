namespace InsurancePoliciesSystem.Api.Users.Domain;

public interface IUserRepository
{
    Task<User?> GetByLoginAsync(Login login);
}