using working_good.business.core.Exceptions;

namespace working_good.business.core.ValueObjects.User;

public sealed record Password
{
    public string Value { get; }
    
    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidPasswordException(value);
        if (value.Length is < 6)
            throw new InvalidPasswordException(value);
        Value = value;
    }

    public static implicit operator Password(string value)
        => new Password(value);

    public static implicit operator string(Password password)
        => password.Value;
}