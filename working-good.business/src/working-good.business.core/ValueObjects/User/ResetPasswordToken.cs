using System.Security.Cryptography;

namespace working_good.business.core.ValueObjects.User;

public sealed record ResetPasswordToken
{
    public string Token { get; }
    public DateTime? Expiry { get; }
    private ResetPasswordToken(DateTime expiry)
    {
        Expiry = expiry;
        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace("/", "").Replace("==", "");
    }
    
    internal static ResetPasswordToken Create()
        => new ResetPasswordToken(DateTime.Now.AddDays(1));

    internal bool CanBeReset()
        => Expiry is not null && Expiry < DateTime.Now;
}