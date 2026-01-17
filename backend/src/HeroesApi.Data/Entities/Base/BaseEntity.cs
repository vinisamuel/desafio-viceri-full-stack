namespace HeroesApi.Data.Entities.Base;

public class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
