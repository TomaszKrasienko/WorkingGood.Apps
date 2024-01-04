namespace working_good.business.infrastructure.DAL.Configuration.Models;

internal sealed record SqlServerOptions
{
    public string ConnectionString { get; init; }
}