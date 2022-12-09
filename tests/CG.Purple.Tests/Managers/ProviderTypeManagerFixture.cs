
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="ProviderTypeManager"/> class.
/// </summary>
[TestClass]
public class ProviderTypeManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="ProviderTypeManager.ProviderTypeManager(IProviderTypeRepository, ICryptographer, ILogger{IProviderTypeManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void ProviderTypeManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IProviderTypeRepository>();
        var crypographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderTypeManager>>();

        // Act ...
        var manager = new ProviderTypeManager(
            repository.Object,
            crypographer.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._providerTypeRepository != null,
            "The _providerTypeRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._cryptographer != null,
            "The _cryptographer field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="ProviderTypeManager.AnyAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ProviderTypeManager_AnyAsync()
    {
        // Arrange ...
        var repository = new Mock<IProviderTypeRepository>();
        var crypographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderTypeManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new ProviderTypeManager(
            repository.Object,
            crypographer.Object,
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
    /// This method ensures the <see cref="ProviderTypeManager.CountAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ProviderTypeManager_CountAsync()
    {
        // Arrange ...
        var repository = new Mock<IProviderTypeRepository>();
        var crypographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderTypeManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new ProviderTypeManager(
            repository.Object,
            crypographer.Object,
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
    /// This method ensures the <see cref="ProviderTypeManager.CreateAsync(ProviderType, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ProviderTypeManager_CreateAsync()
    {
        // Arrange ...
        var repository = new Mock<IProviderTypeRepository>();
        var crypographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderTypeManager>>();

        repository.Setup(x => x.CreateAsync(
            It.IsAny<ProviderType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new ProviderType()
            {
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new ProviderTypeManager(
            repository.Object,
            crypographer.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new ProviderType()
            {
                Name = "test",
                Description = "test",
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
    /// This method ensures the <see cref="ProviderTypeManager.DeleteAsync(ProviderType, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ProviderTypeManager_DeleteAsync()
    {
        // Arrange ...
        var repository = new Mock<IProviderTypeRepository>();
        var crypographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderTypeManager>>();

        repository.Setup(x => x.DeleteAsync(
            It.IsAny<ProviderType>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var manager = new ProviderTypeManager(
            repository.Object,
            crypographer.Object,
            logger.Object
            );

        // Act ...
        await manager.DeleteAsync(
            new ProviderType()
            {
                Name = "test",
                Description = "test",
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
    /// This method ensures the <see cref="ProviderTypeManager.UpdateAsync(ProviderType, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ProviderTypeManager_UpdateAsync()
    {
        // Arrange ...
        var repository = new Mock<IProviderTypeRepository>();
        var crypographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderTypeManager>>();

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<ProviderType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new ProviderType()
            {
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new ProviderTypeManager(
            repository.Object,
            crypographer.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new ProviderType()
            {
                Name = "test",
                Description = "test",
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