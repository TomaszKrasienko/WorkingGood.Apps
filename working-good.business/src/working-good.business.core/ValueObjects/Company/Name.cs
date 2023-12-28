namespace working_good.business.core.Models.Company;

public sealed record Name
{
    public string Value { get; private set; }
    
    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidNameException();
        if (value.Length < 4)
            throw new InvalidNameException(value);
        Value = value;
    }

    public static implicit operator Name(string value)
        => new Name(value);

    public static implicit operator string(Name name)
        => name.Value;
}