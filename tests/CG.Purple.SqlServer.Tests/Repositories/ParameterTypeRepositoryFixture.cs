
namespace CG.Purple.Providers.SqlServer.Repositories;

/// <summary>
/// This class is a test fixture for the <see cref="ParameterTypeRepository"/>
/// class.
/// </summary>
[TestClass]
public class ParameterTypeRepositoryFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="ParameterTypeRepository.ParameterTypeRepository(Microsoft.EntityFrameworkCore.IDbContextFactory{Purple.SqlServer.PurpleDbContext}, AutoMapper.IMapper, Microsoft.Extensions.Logging.ILogger{Purple.Repositories.IParameterTypeRepository})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public void ParameterTypeRepository_ctor()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IParameterTypeRepository>>();

        // Act ...
        var repository = new ParameterTypeRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            repository._dbContextFactory != null,
            "The _dbContextFactory field wasn't initialize!"
            );
        Assert.IsTrue(
            repository._mapper != null,
            "The _mapper field wasn't initialize!"
            );
        Assert.IsTrue(
            repository._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="ParameterTypeRepository.AnyAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the 
    /// result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ParameterTypeRepository_AnyAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IParameterTypeRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.ParameterTypes.Add(new Purple.SqlServer.Entities.ParameterType()
        {
            Id = 1,
            Name = "test",
            Description = "test",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new ParameterTypeRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.AnyAsync()
            .ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            result,
            "The return value was invalid!"
            );

        Mock.Verify(
            factory,
            mapper,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="ParameterTypeRepository.CountAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ParameterTypeRepository_CountAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IParameterTypeRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.ParameterTypes.Add(new Purple.SqlServer.Entities.ParameterType()
        {
            Id = 1,
            Name = "test",
            Description = "test",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new ParameterTypeRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.CountAsync()
            .ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            result == 1,
            "The return value was invalid!"
            );

        Mock.Verify(
            factory,
            mapper,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="ParameterTypeRepository.CreateAsync(Models.ParameterType, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ParameterTypeRepository_CreateAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IParameterTypeRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.ParameterType>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.ParameterType()
            {
                Id = 1,
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        mapper.Setup(x => x.Map<Models.ParameterType>(
            It.IsAny<object>()
            )).Returns(new Models.ParameterType()
            {
                Id = 1,
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new ParameterTypeRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.CreateAsync(
            new Models.ParameterType()
            {
                Id = 1,
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            result is not null,
            "The return value was invalid!"
            );

        Mock.Verify(
            factory,
            mapper,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="ParameterTypeRepository.DeleteAsync(Models.ParameterType, CancellationToken)"/>
    /// method properly calls the data-context method.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task ParameterTypeRepository_DeleteAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IParameterTypeRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.ParameterTypes.Add(new Purple.SqlServer.Entities.ParameterType()
        {
            Id = 1,
            Name = "test",
            Description = "test",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.ParameterType>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.ParameterType()
            {
                Id = 1,
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new ParameterTypeRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        await respository.DeleteAsync(
            new Models.ParameterType()
            {
                Id = 1,
                Name = "test",
                Description = "test",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            });

        // Assert ...
        Mock.Verify(
            factory,
            mapper,
            logger
            );
    }

    #endregion
}
