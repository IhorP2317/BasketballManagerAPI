using System.Net.Http.Headers;
using System.Text;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using BasketballManagerAPI.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Security.Services.Interfaces;

namespace BasketballManagerAPI.Extensions {
    public static class ServiceRegistration {
        public static void RegisterCustomServices(this IServiceCollection serviceCollection, IConfiguration configuration) {

            serviceCollection.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<ValidateModelAttribute>();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            serviceCollection.AddAutoMapper(typeof(Program).Assembly);
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddHttpClient();
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
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IOrderService, OrderService>();
            serviceCollection.AddScoped<ITicketService, TicketService>();
            serviceCollection.AddScoped<PlayerAwardService>();
            serviceCollection.AddScoped<CoachAwardService>();
            serviceCollection.Configure<SecurityHttpClientConstants>(configuration.GetSection("SecurityHttpClient"));
            serviceCollection.Configure<SuperAdminSeedData>(configuration.GetSection("SuperAdminSeedData"));


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
        public static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration) {
            services.AddHttpClient(configuration["SecurityHttpClient:ClientName"], client => {
                client.BaseAddress = new Uri(configuration["SecurityHttpClient:BaseAddress"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/problem+json"));
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            });
        }

    }
}
