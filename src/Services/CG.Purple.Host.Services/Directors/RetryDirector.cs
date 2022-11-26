
namespace CG.Purple.Host.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IRetryDirector"/>
/// </summary>
internal class RetryDirector : IRetryDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the message manager for this director.
    /// </summary>
    internal protected readonly IMessageManager _messageManager = null!;

    /// <summary>
    /// This field contains the process log manager for this director.
    /// </summary>
    internal protected readonly IProcessLogManager _processLogManager = null!;

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<IRetryDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="RetryDirector"/>
    /// class.
    /// </summary>
    /// <param name="messageManager">The message manager to use with this 
    /// director.</param>
    /// <param name="processLogManager">The process log manager to use
    /// with this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public RetryDirector(
        IMessageManager messageManager,
        IProcessLogManager processLogManager,
        ILogger<IRetryDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _messageManager = messageManager;
        _processLogManager = processLogManager;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task RetryMessagesAsync(
        int maxErrorCount,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfLessThanOrEqualZero(maxErrorCount, nameof(maxErrorCount));

        try
        {
            // =======
            // Step 1: Find messages ready to retry (if any).
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for messages that are ready to retry."
                );

            // Get any messages that are ready to retry.
            var messages = await _messageManager.FindReadyToRetryAsync(
                maxErrorCount,
                cancellationToken
                ).ConfigureAwait(false);

            // Are we done?
            if (!messages.Any())
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "No messages were ready to retry."
                    );
                return; // Done!
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Retrying {count} failed messages.",
                messages.Count()
                );

            // Loop and retry these messages.
            foreach (var message in messages)
            {
                // Note: we are setting the state to 'pending' 
                //   here, in case the pipeline needs to assign
                //   a provider type to the message, for processing.

                // Log what we are about to do.
                _logger.LogInformation(
                    "Retrying message: {id}",
                    message.Id
                    );

                // Reset the state of this message.
                await message.ToPendingStateAsync(
                    _messageManager,
                    _processLogManager,
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to retry one or more messages!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to retry one or more messages!",
                innerException: ex
                );
        }
    }

    #endregion
}
