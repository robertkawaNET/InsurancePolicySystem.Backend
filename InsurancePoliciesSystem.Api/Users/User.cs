﻿namespace InsurancePoliciesSystem.Api.Users;

public class User
{
    public UserId UserId { get; }
    public FirstName FirstName { get; set; }
    public LastName LastName { get; set; }
    public Login Login { get; set; }
    public Password Password { get; set; }
    public Role Role { get; set; }

    public User(UserId userId, FirstName firstName, LastName lastName, Login login, Password password, Role role)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Login = login;
        Password = password;
        Role = role;
    }
}

public readonly record struct UserId(Guid Value);
public readonly record struct FirstName(string Value);
public readonly record struct LastName(string Value);
public readonly record struct Login(string Value);
public readonly record struct Password(string Value);

public enum Role
{
    Agent,
    Admin
}