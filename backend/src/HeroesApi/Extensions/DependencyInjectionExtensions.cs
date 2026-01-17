using HeroesApi.Data.Repositories;
using HeroesApi.Data.Repositories.Implementations;
using HeroesApi.Middlewares;
using HeroesApi.Services.Implementations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeroesApi.Extensions;

public static class DependencyInjectionExtensions
{
    public static AppSettings AddAppSettings(this WebApplicationBuilder builder)
    {
        var appSettings = builder.Configuration.Get<AppSettings>()
            ?? throw new ApplicationException("AppSettings not found");

        var (isValid, errorMessage) = appSettings.Validate();
        if (!isValid)
            throw new ApplicationException($"AppSettings validation error: {errorMessage}");

        builder.Services.AddSingleton(appSettings);

        return appSettings;
    }

    public static IServiceCollection AddHeroesAppDatabase(this IServiceCollection services)
    {
        services.AddDbContext<HeroesAppDbContext>();

        services.AddScoped<IUnitOfWork<HeroesAppDbContext>, EfUnitOfWork<HeroesAppDbContext>>();

        return services;
    }

    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<INotification, Notification>();
        
        services.AddScoped<IHeroService, HeroService>();
        services.AddScoped<ISuperpowerService, SuperpowerService>();

        return services;
    }

    public static IMvcBuilder AddCamelCaseJsonOptions(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.WriteIndented = false;
            options.JsonSerializerOptions.AllowTrailingCommas = false;
        });

        return builder;
    }

    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
