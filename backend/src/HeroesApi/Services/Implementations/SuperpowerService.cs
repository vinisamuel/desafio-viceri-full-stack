namespace HeroesApi.Services.Implementations;

public class SuperpowerService(
    INotification notification,
    IUnitOfWork<HeroesAppDbContext> unitOfWork) : ISuperpowerService
{
    public async Task<List<SuperpowerModel>> GetAllAsync()
    {
        var entities = await unitOfWork.GetAllAsync<SuperpowerEntity>(
            orderBy: s => s.OrderBy(o => o.Superpower));

        if (entities.HasData())
        {
            var models = entities.Select(s => new SuperpowerModel(s)).ToList();
            notification.AddResponse(models);
            return models;
        }

        return [];
    }
}
