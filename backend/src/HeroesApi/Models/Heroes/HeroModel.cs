namespace HeroesApi.Models.Heroes;

public class HeroModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string HeroName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public HeroModel() { }

    public HeroModel(HeroEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        HeroName = entity.HeroName;
        BirthDate = entity.BirthDate;
        Height = entity.Height;
        Weight = entity.Weight;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }
}
