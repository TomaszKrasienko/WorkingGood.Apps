using working_good.business.application.Configuration;
using working_good.business.core.Configuration;
using working_good.business.infrastructure.Configuration;

namespace working_good.business.api.Configuration;

public static class Extensions
{
    public static IServiceCollection SetConfiguration(this IServiceCollection services, IConfiguration configuration)
        => services
            .SetCoreConfiguration()
            .SetApplicationConfiguration()
            .SetInfrastructureConfiguration(configuration)
            .SetServices();

    private static IServiceCollection SetServices(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }
}