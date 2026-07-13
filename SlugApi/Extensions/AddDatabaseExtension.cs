using Microsoft.EntityFrameworkCore;
using SlugApi.Date;

namespace SlugApi.Extensions
{
    public static class AddDatabaseExtension
    {
        public static IServiceCollection AddDatabaseService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var constr = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(constr))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            }
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(constr)
            );
            return services;
        }
    }
}
