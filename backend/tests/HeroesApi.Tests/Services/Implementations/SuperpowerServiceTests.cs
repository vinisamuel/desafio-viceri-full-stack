namespace HeroesApi.Tests.Services.Implementations;

public class SuperpowerServiceTests
{
    private readonly Mock<IUnitOfWork<HeroesAppDbContext>> _unitOfWorkMock;
    private readonly Mock<INotification> _notificationMock;
    private readonly SuperpowerService _superpowerService;
    private readonly Faker _faker;

    public SuperpowerServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork<HeroesAppDbContext>>();
        _notificationMock = new Mock<INotification>();
        _faker = new Faker("en");

        _superpowerService = new SuperpowerService(
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

    private void SetupUnitOfWorkGetAllSuperpowers(IList<SuperpowerEntity> entities)
    {
        _unitOfWorkMock.Setup(x => x.GetAllAsync<SuperpowerEntity>(
            It.IsAny<Expression<Func<SuperpowerEntity, SuperpowerEntity>>>(),
            It.IsAny<Expression<Func<SuperpowerEntity, object>>[]>(),
            It.IsAny<Func<IQueryable<SuperpowerEntity>, IOrderedQueryable<SuperpowerEntity>>>(),
            CancellationToken.None))
            .ReturnsAsync(entities);
    }

    private SuperpowerEntity CreateSuperpowerEntity(long id = 1)
    {
        return new SuperpowerEntity
        {
            Id = id,
            Superpower = _faker.Random.Word(),
            Description = _faker.Lorem.Sentence(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_WhenSuperpowersExist_ShouldReturnList()
    {
        var superpowers = new List<SuperpowerEntity>
        {
            CreateSuperpowerEntity(1),
            CreateSuperpowerEntity(2),
            CreateSuperpowerEntity(3),
            CreateSuperpowerEntity(4),
            CreateSuperpowerEntity(5)
        };

        SetupUnitOfWorkGetAllSuperpowers(superpowers);

        var result = await _superpowerService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(5, result.Count);
        _notificationMock.Verify(x => x.AddResponse(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoSuperpowers_ShouldReturnEmptyList()
    {
        SetupUnitOfWorkGetAllSuperpowers([]);

        var result = await _superpowerService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
        _notificationMock.Verify(x => x.AddResponse(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_ShouldOrderSuperpowersByName()
    {
        var superpowers = new List<SuperpowerEntity>
        {
            new SuperpowerEntity { Id = 1, Superpower = "Z Power", Description = "Test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new SuperpowerEntity { Id = 2, Superpower = "A Power", Description = "Test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new SuperpowerEntity { Id = 3, Superpower = "M Power", Description = "Test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        SetupUnitOfWorkGetAllSuperpowers(superpowers);

        var result = await _superpowerService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        _unitOfWorkMock.Verify(x => x.GetAllAsync<SuperpowerEntity>(
            It.IsAny<Expression<Func<SuperpowerEntity, SuperpowerEntity>>>(),
            It.IsAny<Expression<Func<SuperpowerEntity, object>>[]>(),
            It.IsAny<Func<IQueryable<SuperpowerEntity>, IOrderedQueryable<SuperpowerEntity>>>(),
            CancellationToken.None), Times.Once);
    }

    #endregion
}
