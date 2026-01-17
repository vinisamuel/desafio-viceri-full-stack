namespace HeroesApi.Services;

public interface ISuperpowerService
{
    Task<List<SuperpowerModel>> GetAllAsync();
}
