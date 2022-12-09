
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="AttachmentManager"/> class.
/// </summary>
[TestClass]
public class AttachmentManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="AttachmentManager.AttachmentManager(IAttachmentRepository, ILogger{IAttachmentManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void AttachmentManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IAttachmentRepository>();
        var logger = new Mock<ILogger<IAttachmentManager>>();

        // Act ...
        var manager = new AttachmentManager(
            repository.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._attachmentRepository != null,
            "The _attachmentRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="AttachmentManager.AnyAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task AttachmentManager_AnyAsync()
    {
        // Arrange ...
        var repository = new Mock<IAttachmentRepository>();
        var logger = new Mock<ILogger<IAttachmentManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new AttachmentManager(
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
    /// This method ensures the <see cref="AttachmentManager.CountAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task AttachmentManager_CountAsync()
    {
        // Arrange ...
        var repository = new Mock<IAttachmentRepository>();
        var logger = new Mock<ILogger<IAttachmentManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new AttachmentManager(
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
    /// This method ensures the <see cref="AttachmentManager.CreateAsync(Models.Attachment, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task AttachmentManager_CreateAsync()
    {
        // Arrange ...
        var repository = new Mock<IAttachmentRepository>();
        var logger = new Mock<ILogger<IAttachmentManager>>();

        repository.Setup(x => x.CreateAsync(
            It.IsAny<Models.Attachment>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new Models.Attachment()
            {
                Message = new Models.Message(),
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                Data = Array.Empty<byte>(),
                Length = 0,
                MimeType = new Models.MimeType(),
                OriginalFileName = "test.bin"
            }).Verifiable();

        var manager = new AttachmentManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new Models.Attachment()
            {
                Message = new Models.Message(),
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                Data = Array.Empty<byte>(),
                Length = 0,
                MimeType = new Models.MimeType(),
                OriginalFileName = "test.bin"
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
    /// This method ensures the <see cref="AttachmentManager.DeleteAsync(Models.Attachment, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task AttachmentManager_DeleteAsync()
    {
        // Arrange ...
        var repository = new Mock<IAttachmentRepository>();
        var logger = new Mock<ILogger<IAttachmentManager>>();

        repository.Setup(x => x.DeleteAsync(
            It.IsAny<Models.Attachment>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var manager = new AttachmentManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        await manager.DeleteAsync(
            new Models.Attachment()
            {
                Message = new Models.Message(),
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                Data = Array.Empty<byte>(),
                Length = 0,
                MimeType = new Models.MimeType(),
                OriginalFileName = "test.bin"
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
    /// This method ensures the <see cref="AttachmentManager.UpdateAsync(Models.Attachment, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task AttachmentManager_UpdateAsync()
    {
        // Arrange ...
        var repository = new Mock<IAttachmentRepository>();
        var logger = new Mock<ILogger<IAttachmentManager>>();

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<Models.Attachment>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new Models.Attachment()
            {
                Message = new Models.Message(),
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                Data = Array.Empty<byte>(),
                Length = 0,
                MimeType = new Models.MimeType(),
                OriginalFileName = "test.bin"
            }).Verifiable();

        var manager = new AttachmentManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new Models.Attachment()
            {
                Message = new Models.Message(),
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                Data = Array.Empty<byte>(),
                Length = 0,
                MimeType = new Models.MimeType(),
                OriginalFileName = "test.bin"
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