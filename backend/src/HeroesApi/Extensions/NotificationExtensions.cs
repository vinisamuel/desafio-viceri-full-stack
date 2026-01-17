using System.Net;

namespace HeroesApi.Extensions;

public static class NotificationExtensions
{
    public static IActionResult GetResult(this INotification notification, bool forceResultAsOk = false)
    {
        try
        {
            if (forceResultAsOk)
            {
                notification.SetStatusCode(HttpStatusCode.OK);
                return new OkObjectResult(notification);
            }

            if (notification.HasNotification(NotificationType.UntreatedException))
            {
                notification.SetStatusCode(HttpStatusCode.InternalServerError);
                return new JsonResult(notification);
            }

            if (notification.HasNotification(NotificationType.InvalidParameter))
            {
                notification.SetStatusCode(HttpStatusCode.BadRequest);
                return new BadRequestObjectResult(notification);
            }

            if (notification.HasNotification(NotificationType.Error))
            {
                notification.SetStatusCode((HttpStatusCode)422);
                return new UnprocessableEntityObjectResult(notification);
            }

            if (notification.Data != null || notification.HasNotification(NotificationType.Information, NotificationType.Warning))
            {
                notification.SetStatusCode(HttpStatusCode.OK);
                return new OkObjectResult(notification);
            }

            return new NoContentResult();
        }
        catch
        {
            throw;
        }
        finally
        {
            notification.StopTime();
        }
    }
}
