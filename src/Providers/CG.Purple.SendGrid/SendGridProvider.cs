
namespace CG.Purple.SendGrid;

/// <summary>
/// This class is a SendGrid implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
internal class SendGridProvider : IMessageProvider
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
    /// This constructor creates a new instance of the <see cref="SendGridProvider"/>
    /// class.
    /// </summary>
    /// <param name="logger">The logger to use with this provider.</param>
    public SendGridProvider(
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
    public virtual async Task SendMailAsync(
        MailMessage mailMessage,
        ProviderType providerType,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mailMessage, nameof(mailMessage))
            .ThrowIfNull(providerType, nameof(providerType));

        try
        {

        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to process an email!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The provider failed to process an email!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task SendTextAsync(
        TextMessage textMessage,
        ProviderType providerType,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(textMessage, nameof(textMessage))
            .ThrowIfNull(providerType, nameof(providerType));

        try
        {

        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to process a text!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The provider failed to process a text!",
                innerException: ex
                );
        }
    }

    #endregion
}
