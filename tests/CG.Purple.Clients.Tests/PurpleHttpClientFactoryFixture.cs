
namespace CG.Purple.Clients;

/// <summary>
/// This class is a test fixture for the <see cref="PurpleHttpClientFactory"/>
/// class.
/// </summary>
[TestClass]
public class PurpleHttpClientFactoryFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="PurpleHttpClientFactory.PurpleHttpClientFactory(IServiceProvider, IHttpClientFactory)"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public void PurpleHttpClientFactory_ctor()
    {
        // Arrange ...
        var serviceProvider = new Mock<IServiceProvider>();
        var httpClientFactory = new Mock<IHttpClientFactory>();

        // Act ...
        var result = new PurpleHttpClientFactory(
            serviceProvider.Object,
            httpClientFactory.Object
            );

        // Assert ...
        Assert.IsTrue(
            result._serviceProvider != null,
            "The _serviceProvider field is invalid"
            );
        Assert.IsTrue(
            result._clientFactory != null,
            "The _clientFactory field is invalid"
            );

        Mock.Verify(
            serviceProvider,
            httpClientFactory
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="PurpleHttpClientFactory.CreateClient"/>
    /// method properly calls the service provider and the HTTP client, and
    /// returns the results.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public void PurpleHttpClientFactory_CreateClient()
    {
        // Arrange ...
        var serviceProvider = new Mock<IServiceProvider>();
        var httpClientFactory = new Mock<IHttpClientFactory>();

        serviceProvider.Setup(x => x.GetService(typeof(IOptions<PurpleClientOptions>)))
            .Returns(new OptionsWrapper<PurpleClientOptions>(new PurpleClientOptions()))
            .Verifiable();

        httpClientFactory.Setup(x => x.CreateClient(""))
            .Returns(new HttpClient())
            .Verifiable();

        var factory = new PurpleHttpClientFactory(
            serviceProvider.Object,
            httpClientFactory.Object
            );

        // Act ...
        var result = factory.CreateClient();

        // Assert ...
        Assert.IsTrue(
            result != null,
            "The return is invalid"
            );
        Assert.IsTrue(
            result.HttpClient != null,
            "The HttpClient property is invalid"
            );

        Mock.Verify(
            serviceProvider,
            httpClientFactory
            );
    }

    #endregion
}
