using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public static class JwtAuthenticationExtensions
{

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, AuthOptions authOptions)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.RequireHttpsMetadata = false;
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    // укзывает, будет ли валидироваться издатель при валидации токена
                                    ValidateIssuer = true,
                                    // строка, представляющая издателя
                                    ValidIssuer = authOptions.Issuer,

                                    // будет ли валидироваться потребитель токена
                                    ValidateAudience = true,
                                    // установка потребителя токена
                                    ValidAudience = authOptions.Audience,
                                    // будет ли валидироваться время существования
                                    ValidateLifetime = true,

                                    // установка ключа безопасности
                                    IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                                    // валидация ключа безопасности
                                    ValidateIssuerSigningKey = true,
                                };
                            });

        return services;
    }

}
