namespace working_good.business.infrastructure.Services.Security.Configuration.Models;

internal record SecurityOptions
{
    public string SigningKey { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public TimeSpan Expiry { get; init; }
}