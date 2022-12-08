
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="ProviderParameterManager"/> class.
/// </summary>
[TestClass]
public class ProviderParameterManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="ProviderParameterManager.ProviderParameterManager(IProviderParameterRepository, ICryptographer, ILogger{IProviderParameterManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void ProviderParameterManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IProviderParameterRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderParameterManager>>();

        // Act ...
        var manager = new ProviderParameterManager(
            repository.Object,
            cryptographer.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._providerParameterRepository != null,
            "The _providerParameterRepository field wasn't initialize!"
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
    /// This method ensures the <see cref="ProviderParameterManager.AnyAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ProviderParameterManager_AnyAsync()
    {
        // Arrange ...
        var repository = new Mock<IProviderParameterRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderParameterManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new ProviderParameterManager(
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

        repository.Verify();
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="ProviderParameterManager.CountAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ProviderParameterManager_CountAsync()
    {
        // Arrange ...
        var repository = new Mock<IProviderParameterRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderParameterManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new ProviderParameterManager(
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

        repository.Verify();
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="ProviderParameterManager.CreateAsync(ProviderParameter, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ProviderParameterManager_CreateAsync()
    {
        // Arrange ...
        var repository = new Mock<IProviderParameterRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderParameterManager>>();

        repository.Setup(x => x.CreateAsync(
            It.IsAny<ProviderParameter>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new ProviderParameter()
            {
                ParameterType = new ParameterType(),
                ProviderType = new ProviderType(),
                Value = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new ProviderParameterManager(
            repository.Object,
            cryptographer.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new ProviderParameter()
            {
                ParameterType = new ParameterType(),
                ProviderType = new ProviderType(),
                Value = "test",
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
    /// This method ensures the <see cref="ProviderParameterManager.DeleteAsync(ProviderParameter, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ProviderParameterManager_DeleteAsync()
    {
        // Arrange ...
        var repository = new Mock<IProviderParameterRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderParameterManager>>();

        repository.Setup(x => x.DeleteAsync(
            It.IsAny<ProviderParameter>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var manager = new ProviderParameterManager(
            repository.Object,
            cryptographer.Object,
            logger.Object
            );

        // Act ...
        await manager.DeleteAsync(
            new ProviderParameter()
            {
                ParameterType = new ParameterType(),
                ProviderType = new ProviderType(),
                Value = "test",
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
    /// This method ensures the <see cref="ProviderParameterManager.UpdateAsync(ProviderParameter, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ProviderParameterManager_UpdateAsync()
    {
        // Arrange ...
        var repository = new Mock<IProviderParameterRepository>();
        var cryptographer = new Mock<ICryptographer>();
        var logger = new Mock<ILogger<IProviderParameterManager>>();

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<ProviderParameter>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new ProviderParameter()
            {
                ParameterType = new ParameterType(),
                ProviderType = new ProviderType(),
                Value = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new ProviderParameterManager(
            repository.Object,
            cryptographer.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new ProviderParameter()
            {
                ParameterType = new ParameterType(),
                ProviderType = new ProviderType(),
                Value = "test",
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