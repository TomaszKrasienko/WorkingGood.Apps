using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using working_good.business.application.CQRS.Abstractions;
using working_good.business.infrastructure.Logging.Decorators;

namespace working_good.business.infrastructure.Logging.Configuration;

public static class Extensions
{
    internal static IServiceCollection SetLoggingConfiguration(this IServiceCollection services,
        IConfiguration configuration)
        => services
            .SetDecorators();

    private static IServiceCollection SetDecorators(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        return services;
    }
}