namespace HeroesApi.Services.Implementations;

public class HeroService(
    ILogger<HeroService> logger,
    INotification notification,
    IUnitOfWork<HeroesAppDbContext> unitOfWork) : IHeroService
{
    public async Task<List<HeroModel>> GetAllAsync()
    {
        var entities = await unitOfWork.GetAllAsync<HeroEntity>(
            orderBy: h => h.OrderBy(o => o.HeroName));

        if (entities.HasData())
        {
            var models = entities.Select(h => new HeroModel(h)).ToList();
            notification.AddResponse(models);
            return models;
        }

        return [];
    }

    public async Task<HeroDetailModel?> GetByIdAsync(long id)
    {
        var entity = await unitOfWork.FindByIdAsync<HeroEntity>(id);
        if (entity is null)
        {
            notification.AddError(logger, "Hero not found with the provided id.");
            return null;
        }

        entity.HeroSuperpowers = await unitOfWork.GetListAsync<HeroSuperpowerEntity>(
            condition: x => x.HeroId == id,
            includes: [
                x => x.Superpower
            ]);

        var model = new HeroDetailModel(entity);
        notification.AddResponse(model);

        return model;
    }

    public async Task<HeroDetailModel?> CreateAsync(UpsertHeroModel model)
    {
        notification.AddRequest(model);

        if (!model.IsValid(notification))
            return null;

        if (await HeroNameExistsAsync(model.HeroName))
            return null;

        if (!await ValidateSuperpowersAsync(model.SuperpowerIds))
            return null;

        var entity = model.GetEntity();

        try
        {
            await unitOfWork.BeginTransactionAsync();

            await unitOfWork.AddAsync(entity);

            await AddHeroSuperpowersAsync(entity.Id, model.SuperpowerIds);

            await unitOfWork.CommitAsync();

            notification.AddInformation("Hero created successfully.");
        
            return await GetByIdAsync(entity.Id);
        }
        catch (Exception ex)
        {
            notification.AddTreatedException(logger, ex, "An error occurred while creating the hero.");
            await unitOfWork.RollbackAsync();

            return null;
        }
    }

    public async Task<HeroDetailModel?> UpdateAsync(long id, UpsertHeroModel model)
    {
        notification.AddRequest(model);

        if (!model.IsValid(notification))
            return null;

        if (!await unitOfWork.ExistsByIdAsync<HeroEntity>(id))
        {
            notification.AddError(logger, "Hero not found with the provided id.");
            return null;
        }

        if (await HeroNameExistsAsync(model.HeroName, id))
            return null;

        if (!await ValidateSuperpowersAsync(model.SuperpowerIds))
            return null;

        var entity = model.GetEntity(id);

        try
        {
            await unitOfWork.BeginTransactionAsync();

            await unitOfWork.UpdateByIdAsync<HeroEntity>(id,
                fields: h => h.SetProperty(p => p.Name, entity.Name)
                    .SetProperty(p => p.HeroName, entity.HeroName)
                    .SetProperty(p => p.BirthDate, entity.BirthDate)
                    .SetProperty(p => p.Height, entity.Height)
                    .SetProperty(p => p.Weight, entity.Weight)
                    .SetProperty(p => p.UpdatedAt, DateTime.Now));

            await UpdateHeroSuperpowersAsync(id, model.SuperpowerIds);

            await unitOfWork.CommitAsync();

            notification.AddInformation("Hero updated successfully.");
        
            return await GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            notification.AddTreatedException(logger, ex, "An error occurred while updating the hero.");
            await unitOfWork.RollbackAsync();

            return null;
        }
    }

    public async Task<bool> DeleteByIdAsync(long id)
    {
        if (!await unitOfWork.ExistsByIdAsync<HeroEntity>(id))
        {
            notification.AddError(logger, "Hero not found with the provided id.");
            return false;
        }

        await unitOfWork.DeleteByIdAsync<HeroEntity>(id);

        notification.AddInformation("Hero deleted successfully.");

        return true;
    }

    private async Task<bool> HeroNameExistsAsync(string heroName, long? excludeId = null)
    {
        var exists = await unitOfWork.ExistsAsync<HeroEntity>(
            condition: h => h.HeroName == heroName && h.Id != excludeId);

        if (exists)
        {
            notification.AddError(logger, "A hero with this hero name already exists.");
            return true;
        }

        return false;
    }

    private async Task<bool> ValidateSuperpowersAsync(List<long> superpowerIds)
    {
        if (!superpowerIds.HasData())
        {
            notification.AddError(logger, "At least one superpower must be provided.");
            return false;
        }

        foreach (var superpowerId in superpowerIds)
        {
            if (!await unitOfWork.ExistsByIdAsync<SuperpowerEntity>(superpowerId))
                notification.AddError(logger, $"Superpower with id {superpowerId} does not exist.");
        }

        return notification.Success;
    }

    private async Task AddHeroSuperpowersAsync(long heroId, List<long> superpowerIds)
    {
        var heroSuperpowers = superpowerIds.Select(spId => new HeroSuperpowerEntity
        {
            HeroId = heroId,
            SuperpowerId = spId
        }).ToList();

        await unitOfWork.AddMultipleAsync(heroSuperpowers);
    }

    private async Task UpdateHeroSuperpowersAsync(long heroId, List<long> superpowerIds)
    {
        await unitOfWork.DeleteAsync<HeroSuperpowerEntity>(
            condition: hs => hs.HeroId == heroId
        );

        await AddHeroSuperpowersAsync(heroId, superpowerIds);
    }
}
