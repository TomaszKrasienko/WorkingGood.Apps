using Microsoft.Extensions.DependencyInjection;
using working_good.business.core.DomainServices;
using working_good.business.core.DomainServices.Abstractions;
using working_good.business.core.Policies;
using working_good.business.core.Policies.Abstractions;

namespace working_good.business.core.Configuration;

public static class Extensions
{
    public static IServiceCollection SetCoreConfiguration(this IServiceCollection services)
        => services
            .SetPolicies();

    private static IServiceCollection SetPolicies(this IServiceCollection services)
        => services
            .AddSingleton<IPasswordPolicy, UserPasswordPolicy>()
            .AddSingleton<IUserRegistrationService, UserRegistrationService>()
            .AddSingleton<ICompanyRegistrationService, CompanyRegistrationService>();

}