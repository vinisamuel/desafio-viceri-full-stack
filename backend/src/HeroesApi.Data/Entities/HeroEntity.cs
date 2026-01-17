namespace HeroesApi.Data.Entities;

public class HeroEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string HeroName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    
    public ICollection<HeroSuperpowerEntity>? HeroSuperpowers { get; set; }
}
