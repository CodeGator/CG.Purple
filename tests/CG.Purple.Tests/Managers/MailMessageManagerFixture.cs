
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="MailMessageManager"/> class.
/// </summary>
[TestClass]
public class MailMessageManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="MailMessageManager.MailMessageManager(IOptions{BllOptions}, IMailMessageRepository, ILogger{IMailMessageManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void MailMessageManager_ctor()
    {
        // Arrange ...
        var options = new Mock<IOptions<BllOptions>>();
        var repository = new Mock<IMailMessageRepository>();
        var logger = new Mock<ILogger<IMailMessageManager>>();
        var bllOptions = new Mock<BllOptions>();
        var mailMessangerOptions = new Mock<MailMessageManagerOptions>();

        options.SetupGet(x => x.Value).Returns(bllOptions.Object);
        bllOptions.SetupGet(x => x.MailMessageManager).Returns(mailMessangerOptions.Object);

        // Act ...
        var manager = new MailMessageManager(
            options.Object,
            repository.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._managerOptions != null,
            "The _managerOptions field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._mailMessageRepository != null,
            "The _mailMessageRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MailMessageManager.AnyAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MailMessageManager_AnyAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<BllOptions>>();
        var repository = new Mock<IMailMessageRepository>();
        var logger = new Mock<ILogger<IMailMessageManager>>();
        var bllOptions = new Mock<BllOptions>();
        var mailMessangerOptions = new Mock<MailMessageManagerOptions>();

        options.SetupGet(x => x.Value).Returns(bllOptions.Object);
        bllOptions.SetupGet(x => x.MailMessageManager).Returns(mailMessangerOptions.Object);

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new MailMessageManager(
            options.Object,
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.AnyAsync();

        // Assert ...
        Assert.IsTrue(
            result,
            "The return value was invalid!"
            );

        Mock.Verify(
            options,
            repository,
            bllOptions,
            mailMessangerOptions,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MailMessageManager.CountAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MailMessageManager_CountAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<BllOptions>>();
        var repository = new Mock<IMailMessageRepository>();
        var logger = new Mock<ILogger<IMailMessageManager>>();
        var bllOptions = new Mock<BllOptions>();
        var mailMessangerOptions = new Mock<MailMessageManagerOptions>();

        options.SetupGet(x => x.Value).Returns(bllOptions.Object);
        bllOptions.SetupGet(x => x.MailMessageManager).Returns(mailMessangerOptions.Object);

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new MailMessageManager(
            options.Object,
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CountAsync();

        // Assert ...
        Assert.IsTrue(
            result == 1,
            "The return value was invalid!"
            );

        Mock.Verify(
            options,
            repository,
            bllOptions,
            mailMessangerOptions,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MailMessageManager.CreateAsync(Models.MailMessage, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MailMessageManager_CreateAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<BllOptions>>();
        var repository = new Mock<IMailMessageRepository>();
        var logger = new Mock<ILogger<IMailMessageManager>>();
        var bllOptions = new Mock<BllOptions>();
        var mailMessangerOptions = new Mock<MailMessageManagerOptions>();

        options.SetupGet(x => x.Value).Returns(bllOptions.Object);
        bllOptions.SetupGet(x => x.MailMessageManager).Returns(mailMessangerOptions.Object);

        repository.Setup(x => x.CreateAsync(
            It.IsAny<MailMessage>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new MailMessage()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Subject = "test email 1",
                Body = "This is test email 1",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new MailMessageManager(
            options.Object,
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new MailMessage()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Subject = "test email 1",
                Body = "This is test email 1",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            },
            "test"
            );

        // Assert ...
        Assert.IsTrue(
            result is not null,
            "The return value was invalid!"
            );

        Mock.Verify(
            options,
            repository,
            bllOptions,
            mailMessangerOptions,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MailMessageManager.UpdateAsync(Models.MailMessage, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MailMessageManager_UpdateAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<BllOptions>>();
        var repository = new Mock<IMailMessageRepository>();
        var logger = new Mock<ILogger<IMailMessageManager>>();
        var bllOptions = new Mock<BllOptions>();
        var mailMessangerOptions = new Mock<MailMessageManagerOptions>();

        options.SetupGet(x => x.Value).Returns(bllOptions.Object);
        bllOptions.SetupGet(x => x.MailMessageManager).Returns(mailMessangerOptions.Object);

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<MailMessage>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new MailMessage()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Subject = "test email 1",
                Body = "This is test email 1",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var manager = new MailMessageManager(
            options.Object,
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new MailMessage()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Subject = "test email 1",
                Body = "This is test email 1",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            },
            "test"
            );

        // Assert ...
        Assert.IsTrue(
            result is not null,
            "The return value was invalid!"
            );

        Mock.Verify(
            options,
            repository,
            bllOptions,
            mailMessangerOptions,
            logger
            );
    }

    #endregion
}