namespace working_good.business.core.Policies.Abstractions;

public interface IPasswordPolicy
{
    bool VerifyPassword(string password);
}