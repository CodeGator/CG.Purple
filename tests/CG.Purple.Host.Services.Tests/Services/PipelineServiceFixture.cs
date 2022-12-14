
using Castle.Core.Logging;
using CG.Purple.Host.Services.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;

namespace CG.Purple.Host.Services;

/// <summary>
/// This class is a test fixture for the <see cref="PipelineService"/>
/// class.
/// </summary>
[TestClass]
public class PipelineServiceFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="PipelineService.PipelineService(Microsoft.Extensions.Options.IOptions{Options.HostedServiceOptions}, IServiceProvider, Microsoft.Extensions.Logging.ILogger{PipelineService})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public void PipelineService_ctor()
    {
        // Arrange ...
        var options = new Mock<IOptions<HostedServiceOptions>>();
        var serviceProvider = new Mock<IServiceProvider>();
        var logger = new Mock<ILogger<PipelineService>>();

        options.SetupGet(x => x.Value)
            .Returns(new HostedServiceOptions() { PipelineService = new PipelineServiceOptions() })
            .Verifiable();

        // Act ...
        var result = new PipelineService(
            options.Object,
            serviceProvider.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            result._pipelineServiceOptions != null,
            "The _pipelineServiceOptions field is invalid"
            );
        Assert.IsTrue(
            result._serviceProvider != null,
            "The _serviceProvider field is invalid"
            );
        Assert.IsTrue(
            result._logger != null,
            "The _logger field is invalid"
            );

        Mock.Verify(
            options,
            serviceProvider,
            options
            );
    }

    #endregion
}
