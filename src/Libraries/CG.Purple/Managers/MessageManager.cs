
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IMessageManager"/>
/// interface.
/// </summary>
internal class MessageManager : IMessageManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IMessageRepository _messageRepository = null!;

    /// <summary>
    /// This field contains the cryptographer for this manager.
    /// </summary>
    internal protected readonly ICryptographer _cryptographer = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IMessageManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessageManager"/>
    /// class.
    /// </summary>
    /// <param name="messageRepository">The message repository to use
    /// with this manager.</param>
    /// <param name="cryptographer">The cryptographer to use with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public MessageManager(
        IMessageRepository messageRepository,
        ICryptographer cryptographer,
        ILogger<IMessageManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(messageRepository, nameof(messageRepository))
            .ThrowIfNull(cryptographer, nameof(cryptographer))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _messageRepository = messageRepository;
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
                nameof(IMessageRepository.AnyAsync)
                );

            // Perform the search.
            return await _messageRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for messages!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for messages!",
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
                nameof(IMessageRepository.CountAsync)
                );

            // Perform the search.
            var result = await _messageRepository.CountAsync(
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
                "Failed to count messages!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        Message message,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MailMessage)
                );

            // Ensure the stats are correct.
            message.LastUpdatedOnUtc = DateTime.UtcNow;
            message.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessageRepository.DeleteAsync)
                );

            // Perform the operation.
            await _messageRepository.DeleteAsync(
                message,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a message!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a message!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<Message>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessageRepository.FindAllAsync)
                );

            // Perform the operation.
            var result = (await _messageRepository.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false)).ToArray();

            // This result includes an associated provider, with parameters. Since
            //   provider parameters are encrypted, at rest, we'll need to decrypt
            //   the values before we send the data to the caller.
            foreach (var message in result.Where(x => x.ProviderType != null))
            {
                // Loop through the parameters.
                foreach (var parameter in message?.ProviderType?.Parameters ?? Array.Empty<ProviderParameter>())
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
                "Failed to search for messages!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for  " +
                "messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<Message?> FindByIdAsync(
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
                nameof(IMessageRepository.FindByIdAsync)
                );

            // Perform the operation.
            var result = await _messageRepository.FindByIdAsync(
                id,
                cancellationToken
                ).ConfigureAwait(false);

            // This result might include an associated provider, with parameters.
            //   Since provider parameters are encrypted, at rest, we'll need to
            //   decrypt the values before we send the data to the caller.
            if (result?.ProviderType is not null)
            {
                // Loop through the parameters.
                foreach (var parameter in result?.ProviderType?.Parameters ?? Array.Empty<ProviderParameter>())
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
                "Failed to search for a message by id!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for a  " +
                "message by id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<Message?> FindByKeyAsync(
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
                nameof(IMessageRepository.FindByKeyAsync)
                );

            // Perform the operation.
            var result = await _messageRepository.FindByKeyAsync(
                messageKey,
                cancellationToken
                ).ConfigureAwait(false);

            // This result might include an associated provider, with parameters.
            //   Since provider parameters are encrypted, at rest, we'll need to
            //   decrypt the values before we send the data to the caller.
            if (result?.ProviderType is not null)
            {
                // Loop through the parameters.
                foreach (var parameter in result?.ProviderType?.Parameters ?? Array.Empty<ProviderParameter>())
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
                "Failed to search for a message by key!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for a  " +
                "message by key!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<Message>> FindReadyToArchiveAsync(
        int maxDaysToLive,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfLessThanOrEqualZero(maxDaysToLive, nameof(maxDaysToLive));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessageRepository.FindReadyToArchiveAsync)
                );

            // Perform the operation.
            var result = (await _messageRepository.FindReadyToArchiveAsync(
                maxDaysToLive,
                cancellationToken
                ).ConfigureAwait(false)).ToArray();

            // This result includes an associated provider, with parameters. Since
            //   provider parameters are encrypted, at rest, we'll need to decrypt
            //   the values before we send the data to the caller.
            foreach (var message in result.Where(x => x.ProviderType != null))
            {
                // Loop through the parameters.
                foreach (var parameter in message?.ProviderType?.Parameters ?? Array.Empty<ProviderParameter>())
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
                "Failed to search for messages that are ready to archive!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for messages " +
                "that are ready to archive!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<Message>> FindReadyToProcessAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessageRepository.FindReadyToProcessAsync)
                );

            // Perform the operation.
            var result = (await _messageRepository.FindReadyToProcessAsync(
                cancellationToken
                ).ConfigureAwait(false)).ToArray();

            // This result includes an associated provider, with parameters. Since
            //   provider parameters are encrypted, at rest, we'll need to decrypt
            //   the values before we send the data to the caller.
            foreach (var message in result.Where(x => x.ProviderType != null))
            {
                // Loop through the parameters.
                foreach (var parameter in message?.ProviderType?.Parameters ?? Array.Empty<ProviderParameter>())
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
                "Failed to search for messages that are ready to process!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for messages " +
                "that are ready to process!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<Message>> FindReadyToRetryAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessageRepository.FindReadyToRetryAsync)
                );

            // Perform the operation.
            var result = (await _messageRepository.FindReadyToRetryAsync(
                cancellationToken
                ).ConfigureAwait(false)).ToArray();

            // This result includes an associated provider, with parameters. Since
            //   provider parameters are encrypted, at rest, we'll need to decrypt
            //   the values before we send the data to the caller.
            foreach (var message in result.Where(x => x.ProviderType != null))
            {
                // Loop through the parameters.
                foreach (var parameter in message?.ProviderType?.Parameters ?? Array.Empty<ProviderParameter>())
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
                "Failed to search for messages that are ready to retry!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for messages " +
                "that are ready to retry!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<Message> UpdateAsync(
        Message message,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(Message)
                );

            // Ensure the stats are correct.
            message.LastUpdatedOnUtc = DateTime.UtcNow;
            message.LastUpdatedBy = userName;

            // Do we have an associated provider?
            if (message.ProviderType is not null)
            {
                // Provider parameters are encrypted, at rest, so we'll need
                //   to deal with those values now.
                foreach (var parameter in message.ProviderType.Parameters)
                {
                    // Encrypt the value
                    parameter.Value = await _cryptographer.AesEncryptAsync(
                        parameter.Value,
                        cancellationToken
                        ).ConfigureAwait(false);   
                }
            }

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessageRepository.UpdateAsync)
                );

            // Perform the operation.
            var result = await _messageRepository.UpdateAsync(
                message,
                cancellationToken
                ).ConfigureAwait(false);

            // Do we have an associated provider?
            if (message.ProviderType is not null)
            {
                // Provider parameters are encrypted, at rest, so we'll need
                //   to deal with those values now.
                foreach (var parameter in message.ProviderType.Parameters)
                {
                    // Decrypt the value
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
                "Failed to update a message!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a message!",
                innerException: ex
                );
        }
    }

    #endregion
}
