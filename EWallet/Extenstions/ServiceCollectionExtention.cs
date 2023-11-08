using EWallet.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EWallet.Extenstions
{
    public static class ServiceCollectionExtention
    {
        internal static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                               provider => provider.EnableRetryOnFailure(8, TimeSpan.FromSeconds(25), null)));
            return services;
        }

        internal static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            string issuer = configuration.GetSection("Jwt:Issuer").Get<string>()!;
            string audience = configuration.GetSection("Jwt:Audience").Get<string>()!;
            string secret = configuration.GetSection("Jwt:Secret").Get<string>()!;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}
