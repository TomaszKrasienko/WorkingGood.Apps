namespace working_good.business.core.Models.Company;

public sealed record IsOwner(bool Value)
{
    public static implicit operator IsOwner(bool value)
        => new IsOwner(value);

    public static implicit operator bool(IsOwner isOwner)
        => isOwner.Value;
}