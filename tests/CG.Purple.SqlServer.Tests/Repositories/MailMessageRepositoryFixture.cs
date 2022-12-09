
namespace CG.Purple.Providers.SqlServer.Repositories;

/// <summary>
/// This class is a test fixture for the <see cref="MailMessageRepository"/>
/// class.
/// </summary>
[TestClass]
public class MailMessageRepositoryFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="MailMessageRepository.MailMessageRepository(Microsoft.EntityFrameworkCore.IDbContextFactory{Purple.SqlServer.PurpleDbContext}, AutoMapper.IMapper, Microsoft.Extensions.Logging.ILogger{Purple.Repositories.IMailMessageRepository})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public void MailMessageRepository_ctor()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMailMessageRepository>>();

        // Act ...
        var repository = new MailMessageRepository(
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
    /// This method ensures the <see cref="MailMessageRepository.AnyAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the 
    /// result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MailMessageRepository_AnyAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMailMessageRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():D}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.MailMessages.Add(new Purple.SqlServer.Entities.MailMessage()
        {
            MessageKey = $"{Guid.NewGuid():D}",
            Attachments = Array.Empty<Purple.SqlServer.Entities.Attachment>(),
            MessageProperties = Array.Empty<Purple.SqlServer.Entities.MessageProperty>(),
            MessageType = Models.MessageType.Mail,
            From = "test1@codegator.com",
            To = "test1@codegator.com",
            Subject = "test email 1",
            Body = "this is a test message",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new MailMessageRepository(
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
    /// This method ensures the <see cref="MailMessageRepository.CountAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MailMessageRepository_CountAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMailMessageRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():D}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.MailMessages.Add(new Purple.SqlServer.Entities.MailMessage()
        {
            MessageKey = $"{Guid.NewGuid():D}",
            Attachments = Array.Empty<Purple.SqlServer.Entities.Attachment>(),
            MessageProperties = Array.Empty<Purple.SqlServer.Entities.MessageProperty>(),
            MessageType = Models.MessageType.Mail,
            From = "test1@codegator.com",
            To = "test1@codegator.com",
            Subject = "test email 1",
            Body = "this is a test message",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new MailMessageRepository(
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
    /// This method ensures the <see cref="MailMessageRepository.CreateAsync(Models.MailMessage, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MailMessageRepository_CreateAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IMailMessageRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():D}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.MailMessage>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.MailMessage()
            {
                Id = 1,
                MessageKey = $"{Guid.NewGuid():D}",
                Attachments = Array.Empty<Purple.SqlServer.Entities.Attachment>(),
                MessageProperties = Array.Empty<Purple.SqlServer.Entities.MessageProperty>(),
                MessageType = Models.MessageType.Mail,
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Subject = "test email 1",
                Body = "this is a test message",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        mapper.Setup(x => x.Map<Models.MailMessage>(
            It.IsAny<object>()
            )).Returns(new Models.MailMessage()
            {
                Id = 1,
                MessageKey = $"{Guid.NewGuid():D}",
                Attachments = Array.Empty<Models.Attachment>(),
                MessageProperties = Array.Empty<Models.MessageProperty>(),
                MessageType = Models.MessageType.Mail,
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Subject = "test email 1",
                Body = "this is a test message",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new MailMessageRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.CreateAsync(
            new Models.MailMessage()
            {
                Id = 1,
                MessageKey = $"{Guid.NewGuid():D}",
                Attachments = Array.Empty<Models.Attachment>(),
                MessageProperties = Array.Empty<Models.MessageProperty>(),
                MessageType = Models.MessageType.Mail,
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Subject = "test email 1",
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
