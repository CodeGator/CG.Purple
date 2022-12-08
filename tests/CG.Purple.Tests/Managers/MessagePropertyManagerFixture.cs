
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="MessagePropertyManager"/> class.
/// </summary>
[TestClass]
public class MessagePropertyManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="MessagePropertyManager.MessagePropertyManager(IMessagePropertyRepository, ILogger{IMessagePropertyManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void MessagePropertyManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IMessagePropertyRepository>();
        var logger = new Mock<ILogger<IMessagePropertyManager>>();

        // Act ...
        var manager = new MessagePropertyManager(
            repository.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._messagePropertyRepository != null,
            "The _messagePropertyRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessagePropertyManager.AnyAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessagePropertyManager_AnyAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessagePropertyRepository>();
        var logger = new Mock<ILogger<IMessagePropertyManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new MessagePropertyManager(
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
    /// This method ensures the <see cref="MessagePropertyManager.CountAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessagePropertyManager_CountAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessagePropertyRepository>();
        var logger = new Mock<ILogger<IMessagePropertyManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new MessagePropertyManager(
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
    /// This method ensures the <see cref="MessagePropertyManager.CreateAsync(MessageProperty, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessagePropertyManager_CreateAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessagePropertyRepository>();
        var logger = new Mock<ILogger<IMessagePropertyManager>>();

        repository.Setup(x => x.CreateAsync(
            It.IsAny<MessageProperty>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new MessageProperty()
            {
                Message = new Message() { },
                Value = "",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new MessagePropertyManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new MessageProperty()
            {
                Message = new Message() { },
                Value = "",
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

        repository.Verify();
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessagePropertyManager.DeleteAsync(MessageProperty, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessagePropertyManager_DeleteAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessagePropertyRepository>();
        var logger = new Mock<ILogger<IMessagePropertyManager>>();

        repository.Setup(x => x.DeleteAsync(
            It.IsAny<MessageProperty>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var manager = new MessagePropertyManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        await manager.DeleteAsync(
            new MessageProperty()
            {
                Message = new Message() { },
                Value = "",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            },
            "test"
            );

        // Assert ...
        repository.Verify();
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MessagePropertyManager.UpdateAsync(MessageProperty, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MessagePropertyManager_UpdateAsync()
    {
        // Arrange ...
        var repository = new Mock<IMessagePropertyRepository>();
        var logger = new Mock<ILogger<IMessagePropertyManager>>();

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<MessageProperty>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new MessageProperty()
            {
                Message = new Message() { },
                Value = "",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new MessagePropertyManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new MessageProperty()
            {
                Message = new Message() { },
                Value = "",
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

        repository.Verify();
    }

    #endregion
}