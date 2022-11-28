
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="ITextMessageManager"/>
/// interface.
/// </summary>
internal class TextMessageManager : ITextMessageManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly ITextMessageRepository _textMessageRepository = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<ITextMessageManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="TextMessageManager"/>
    /// class.
    /// </summary>
    /// <param name="textMessageRepository">The text message repository to use
    /// with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public TextMessageManager(
        ITextMessageRepository textMessageRepository,
        ILogger<ITextMessageManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(textMessageRepository, nameof(textMessageRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _textMessageRepository = textMessageRepository;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ITextMessageRepository.AnyAsync)
                );

            // Perform the search.
            return await _textMessageRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for text messages!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for text messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<long> CountAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ITextMessageRepository.CountAsync)
                );

            // Perform the search.
            return await _textMessageRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count text messages!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count text messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<TextMessage> CreateAsync(
        TextMessage textMessage,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(textMessage, nameof(textMessage))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(TextMessage)
                );

            // Ensure the stats are correct.
            textMessage.CreatedOnUtc = DateTime.UtcNow;
            textMessage.CreatedBy = userName;
            textMessage.LastUpdatedBy = null;
            textMessage.LastUpdatedOnUtc = null;

            // Nothing else makes any sense for this type.
            textMessage.MessageType = MessageType.Text;

            // Always create messages in this state.
            textMessage.MessageState = MessageState.Pending;

            // Should we generate a message key?
            if (string.IsNullOrEmpty(textMessage.MessageKey))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Generating a unique key for the message."
                    );

                // Generate the unique message key.
                textMessage.MessageKey = $"{Guid.NewGuid()}";
            }

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ITextMessageRepository.CreateAsync)
                );

            // Perform the operation.
            return await _textMessageRepository.CreateAsync(
                textMessage,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new text message!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new text message!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TextMessage>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ITextMessageRepository.FindAllAsync)
                );

            // Perform the operation.
            return await _textMessageRepository.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for text messages!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for text " +
                "messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<TextMessage?> FindByIdAsync(
        long id,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfZero(id, nameof(id));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ITextMessageRepository.FindByIdAsync)
                );

            // Perform the operation.
            return await _textMessageRepository.FindByIdAsync(
                id,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a text message by id!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for a text " +
                "message by id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<TextMessage?> FindByKeyAsync(
        string messageKey,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(messageKey, nameof(messageKey));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ITextMessageRepository.FindByKeyAsync)
                );

            // Perform the operation.
            return await _textMessageRepository.FindByKeyAsync(
                messageKey,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a text message by key!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for a text " +
                "message by key!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<TextMessage> UpdateAsync(
        TextMessage textMessage,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(textMessage, nameof(textMessage))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(TextMessage)
                );

            // Ensure the stats are correct.
            textMessage.LastUpdatedOnUtc = DateTime.UtcNow;
            textMessage.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ITextMessageRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _textMessageRepository.UpdateAsync(
                textMessage,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a text message!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a text message!",
                innerException: ex
                );
        }
    }

    #endregion
}
