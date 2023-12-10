namespace working_good.business.core.Abstractions;

public interface IPasswordManager
{
    string Secure(string password);
    bool IsValidPassword(string password, string securedPassword);
}