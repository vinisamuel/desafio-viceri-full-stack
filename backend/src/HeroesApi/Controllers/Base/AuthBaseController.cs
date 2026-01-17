using Microsoft.AspNetCore.Authorization;

namespace HeroesApi.Controllers.Base;

[Authorize]
public class AuthBaseController() : BaseController()
{ }
