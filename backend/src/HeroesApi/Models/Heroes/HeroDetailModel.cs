namespace HeroesApi.Models.Heroes;

public class HeroDetailModel : HeroModel
{
    public List<SuperpowerModel> Superpowers { get; set; } = new();

    public HeroDetailModel() { }

    public HeroDetailModel(HeroEntity entity) : base(entity)
    {
        if (entity.HeroSuperpowers?.Count > 0)
            Superpowers = [.. entity.HeroSuperpowers.Select(hs => new SuperpowerModel(hs.Superpower))];
    }
}
