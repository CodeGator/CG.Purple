
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="TextMessageManager"/> class.
/// </summary>
[TestClass]
public class TextMessageManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="TextMessageManager.TextMessageManager(IOptions{BllOptions}, ITextMessageRepository, ILogger{ITextMessageManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void TextMessageManager_ctor()
    {
        // Arrange ...
        var options = new Mock<IOptions<BllOptions>>();
        var repository = new Mock<ITextMessageRepository>();
        var logger = new Mock<ILogger<ITextMessageManager>>();

        // Act ...
        var manager = new TextMessageManager(
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
            manager._textMessageRepository != null,
            "The _textMessageRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="TextMessageManager.AnyAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task TextMessageManager_AnyAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<BllOptions>>();
        var repository = new Mock<ITextMessageRepository>();
        var logger = new Mock<ILogger<ITextMessageManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new TextMessageManager(
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

        repository.Verify();
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="TextMessageManager.CountAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task TextMessageManager_CountAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<BllOptions>>();
        var repository = new Mock<ITextMessageRepository>();
        var logger = new Mock<ILogger<ITextMessageManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new TextMessageManager(
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

        repository.Verify();
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="TextMessageManager.CreateAsync(Models.TextMessage, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task TextMessageManager_CreateAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<BllOptions>>();
        var repository = new Mock<ITextMessageRepository>();
        var logger = new Mock<ILogger<ITextMessageManager>>();

        repository.Setup(x => x.CreateAsync(
            It.IsAny<TextMessage>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new TextMessage()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Body = "This is test email 1",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new TextMessageManager(
            options.Object,
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new TextMessage()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
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

        repository.Verify();
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="TextMessageManager.UpdateAsync(Models.TextMessage, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task TextMessageManager_UpdateAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<BllOptions>>();
        var repository = new Mock<ITextMessageRepository>();
        var logger = new Mock<ILogger<ITextMessageManager>>();

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<TextMessage>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new TextMessage()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Body = "This is test email 1",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var manager = new TextMessageManager(
            options.Object,
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new TextMessage()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
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

        repository.Verify();
    }

    #endregion
}