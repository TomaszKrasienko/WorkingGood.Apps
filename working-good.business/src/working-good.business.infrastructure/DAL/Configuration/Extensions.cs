using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.infrastructure.DAL.Repositories;

namespace working_good.business.infrastructure.DAL.Configuration;

internal static class Extensions
{
    internal static IServiceCollection SetDalConfiguration(this IServiceCollection services,
        IConfiguration configuration)
        => services
            .SetServices();

    private static IServiceCollection SetServices(this IServiceCollection services)
        => services.AddSingleton<ICompanyRepository, InMemoryCompanyRepository>();
}