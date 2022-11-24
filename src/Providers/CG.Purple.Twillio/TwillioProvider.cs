
namespace CG.Purple.Twillio;

/// <summary>
/// This class is a Twillio implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
internal class TwillioProvider : IMessageProvider
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the logger for this provider.
    /// </summary>
    internal protected readonly ILogger<IMessageProvider> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="TwillioProvider"/>
    /// class.
    /// </summary>
    /// <param name="logger">The logger to use with this provider.</param>
    public TwillioProvider(
        ILogger<IMessageProvider> logger
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
    public virtual async Task ProcessMessagesAsync(
        IEnumerable<Message> messages,
        ProviderType providerType,
        PropertyType providerPropertyType,
        CancellationToken cancellationToken = default
        )
    {
        // TODO : write the code for this.
    }

    #endregion
}
