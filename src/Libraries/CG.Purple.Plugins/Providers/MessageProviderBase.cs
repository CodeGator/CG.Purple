
namespace CG.Purple.Plugins.Providers;

/// <summary>
/// This class is a base implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
public abstract class MessageProviderBase<T> : IMessageProvider
    where T : MessageProviderBase<T>
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the logger for this provider.
    /// </summary>
    internal protected readonly ILogger<T> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor create a new instance of the <see cref="MessageProviderBase{T}"/>
    /// class.
    /// </summary>
    /// <param name="logger">The logger to use with this provider.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    protected MessageProviderBase(
        ILogger<T> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public abstract Task ProcessMessagesAsync(
        IEnumerable<Message> messages,
        ProviderType providerType,
        CancellationToken cancellationToken = default
        );

    #endregion
}
