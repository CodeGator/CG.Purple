
namespace CG.Purple.Providers.SqlServer.Repositories;

/// <summary>
/// This class is a test fixture for the <see cref="TextMessageRepository"/>
/// class.
/// </summary>
[TestClass]
public class TextMessageRepositoryFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="TextMessageRepository.TextMessageRepository(Microsoft.EntityFrameworkCore.IDbContextFactory{Purple.SqlServer.PurpleDbContext}, AutoMapper.IMapper, Microsoft.Extensions.Logging.ILogger{Purple.Repositories.ITextMessageRepository})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    public void TextMessageRepository_ctor()
    {
        // Arrange ...
        var factory = new Mock<IDbContextFactory<PurpleDbContext>>();
        var mapper = new Mock<IMapper>();
        var logger = new Mock<ILogger<ITextMessageRepository>>();

        // Act ...
        var repository = new TextMessageRepository(
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

    #endregion
}
