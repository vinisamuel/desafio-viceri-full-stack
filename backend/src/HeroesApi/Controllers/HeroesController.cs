namespace HeroesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HeroesController(
    INotification notification,
    IHeroService heroService) : BaseController
{
    /// <summary>
    /// Get all heroes
    /// </summary>
    /// <returns>List of all heroes</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<HeroModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        await heroService.GetAllAsync();
        return notification.GetResult();
    }

    /// <summary>
    /// Get hero by id
    /// </summary>
    /// <param name="id">Hero id</param>
    /// <returns>Hero details with superpowers</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HeroDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        await heroService.GetByIdAsync(id);
        return notification.GetResult();
    }

    /// <summary>
    /// Create a new hero
    /// </summary>
    /// <param name="model">Hero data</param>
    /// <returns>Created hero with details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(HeroDetailModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] UpsertHeroModel model)
    {
        await heroService.CreateAsync(model);
        return notification.GetResult();
    }

    /// <summary>
    /// Update an existing hero
    /// </summary>
    /// <param name="id">Hero id</param>
    /// <param name="model">Updated hero data</param>
    /// <returns>Updated hero with details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(HeroDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] UpsertHeroModel model)
    {
        await heroService.UpdateAsync(id, model);
        return notification.GetResult();
    }

    /// <summary>
    /// Delete a hero
    /// </summary>
    /// <param name="id">Hero id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteByIdAsync(long id)
    {
        await heroService.DeleteByIdAsync(id);
        return notification.GetResult();
    }
}
