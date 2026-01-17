using HeroesApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HeroesApi.Tests.Controllers;

public class HeroesControllerTests
{
    private readonly Mock<INotification> _notificationMock;
    private readonly Mock<IHeroService> _heroServiceMock;
    private readonly HeroesController _controller;
    private readonly Faker _faker;

    public HeroesControllerTests()
    {
        _notificationMock = new Mock<INotification>();
        _heroServiceMock = new Mock<IHeroService>();
        _faker = new Faker("en");

        _controller = new HeroesController(
            _notificationMock.Object,
            _heroServiceMock.Object
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

    private HeroModel CreateHeroModel(long id = 1)
    {
        return new HeroModel
        {
            Id = id,
            Name = _faker.Person.FullName,
            HeroName = $"{_faker.Hacker.Adjective()} {_faker.Hacker.Noun()}",
            BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
            Height = _faker.Random.Double(1.5, 2.2),
            Weight = _faker.Random.Double(50, 120),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private HeroDetailModel CreateHeroDetailModel(long id = 1)
    {
        var model = new HeroDetailModel
        {
            Id = id,
            Name = _faker.Person.FullName,
            HeroName = $"{_faker.Hacker.Adjective()} {_faker.Hacker.Noun()}",
            BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
            Height = _faker.Random.Double(1.5, 2.2),
            Weight = _faker.Random.Double(50, 120),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Superpowers =
            [
                new SuperpowerModel { Id = 1, Superpower = "Super Strength", Description = "Test" },
                new SuperpowerModel { Id = 2, Superpower = "Flight", Description = "Test" }
            ]
        };
        return model;
    }

    private UpsertHeroModel CreateUpsertHeroModel()
    {
        return new UpsertHeroModel
        {
            Name = _faker.Person.FullName,
            HeroName = $"{_faker.Hacker.Adjective()} {_faker.Hacker.Noun()}",
            BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
            Height = _faker.Random.Double(1.5, 2.2),
            Weight = _faker.Random.Double(50, 120),
            SuperpowerIds = [1, 2, 3]
        };
    }

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenSuccess_ShouldReturnOkWithData()
    {
        var heroes = new List<HeroModel>
        {
            CreateHeroModel(1),
            CreateHeroModel(2),
            CreateHeroModel(3)
        };

        _heroServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(heroes);
        _notificationMock.Setup(x => x.Data).Returns(heroes);

        var result = await _controller.GetAllAsync();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        _heroServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.OK), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenNoHeroes_ShouldReturnNoContent()
    {
        _heroServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync([]);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.HasNotification(It.IsAny<NotificationType[]>())).Returns(false);

        var result = await _controller.GetAllAsync();

        Assert.IsType<NoContentResult>(result);
        _heroServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenInternalError_ShouldReturnInternalServerError()
    {
        _heroServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync([]);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.UntreatedException)).Returns(true);

        var result = await _controller.GetAllAsync();

        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.Equal(_notificationMock.Object, jsonResult.Value);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.InternalServerError), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WhenHeroExists_ShouldReturnOkWithData()
    {
        long id = 1;
        var hero = CreateHeroDetailModel(id);

        _heroServiceMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(hero);
        _notificationMock.Setup(x => x.Data).Returns(hero);

        var result = await _controller.GetByIdAsync(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        _heroServiceMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.OK), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenHeroNotFound_ShouldReturnUnprocessableEntity()
    {
        long id = 999;

        _heroServiceMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((HeroDetailModel?)null);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.Error)).Returns(true);

        var result = await _controller.GetByIdAsync(id);

        var unprocessableResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
        Assert.Equal(422, unprocessableResult.StatusCode);
        _heroServiceMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode((HttpStatusCode)422), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_WhenCreateSucceeds_ShouldReturnOkWithData()
    {
        var model = CreateUpsertHeroModel();
        var createdHero = CreateHeroDetailModel(1);

        _heroServiceMock.Setup(x => x.CreateAsync(model)).ReturnsAsync(createdHero);
        _notificationMock.Setup(x => x.Data).Returns(createdHero);

        var result = await _controller.CreateAsync(model);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        _heroServiceMock.Verify(x => x.CreateAsync(model), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.OK), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task Create_WhenValidationFails_ShouldReturnBadRequest()
    {
        var model = CreateUpsertHeroModel();

        _heroServiceMock.Setup(x => x.CreateAsync(model)).ReturnsAsync((HeroDetailModel?)null);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.InvalidParameter)).Returns(true);

        var result = await _controller.CreateAsync(model);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        _heroServiceMock.Verify(x => x.CreateAsync(model), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.BadRequest), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task Create_WhenDuplicateHeroName_ShouldReturnUnprocessableEntity()
    {
        var model = CreateUpsertHeroModel();

        _heroServiceMock.Setup(x => x.CreateAsync(model)).ReturnsAsync((HeroDetailModel?)null);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.Error)).Returns(true);

        var result = await _controller.CreateAsync(model);

        var unprocessableResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
        Assert.Equal(422, unprocessableResult.StatusCode);
        _heroServiceMock.Verify(x => x.CreateAsync(model), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode((HttpStatusCode)422), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task Create_WhenInternalError_ShouldReturnInternalServerError()
    {
        var model = CreateUpsertHeroModel();

        _heroServiceMock.Setup(x => x.CreateAsync(model)).ReturnsAsync((HeroDetailModel?)null);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.UntreatedException)).Returns(true);

        var result = await _controller.CreateAsync(model);

        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.Equal(_notificationMock.Object, jsonResult.Value);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.InternalServerError), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_WhenUpdateSucceeds_ShouldReturnOkWithData()
    {
        long id = 1;
        var model = CreateUpsertHeroModel();
        var updatedHero = CreateHeroDetailModel(id);

        _heroServiceMock.Setup(x => x.UpdateAsync(id, model)).ReturnsAsync(updatedHero);
        _notificationMock.Setup(x => x.Data).Returns(updatedHero);

        var result = await _controller.UpdateAsync(id, model);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        _heroServiceMock.Verify(x => x.UpdateAsync(id, model), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.OK), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task Update_WhenValidationFails_ShouldReturnBadRequest()
    {
        long id = 1;
        var model = CreateUpsertHeroModel();

        _heroServiceMock.Setup(x => x.UpdateAsync(id, model)).ReturnsAsync((HeroDetailModel?)null);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.InvalidParameter)).Returns(true);

