using working_good.business.core.Exceptions;

namespace working_good.business.core.ValueObjects.Company;

public sealed record SlaTimeSpan
{
    public TimeSpan Value { get; private set; }

    public SlaTimeSpan(TimeSpan value)
    {
        if (TimeSpan.Zero == value)
            throw new SlaTimeMustBeGreaterThanZeroException();
        Value = value;
    }
    
    public static implicit operator SlaTimeSpan(TimeSpan value)
        => new SlaTimeSpan(value);

    public static implicit operator TimeSpan?(SlaTimeSpan slaTimeSpan)
        => slaTimeSpan?.Value;
}