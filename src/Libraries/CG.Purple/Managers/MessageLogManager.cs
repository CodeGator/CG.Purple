
using CG.Cryptography;

namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IMessageLogManager"/>
/// interface.
/// </summary>
internal class MessageLogManager : IMessageLogManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IMessageLogRepository _messageLogRepository = null!;

    /// <summary>
    /// This field contains the cryptographer for this manager.
    /// </summary>
    internal protected readonly ICryptographer _cryptographer = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IMessageLogManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessageLogManager"/>
    /// class.
    /// </summary>
    /// <param name="messageLogRepository">The message log repository to 
    /// use with this manager.</param>
    /// <param name="cryptographer">The cryptographer to use with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public MessageLogManager(
        IMessageLogRepository messageLogRepository,
        ICryptographer cryptographer,
        ILogger<IMessageLogManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(messageLogRepository, nameof(messageLogRepository))
        .ThrowIfNull(cryptographer, nameof(cryptographer))
        .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _messageLogRepository = messageLogRepository;
        _cryptographer = cryptographer;
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
                nameof(IMessageLogRepository.AnyAsync)
                );

            // Perform the search.
            return await _messageLogRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for message logs!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for message logs!",
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
                nameof(IMessageLogRepository.CountAsync)
                );

            // Perform the search.
            return await _messageLogRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count message logs!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count message logs!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MessageLog> CreateAsync(
        MessageLog messageLog,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messageLog, nameof(messageLog))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MessageLog)
                );

            // Ensure the stats are correct.
            messageLog.CreatedOnUtc = DateTime.UtcNow;
            messageLog.CreatedBy = userName;
            messageLog.LastUpdatedBy = null;
            messageLog.LastUpdatedOnUtc = null;

            // Do we have an associated provider?
            if (messageLog.ProviderType is not null)
            {
                // Provider parameters are encrypted, at rest, so we'll need
                //   to deal with those values now.
                foreach (var parameter in messageLog.ProviderType.Parameters)
                {
                    // Encrypt the value.
                    parameter.Value = await _cryptographer.AesEncryptAsync(
                        parameter.Value,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessageLogRepository.CreateAsync)
                );

            // Perform the operation.
            var result = await _messageLogRepository.CreateAsync(
                messageLog,
                cancellationToken
                ).ConfigureAwait(false);

            // Do we have an associated provider?
            if (messageLog.ProviderType is not null)
            {
                // Provider parameters are encrypted, at rest, so we'll need
                //   to deal with those values now.
                foreach (var parameter in messageLog.ProviderType.Parameters)
                {
                    // Decrypt the value.
                    parameter.Value = await _cryptographer.AesDecryptAsync(
                        parameter.Value,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new message log!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new message log!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<MessageLog>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessageLogRepository.FindAllAsync)
                );

            // Perform the operation.
            var result = await _messageLogRepository.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for message logs!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for  " +
                "message logs!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<MessageLog>> FindByMessageAsync(
        Message message,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessageLogRepository.FindByMessageAsync)
                );

            // Perform the operation.
            var result = await _messageLogRepository.FindByMessageAsync(
                message,
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for message logs for a message!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for  " +
                "message logs for a message!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        MessageLog messageLog,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messageLog, nameof(messageLog))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MessageLog)
                );

            // Ensure the stats are correct.
            messageLog.LastUpdatedOnUtc = DateTime.UtcNow;
            messageLog.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessageLogRepository.DeleteAsync)
                );

            // Perform the operation.
            await _messageLogRepository.DeleteAsync(
                messageLog,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a message log!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a message log!",
                innerException: ex
                );
        }
    }

    #endregion
}
