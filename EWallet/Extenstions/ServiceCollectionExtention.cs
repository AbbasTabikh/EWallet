using EWallet.Data;
using Microsoft.EntityFrameworkCore;

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
    }
}
