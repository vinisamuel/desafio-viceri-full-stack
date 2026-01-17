namespace HeroesApi.Services;

public interface IHeroService
{
    Task<HeroDetailModel?> GetByIdAsync(long id);
    Task<List<HeroModel>> GetAllAsync();
    Task<HeroDetailModel?> CreateAsync(UpsertHeroModel model);
    Task<HeroDetailModel?> UpdateAsync(long id, UpsertHeroModel model);
    Task<bool> DeleteByIdAsync(long id);
}
