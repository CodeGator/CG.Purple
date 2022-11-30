
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="FileTypeManager"/> class.
/// </summary>
[TestClass]
public class FileTypeManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="FileTypeManager.FileTypeManager(IFileTypeRepository, ILogger{IFileTypeManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void FileTypeManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IFileTypeRepository>();
        var logger = new Mock<ILogger<IFileTypeManager>>();

        // Act ...
        var manager = new FileTypeManager(
            repository.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._fileTypeRepository != null,
            "The _fileTypeRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="FileTypeManager.AnyAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task FileTypeManager_AnyAsync()
    {
        // Arrange ...
        var repository = new Mock<IFileTypeRepository>();
        var logger = new Mock<ILogger<IFileTypeManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new FileTypeManager(
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
    /// This method ensures the <see cref="FileTypeManager.CountAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task FileTypeManager_CountAsync()
    {
        // Arrange ...
        var repository = new Mock<IFileTypeRepository>();
        var logger = new Mock<ILogger<IFileTypeManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new FileTypeManager(
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
    /// This method ensures the <see cref="FileTypeManager.CreateAsync(Models.FileType, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task FileTypeManager_CreateAsync()
    {
        // Arrange ...
        var repository = new Mock<IFileTypeRepository>();
        var logger = new Mock<ILogger<IFileTypeManager>>();

        repository.Setup(x => x.CreateAsync(
            It.IsAny<Models.FileType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new Models.FileType()
            {
                Extension = ".bin",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var manager = new FileTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new Models.FileType()
            {
                Extension = ".bin",
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
    /// This method ensures the <see cref="FileTypeManager.DeleteAsync(Models.FileType, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task FileTypeManager_DeleteAsync()
    {
        // Arrange ...
        var repository = new Mock<IFileTypeRepository>();
        var logger = new Mock<ILogger<IFileTypeManager>>();

        repository.Setup(x => x.DeleteAsync(
            It.IsAny<Models.FileType>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var manager = new FileTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        await manager.DeleteAsync(
            new Models.FileType()
            {
                Extension = ".bin",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            },
            "test"
            );

        // Assert ...
        repository.Verify();
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="FileTypeManager.UpdateAsync(Models.FileType, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task FileTypeManager_UpdateAsync()
    {
        // Arrange ...
        var repository = new Mock<IFileTypeRepository>();
        var logger = new Mock<ILogger<IFileTypeManager>>();

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<Models.FileType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new Models.FileType()
            {
                Extension = ".bin",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var manager = new FileTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new Models.FileType()
            {
                Extension = ".bin",
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