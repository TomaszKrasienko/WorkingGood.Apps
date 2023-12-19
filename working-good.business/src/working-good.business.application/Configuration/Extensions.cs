using Microsoft.Extensions.DependencyInjection;
using working_good.business.application.CQRS.Abstractions;

namespace working_good.business.application.Configuration;

public static class Extensions
{
    public static IServiceCollection SetApplicationConfiguration(this IServiceCollection services)
        => services
            .SetCommandsHandlersConfiguration()
            .SetQueriesHandlersConfiguration();

    private static IServiceCollection SetCommandsHandlersConfiguration(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ICommandHandler<>).Assembly;
        services.Scan(s => s.FromAssemblies(applicationAssembly)
            .AddClasses(x => x.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }

    private static IServiceCollection SetQueriesHandlersConfiguration(this IServiceCollection services)
    {
        var applicationAssembly = typeof(IQueryHandler<,>).Assembly;
        services.Scan(s => s.FromAssemblies(applicationAssembly)
            .AddClasses(x => x.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }
}