namespace HeroesApi.Models.Heroes;

public class UpsertHeroModel
{
    public string Name { get; set; } = string.Empty;
    public string HeroName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public List<long> SuperpowerIds { get; set; } = new();

    public bool IsValid(INotification notification)
    {
        if (string.IsNullOrWhiteSpace(Name))
            notification.AddInvalidParameter("Name is required.");

        if (string.IsNullOrWhiteSpace(HeroName))
            notification.AddInvalidParameter("Hero name is required.");

        if (BirthDate == default || BirthDate > DateTime.Now)
            notification.AddInvalidParameter("Birth date is invalid.");

        if (Height <= 0)
            notification.AddInvalidParameter("Height must be greater than zero.");

        if (Weight <= 0)
            notification.AddInvalidParameter("Weight must be greater than zero.");

        if (SuperpowerIds == null || !SuperpowerIds.Any())
            notification.AddInvalidParameter("At least one superpower is required.");

        return notification.Success;
    }

    public HeroEntity GetEntity(long? id = null)
    {
        return new HeroEntity
        {
            Id = id ?? 0,
            Name = Name,
            HeroName = HeroName,
            BirthDate = BirthDate,
            Height = Height,
            Weight = Weight
        };
    }
}
