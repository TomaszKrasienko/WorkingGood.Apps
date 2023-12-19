namespace working_good.business.infrastructure.Configuration.Models;

internal record AppOptions
{
    public string Name { get; init; }
}

internal record CorsPolicyOptions
{
    public string Name { get; init; }
}