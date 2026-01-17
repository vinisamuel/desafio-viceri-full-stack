using HeroesApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HeroesApi.Tests.Controllers;

public class SuperpowersControllerTests
{
    private readonly Mock<INotification> _notificationMock;
    private readonly Mock<ISuperpowerService> _superpowerServiceMock;
    private readonly SuperpowersController _controller;
    private readonly Faker _faker;

    public SuperpowersControllerTests()
    {
        _notificationMock = new Mock<INotification>();
        _superpowerServiceMock = new Mock<ISuperpowerService>();
        _faker = new Faker("en");

        _controller = new SuperpowersController(
            _notificationMock.Object,
            _superpowerServiceMock.Object
        );

        SetupDefaultNotificationBehavior();
    }

    private void SetupDefaultNotificationBehavior()
    {
        _notificationMock.Setup(x => x.Success).Returns(true);
        _notificationMock.Setup(x => x.Failure).Returns(false);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.Notifications).Returns([]);
        _notificationMock.Setup(x => x.HasNotification(It.IsAny<NotificationType[]>())).Returns(false);
    }

    private SuperpowerModel CreateSuperpowerModel(long id = 1)
    {
        return new SuperpowerModel
        {
            Id = id,
            Superpower = _faker.Random.Word(),
            Description = _faker.Lorem.Sentence()
        };
    }

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenSuccess_ShouldReturnOkWithData()
    {
        var superpowers = new List<SuperpowerModel>
        {
            CreateSuperpowerModel(1),
            CreateSuperpowerModel(2),
            CreateSuperpowerModel(3)
        };

        _superpowerServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(superpowers);
        _notificationMock.Setup(x => x.Data).Returns(superpowers);

        var result = await _controller.GetAllAsync();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        _superpowerServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.OK), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenNoSuperpowers_ShouldReturnNoContent()
    {
        _superpowerServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync([]);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.HasNotification(It.IsAny<NotificationType[]>())).Returns(false);

        var result = await _controller.GetAllAsync();

        Assert.IsType<NoContentResult>(result);
        _superpowerServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenInternalError_ShouldReturnInternalServerError()
    {
        _superpowerServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync([]);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.UntreatedException)).Returns(true);

        var result = await _controller.GetAllAsync();

        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.Equal(_notificationMock.Object, jsonResult.Value);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.InternalServerError), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenHasWarning_ShouldReturnOk()
    {
        _superpowerServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync([]);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.Information, NotificationType.Warning)).Returns(true);

        var result = await _controller.GetAllAsync();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.OK), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    #endregion
}
