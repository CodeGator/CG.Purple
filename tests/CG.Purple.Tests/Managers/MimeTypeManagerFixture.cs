
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="MimeTypeManager"/> class.
/// </summary>
[TestClass]
public class MimeTypeManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="MimeTypeManager.MimeTypeManager(IMimeTypeRepository, ILogger{IMimeTypeManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void MimeTypeManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IMimeTypeRepository>();
        var logger = new Mock<ILogger<IMimeTypeManager>>();

        // Act ...
        var manager = new MimeTypeManager(
            repository.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._mimeTypeRepository != null,
            "The _mimeTypeRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MimeTypeManager.AnyAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MimeTypeManager_AnyAsync()
    {
        // Arrange ...
        var repository = new Mock<IMimeTypeRepository>();
        var logger = new Mock<ILogger<IMimeTypeManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new MimeTypeManager(
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
            repository,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MimeTypeManager.CountAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MimeTypeManager_CountAsync()
    {
        // Arrange ...
        var repository = new Mock<IMimeTypeRepository>();
        var logger = new Mock<ILogger<IMimeTypeManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new MimeTypeManager(
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
             repository,
             logger
             );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MimeTypeManager.CreateAsync(MimeType, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MimeTypeManager_CreateAsync()
    {
        // Arrange ...
        var repository = new Mock<IMimeTypeRepository>();
        var logger = new Mock<ILogger<IMimeTypeManager>>();

        repository.Setup(x => x.CreateAsync(
            It.IsAny<MimeType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new MimeType()
            {
                Type = "application",
                SubType = "json",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new MimeTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new MimeType()
            {
                Type = "application",
                SubType = "json",
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

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MimeTypeManager.DeleteAsync(MimeType, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MimeTypeManager_DeleteAsync()
    {
        // Arrange ...
        var repository = new Mock<IMimeTypeRepository>();
        var logger = new Mock<ILogger<IMimeTypeManager>>();

        repository.Setup(x => x.DeleteAsync(
            It.IsAny<MimeType>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var manager = new MimeTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        await manager.DeleteAsync(
            new MimeType()
            {
                Type = "application",
                SubType = "json",
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
    /// This method ensures the <see cref="MimeTypeManager.UpdateAsync(MimeType, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MimeTypeManager_UpdateAsync()
    {
        // Arrange ...
        var repository = new Mock<IMimeTypeRepository>();
        var logger = new Mock<ILogger<IMimeTypeManager>>();

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<MimeType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new MimeType()
            {
                Type = "application",
                SubType = "json",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new MimeTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new MimeType()
            {
                Type = "application",
                SubType = "json",
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