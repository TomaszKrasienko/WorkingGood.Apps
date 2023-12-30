using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using working_good.business.application.Services;
using working_good.business.application.Services.Security;
using working_good.business.infrastructure.Services.Security.Configuration.Models;

namespace working_good.business.infrastructure.Services.Security.Configuration;

internal static class Extensions
{
    internal static IServiceCollection SetSecurityConfiguration(this IServiceCollection services,
        IConfiguration configuration)
        => services
            .SetServices()
            .SetOptions(configuration);
    
    private static IServiceCollection SetServices(this IServiceCollection services)
        => services
            .AddHttpContextAccessor()
            .AddSingleton<IAccessTokenStorage, HttpContextTokenStorage>()
            .AddSingleton<IAuthenticator, Authenticator>();

    private static IServiceCollection SetOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<SecurityOptions>(configuration.GetSection("Security"));

}