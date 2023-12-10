using System.Security.Cryptography;
using working_good.business.core.Exceptions;

namespace working_good.business.core.ValueObjects.User;

public sealed record VerificationToken
{
    public string Token { get; }
    public DateTime? VerificationDate { get; set; }

    private VerificationToken()
    {
        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace("/", "").Replace("==", "");
    }

    internal static VerificationToken Create()
    {
        return new VerificationToken();
    }

    internal void Verify(string value)
    {
        if (Token != value)
            throw new InvalidAccountVerificationException();
        VerificationDate = DateTime.Now;
    }
}