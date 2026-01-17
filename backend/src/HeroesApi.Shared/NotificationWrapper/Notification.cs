using System.Diagnostics;

namespace HeroesApi.Shared.NotificationWrapper;

public class Notification : NotificationBase<object, object>, INotification
{
    private readonly Stopwatch _stopwatch;

    public Notification()
    {
        _stopwatch = new Stopwatch();
        _stopwatch.Start();

        TraceId = Guid.NewGuid().ToString();
    }

    #region Add Notifications

    public void AddInformation(params string[] messages) => AddNotifications(NotificationType.Information, messages);

    public void AddInformation(ILogger logger, params string[] messages)
    {
        AddInformation(messages);
        foreach (var message in messages)
            logger.LogInformation(message);
    }

    public void AddWarning(params string[] messages) => AddNotifications(NotificationType.Warning, messages);

    public void AddWarning(ILogger logger, params string[] messages)
    {
        AddWarning(messages);
        foreach (var message in messages)
            logger.LogWarning(message);
    }

    public void AddError(params string[] messages) => AddNotifications(NotificationType.Error, messages);

    public void AddError(ILogger logger, params string[] messages)
    {
        AddError(messages);
        foreach (var message in messages)
            logger.LogError(message);
    }

    public void AddTreatedException(Exception exception, string message) => AddError($"{message}: {exception.Message}");

    public void AddTreatedException(ILogger logger, Exception exception, string message)
    {
        AddTreatedException(exception, message);
        logger.LogError(exception, message);
    }

    public void AddUntreatedException(ILogger logger, Exception exception, IHostEnvironment environment)
    {
        AddNotifications(NotificationType.UntreatedException, "Ocorreu um erro inesperado. Se o problema persistir, favor contatar o suporte.");
        logger.LogError(exception, "Exceção não tratada");

        if (environment.IsDevelopment())
            AddResponse(exception);
    }

    public void AddInvalidParameter(string message) => AddNotification(NotificationType.InvalidParameter, message);

    public void AddInvalidParameter(ILogger logger, string message)
    {
        AddInvalidParameter(message);
        logger.LogError(message);
    }

    private void AddNotification(NotificationType notificationType, string message)
        => Notifications.Add(new NotificationItem(notificationType, message));

    private void AddNotifications(NotificationType notificationType, params string[] messages)
        => messages.ToList().ForEach(msg => AddNotification(notificationType, msg));

    #endregion

    public bool HasNotification(params NotificationType[] notificationTypes)
        => Notifications.Any(x => notificationTypes.Contains(x.TypeId));

    public void SetStatusCode(HttpStatusCode statusCode)
        => StatusCode = statusCode;

    public void StopTime()
    {
        _stopwatch.Stop();
        ExecutionTime = $"{_stopwatch.Elapsed}";
    }

    public void AddRequest(object data)
    {
        Request = data;
    }

    public void AddResponse(object data, params string[] messages)
    {
        Data = data;

        if (messages.HasData())
            messages.ToList().ForEach(msg => Notifications.Add(new NotificationItem(NotificationType.Information, msg)));
    }
}

public class NotificationBase<TRequest, TResponse> : NotificationBase<TResponse>
{
    public TRequest? Request { get; protected set; }
}

public class NotificationBase<TResponse> : NotificationBase
{
    public TResponse? Data { get; protected set; }
}

public class NotificationBase
{
    private readonly List<NotificationType> _errorNotificationTypes = 
    [
        NotificationType.Error,
        NotificationType.InvalidParameter,
        NotificationType.UntreatedException
    ];

    public string TraceId { get; protected set; } = string.Empty;
    public string ExecutionTime { get; protected set; } = string.Empty;
    public HttpStatusCode StatusCode { get; protected set; } = HttpStatusCode.OK;
    [JsonIgnore]
    public bool Failure => Notifications.Any(x => _errorNotificationTypes.Contains(x.TypeId));
    public bool Success => !Failure;
    public IList<NotificationItem> Notifications { get; protected set; } = [];
}
