using System.Text;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Security.Services.Interfaces;

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
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basketball Manager Api", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
            serviceCollection.AddAutoMapper(typeof(Program).Assembly);
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddScoped<ITeamService, TeamService>();
            serviceCollection.AddScoped<IPlayerService, PlayerService>();
            serviceCollection.AddScoped<ICoachService, CoachService>();
            serviceCollection.AddScoped<IMatchService, MatchService>();
            serviceCollection.AddScoped<IStatisticService, StatisticService>();
            serviceCollection.AddScoped<IAwardService, AwardService>();
            serviceCollection.AddScoped<ICurrentUserService, CurrentUserService>();
            serviceCollection.AddScoped<IFileService, ImageService>();
            serviceCollection.AddScoped<IPlayerExperienceService, PlayerExperienceService>();
            serviceCollection.AddScoped<ICoachExperienceService, CoachExperienceService>();
            serviceCollection.AddScoped<IStaffAwardServiceFactory, StaffAwardServiceFactory>();
            serviceCollection.AddScoped<PlayerAwardService>();
            serviceCollection.AddScoped<CoachAwardService>();


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
