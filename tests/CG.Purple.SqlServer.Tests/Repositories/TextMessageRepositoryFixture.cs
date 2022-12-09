
namespace CG.Purple.Providers.SqlServer.Repositories;

/// <summary>
/// This class is a test fixture for the <see cref="TextMessageRepository"/>
/// class.
/// </summary>
[TestClass]
public class TextMessageRepositoryFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="TextMessageRepository.TextMessageRepository(Microsoft.EntityFrameworkCore.IDbContextFactory{Purple.SqlServer.PurpleDbContext}, AutoMapper.IMapper, Microsoft.Extensions.Logging.ILogger{Purple.Repositories.ITextMessageRepository})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public void TextMessageRepository_ctor()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<ITextMessageRepository>>();

        // Act ...
        var repository = new TextMessageRepository(
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
    /// This method ensures the <see cref="TextMessageRepository.AnyAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the 
    /// result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task TextMessageRepository_AnyAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<ITextMessageRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():D}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.TextMessages.Add(new Purple.SqlServer.Entities.TextMessage()
        {
            MessageKey = $"{Guid.NewGuid():D}",
            Attachments = Array.Empty<Purple.SqlServer.Entities.Attachment>(),
            MessageProperties = Array.Empty<Purple.SqlServer.Entities.MessageProperty>(),
            MessageType = Models.MessageType.Text,
            From = "test1@codegator.com",
            To = "test1@codegator.com",
            Body = "this is a test message",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new TextMessageRepository(
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
    /// This method ensures the <see cref="TextMessageRepository.CountAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task TextMessageRepository_CountAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<ITextMessageRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():D}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.TextMessages.Add(new Purple.SqlServer.Entities.TextMessage()
        {
            MessageKey = $"{Guid.NewGuid():D}",
            Attachments = Array.Empty<Purple.SqlServer.Entities.Attachment>(),
            MessageProperties = Array.Empty<Purple.SqlServer.Entities.MessageProperty>(),
            MessageType = Models.MessageType.Text,
            From = "test1@codegator.com",
            To = "test1@codegator.com",
            Body = "this is a test message",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new TextMessageRepository(
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
    /// This method ensures the <see cref="TextMessageRepository.CreateAsync(Models.TextMessage, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task TextMessageRepository_CreateAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<ITextMessageRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():D}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.TextMessage>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.TextMessage()
            {
                Id = 1,
                MessageKey = $"{Guid.NewGuid():D}",
                Attachments = Array.Empty<Purple.SqlServer.Entities.Attachment>(),
                MessageProperties = Array.Empty<Purple.SqlServer.Entities.MessageProperty>(),
                MessageType = Models.MessageType.Text,
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Body = "this is a test message",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        mapper.Setup(x => x.Map<Models.TextMessage>(
            It.IsAny<object>()
            )).Returns(new Models.TextMessage()
            {
                Id = 1,
                MessageKey = $"{Guid.NewGuid():D}",
                Attachments = Array.Empty<Models.Attachment>(),
                MessageProperties = Array.Empty<Models.MessageProperty>(),
                MessageType = Models.MessageType.Text,
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Body = "this is a test message",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new TextMessageRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.CreateAsync(
            new Models.TextMessage()
            {
                Id = 1,
                MessageKey = $"{Guid.NewGuid():D}",
                Attachments = Array.Empty<Models.Attachment>(),
                MessageProperties = Array.Empty<Models.MessageProperty>(),
                MessageType = Models.MessageType.Text,
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Body = "this is a test message",
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

    #endregion
}
