using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Data;
using Security.Models;

namespace Security.Extensions
{
    public static class SecurityServiceRegistration
    {
        public static void RegisterSecurityServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            
        }
    }
}
