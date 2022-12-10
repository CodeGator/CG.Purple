
namespace CG.Purple.Providers.SqlServer.Repositories;

/// <summary>
/// This class is a test fixture for the <see cref="MessagePropertyRepository"/>
/// class.
/// </summary>
[TestClass]
public class MessagePropertyRepositoryFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="MessagePropertyRepository.MessagePropertyRepository(Microsoft.EntityFrameworkCore.IDbContextFactory{Purple.SqlServer.PurpleDbContext}, AutoMapper.IMapper, Microsoft.Extensions.Logging.ILogger{Purple.Repositories.IMessagePropertyRepository})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public void MessagePropertyRepository_ctor()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessagePropertyRepository>>();

        // Act ...
        var repository = new MessagePropertyRepository(
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
    /// This method ensures the <see cref="MessagePropertyRepository.AnyAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the 
    /// result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessagePropertyRepository_AnyAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessagePropertyRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.MessageProperties.Add(new Purple.SqlServer.Entities.MessageProperty()
        {
            MessageId = 1,
            PropertyTypeId = 1,
            Value = "test",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new MessagePropertyRepository(
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
    /// This method ensures the <see cref="MessagePropertyRepository.CountAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessagePropertyRepository_CountAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessagePropertyRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.MessageProperties.Add(new Purple.SqlServer.Entities.MessageProperty()
        {
            MessageId = 1,
            PropertyTypeId = 1,
            Value = "test",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new MessagePropertyRepository(
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
    /// This method ensures the <see cref="MessagePropertyRepository.CreateAsync(Models.MessageProperty, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessagePropertyRepository_CreateAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessagePropertyRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.MessageProperty>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.MessageProperty()
            {
                MessageId = 1,
                PropertyTypeId = 1,
                Value = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        mapper.Setup(x => x.Map<Models.MessageProperty>(
            It.IsAny<object>()
            )).Returns(new Models.MessageProperty()
            {
                Message = new Models.Message(),
                PropertyType = new Models.PropertyType(),
                Value = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new MessagePropertyRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.CreateAsync(
            new Models.MessageProperty()
            {
                Message = new Models.Message(),
                PropertyType = new Models.PropertyType(),
                Value = "test",
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
    /// This method ensures the <see cref="MessagePropertyRepository.DeleteAsync(Models.MessageProperty, CancellationToken)"/>
    /// method properly calls the data-context method.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessagePropertyRepository_DeleteAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessagePropertyRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.MessageProperties.Add(new Purple.SqlServer.Entities.MessageProperty()
        {
            MessageId = 1,
            PropertyTypeId = 1,
            Value = "test",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.MessageProperty>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.MessageProperty()
            {
                MessageId = 1,
                PropertyTypeId = 1,
                Value = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new MessagePropertyRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        await respository.DeleteAsync(
            new Models.MessageProperty()
            {
                Message = new Models.Message(),
                PropertyType = new Models.PropertyType(),
                Value = "test",
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
