﻿
namespace CG.Purple.Smtp;

/// <summary>
/// This class is a SMTP implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
internal class SmtpProvider : IMessageProvider
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
    /// This constructor creates a new instance of the <see cref="SmtpProvider"/>
    /// class.
    /// </summary>
    /// <param name="logger">The logger to use with this provider.</param>
    public SmtpProvider(
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
    public virtual async Task ProcessAsync<TMessage>(
        ProviderRequest<TMessage> request,
        CancellationToken cancellationToken = default
        ) where TMessage : Message
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(request, nameof(request));

        try
        {
            // TODO : write the code for this.
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to process a request!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The provider failed to process a request!",
                innerException: ex
                );
        }
    }

    #endregion
}