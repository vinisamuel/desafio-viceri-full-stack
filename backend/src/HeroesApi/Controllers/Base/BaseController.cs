using System.Net.Mime;

namespace HeroesApi.Controllers.Base;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerResponse(StatusCodes.Status200OK, Description = "Processado com sucesso")]
[SwaggerResponse(StatusCodes.Status204NoContent, Description = "Processado com sucesso sem retorno")]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(NotificationBase), Description = "Parâmetros de entrada inválidos")]
[SwaggerResponse(StatusCodes.Status422UnprocessableEntity, Type = typeof(NotificationBase), Description = "Ocorreu algum erro recuperável durante o processamento")]
[SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(NotificationBase), Description = "Ocorreu algum erro interno durante o processamento")]
public class BaseController() : ControllerBase
{ }
