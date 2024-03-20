using System.Net.Http.Headers;
using System.Text;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<MailTemplatesConstants>(configuration.GetSection("MailTemplatesSettings"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddControllers(options => {
                options.Filters.Add<ValidateModelAttribute>();
                options.Filters.Add<GlobalExceptionFilter>();
            });


        }
        public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration) {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["AuthSettings:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = configuration["AuthSettings:Audience"],

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AuthSettings:SecretKey"])),
                    };
                });
        }
        

    }


}

