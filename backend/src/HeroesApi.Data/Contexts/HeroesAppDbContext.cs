using HeroesApi.Shared.Settings;

namespace HeroesApi.Data.Contexts;

public class HeroesAppDbContext(
    DbContextOptions<HeroesAppDbContext> options,
    AppSettings appSettings) : BaseContext<HeroesAppDbContext>(options, appSettings.HeroesAppDatabase)
{
    public DbSet<HeroEntity> Heroes { get; set; }
    public DbSet<SuperpowerEntity> Superpowers { get; set; }
    public DbSet<HeroSuperpowerEntity> HeroSuperpowers { get; set; }
}
