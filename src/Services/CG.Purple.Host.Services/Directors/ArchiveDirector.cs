
namespace CG.Purple.Host.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IArchiveDirector"/>
/// </summary>
internal class ArchiveDirector : IArchiveDirector
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
    internal protected readonly ILogger<IArchiveDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ArchiveDirector"/>
    /// class.
    /// </summary>
    /// <param name="messageManager">The message manager to use with this 
    /// director.</param>
    /// <param name="processLogManager">The process log manager to use
    /// with this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public ArchiveDirector(
        IMessageManager messageManager,
        IProcessLogManager processLogManager,
        ILogger<IArchiveDirector> logger
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
    public virtual async Task ArchiveMessagesAsync(
        int maxDaysToLive,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfLessThanOrEqualZero(maxDaysToLive, nameof(maxDaysToLive))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for messages that are ready to archive."
                );

            // Look for messages ready to archive.
            var messages = await _messageManager.FindReadyToArchiveAsync(
                maxDaysToLive,
                cancellationToken
                );

            // Log what we are about to do.
            _logger.LogDebug(
                "Looping through {count} messages.",
                messages.Count()
                );

            // Loop through the messages.
            foreach (var message in messages )
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Removing message: {id}.",
                    message.Id
                    );

                // Remove the message (along with associated rows).
                await _messageManager.DeleteAsync(
                    message,
                    userName,
                    cancellationToken
                    );
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to archive one or more messages!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to archive one or more messages!",
                innerException: ex
                );
        }
    }

    #endregion
}
