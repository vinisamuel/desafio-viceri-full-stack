namespace HeroesApi.Models.Superpowers;

public class SuperpowerModel
{
    public long Id { get; set; }
    public string Superpower { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public SuperpowerModel() { }

    public SuperpowerModel(SuperpowerEntity entity)
    {
        Id = entity.Id;
        Superpower = entity.Superpower;
        Description = entity.Description;
    }
}
