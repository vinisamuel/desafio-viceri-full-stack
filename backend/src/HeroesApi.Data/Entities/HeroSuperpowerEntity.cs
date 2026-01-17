namespace HeroesApi.Data.Entities;

public class HeroSuperpowerEntity : BaseEntity
{
    public long HeroId { get; set; }
    public HeroEntity Hero { get; set; } = null!;
    
    public long SuperpowerId { get; set; }
    public SuperpowerEntity Superpower { get; set; } = null!;
}
