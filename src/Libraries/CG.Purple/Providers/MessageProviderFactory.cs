
namespace CG.Purple.Providers;

/// <summary>
/// This class is a default implementation of the <see cref="IMessageProviderFactory"/>
/// interface.
/// </summary>
internal class MessageProviderFactory : IMessageProviderFactory
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the service provider for this factory.
    /// </summary>
    internal protected readonly IServiceProvider _serviceProvider = null!;

    /// <summary>
    /// This field contains the logger for this factory.
    /// </summary>
    internal protected readonly ILogger<IMessageProviderFactory> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessageProviderFactory"/>
    /// class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use with
    /// this factory.</param>
    /// <param name="logger">The logger to use with this factory.</param>
    public MessageProviderFactory(
        IServiceProvider serviceProvider,
        ILogger<IMessageProviderFactory> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceProvider, nameof(serviceProvider))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public Task<IMessageProvider?> CreateAsync(
        ProviderType providerType, 
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(providerType, nameof(providerType));

        // The concrete providers are loaded via plugins, which means
        //   finding their .NET types can be tricky. So, instead of
        //   calling Type.GetType, which fails for dynamically loaded
        //   types (plugins), we'll look in the app-domain itself for
        //   the type(s) we want. The app-domain knows about these types
        //   because they were loaded, previously, by the plugin(s).

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for the loaded provider type: '{name}' in the app-domain",
            providerType.FactoryType
            );

        // Look for a matching concrete provider.
        var type = AppDomain.CurrentDomain.FindConcreteTypes<IMessageProvider>()
            .FirstOrDefault(x => x.Name == providerType.FactoryType);

        // Did we fail?
        if (type is null)
        {
            // Log what we are about to do.
            _logger.LogError(
                "Failed to find provider type: {name} in the app-domain!",
                providerType.FactoryType
                );
            return Task.FromResult<IMessageProvider?>(null); // Nothing to create!
        }

        // Log what we are about to do.
        _logger.LogDebug(
            "Creating a DI scope"
            );

        // Create a DI scope.
        using var scope = _serviceProvider.CreateScope();

        // Log what we are about to do.
        _logger.LogDebug(
            "Creating a provider of type: {t}.",
            type.FullName
            );

        // The providers are all registered with the DI container,
        //   so this call should stand up this instance without a
        //   problem. Creating the providers this way also allows
        //   us to inject any supporting types via the DI container,
        //   and allows the DI container to control the lifetime of
        //   each provider instance.

        // Create the provider (with scope).
        var provider = ActivatorUtilities.CreateInstance(
            scope.ServiceProvider,
            type
            ) as IMessageProvider;

        // Log what we are about to do.
        _logger.LogDebug(
            "Returning a message provider"
            );

        // Return the results.
        return Task.FromResult(provider);
    }

    #endregion
}
