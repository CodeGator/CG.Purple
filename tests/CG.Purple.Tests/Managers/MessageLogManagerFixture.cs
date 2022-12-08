
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="MessageLogManager"/> class.
/// </summary>
[TestClass]
public class MessageLogManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="MessageLogManager.MessageLogManager(IMessageLogRepository, ILogger{IMessageLogManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void MessageLogManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IMessageLogRepository>();
        var logger = new Mock<ILogger<IMessageLogManager>>();

        // Act ...
        var manager = new MessageLogManager(
            repository.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._messageLogRepository != null,
            "The _messageLogRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessageLogManager.AnyAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageLogManager_AnyAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessageLogRepository>();
        var logger = new Mock<ILogger<IMessageLogManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new MessageLogManager(
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
    /// This method ensures the <see cref="MessageLogManager.CountAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageLogManager_CountAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessageLogRepository>();
        var logger = new Mock<ILogger<IMessageLogManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new MessageLogManager(
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
    /// This method ensures the <see cref="MessageLogManager.CreateAsync(Models.MessageLog, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageLogManager_CreateAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessageLogRepository>();
        var logger = new Mock<ILogger<IMessageLogManager>>();

        repository.Setup(x => x.CreateAsync(
            It.IsAny<MessageLog>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new Models.MessageLog()
            {
                Message = new Message() { },
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                MessageEvent = MessageEvent.Assigned
            }).Verifiable();

        var manager = new MessageLogManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new MessageLog()
            {
                Message = new Message() { },
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                MessageEvent = MessageEvent.Assigned
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
    /// This method ensures the <see cref="MessageLogManager.DeleteAsync(Models.MessageLog, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageLogManager_DeleteAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessageLogRepository>();
        var logger = new Mock<ILogger<IMessageLogManager>>();

        repository.Setup(x => x.DeleteAsync(
            It.IsAny<MessageLog>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var manager = new MessageLogManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        await manager.DeleteAsync(
            new Models.MessageLog()
            {
                Message = new Message() { },
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                MessageEvent = MessageEvent.Assigned
            },
            "test"
            );

        // Assert ...
        repository.Verify();
    }

    #endregion
}