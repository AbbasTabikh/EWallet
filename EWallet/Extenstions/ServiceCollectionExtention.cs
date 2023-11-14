using EWallet.Data;
using EWallet.InputModels;
using EWallet.Repository;
using EWallet.Services;
using EWallet.Services.Interfaces;
using EWallet.Validations;
using EWallet.Validations.ValidationModels;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

        internal static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UserInput>, UserValidation>();
            services.AddScoped<IValidator<BudgetValidationModel>, BudgetValidation>();
            return services;
        }
        internal static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Wallet Api", Version = "v1.2" });

                    // Define the BearerAuth scheme that's in use
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer", // must be lowercase
                        BearerFormat = "JWT"
                    });

                    // Make sure swagger UI asks for the required parameters
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {{
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()}});
            });

            return services;
        }

        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBudgetService, BudgetService>();
            services.AddScoped<IHttpContextService, HttpContextService>();
            return services;
        }
    }
}
