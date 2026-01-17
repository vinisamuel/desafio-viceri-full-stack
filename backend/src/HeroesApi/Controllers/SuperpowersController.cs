namespace HeroesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuperpowersController(
    INotification notification,
    ISuperpowerService superpowerService) : BaseController
{
    /// <summary>
    /// Get all superpowers
    /// </summary>
    /// <returns>List of all available superpowers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<SuperpowerModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        await superpowerService.GetAllAsync();
        return notification.GetResult();
    }
}
