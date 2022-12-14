
namespace CG.Purple.Clients;

/// <summary>
/// This class is a test fixture for the <see cref="PurpleHttpClient"/>
/// class.
/// </summary>
[TestClass]
public class PurpleHttpClientFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="PurpleHttpClient.PurpleHttpClient(HttpClient, Microsoft.Extensions.Options.IOptions{Options.PurpleClientOptions})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public void PurpleHttpClient_ctor()
    {
        // Arrange ...
        var httpClient = new Mock<HttpClient>();
        var options = new Mock<IOptions<PurpleClientOptions>>();

        options.SetupGet(x => x.Value)
            .Returns(new PurpleClientOptions() { DefaultBaseAddress = "https://localhost" })
            .Verifiable();

        // Act ...
        var result = new PurpleHttpClient(
            httpClient.Object,
            options.Object
            );

        // Assert ...
        Assert.IsTrue(
            result._httpClient != null,
            "The _httpClient field is invalid"
            );
        Assert.IsTrue(
            result.HttpClient != null,
            "The HttpClient property is invalid"
            );

        Mock.Verify(
            httpClient,
            options
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="PurpleHttpClient.GetMailStatusByKeyAsync(string, CancellationToken)"/>
    /// method properly calls the REST client and returns the results.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task PurpleHttpClient_GetMailStatusByKeyAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<PurpleClientOptions>>();
        var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var now = DateTime.UtcNow;
        httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(
                    new StatusResponse() 
                    { 
                        MessageKey = "123", 
                        CreatedOnUtc = now,
                        SentOnUtc = now
                    })),
            }).Verifiable();

        options.SetupGet(x => x.Value)
            .Returns(new PurpleClientOptions() { DefaultBaseAddress = "https://localhost" })
            .Verifiable();

        var httpClient = new HttpClient(
            httpMessageHandler.Object
            );

        var client = new PurpleHttpClient(
            httpClient,
            options.Object
            );

        // Act ...
        var result = await client.GetMailStatusByKeyAsync(
            "test"
            ).ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            result != null,
            "The return is invalid!"
            );
        Assert.IsTrue(
            result.MessageKey == "123",
            "The returned MessageKey is invalid!"
            );
        Assert.IsTrue(
            result.CreatedOnUtc == now,
            "The returned CreatedOnUtc is invalid!"
            );
        Assert.IsTrue(
            result.SentOnUtc == now,
            "The returned SentOnUtc is invalid!"
            );

        Mock.Verify(
            httpMessageHandler,
            options
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="PurpleHttpClient.GetTextStatusByKeyAsync(string, CancellationToken)"/>
    /// method properly calls the REST client and returns the results.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task PurpleHttpClient_GetTextStatusByKeyAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<PurpleClientOptions>>();
        var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var now = DateTime.UtcNow;
        httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(
                    new StatusResponse()
                    {
                        MessageKey = "123",
                        CreatedOnUtc = now,
                        SentOnUtc = now
                    })),
            }).Verifiable();

        options.SetupGet(x => x.Value)
            .Returns(new PurpleClientOptions() { DefaultBaseAddress = "https://localhost" })
            .Verifiable();

        var httpClient = new HttpClient(
            httpMessageHandler.Object
            );

        var client = new PurpleHttpClient(
            httpClient,
            options.Object
            );

        // Act ...
        var result = await client.GetTextStatusByKeyAsync(
            "test"
            ).ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            result != null,
            "The return is invalid!"
            );
        Assert.IsTrue(
            result.MessageKey == "123",
            "The returned MessageKey is invalid!"
            );
        Assert.IsTrue(
            result.CreatedOnUtc == now,
            "The returned CreatedOnUtc is invalid!"
            );
        Assert.IsTrue(
            result.SentOnUtc == now,
            "The returned SentOnUtc is invalid!"
            );

        Mock.Verify(
            httpMessageHandler,
            options
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="PurpleHttpClient.SendMailMessageAsync(MailStorageRequest, CancellationToken)"/>
    /// method properly calls the REST client and returns the results.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task PurpleHttpClient_SendMailMessageAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<PurpleClientOptions>>();
        var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var now = DateTime.UtcNow;
        httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(
                    new StorageResponse()
                    {
                        MessageKey = "123",
                        CreatedOnUtc = now
                    })),
            }).Verifiable();

        options.SetupGet(x => x.Value)
            .Returns(new PurpleClientOptions() { DefaultBaseAddress = "https://localhost" })
            .Verifiable();

        var httpClient = new HttpClient(
            httpMessageHandler.Object
            );

        var client = new PurpleHttpClient(
            httpClient,
            options.Object
            );

        // Act ...
        var result = await client.SendMailMessageAsync(
            new MailStorageRequest()
            ).ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            result != null,
            "The return is invalid!"
            );
        Assert.IsTrue(
            result.MessageKey == "123",
            "The returned MessageKey is invalid!"
            );
        Assert.IsTrue(
            result.CreatedOnUtc == now,
            "The returned CreatedOnUtc is invalid!"
            );

        Mock.Verify(
            httpMessageHandler,
            options
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="PurpleHttpClient.SendTextMessageAsync(TextStorageRequest, CancellationToken)"/>
    /// method properly calls the REST client and returns the results.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task PurpleHttpClient_SendTextMessageAsync()
    {
        // Arrange ...
        var options = new Mock<IOptions<PurpleClientOptions>>();
        var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var now = DateTime.UtcNow;
        httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(
                    new StorageResponse()
                    {
                        MessageKey = "123",
                        CreatedOnUtc = now
                    })),
            }).Verifiable();

        options.SetupGet(x => x.Value)
            .Returns(new PurpleClientOptions() { DefaultBaseAddress = "https://localhost" })
            .Verifiable();

        var httpClient = new HttpClient(
            httpMessageHandler.Object
            );

        var client = new PurpleHttpClient(
            httpClient,
            options.Object
            );

        // Act ...
        var result = await client.SendTextMessageAsync(
            new TextStorageRequest()
            ).ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            result != null,
            "The return is invalid!"
            );
        Assert.IsTrue(
            result.MessageKey == "123",
            "The returned MessageKey is invalid!"
            );
        Assert.IsTrue(
            result.CreatedOnUtc == now,
            "The returned CreatedOnUtc is invalid!"
            );

        Mock.Verify(
            httpMessageHandler,
            options
            );
    }

    #endregion
}
