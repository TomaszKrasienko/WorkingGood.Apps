using Microsoft.Extensions.DependencyInjection;

namespace working_good.business.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection SetInfrastructureConfiguration(this IServiceCollection services)
        => services;
}