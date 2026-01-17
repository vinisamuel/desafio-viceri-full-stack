namespace HeroesApi.Data.Mappings;

public class HeroMapping : IEntityTypeConfiguration<HeroEntity>
{
    public void Configure(EntityTypeBuilder<HeroEntity> builder)
    {
        builder.ToTable("Heroes");

        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).IsRequired().UseIdentityColumn();

        builder.Property(h => h.Name).IsRequired().HasMaxLength(120);
        builder.Property(h => h.HeroName).IsRequired().HasMaxLength(120);
        builder.Property(h => h.BirthDate).IsRequired().HasColumnType("datetime2(7)");
        builder.Property(h => h.Height).IsRequired();
        builder.Property(h => h.Weight).IsRequired();

        builder.Property(h => h.CreatedAt).IsRequired();
        builder.Property(h => h.UpdatedAt).IsRequired();

        builder.HasIndex(h => h.HeroName).IsUnique();

        builder.HasMany(h => h.HeroSuperpowers)
            .WithOne(hs => hs.Hero)
            .HasForeignKey(hs => hs.HeroId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
