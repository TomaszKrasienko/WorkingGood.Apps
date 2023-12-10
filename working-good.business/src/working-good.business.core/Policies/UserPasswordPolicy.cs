using working_good.business.core.Policies.Abstractions;

namespace working_good.business.core.Policies;

internal sealed class UserPasswordPolicy : IPasswordPolicy
{
    public bool VerifyPassword(string password)
    {
        var hasLower = password.Any(char.IsLower);
        var hasUpper = password.Any(char.IsUpper);
        var hasNumber = password.Any(char.IsNumber);
        return hasLower && hasUpper && hasNumber;
    }
}