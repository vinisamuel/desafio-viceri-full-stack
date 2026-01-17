namespace HeroesApi.Shared.NotificationWrapper;

public class NotificationItem
{
    public DateTime Date { get; set; }
    public NotificationType TypeId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public NotificationItem() { }

    public NotificationItem(NotificationType notificationType, string message)
    {
        Date = DateTime.Now;
        TypeId = notificationType;
        Type = TypeId.GetDescription();
        Message = message;
    }

    public string GetFormattedMessage(bool addDateTime) => (addDateTime ? $"[{Date.ToBrazilDateTime()}] " : string.Empty) + $"[{Type}] {Message}";
}
