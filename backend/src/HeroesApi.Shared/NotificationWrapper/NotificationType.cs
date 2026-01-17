using System.ComponentModel;

namespace HeroesApi.Shared.NotificationWrapper;

public enum NotificationType
{
    Information = 1,
    Warning,
    [Description("Invalid Parameter")] InvalidParameter,
    Error,
    [Description("Exception")] UntreatedException = 99
}
