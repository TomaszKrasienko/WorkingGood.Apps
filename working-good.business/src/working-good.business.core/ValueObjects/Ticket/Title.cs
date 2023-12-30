using working_good.business.core.Exceptions;

namespace working_good.business.core.ValueObjects.Ticket;

public record Title
{
    private string Value { get; }
    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidTitleException();
        }

        Value = value;
    }
    
    public static implicit operator Title(string value)
        => new Title(value);
    
    public static implicit operator string(Title title)
        => title.Value;
}