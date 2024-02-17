using System.Text;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Extensions {
    public static class ServiceRegistration {
        public static void RegisterCustomServices(this IServiceCollection serviceCollection) {

            serviceCollection.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<ValidateModelAttribute>();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen();
            serviceCollection.AddScoped<ITeamService, TeamService>();
            serviceCollection.AddScoped<IPlayerService, PlayerService>();
            serviceCollection.AddScoped<ICoachService, CoachService>();
            serviceCollection.AddScoped<IMatchService, MatchService>();
            serviceCollection.AddScoped<IStatisticService, StatisticService>();
            serviceCollection.AddScoped<IAwardService, PlayerAwardService>();
            serviceCollection.AddAutoMapper(typeof(Program).Assembly);

        }
        public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration) {
            services.AddAuthentication(cfg =>
                {
                    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Auth:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = configuration["Auth:Audience"],

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Auth:SecretKey"])),
                    };
                });
        }

    }
}
