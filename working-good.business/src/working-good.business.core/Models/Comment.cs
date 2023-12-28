using working_good.business.core.Exceptions;
using working_good.business.core.ValueObjects;

namespace working_good.business.core.Models;

public sealed class Comment : Entity
{
    public SequenceNumber SequenceNumber { get; private set; }
    public Content Content { get; private set; }
    public string AuthorEmail { get; private set; }
    
    public Comment() : base(Guid.NewGuid())
    {
        
    }
}

public record SequenceNumber
{
    public int Value { get; private set; }
    
    internal SequenceNumber(int value)
    {
        if (value < 1)
        {
            throw new InvalidSequenceNumberException(value);
        }
        Value = value;
    }

    public static implicit operator SequenceNumber(int value)
        => new SequenceNumber(value);

    public static implicit operator int(SequenceNumber sequenceNumber)
        => sequenceNumber.Value;
}

public sealed class InvalidSequenceNumberException(int value)
    : CustomException($"Value: {value} is incorrect for sequence number", "invalid_sequence_number");