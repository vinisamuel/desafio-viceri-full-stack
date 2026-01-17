using Microsoft.Extensions.Hosting;
using System.Net;

namespace HeroesApi.Shared.Tests.NotificationWrapper;

public class NotificationTests
{
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<IHostEnvironment> _hostEnvironmentMock;

    public NotificationTests()
    {
        _loggerMock = new Mock<ILogger>();
        _hostEnvironmentMock = new Mock<IHostEnvironment>();
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldInitializeStopwatchAndTraceId()
    {
        var notification = new Notification();

        Assert.NotNull(notification.TraceId);
        Assert.NotEmpty(notification.TraceId);
        Assert.True(Guid.TryParse(notification.TraceId, out _));
        Assert.Empty(notification.ExecutionTime);
        Assert.Equal(HttpStatusCode.OK, notification.StatusCode);
        Assert.Empty(notification.Notifications);
        Assert.True(notification.Success);
        Assert.False(notification.Failure);
    }

    #endregion

    #region AddInformation Tests

    [Fact]
    public void AddInformation_WithSingleMessage_ShouldAddInformationNotification()
    {
        var notification = new Notification();

        notification.AddInformation("Test information");

        Assert.Single(notification.Notifications);
        Assert.Equal(NotificationType.Information, notification.Notifications[0].TypeId);
        Assert.Equal("Test information", notification.Notifications[0].Message);
        Assert.True(notification.Success);
    }

    [Fact]
    public void AddInformation_WithMultipleMessages_ShouldAddAllInformations()
    {
        var notification = new Notification();

        notification.AddInformation("Message 1", "Message 2", "Message 3");

        Assert.Equal(3, notification.Notifications.Count);
        Assert.All(notification.Notifications, n => Assert.Equal(NotificationType.Information, n.TypeId));
    }

    [Fact]
    public void AddInformation_WithLogger_ShouldAddNotificationAndLog()
    {
        var notification = new Notification();

        notification.AddInformation(_loggerMock.Object, "Test log");

        Assert.Single(notification.Notifications);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test log")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void AddInformation_WithLoggerAndMultipleMessages_ShouldLogAll()
    {
        var notification = new Notification();

        notification.AddInformation(_loggerMock.Object, "Message 1", "Message 2");

        Assert.Equal(2, notification.Notifications.Count);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(2));
    }

    #endregion

    #region AddWarning Tests

    [Fact]
    public void AddWarning_WithSingleMessage_ShouldAddWarningNotification()
    {
        var notification = new Notification();

        notification.AddWarning("Test warning");

        Assert.Single(notification.Notifications);
        Assert.Equal(NotificationType.Warning, notification.Notifications[0].TypeId);
        Assert.Equal("Test warning", notification.Notifications[0].Message);
        Assert.True(notification.Success);
    }

    [Fact]
    public void AddWarning_WithMultipleMessages_ShouldAddAllWarnings()
    {
        var notification = new Notification();

        notification.AddWarning("Warning 1", "Warning 2");

        Assert.Equal(2, notification.Notifications.Count);
        Assert.All(notification.Notifications, n => Assert.Equal(NotificationType.Warning, n.TypeId));
    }

    [Fact]
    public void AddWarning_WithLogger_ShouldAddNotificationAndLog()
    {
        var notification = new Notification();

        notification.AddWarning(_loggerMock.Object, "Test warning log");

        Assert.Single(notification.Notifications);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test warning log")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region AddError Tests

    [Fact]
    public void AddError_WithSingleMessage_ShouldAddErrorNotification()
    {
        var notification = new Notification();

        notification.AddError("Test error");

        Assert.Single(notification.Notifications);
        Assert.Equal(NotificationType.Error, notification.Notifications[0].TypeId);
        Assert.Equal("Test error", notification.Notifications[0].Message);
        Assert.False(notification.Success);
        Assert.True(notification.Failure);
    }

    [Fact]
    public void AddError_WithMultipleMessages_ShouldAddAllErrors()
    {
        var notification = new Notification();

        notification.AddError("Error 1", "Error 2", "Error 3");

        Assert.Equal(3, notification.Notifications.Count);
        Assert.All(notification.Notifications, n => Assert.Equal(NotificationType.Error, n.TypeId));
        Assert.True(notification.Failure);
    }

    [Fact]
    public void AddError_WithLogger_ShouldAddNotificationAndLog()
    {
        var notification = new Notification();

        notification.AddError(_loggerMock.Object, "Test error log");

        Assert.Single(notification.Notifications);
        Assert.True(notification.Failure);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test error log")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region AddTreatedException Tests

    [Fact]
    public void AddTreatedException_WithException_ShouldAddErrorWithExceptionMessage()
    {
        var notification = new Notification();
        var exception = new InvalidOperationException("Test exception");

        notification.AddTreatedException(exception, "Custom message");

        Assert.Single(notification.Notifications);
        Assert.Equal(NotificationType.Error, notification.Notifications[0].TypeId);
        Assert.Contains("Custom message", notification.Notifications[0].Message);
        Assert.Contains("Test exception", notification.Notifications[0].Message);
        Assert.True(notification.Failure);
    }

    [Fact]
    public void AddTreatedException_WithLogger_ShouldAddNotificationAndLog()
    {
        var notification = new Notification();
        var exception = new InvalidOperationException("Test exception");

        notification.AddTreatedException(_loggerMock.Object, exception, "Custom error");

        Assert.Single(notification.Notifications);
        Assert.True(notification.Failure);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region AddUntreatedException Tests

    [Fact]
    public void AddUntreatedException_InProduction_ShouldAddGenericMessage()
    {
        var notification = new Notification();
        var exception = new Exception("Internal error");
        _hostEnvironmentMock.Setup(x => x.EnvironmentName).Returns("Production");

        notification.AddUntreatedException(_loggerMock.Object, exception, _hostEnvironmentMock.Object);

        Assert.Single(notification.Notifications);
        Assert.Equal(NotificationType.UntreatedException, notification.Notifications[0].TypeId);
        Assert.Contains("Ocorreu um erro inesperado", notification.Notifications[0].Message);
        Assert.True(notification.Failure);
        Assert.Null(notification.Data);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void AddUntreatedException_InDevelopment_ShouldAddGenericMessageAndException()
    {
        var notification = new Notification();
        var exception = new Exception("Internal error");
        _hostEnvironmentMock.Setup(x => x.EnvironmentName).Returns("Development");

        notification.AddUntreatedException(_loggerMock.Object, exception, _hostEnvironmentMock.Object);

        Assert.Single(notification.Notifications);
        Assert.Equal(NotificationType.UntreatedException, notification.Notifications[0].TypeId);
        Assert.NotNull(notification.Data);
        Assert.Equal(exception, notification.Data);
        Assert.True(notification.Failure);
    }

    #endregion

    #region AddInvalidParameter Tests

    [Fact]
    public void AddInvalidParameter_WithMessage_ShouldAddInvalidParameterNotification()
    {
        var notification = new Notification();

        notification.AddInvalidParameter("Invalid parameter message");

        Assert.Single(notification.Notifications);
        Assert.Equal(NotificationType.InvalidParameter, notification.Notifications[0].TypeId);
        Assert.Equal("Invalid parameter message", notification.Notifications[0].Message);
        Assert.True(notification.Failure);
    }

    [Fact]
    public void AddInvalidParameter_WithLogger_ShouldAddNotificationAndLog()
    {
        var notification = new Notification();

        notification.AddInvalidParameter(_loggerMock.Object, "Invalid parameter log");

        Assert.Single(notification.Notifications);
        Assert.True(notification.Failure);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Invalid parameter log")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region HasNotification Tests

    [Fact]
    public void HasNotification_WhenNoNotificationsOfType_ShouldReturnFalse()
    {
        var notification = new Notification();
        notification.AddInformation("Info");

        var result = notification.HasNotification(NotificationType.Error);

        Assert.False(result);
    }

    [Fact]
    public void HasNotification_WhenNotificationsOfTypeExist_ShouldReturnTrue()
    {
        var notification = new Notification();
        notification.AddError("Error");

        var result = notification.HasNotification(NotificationType.Error);

        Assert.True(result);
    }

    [Fact]
    public void HasNotification_WithMultipleTypes_ShouldReturnTrueIfAnyMatch()
    {
        var notification = new Notification();
        notification.AddWarning("Warning");

        var result = notification.HasNotification(NotificationType.Error, NotificationType.Warning);

        Assert.True(result);
    }

    [Fact]
    public void HasNotification_WithMultipleTypes_ShouldReturnFalseIfNoneMatch()
    {
        var notification = new Notification();
        notification.AddInformation("Info");

        var result = notification.HasNotification(NotificationType.Error, NotificationType.Warning);

        Assert.False(result);
    }

    #endregion

    #region SetStatusCode Tests

    [Fact]
    public void SetStatusCode_ShouldUpdateStatusCode()
    {
        var notification = new Notification();

        notification.SetStatusCode(HttpStatusCode.BadRequest);

        Assert.Equal(HttpStatusCode.BadRequest, notification.StatusCode);
    }

    [Fact]
    public void SetStatusCode_WithMultipleCalls_ShouldUpdateToLatest()
    {
        var notification = new Notification();

        notification.SetStatusCode(HttpStatusCode.NotFound);
        notification.SetStatusCode(HttpStatusCode.InternalServerError);

        Assert.Equal(HttpStatusCode.InternalServerError, notification.StatusCode);
    }

    #endregion

    #region StopTime Tests

    [Fact]
    public void StopTime_ShouldSetExecutionTime()
    {
        var notification = new Notification();
        
        Thread.Sleep(10);
        notification.StopTime();

        Assert.NotEmpty(notification.ExecutionTime);
        Assert.Contains(":", notification.ExecutionTime);
    }

    [Fact]
    public void StopTime_ExecutionTime_ShouldBeValidTimeSpanFormat()
    {
        var notification = new Notification();
        
        notification.StopTime();

        Assert.NotEmpty(notification.ExecutionTime);
        Assert.True(TimeSpan.TryParse(notification.ExecutionTime, out _));
    }

    #endregion

    #region AddRequest Tests

    [Fact]
    public void AddRequest_WithData_ShouldSetRequest()
    {
        var notification = new Notification();
        var requestData = new { Id = 1, Name = "Test" };

        notification.AddRequest(requestData);

        Assert.NotNull(notification.Request);
        Assert.Equal(requestData, notification.Request);
    }

    [Fact]
    public void AddRequest_WithNull_ShouldSetRequestToNull()
    {
        var notification = new Notification();

        notification.AddRequest(null!);

        Assert.Null(notification.Request);
    }

    #endregion

    #region AddResponse Tests

    [Fact]
    public void AddResponse_WithDataOnly_ShouldSetData()
    {
        var notification = new Notification();
        var responseData = new { Id = 1, Name = "Response" };

        notification.AddResponse(responseData);

        Assert.NotNull(notification.Data);
        Assert.Equal(responseData, notification.Data);
        Assert.Empty(notification.Notifications);
    }

    [Fact]
    public void AddResponse_WithDataAndMessages_ShouldSetDataAndAddInformations()
    {
        var notification = new Notification();
        var responseData = new { Id = 1 };

        notification.AddResponse(responseData, "Message 1", "Message 2");

        Assert.NotNull(notification.Data);
        Assert.Equal(2, notification.Notifications.Count);
        Assert.All(notification.Notifications, n => Assert.Equal(NotificationType.Information, n.TypeId));
    }

    [Fact]
    public void AddResponse_WithEmptyMessages_ShouldOnlySetData()
    {
        var notification = new Notification();
        var responseData = new { Id = 1 };

        notification.AddResponse(responseData, []);

        Assert.NotNull(notification.Data);
        Assert.Empty(notification.Notifications);
    }

    #endregion

    #region Success and Failure Tests

    [Fact]
    public void Success_WhenNoErrors_ShouldReturnTrue()
    {
        var notification = new Notification();
        notification.AddInformation("Info");
        notification.AddWarning("Warning");

        Assert.True(notification.Success);
        Assert.False(notification.Failure);
    }

    [Fact]
    public void Failure_WhenHasError_ShouldReturnTrue()
    {
        var notification = new Notification();
        notification.AddError("Error");

        Assert.False(notification.Success);
        Assert.True(notification.Failure);
    }

    [Fact]
    public void Failure_WhenHasInvalidParameter_ShouldReturnTrue()
    {
        var notification = new Notification();
        notification.AddInvalidParameter("Invalid");

        Assert.False(notification.Success);
        Assert.True(notification.Failure);
    }

    [Fact]
    public void Failure_WhenHasUntreatedException_ShouldReturnTrue()
    {
        var notification = new Notification();
        var exception = new Exception("Test");
        _hostEnvironmentMock.Setup(x => x.EnvironmentName).Returns("Production");

        notification.AddUntreatedException(_loggerMock.Object, exception, _hostEnvironmentMock.Object);

        Assert.False(notification.Success);
        Assert.True(notification.Failure);
    }

    [Fact]
    public void Success_WhenMultipleNotificationTypes_ShouldBeFalseIfAnyError()
    {
        var notification = new Notification();
        notification.AddInformation("Info");
        notification.AddWarning("Warning");
        notification.AddError("Error");

        Assert.False(notification.Success);
        Assert.True(notification.Failure);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Notification_CompleteFlow_ShouldWorkCorrectly()
    {
        var notification = new Notification();
        var requestData = new { UserId = 1 };
        var responseData = new { Result = "Success" };

        notification.AddRequest(requestData);
        notification.AddInformation("Processing started");
        notification.AddResponse(responseData, "Processing completed");
        notification.SetStatusCode(HttpStatusCode.OK);
        notification.StopTime();

        Assert.NotNull(notification.TraceId);
        Assert.Equal(requestData, notification.Request);
        Assert.Equal(responseData, notification.Data);
        Assert.Equal(2, notification.Notifications.Count);
        Assert.Equal(HttpStatusCode.OK, notification.StatusCode);
        Assert.NotEmpty(notification.ExecutionTime);
        Assert.True(notification.Success);
    }

    [Fact]
    public void Notification_ErrorFlow_ShouldWorkCorrectly()
    {
        var notification = new Notification();

        notification.AddError("Validation failed");
        notification.AddInvalidParameter("Field is required");
        notification.SetStatusCode(HttpStatusCode.BadRequest);
        notification.StopTime();

        Assert.Equal(2, notification.Notifications.Count);
        Assert.Equal(HttpStatusCode.BadRequest, notification.StatusCode);
        Assert.True(notification.Failure);
        Assert.False(notification.Success);
    }

    #endregion
}
