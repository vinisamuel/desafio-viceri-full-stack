namespace HeroesApi.Tests.Services.Implementations;

public class HeroServiceTests
{
    private readonly Mock<IUnitOfWork<HeroesAppDbContext>> _unitOfWorkMock;
    private readonly Mock<INotification> _notificationMock;
    private readonly Mock<ILogger<HeroService>> _loggerMock;
    private readonly HeroService _heroService;
    private readonly Faker _faker;

    public HeroServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork<HeroesAppDbContext>>();
        _notificationMock = new Mock<INotification>();
        _loggerMock = new Mock<ILogger<HeroService>>();
        _faker = new Faker("en");

        _heroService = new HeroService(
            _loggerMock.Object,
            _notificationMock.Object,
            _unitOfWorkMock.Object
        );

        SetupDefaultNotificationBehavior();
    }

    private void SetupDefaultNotificationBehavior()
    {
        _notificationMock.Setup(x => x.Success).Returns(true);
        _notificationMock.Setup(x => x.Failure).Returns(false);
    }

    private void SetupUnitOfWorkGetAllHeroes(IList<HeroEntity> entities)
    {
        _unitOfWorkMock.Setup(x => x.GetAllAsync<HeroEntity>(
            It.IsAny<Expression<Func<HeroEntity, HeroEntity>>>(),
            It.IsAny<Expression<Func<HeroEntity, object>>[]>(),
            It.IsAny<Func<IQueryable<HeroEntity>, IOrderedQueryable<HeroEntity>>>(),
            CancellationToken.None))
            .ReturnsAsync(entities);
    }

    private void SetupUnitOfWorkExistsHero(bool exists = false)
    {
        _unitOfWorkMock.Setup(x => x.ExistsAsync<HeroEntity>(
            It.IsAny<Expression<Func<HeroEntity, bool>>>(),
            CancellationToken.None))
            .ReturnsAsync(exists);
    }

    private void SetupUnitOfWorkExistsByIdHero(long id, bool exists = false)
    {
        _unitOfWorkMock.Setup(x => x.ExistsByIdAsync<HeroEntity>(
            id,
            CancellationToken.None))
            .ReturnsAsync(exists);
    }

    private void SetupUnitOfWorkExistsSuperpower(long id, bool exists = true)
    {
        _unitOfWorkMock.Setup(x => x.ExistsByIdAsync<SuperpowerEntity>(
            id,
            CancellationToken.None))
            .ReturnsAsync(exists);
    }

    private void SetupUnitOfWorkUpdateByIdHero(long id, int affectedRows)
    {
        _unitOfWorkMock.Setup(x => x.UpdateByIdAsync<HeroEntity>(
            id,
            It.IsAny<Expression<Func<SetPropertyCalls<HeroEntity>, SetPropertyCalls<HeroEntity>>>>(),
            CancellationToken.None))
            .ReturnsAsync(affectedRows);
    }

    private UpsertHeroModel CreateValidHeroModel()
    {
        return new UpsertHeroModel
        {
            Name = _faker.Person.FullName,
            HeroName = $"{_faker.Hacker.Adjective()} {_faker.Hacker.Noun()}",
            BirthDate = _faker.Date.Past(30, DateTime.Now.AddYears(-18)),
            Height = _faker.Random.Double(1.5, 2.2),
            Weight = _faker.Random.Double(50, 120),
            SuperpowerIds = new List<long> { 1, 2, 3 }
        };
    }

    private HeroEntity CreateHeroEntity(long id = 1)
    {
        return new HeroEntity
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

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_WhenHeroesExist_ShouldReturnList()
    {
        var heroes = new List<HeroEntity>
        {
            CreateHeroEntity(1),
            CreateHeroEntity(2),
            CreateHeroEntity(3)
        };

        SetupUnitOfWorkGetAllHeroes(heroes);

        var result = await _heroService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        _notificationMock.Verify(x => x.AddResponse(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoHeroes_ShouldReturnEmptyList()
    {
        SetupUnitOfWorkGetAllHeroes([]);

        var result = await _heroService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
        _notificationMock.Verify(x => x.AddResponse(It.IsAny<object>()), Times.Never);
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateHero()
    {
        var model = CreateValidHeroModel();
        var createdEntity = CreateHeroEntity(1);

        SetupUnitOfWorkExistsHero(false);
        SetupUnitOfWorkExistsSuperpower(1, true);
        SetupUnitOfWorkExistsSuperpower(2, true);
        SetupUnitOfWorkExistsSuperpower(3, true);

        _unitOfWorkMock.Setup(x => x.AddAsync(It.IsAny<HeroEntity>(), CancellationToken.None))
            .ReturnsAsync(1);

        var result = await _heroService.CreateAsync(model);

        _notificationMock.Verify(x => x.AddRequest(model), Times.Once);
        _unitOfWorkMock.Verify(x => x.AddAsync(It.IsAny<HeroEntity>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyName_ShouldReturnNull()
    {
        var model = CreateValidHeroModel();
        model.Name = "";

        _notificationMock.Setup(x => x.Success).Returns(false);

        var result = await _heroService.CreateAsync(model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddInvalidParameter("Name is required."), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyHeroName_ShouldReturnNull()
    {
        var model = CreateValidHeroModel();
        model.HeroName = "";

        _notificationMock.Setup(x => x.Success).Returns(false);

        var result = await _heroService.CreateAsync(model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddInvalidParameter("Hero name is required."), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidBirthDate_ShouldReturnNull()
    {
        var model = CreateValidHeroModel();
        model.BirthDate = DateTime.Now.AddDays(1);

        _notificationMock.Setup(x => x.Success).Returns(false);

        var result = await _heroService.CreateAsync(model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddInvalidParameter("Birth date is invalid."), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithZeroHeight_ShouldReturnNull()
    {
        var model = CreateValidHeroModel();
        model.Height = 0;

        _notificationMock.Setup(x => x.Success).Returns(false);

        var result = await _heroService.CreateAsync(model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddInvalidParameter("Height must be greater than zero."), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithNegativeWeight_ShouldReturnNull()
    {
        var model = CreateValidHeroModel();
        model.Weight = -10;

        _notificationMock.Setup(x => x.Success).Returns(false);

        var result = await _heroService.CreateAsync(model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddInvalidParameter("Weight must be greater than zero."), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithoutSuperpowers_ShouldReturnNull()
    {
        var model = CreateValidHeroModel();
        model.SuperpowerIds = new List<long>();

        _notificationMock.Setup(x => x.Success).Returns(false);

        var result = await _heroService.CreateAsync(model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddInvalidParameter("At least one superpower is required."), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateHeroName_ShouldReturnNull()
    {
        var model = CreateValidHeroModel();

        SetupUnitOfWorkExistsHero(true);

        var result = await _heroService.CreateAsync(model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddError(
            It.IsAny<ILogger>(),
            "A hero with this hero name already exists."), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithNonExistentSuperpower_ShouldReturnNull()
    {
        var model = CreateValidHeroModel();

        SetupUnitOfWorkExistsHero(false);
        SetupUnitOfWorkExistsSuperpower(1, true);
        SetupUnitOfWorkExistsSuperpower(2, false);

        var result = await _heroService.CreateAsync(model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddError(
            It.IsAny<ILogger>(),
            "Superpower with id 2 does not exist."), Times.Once);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateHero()
    {
        long id = 1;
        int affectedRows = 1;

        var model = CreateValidHeroModel();

        SetupUnitOfWorkExistsByIdHero(id, true);
        SetupUnitOfWorkExistsHero(false);
        SetupUnitOfWorkExistsSuperpower(1, true);
        SetupUnitOfWorkExistsSuperpower(2, true);
        SetupUnitOfWorkExistsSuperpower(3, true);
        SetupUnitOfWorkUpdateByIdHero(id, affectedRows);

        var result = await _heroService.UpdateAsync(id, model);

        _notificationMock.Verify(x => x.AddRequest(model), Times.Once);
        _unitOfWorkMock.Verify(x => x.UpdateByIdAsync<HeroEntity>(
            id,
            It.IsAny<Expression<Func<SetPropertyCalls<HeroEntity>, SetPropertyCalls<HeroEntity>>>>(),
            CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenHeroNotExists_ShouldReturnNull()
    {
        long id = 999;
        var model = CreateValidHeroModel();

        SetupUnitOfWorkExistsByIdHero(id, false);

        var result = await _heroService.UpdateAsync(id, model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddError(
            It.IsAny<ILogger>(),
            "Hero not found with the provided id."), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateHeroName_ShouldReturnNull()
    {
        long id = 1;
        var model = CreateValidHeroModel();

        SetupUnitOfWorkExistsByIdHero(id, true);
        SetupUnitOfWorkExistsHero(true);

        var result = await _heroService.UpdateAsync(id, model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddError(
            It.IsAny<ILogger>(),
            "A hero with this hero name already exists."), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenUpdateFails_ShouldReturnNull()
    {
        long id = 1;
        var model = CreateValidHeroModel();

        SetupUnitOfWorkExistsByIdHero(id, true);
        SetupUnitOfWorkExistsHero(false);
        SetupUnitOfWorkExistsSuperpower(1, true);
        SetupUnitOfWorkExistsSuperpower(2, true);
        SetupUnitOfWorkExistsSuperpower(3, true);
        SetupUnitOfWorkUpdateByIdHero(id, 0);
        
        // Setup FindByIdAsync to return null so GetByIdAsync fails
        _unitOfWorkMock.Setup(x => x.FindByIdAsync<HeroEntity>(
            id,
            It.IsAny<Expression<Func<HeroEntity, HeroEntity>>>(),
            It.IsAny<Expression<Func<HeroEntity, object>>[]>(),
            It.IsAny<Func<IQueryable<HeroEntity>, IOrderedQueryable<HeroEntity>>>(),
            CancellationToken.None))
            .ReturnsAsync((HeroEntity?)null);

        var result = await _heroService.UpdateAsync(id, model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddError(
            It.IsAny<ILogger>(),
            "Hero not found with the provided id."), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidData_ShouldReturnNull()
    {
        long id = 1;
        var model = CreateValidHeroModel();
        model.Name = "";

        _notificationMock.Setup(x => x.Success).Returns(false);

        var result = await _heroService.UpdateAsync(id, model);

        Assert.Null(result);
        _notificationMock.Verify(x => x.AddInvalidParameter("Name is required."), Times.Once);
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_WhenHeroExists_ShouldDeleteHero()
    {
        long id = 1;
        int affectedRows = 1;

        SetupUnitOfWorkExistsByIdHero(id, true);
        _unitOfWorkMock.Setup(x => x.DeleteByIdAsync<HeroEntity>(id, CancellationToken.None))
            .ReturnsAsync(affectedRows);

        var result = await _heroService.DeleteByIdAsync(id);

        Assert.True(result);
        _notificationMock.Verify(x => x.AddInformation("Hero deleted successfully."), Times.Once);
        _unitOfWorkMock.Verify(x => x.DeleteByIdAsync<HeroEntity>(id, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenHeroNotExists_ShouldReturnFalse()
    {
        long id = 999;

        SetupUnitOfWorkExistsByIdHero(id, false);

        var result = await _heroService.DeleteByIdAsync(id);

        Assert.False(result);
        _notificationMock.Verify(x => x.AddError(
            It.IsAny<ILogger>(),
            "Hero not found with the provided id."), Times.Once);
        _unitOfWorkMock.Verify(x => x.DeleteByIdAsync<HeroEntity>(It.IsAny<long>(), CancellationToken.None), Times.Never);
    }

    #endregion
}
