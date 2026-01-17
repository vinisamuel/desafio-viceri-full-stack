using System.Net;
using System.Net.Mime;

namespace HeroesApi.Middlewares;

/// <summary>
/// Classe que para tratamento de exceções não capturadas
/// </summary>
/// <remarks>
/// Cria uma instância <see cref="ExceptionHandlerMiddleware"/>
/// </remarks>
/// <param name="next"></param>
public class ExceptionHandlerMiddleware(RequestDelegate next)
{

    /// <summary>
    /// Método chamado no pipeline da requisição. Caso ocorra uma exceção não tratada, uma mensagem é logada e é retornado erro 500.
    /// </summary>
    /// <param name="context">contexto da requisição</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var logger = context.RequestServices.GetService<ILogger<ExceptionHandlerMiddleware>>()!;
            var env = context.RequestServices.GetService<IHostEnvironment>()!;
            var notification = context.RequestServices.GetService<INotification>()!;

            notification.AddUntreatedException(logger, ex, env);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var jsonResult = notification.GetResult() as JsonResult;
            if (jsonResult?.Value is not null)
                await context.Response.WriteAsync(jsonResult.Value.ToJson());
        }
    }
}
