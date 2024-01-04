using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using working_good.business.application.Services.QueryRepositories;
using working_good.business.core.Abstractions.Repositories;
using working_good.business.infrastructure.Configuration;
using working_good.business.infrastructure.DAL.Configuration.Models;
using working_good.business.infrastructure.DAL.QueryRepositories;
using working_good.business.infrastructure.DAL.Repositories;

namespace working_good.business.infrastructure.DAL.Configuration;

internal static class Extensions
{
    internal static IServiceCollection SetDalConfiguration(this IServiceCollection services,
        IConfiguration configuration)
        => services
            .SetOptions(configuration)
            .SetDbContext(configuration)
            .SetDbConnection()
            .SetServices()
            .SetDbInitializer();

    private static IServiceCollection SetOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<SqlServerOptions>(configuration.GetSection("SqlServer"));
    
    private static IServiceCollection SetDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = configuration.GetOptions<SqlServerOptions>("SqlServer");
        services.AddDbContext<WgDbContext>(options => options.UseSqlServer(dbOptions.ConnectionString));
        return services;
    }

    private static IServiceCollection SetDbConnection(this IServiceCollection services)
        => services.AddScoped<IWgDbConnection, WgDbConnection>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<SqlServerOptions>>().Value;
            return new WgDbConnection(options.ConnectionString);
        });

    private static IServiceCollection SetServices(this IServiceCollection services)
        => services
            .AddScoped<ICompanyRepository, PostgreCompanyRepository>()
            .AddScoped<ICompanyQueryRepository, CompanyQueryRepository>()
            .AddScoped<IEmployeesQueryRepository, EmployeeQueryRepository>();

    private static IServiceCollection SetDbInitializer(this IServiceCollection services)
        => services.AddHostedService<DatabaseInitializer>();
}