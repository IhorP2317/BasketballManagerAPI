using BasketballManagerAPI.Data;
using BasketballManagerAPI.Extensions;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
);
// Add services to the container.
builder.Services.RegisterCustomServices();
builder.Services.ConfigureAuth(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors("AllowAny");
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions() {
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), app.Configuration["FileStorage:PhysicalFileProviderRoot"])
    ),
    RequestPath = new PathString(app.Configuration["FileStorage:RequestPath"])
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
