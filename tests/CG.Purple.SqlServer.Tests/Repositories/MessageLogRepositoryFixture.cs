
namespace CG.Purple.Providers.SqlServer.Repositories;

/// <summary>
/// This class is a test fixture for the <see cref="MessageLogRepository"/>
/// class.
/// </summary>
[TestClass]
public class MessageLogRepositoryFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="MessageLogRepository.MessageLogRepository(Microsoft.EntityFrameworkCore.IDbContextFactory{Purple.SqlServer.PurpleDbContext}, AutoMapper.IMapper, Microsoft.Extensions.Logging.ILogger{Purple.Repositories.IMessageLogRepository})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public void MesssageLogRepository_ctor()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessageLogRepository>>();

        // Act ...
        var repository = new MessageLogRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            repository._dbContextFactory != null,
            "The _dbContextFactory field wasn't initialize!"
            );
        Assert.IsTrue(
            repository._mapper != null,
            "The _mapper field wasn't initialize!"
            );
        Assert.IsTrue(
            repository._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessageLogRepository.AnyAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the 
    /// result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageLogRepository_AnyAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessageLogRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.MessageLogs.Add(new Purple.SqlServer.Entities.MessageLog()
        {
            Id = 1,
            MessageId = 1,
            MessageEvent = Models.MessageEvent.Assigned,
            ProviderTypeId = 1,
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new MessageLogRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.AnyAsync()
            .ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            result,
            "The return value was invalid!"
            );

        Mock.Verify(
            factory,
            mapper,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessageLogRepository.CountAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageLogRepository_CountAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessageLogRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.MessageLogs.Add(new Purple.SqlServer.Entities.MessageLog()
        {
            Id = 1,
            MessageId = 1,
            MessageEvent = Models.MessageEvent.Assigned,
            ProviderTypeId = 1,
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new MessageLogRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.CountAsync()
            .ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            result == 1,
            "The return value was invalid!"
            );

        Mock.Verify(
            factory,
            mapper,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessageLogRepository.CreateAsync(Models.MessageLog, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageLogRepository_CreateAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessageLogRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.MessageLog>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.MessageLog()
            {
                Id = 1,
                MessageId = 1,
                MessageEvent = Models.MessageEvent.Assigned,
                ProviderTypeId = 1,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        mapper.Setup(x => x.Map<Models.MessageLog>(
            It.IsAny<object>()
            )).Returns(new Models.MessageLog()
            {
                Id = 1,
                Message = new Models.Message(),
                MessageEvent = Models.MessageEvent.Assigned,
                ProviderType = new Models.ProviderType(),
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new MessageLogRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.CreateAsync(
            new Models.MessageLog()
            {
                Id = 1,
                Message = new Models.Message(),
                MessageEvent = Models.MessageEvent.Assigned,
                ProviderType = new Models.ProviderType(),
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            result is not null,
            "The return value was invalid!"
            );

        Mock.Verify(
            factory,
            mapper,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessageLogRepository.DeleteAsync(Models.MessageLog, CancellationToken)"/>
    /// method properly calls the data-context method.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageLogRepository_DeleteAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessageLogRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.MessageLogs.Add(new Purple.SqlServer.Entities.MessageLog()
        {
            Id = 1,
            MessageId = 1,
            MessageEvent = Models.MessageEvent.Assigned,
            ProviderTypeId = 1,
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.MessageLog>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.MessageLog()
            {
                Id = 1,
                MessageId = 1,
                MessageEvent = Models.MessageEvent.Assigned,
                ProviderTypeId = 1,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new MessageLogRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        await respository.DeleteAsync(
            new Models.MessageLog()
            {
                Id = 1,
                Message = new Models.Message(),
                MessageEvent = Models.MessageEvent.Assigned,
                ProviderType = new Models.ProviderType(),
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            });

        // Assert ...
        Mock.Verify(
            factory,
            mapper,
            logger
            );
    }

    #endregion
}
