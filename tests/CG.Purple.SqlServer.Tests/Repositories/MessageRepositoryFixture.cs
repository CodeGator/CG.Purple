
namespace CG.Purple.Providers.SqlServer.Repositories;

/// <summary>
/// This class is a test fixture for the <see cref="MessageRepository"/>
/// class.
/// </summary>
[TestClass]
public class MessageRepositoryFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="MessageRepository.MessageRepository(Microsoft.EntityFrameworkCore.IDbContextFactory{Purple.SqlServer.PurpleDbContext}, AutoMapper.IMapper, Microsoft.Extensions.Logging.ILogger{Purple.Repositories.IMessageRepository})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public void MessageRepository_ctor()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessageRepository>>();

        // Act ...
        var repository = new MessageRepository(
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
    /// This method ensures the <see cref="MessageRepository.AnyAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the 
    /// result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageRepository_AnyAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessageRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.Messages.Add(new Purple.SqlServer.Entities.Message()
        {
            Id = 1,
            Attachments = Array.Empty<Purple.SqlServer.Entities.Attachment>(),
            From = "test1@codegator.com",
            MessageKey = $"{Guid.NewGuid():N}",
            MessageProperties = Array.Empty<Purple.SqlServer.Entities.MessageProperty>(),
            MessageState = Models.MessageState.Pending,
            MessageType = Models.MessageType.Text,             
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new MessageRepository(
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
    /// This method ensures the <see cref="MessageRepository.CountAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageRepository_CountAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessageRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{  Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.Messages.Add(new Purple.SqlServer.Entities.Message()
        {
            Id = 1,
            Attachments = Array.Empty<Purple.SqlServer.Entities.Attachment>(),
            From = "test1@codegator.com",
            MessageKey = $"{Guid.NewGuid():N}",
            MessageProperties = Array.Empty<Purple.SqlServer.Entities.MessageProperty>(),
            MessageState = Models.MessageState.Pending,
            MessageType = Models.MessageType.Text,
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new MessageRepository(
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
    /// This method ensures the <see cref="MessageRepository.DeleteAsync(Models.Message, CancellationToken)"/>
    /// method properly calls the data-context method.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageRepository_DeleteAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMessageRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.Messages.Add(new Purple.SqlServer.Entities.Message()
        {
            Id = 1,
            Attachments = Array.Empty<Purple.SqlServer.Entities.Attachment>(),
            From = "test1@codegator.com",
            MessageKey = $"{Guid.NewGuid():N}",
            MessageProperties = Array.Empty<Purple.SqlServer.Entities.MessageProperty>(),
            MessageState = Models.MessageState.Pending,
            MessageType = Models.MessageType.Text,
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.Message>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.Message()
            {
                Id = 1,
                Attachments = Array.Empty<Purple.SqlServer.Entities.Attachment>(),
                From = "test1@codegator.com",
                MessageKey = $"{Guid.NewGuid():N}",
                MessageProperties = Array.Empty<Purple.SqlServer.Entities.MessageProperty>(),
                MessageState = Models.MessageState.Pending,
                MessageType = Models.MessageType.Text,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new MessageRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        await respository.DeleteAsync(
            new Models.Message()
            {
                Id = 1,
                Attachments = Array.Empty<Models.Attachment>(),
                From = "test1@codegator.com",
                MessageKey = $"{Guid.NewGuid():N}",
                MessageProperties = Array.Empty<Models.MessageProperty>(),
                MessageState = Models.MessageState.Pending,
                MessageType = Models.MessageType.Text,
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
