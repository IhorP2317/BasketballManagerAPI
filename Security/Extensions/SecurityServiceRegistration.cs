using BasketballManagerAPI.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Data;
using Security.Filters;
using Security.Models;
using Security.Services.Implementations;
using Security.Services.Interfaces;
using Security.Settings;

namespace Security.Extensions {
    public static class SecurityServiceRegistration {
        public static void RegisterSecurityServices(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
            services.Configure<SuperAdminSettings>(configuration.GetSection("SuperAdminSettings"));


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAuthService, UserService>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddControllers(options => {
                options.Filters.Add<ValidateModelAttribute>();
                options.Filters.Add<GlobalExceptionFilter>();
            });


        }
    }
}
