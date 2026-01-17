using Microsoft.Extensions.Logging;
using HeroesApi.Shared.Settings;
using System.Diagnostics;

namespace HeroesApi.Data.Contexts.Base;

public class BaseContext<TContext>(
    DbContextOptions<TContext> options,
    DatabaseSettings settings) : DbContext(options) where TContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (settings is null)
            throw new ApplicationException($"Invalid database settings.");

        optionsBuilder.UseSqlServer(settings.ConnectionString, options =>
        {
            options.CommandTimeout(settings.Timeout);
            options.MigrationsAssembly(typeof(TContext).Assembly.FullName);
        });

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);

        if (settings.EnableLog)
            optionsBuilder
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .LogTo(message => Debug.WriteLine(message), LogLevel.Debug);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
