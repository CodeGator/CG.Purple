
namespace CG.Purple.Managers;

/// <summary>
/// This class is a test fixture for the <see cref="PropertyTypeManager"/> class.
/// </summary>
[TestClass]
public class PropertyTypeManagerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="PropertyTypeManager.PropertyTypeManager(IPropertyTypeRepository, ILogger{IPropertyTypeManager})"/>
    /// method property initializes the object.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void PropertyTypeManager_ctor()
    {
        // Arrange ...
        var repository = new Mock<IPropertyTypeRepository>();
        var logger = new Mock<ILogger<IPropertyTypeManager>>();

        // Act ...
        var manager = new PropertyTypeManager(
            repository.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            manager._propertyTypeRepository != null,
            "The _propertyTypeRepository field wasn't initialize!"
            );
        Assert.IsTrue(
            manager._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="PropertyTypeManager.AnyAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task PropertyTypeManager_AnyAsync()
    {
        // Arrange ...
        var repository = new Mock<IPropertyTypeRepository>();
        var logger = new Mock<ILogger<IPropertyTypeManager>>();

        repository.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true)
            .Verifiable();

        var manager = new PropertyTypeManager(
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
    /// This method ensures the <see cref="PropertyTypeManager.CountAsync(CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task PropertyTypeManager_CountAsync()
    {
        // Arrange ...
        var repository = new Mock<IPropertyTypeRepository>();
        var logger = new Mock<ILogger<IPropertyTypeManager>>();

        repository.Setup(x => x.CountAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(1)
            .Verifiable();

        var manager = new PropertyTypeManager(
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
    /// This method ensures the <see cref="PropertyTypeManager.CreateAsync(PropertyType, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task PropertyTypeManager_CreateAsync()
    {
        // Arrange ...
        var repository = new Mock<IPropertyTypeRepository>();
        var logger = new Mock<ILogger<IPropertyTypeManager>>();

        repository.Setup(x => x.CreateAsync(
            It.IsAny<PropertyType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new PropertyType()
            {
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new PropertyTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.CreateAsync(
            new PropertyType()
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

        repository.Verify();
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="PropertyTypeManager.DeleteAsync(PropertyType, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task PropertyTypeManager_DeleteAsync()
    {
        // Arrange ...
        var repository = new Mock<IPropertyTypeRepository>();
        var logger = new Mock<ILogger<IPropertyTypeManager>>();

        repository.Setup(x => x.DeleteAsync(
            It.IsAny<PropertyType>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var manager = new PropertyTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        await manager.DeleteAsync(
            new PropertyType()
            {
                Name = "test",
                Description = "test",
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
    /// This method ensures the <see cref="PropertyTypeManager.UpdateAsync(PropertyType, string, CancellationToken)"/>
    /// method property calls the proper repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task PropertyTypeManager_UpdateAsync()
    {
        // Arrange ...
        var repository = new Mock<IPropertyTypeRepository>();
        var logger = new Mock<ILogger<IPropertyTypeManager>>();

        repository.Setup(x => x.UpdateAsync(
            It.IsAny<PropertyType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            new PropertyType()
            {
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }).Verifiable();

        var manager = new PropertyTypeManager(
            repository.Object,
            logger.Object
            );

        // Act ...
        var result = await manager.UpdateAsync(
            new PropertyType()
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

        repository.Verify();
    }

    #endregion
}