namespace HeroesApi.Data.Mappings;

public class SuperpowerMapping : IEntityTypeConfiguration<SuperpowerEntity>
{
    public void Configure(EntityTypeBuilder<SuperpowerEntity> builder)
    {
        builder.ToTable("Superpowers");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).IsRequired().UseIdentityColumn();

        builder.Property(s => s.Superpower).IsRequired().HasMaxLength(50);
        builder.Property(s => s.Description).IsRequired().HasMaxLength(250);

        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired();

        builder.HasIndex(s => s.Superpower).IsUnique();

        builder.HasMany(s => s.HeroSuperpowers)
            .WithOne(hs => hs.Superpower)
            .HasForeignKey(hs => hs.SuperpowerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
