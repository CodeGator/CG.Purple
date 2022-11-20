
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

        // Log what we are about to do.
        _logger.LogDebug(
            "Converting factory type to .NET type, for the provider."
            );

        // Create the .NET type.
        var type = Type.GetType(providerType.FactoryType);

        // Did we fail?
        if (type is null)
        {
            // Log what we are about to do.
            _logger.LogError(
                "Failed to convert factory type: {ft} to .NET type, for provider type: {pt}!",
                providerType.FactoryType,
                providerType.Name
                );

            return Task.FromResult<IMessageProvider?>(null); // Nothing to create!
        }

        // Log what we are about to do.
        _logger.LogDebug(
            "Creating a provider of type: {t}.",
            type.FullName
            );

        // Create the provider.
        var provider = ActivatorUtilities.CreateInstance(
            _serviceProvider,
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
