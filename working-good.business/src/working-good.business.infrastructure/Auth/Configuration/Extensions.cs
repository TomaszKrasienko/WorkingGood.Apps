using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using working_good.business.infrastructure.Configuration;
using working_good.business.infrastructure.Services.Security.Configuration.Models;

namespace working_good.business.infrastructure.Auth.Configuration;

internal static class Extensions
{
    internal static IServiceCollection SetAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<SecurityOptions>("Security");

        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Audience = options.Audience;
                //Todo: Change before deploy
                x.IncludeErrorDetails = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = options.Issuer,
                    ValidateAudience = true,
                    ValidAudience = options.Audience,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)),
                    ValidateLifetime = true
                };
            });
        services.AddAuthorization();
        return services;
    }

    internal static WebApplication UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}