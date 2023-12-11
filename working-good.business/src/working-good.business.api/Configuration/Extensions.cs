using working_good.business.application.Configuration;

namespace working_good.business.api.Configuration;

public static class Extensions
{
    public static IServiceCollection SetConfiguration(this IServiceCollection services, IConfiguration configuration)
        => services
            .SetApplicationConfiguration()
            .SetServices();

    private static IServiceCollection SetServices(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }
}