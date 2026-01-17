namespace HeroesApi.Models.Base;

public class BaseIdNameModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public BaseIdNameModel() { }

    public BaseIdNameModel(long id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class BaseIdNameModel<TEnum> : BaseIdNameModel
    where TEnum : Enum
{
    public BaseIdNameModel() { }

    public BaseIdNameModel(TEnum @enum)
    {
        Id = Convert.ToInt64(@enum);
        Name = @enum.GetDescription();
    }
}