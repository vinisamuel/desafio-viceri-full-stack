namespace HeroesApi.Data.Mappings;

public class HeroSuperpowerMapping : IEntityTypeConfiguration<HeroSuperpowerEntity>
{
    public void Configure(EntityTypeBuilder<HeroSuperpowerEntity> builder)
    {
        builder.ToTable("HeroSuperpowers");

        builder.HasKey(hs => new { hs.HeroId, hs.SuperpowerId });

        builder.Property(hs => hs.HeroId).IsRequired();
        builder.Property(hs => hs.SuperpowerId).IsRequired();

        builder.HasOne(hs => hs.Hero)
            .WithMany(h => h.HeroSuperpowers)
            .HasForeignKey(hs => hs.HeroId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(hs => hs.Superpower)
            .WithMany(s => s.HeroSuperpowers)
            .HasForeignKey(hs => hs.SuperpowerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
