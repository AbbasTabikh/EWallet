using EWallet.Data;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Extenstions
{
    public static class WebApplicationBuilderExtention
    {
        public static WebApplication ApplyMigration(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.Migrate();
            return app;
        }
    }
}
