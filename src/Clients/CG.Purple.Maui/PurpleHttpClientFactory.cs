
using CG.Purple.Maui.Options;
using Microsoft.Extensions.Options;

namespace CG.Purple.Maui;

/// <summary>
/// This class is a factory for creating <see cref="PurpleHttpClient"/>
/// instances, at runtime.
/// </summary>
public class PurpleHttpClientFactory
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

    /// <summary>
    /// This method creates and returns a <see cref="PurpleHttpClient"/>
    /// object instance.
    /// </summary>
    /// <returns></returns>
    public virtual PurpleHttpClient CreateClient()
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
