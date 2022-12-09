
namespace CG.Purple.Providers.SqlServer.Repositories;

/// <summary>
/// This class is a test fixture for the <see cref="AttachmentRepository"/>
/// class.
/// </summary>
[TestClass]
public class AttachmentRepositoryFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="AttachmentRepository.AttachmentRepository(Microsoft.EntityFrameworkCore.IDbContextFactory{Purple.SqlServer.PurpleDbContext}, AutoMapper.IMapper, Microsoft.Extensions.Logging.ILogger{Purple.Repositories.IAttachmentRepository})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public void AttachmentRepository_ctor()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IAttachmentRepository>>();

        // Act ...
        var repository = new AttachmentRepository(
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
    /// This method ensures the <see cref="AttachmentRepository.AnyAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the 
    /// result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task AttachmentRepository_AnyAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();   
        var logger = new Mock<ILogger<IAttachmentRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():D}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.Attachments.Add(new Purple.SqlServer.Entities.Attachment()
        {
            Id = 1,
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow,
            Data = new byte[] { 0, 1, 2, 3 },
            Length = 4,
            MessageId = 1,
            MimeTypeId = 1,
            OriginalFileName = "test.bin"
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new AttachmentRepository(
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
    /// This method ensures the <see cref="AttachmentRepository.CountAsync(CancellationToken)"/>
    /// method properly calls the data-context methods and returns the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task AttachmentRepository_CountAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IAttachmentRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():D}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.Attachments.Add(new Purple.SqlServer.Entities.Attachment()
        {
            Id = 1,
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow,
            Data = new byte[] { 0, 1, 2, 3 },
            Length = 4,
            MessageId = 1,
            MimeTypeId = 1,
            OriginalFileName = "test.bin"
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        var respository = new AttachmentRepository(
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
    /// This method ensures the <see cref="AttachmentRepository.CreateAsync(Models.Attachment, CancellationToken)"/>
    /// method properly calls the repository methods and returns 
    /// the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task AttachmentRepository_CreateAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IAttachmentRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():D}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.Attachment>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.Attachment()
            {
                Id = 1,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                Data = new byte[] { 0, 1, 2, 3 },
                Length = 4,
                MessageId = 1,
                MimeTypeId = 1,
                OriginalFileName = "test.bin"
            }).Verifiable();

        mapper.Setup(x => x.Map<Models.Attachment>(
            It.IsAny<object>()
            )).Returns(new Models.Attachment()
            {
                Id = 1,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                Data = new byte[] { 0, 1, 2, 3 },
                Length = 4,
                Message = new Models.Message(),
                MimeType = new Models.MimeType(),
                OriginalFileName = "test.bin"
            }).Verifiable();

        var respository = new AttachmentRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        var result = await respository.CreateAsync(
            new Models.Attachment()
            {
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                Message = new Models.Message(),
                MimeType = new Models.MimeType(),
                Data = new byte[] { 0, 1, 2, 3 },
                Length = 4,
                OriginalFileName = "test.bin"
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
    /// This method ensures the <see cref="AttachmentRepository.DeleteAsync(Models.Attachment, CancellationToken)"/>
    /// method properly calls the data-context method.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task AttachmentRepository_DeleteAsync()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<IAttachmentRepository>>();

        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();
        optionsBuilder.UseInMemoryDatabase($"{Guid.NewGuid():D}");
        var dbContext = new PurpleDbContext(optionsBuilder.Options);

        dbContext.Attachments.Add(new Purple.SqlServer.Entities.Attachment()
        {
            Id = 1,
            CreatedBy = "test",
            CreatedOnUtc = DateTime.UtcNow,
            Data = new byte[] { 0, 1, 2, 3 },
            Length = 4,
            MessageId = 1,
            MimeTypeId = 1,
            OriginalFileName = "test.bin"
        });
        dbContext.SaveChanges();

        factory.Setup(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(dbContext)
            .Verifiable();

        mapper.Setup(x => x.Map<CG.Purple.SqlServer.Entities.Attachment>(
            It.IsAny<object>()
            )).Returns(new CG.Purple.SqlServer.Entities.Attachment()
            {
                Id = 1,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                Data = new byte[] { 0, 1, 2, 3 },
                Length = 4,
                MessageId = 1,
                MimeTypeId = 1,
                OriginalFileName = "test.bin"
            }).Verifiable();

        var respository = new AttachmentRepository(
            factory.Object,
            mapper.Object,
            logger.Object
            );

        // Act ...
        await respository.DeleteAsync(
            new Models.Attachment()
            {
                Id = 1,
                Message = new Models.Message(),
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
                Data = Array.Empty<byte>(),
                Length = 0,
                MimeType = new Models.MimeType(),
                OriginalFileName = "test.bin"
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
