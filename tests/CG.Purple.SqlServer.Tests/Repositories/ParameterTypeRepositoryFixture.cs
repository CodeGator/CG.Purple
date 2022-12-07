
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

    #endregion
}
