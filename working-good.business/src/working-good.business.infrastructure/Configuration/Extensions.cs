using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using working_good.business.application.Services;
using working_good.business.core.Abstractions;
using working_good.business.core.Models;
using working_good.business.core.Models.Company;
using working_good.business.infrastructure.Configuration.Models;
using working_good.business.infrastructure.DAL.Configuration;
using working_good.business.infrastructure.Exceptions;
using working_good.business.infrastructure.Exceptions.Middlewares;
using working_good.business.infrastructure.Logging.Configuration;
using working_good.business.infrastructure.Services;
using working_good.business.infrastructure.Services.Security;
using working_good.business.infrastructure.Services.Security.Configuration;

namespace working_good.business.infrastructure.Configuration;

public static class Extensions
{
    public static IServiceCollection SetInfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
        => services
            .SetDalConfiguration(configuration)
            .SetSecurityConfiguration(configuration)
            .SetLoggingConfiguration(configuration)
            .SetServices()
            .SetMiddlewares()
            .SetBanner(configuration)
            .SetCorsPolicy(configuration);

    private static IServiceCollection SetServices(this IServiceCollection services)
        => services
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

    private static IServiceCollection SetMiddlewares(this IServiceCollection services)
        => services.AddSingleton<CustomExceptionMiddleware>();

    private static IServiceCollection SetBanner(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<AppOptions>("App");
        Console.WriteLine(FiggleFonts.Doom.Render(options.Name));
        return services;
    }

    private static IServiceCollection SetCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<CorsPolicyOptions>("CorsPolicy");
        services.AddCors(opt =>
        {
            opt.AddPolicy(options.Name, builder
                => builder
                    .AllowAnyHeader()
                    .AllowAnyHeader()
                    .AllowAnyOrigin());
        });
        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
        => app
            .UseCorsPolicy()
            .UseCustomMiddlewares();

    private static WebApplication UseCustomMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<CustomExceptionMiddleware>();
        return app;   
    }

    private static WebApplication UseCorsPolicy(this WebApplication app)
    {
        var options = app.Configuration.GetOptions<CorsPolicyOptions>("CorsPolicy");
        app.UseCors(options.Name);
        return app;
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName = null) where T : class, new()
    {
        T opt = new T();
        if(sectionName is not null)
        {
            configuration.Bind(sectionName, opt);   
        }
        else
        {
            configuration.Bind(opt);
        }
        return opt;
    }
}