        var result = await _controller.UpdateAsync(id, model);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        _heroServiceMock.Verify(x => x.UpdateAsync(id, model), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.BadRequest), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task Update_WhenHeroNotFound_ShouldReturnUnprocessableEntity()
    {
        long id = 999;
        var model = CreateUpsertHeroModel();

        _heroServiceMock.Setup(x => x.UpdateAsync(id, model)).ReturnsAsync((HeroDetailModel?)null);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.Error)).Returns(true);

        var result = await _controller.UpdateAsync(id, model);

        var unprocessableResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
        Assert.Equal(422, unprocessableResult.StatusCode);
        _heroServiceMock.Verify(x => x.UpdateAsync(id, model), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode((HttpStatusCode)422), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task Update_WhenUpdateFails_ShouldReturnUnprocessableEntity()
    {
        long id = 1;
        var model = CreateUpsertHeroModel();

        _heroServiceMock.Setup(x => x.UpdateAsync(id, model)).ReturnsAsync((HeroDetailModel?)null);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.Error)).Returns(true);

        var result = await _controller.UpdateAsync(id, model);

        var unprocessableResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
        Assert.Equal(422, unprocessableResult.StatusCode);
        _heroServiceMock.Verify(x => x.UpdateAsync(id, model), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode((HttpStatusCode)422), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_WhenDeleteSucceeds_ShouldReturnOkWithInformation()
    {
        long id = 1;

        _heroServiceMock.Setup(x => x.DeleteByIdAsync(id)).ReturnsAsync(true);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.Information, NotificationType.Warning)).Returns(true);

        var result = await _controller.DeleteByIdAsync(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        _heroServiceMock.Verify(x => x.DeleteByIdAsync(id), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode(HttpStatusCode.OK), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenHeroNotFound_ShouldReturnUnprocessableEntity()
    {
        long id = 999;

        _heroServiceMock.Setup(x => x.DeleteByIdAsync(id)).ReturnsAsync(false);
        _notificationMock.Setup(x => x.Data).Returns((object?)null);
        _notificationMock.Setup(x => x.HasNotification(NotificationType.Error)).Returns(true);

        var result = await _controller.DeleteByIdAsync(id);

        var unprocessableResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
        Assert.Equal(422, unprocessableResult.StatusCode);
        _heroServiceMock.Verify(x => x.DeleteByIdAsync(id), Times.Once);
        _notificationMock.Verify(x => x.SetStatusCode((HttpStatusCode)422), Times.Once);
        _notificationMock.Verify(x => x.StopTime(), Times.Once);
    }

    #endregion

    #region Additional Coverage Tests

    [Fact]
    public async Task GetAll_WhenHasWarning_ShouldReturnOk()
    {
        _heroServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync([]);
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
