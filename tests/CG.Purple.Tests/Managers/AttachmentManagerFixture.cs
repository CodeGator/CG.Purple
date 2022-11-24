
using CG.Purple.Repositories;
using Microsoft.Extensions.Caching.Distributed;

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
    /// This method ensures the <see cref="AttachmentManager.AttachmentManager(Repositories.IAttachmentRepository, Microsoft.Extensions.Caching.Distributed.IDistributedCache, Microsoft.Extensions.Logging.ILogger{IAttachmentManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void AttachmentManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IAttachmentRepository>();
        var cache = new Mock<IDistributedCache>();
        var logger = new Mock<ILogger<IAttachmentManager>>();

        // Act ...
        var manager = new AttachmentManager(
            repository.Object,
            cache.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._attachmentRepository != null,
            "The _attachmentRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._distributedCache != null,
            "The _distributedCache field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    #endregion
}