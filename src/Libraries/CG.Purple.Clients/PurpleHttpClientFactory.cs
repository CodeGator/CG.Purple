
namespace CG.Purple.Clients;

/// <summary>
/// This class is a default implementation of the <see cref="IPurpleHttpClientFactory"/>
/// interface.
/// </summary>
internal class PurpleHttpClientFactory : IPurpleHttpClientFactory
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the service provider for this factory.
    /// </summary>
    internal protected readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// This field contains the HTTP client factory for this factory.
    /// </summary>
    internal protected readonly IHttpClientFactory _clientFactory;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PurpleHttpClientFactory"/>
    /// class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use with 
    /// this factory.</param>
    /// <param name="clientFactory">The HTTP client factory to use with 
    /// this factory.</param>
    public PurpleHttpClientFactory(
        IServiceProvider serviceProvider,
        IHttpClientFactory clientFactory
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceProvider, nameof(serviceProvider))
            .ThrowIfNull(clientFactory, nameof(clientFactory));

        // Save the reference(s).
        _serviceProvider = serviceProvider;
        _clientFactory = clientFactory;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual IPurpleHttpClient CreateClient()
    {
        // Create the HTTP client.
        var httpClient = _clientFactory.CreateClient();

        // Create the client options.
        var options = _serviceProvider.GetRequiredService<
            IOptions<PurpleClientOptions>
            >();

        // Create the Purple REST client.
        var purpleClient = new PurpleHttpClient(
            httpClient,
            options
            );

        // Return the results.
        return purpleClient;
    }

    #endregion
}
