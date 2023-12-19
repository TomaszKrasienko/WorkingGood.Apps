using Microsoft.AspNetCore.Identity;
using working_good.business.core.Abstractions;
using working_good.business.core.Models;

namespace working_good.business.infrastructure.Services;

internal sealed class PasswordManager(IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string Secure(string password)
        => passwordHasher.HashPassword(default!, password);

    public bool IsValidPassword(string password, string securedPassword)
        => passwordHasher
            .VerifyHashedPassword(default!, securedPassword, password) == PasswordVerificationResult.Success;


}