
namespace CG.Purple.Providers.SqlServer.Repositories;

/// <summary>
/// This class is a test fixture for the <see cref="FileTypeRepository"/>
/// class.
/// </summary>
[TestClass]
public class FileTypeRepositoryFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="FileTypeRepository.FileTypeRepository(Microsoft.EntityFrameworkCore.IDbContextFactory{Purple.SqlServer.PurpleDbContext}, AutoMapper.IMapper, Microsoft.Extensions.Logging.ILogger{Purple.Repositories.IFileTypeRepository})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public void FileTypeRepository_ctor()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IFileTypeRepository>>();

        // Act ...
        var repository = new FileTypeRepository(
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
    /// This method ensures the <see cref="FileTypeRepository.AnyAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the 
    /// result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task FileTypeRepository_AnyAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IFileTypeRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.FileTypes.Add(new Purple.SqlServer.Entities.FileType()
        {
            Id = 1,
            MimeTypeId = 1,
            Extension = ".bin",            
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow            
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new FileTypeRepository(
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
    /// This method ensures the <see cref="FileTypeRepository.CountAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task FileTypeRepository_CountAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IFileTypeRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.FileTypes.Add(new Purple.SqlServer.Entities.FileType()
        {
            Id = 1,
            MimeTypeId = 1,
            Extension = ".bin",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new FileTypeRepository(
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
    /// This method ensures the <see cref="FileTypeRepository.CreateAsync(Models.FileType, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task FileTypeRepository_CreateAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IFileTypeRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.FileType>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.FileType()
            {
                Id = 1,
                MimeTypeId = 1,
                Extension = ".bin",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        mapper.Setup(x => x.Map<Models.FileType>(
            It.IsAny<object>()
            )).Returns(new Models.FileType()
            {
                Id = 1,
                MimeType = new Models.MimeType(),
                Extension = ".bin",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new FileTypeRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.CreateAsync(
            new Models.FileType()
            {
                MimeType = new Models.MimeType(),
                Extension = ".bin",
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
    /// This method ensures the <see cref="FileTypeRepository.DeleteAsync(Models.FileType, CancellationToken)"/>
    /// method properly calls the data-context method.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task FileTypeRepository_DeleteAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IFileTypeRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():N}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.FileTypes.Add(new Purple.SqlServer.Entities.FileType()
        {
            MimeTypeId = 1,
            Extension = ".bin",
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.FileType>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.FileType()
            {
                Id = 1,
                MimeTypeId = 1,
                Extension = ".bin",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow
            }).Verifiable();

        var respository = new FileTypeRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        await respository.DeleteAsync(
            new Models.FileType()
            {
                Id = 1,
                MimeType = new Models.MimeType(),
                Extension = ".bin",
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
