
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="MessageManager"/> class.
/// </summary>
[TestClass]
public class MessageManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="MessageManager.MessageManager(IMessageRepository, ICryptographer, ILogger{IMessageManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void MessageManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IMessageRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IMessageManager>>();

        // Act ...
        var manager = new MessageManager(
            repository.Object,
            cryptographer.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._messageRepository != null,
            "The _messageRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessageManager.AnyAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageManager_AnyAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessageRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IMessageManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new MessageManager(
            repository.Object,
            cryptographer.Object,
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
            repository,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessageManager.CountAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageManager_CountAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessageRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IMessageManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new MessageManager(
            repository.Object,
            cryptographer.Object,
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
            repository,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessageManager.DeleteAsync(Message, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageManager_DeleteAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessageRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IMessageManager>>();

        repository.Setup(x => x.DeleteAsync(
            It.IsAny<Message>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var manager = new MessageManager(
            repository.Object,
            cryptographer.Object,
            logger.Object
            );

        // Act ...
        await manager.DeleteAsync(
            new Message()
            {
                From = "test1@codegator.com",
                MessageKey = $"{Guid.NewGuid():D}",
                MessageType = MessageType.Mail,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            },
            "test"
            );

        // Assert ...
        Mock.Verify(
            repository,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessageManager.UpdateAsync(Message, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessageManager_UpdateAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessageRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IMessageManager>>();

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<Message>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new Message()
            {
                From = "test1@codegator.com",
                MessageKey = $"{Guid.NewGuid():D}",
                MessageType = MessageType.Mail,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new MessageManager(
            repository.Object,
            cryptographer.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new Message()
            {
                From = "test1@codegator.com",
                MessageKey = $"{Guid.NewGuid():D}",
                MessageType = MessageType.Mail,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            },
            "test"
            );

        // Assert ...
        Assert.IsTrue(
            result is not null,
            "The return value was invalid!"
            );

        Mock.Verify(
            repository,
            logger
            );
    }

    #endregion
}