namespace working_good.business.core.Models.Company;

public record EmailDomain
{
    public string Value { get; private set; }

    public EmailDomain(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEmailDomainException();
        Value = value;
    }
    
    public static implicit operator EmailDomain(string value)
        => new EmailDomain(value);

    public static implicit operator string(EmailDomain emailDomain)
        => emailDomain.Value;
}