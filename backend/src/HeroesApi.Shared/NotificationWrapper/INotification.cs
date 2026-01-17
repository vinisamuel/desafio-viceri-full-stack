namespace HeroesApi.Shared.NotificationWrapper;

public interface INotification
{
    #region Properties

    string TraceId { get; }
    string ExecutionTime { get; }
    HttpStatusCode StatusCode { get; }
    bool Failure { get; }
    bool Success { get; }
    object? Request { get; }
    IList<NotificationItem> Notifications { get; }
    object? Data { get; }

    #endregion

    #region Add Notifications

    void AddInformation(params string[] messages);
    void AddInformation(ILogger logger, params string[] messages);
    void AddWarning(params string[] messages);
    void AddWarning(ILogger logger, params string[] messages);
    void AddError(params string[] messages);
    void AddError(ILogger logger, params string[] messages);
    void AddTreatedException(Exception exception, string message);
    void AddTreatedException(ILogger logger, Exception exception, string message);
    void AddUntreatedException(ILogger logger, Exception exception, IHostEnvironment environment);
    void AddInvalidParameter(string message);
    void AddInvalidParameter(ILogger logger, string message);
    
    #endregion

    bool HasNotification(params NotificationType[] notificationTypes);
    void SetStatusCode(HttpStatusCode statusCode);
    void StopTime();
    void AddRequest(object data);
    void AddResponse(object data, params string[] messages);
}
