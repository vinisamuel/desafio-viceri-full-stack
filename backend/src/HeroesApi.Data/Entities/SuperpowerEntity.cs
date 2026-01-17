namespace HeroesApi.Data.Entities;

public class SuperpowerEntity : BaseEntity
{
    public string Superpower { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public ICollection<HeroSuperpowerEntity>? HeroSuperpowers { get; set; }
}
