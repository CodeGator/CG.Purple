
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="ParameterTypeManager"/> class.
/// </summary>
[TestClass]
public class ParameterTypeManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="ParameterTypeManager.ParameterTypeManager(IParameterTypeRepository, ILogger{IParameterTypeManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void ParameterTypeManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IParameterTypeRepository>();
        var logger = new Mock<ILogger<IParameterTypeManager>>();

        // Act ...
        var manager = new ParameterTypeManager(
            repository.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._parameterTypeRepository != null,
            "The _parameterTypeRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="ParameterTypeManager.AnyAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ParameterTypeManager_AnyAsync()
    {
        // Arrange ...
        var repository = new Mock<IParameterTypeRepository>();
        var logger = new Mock<ILogger<IParameterTypeManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new ParameterTypeManager(
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
    /// This method ensures the <see cref="ParameterTypeManager.CountAsync(CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ParameterTypeManager_CountAsync()
    {
        // Arrange ...
        var repository = new Mock<IParameterTypeRepository>();
        var logger = new Mock<ILogger<IParameterTypeManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new ParameterTypeManager(
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
    /// This method ensures the <see cref="ParameterTypeManager.CreateAsync(ParameterType, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ParameterTypeManager_CreateAsync()
    {
        // Arrange ...
        var repository = new Mock<IParameterTypeRepository>();
        var logger = new Mock<ILogger<IParameterTypeManager>>();

        repository.Setup(x => x.CreateAsync(
            It.IsAny<ParameterType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new ParameterType()
            {
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new ParameterTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new ParameterType()
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
    /// This method ensures the <see cref="ParameterTypeManager.DeleteAsync(ParameterType, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ParameterTypeManager_DeleteAsync()
    {
        // Arrange ...
        var repository = new Mock<IParameterTypeRepository>();
        var logger = new Mock<ILogger<IParameterTypeManager>>();

        repository.Setup(x => x.DeleteAsync(
            It.IsAny<ParameterType>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var manager = new ParameterTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        await manager.DeleteAsync(
            new ParameterType()
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
    /// This method ensures the <see cref="ParameterTypeManager.UpdateAsync(ParameterType, string, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ParameterTypeManager_UpdateAsync()
    {
        // Arrange ...
        var repository = new Mock<IParameterTypeRepository>();
        var logger = new Mock<ILogger<IParameterTypeManager>>();

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<ParameterType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new ParameterType()
            {
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new ParameterTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new ParameterType()
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