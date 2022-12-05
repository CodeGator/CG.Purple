
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
    /// This field contains the options for this manager.
    /// </summary>
    internal protected readonly TextMessageManagerOptions? _managerOptions;

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly ITextMessageRepository _textMessageRepository = null!;

    /// <summary>
    /// This field contains the cryptographer for this manager.
    /// </summary>
    internal protected readonly ICryptographer _cryptographer = null!;

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
    /// <param name="bllOptions">The business logic layer options to use 
    /// with this manager.</param>
    /// <param name="textMessageRepository">The text message repository to use
    /// with this manager.</param>
    /// <param name="cryptographer">The cryptographer to use with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public TextMessageManager(
        IOptions<BllOptions> bllOptions,
        ITextMessageRepository textMessageRepository,
        ICryptographer cryptographer,
        ILogger<ITextMessageManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(bllOptions, nameof(bllOptions))
            .ThrowIfNull(textMessageRepository, nameof(textMessageRepository))
            .ThrowIfNull(cryptographer, nameof(cryptographer))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _managerOptions = bllOptions.Value.TextMessageManager;
        _textMessageRepository = textMessageRepository;
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

            // Do we have an associated provider?
            if (textMessage.ProviderType is not null)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Encrypting the provider parameter values for message: {id}",
                    textMessage.Id
                    );

                // Provider parameters are encrypted, at rest, so we'll need
                //   to deal with those values now.
                foreach (var parameter in textMessage.ProviderType.Parameters)
                {
                    // Encrypt the value.
                    parameter.Value = await _cryptographer.AesEncryptAsync(
                        parameter.Value,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }

            // Were manager options provided?
            if (_managerOptions is not null)
            {
                // Was a default processing delay specified?
                if (_managerOptions.DefaultProcessDelay.HasValue)
                {
                    // Should we generate a default processing delay?
                    if (textMessage.ProcessAfterUtc is null)
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Setting a processing delay of {ts} for message: {id}",
                            textMessage.ProcessAfterUtc,
                            textMessage.Id
                            );

                        // Generate a default processing delay for the message.
                        textMessage.ProcessAfterUtc = DateTime.UtcNow +
                            _managerOptions.DefaultProcessDelay;
                    }
                }

                // Was a default archive delay specified?
                if (_managerOptions.DefaultArchiveDelay.HasValue)
                {
                    // Should we generate a default archive delay?
                    if (textMessage.ArchiveAfterUtc is null)
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Setting an archive delay of {ts} for message: {id}",
                            textMessage.ProcessAfterUtc,
                            textMessage.Id
                            );

                        // Generate a default archive delay for the message.
                        textMessage.ArchiveAfterUtc = DateTime.UtcNow +
                            _managerOptions.DefaultArchiveDelay;
                    }
                }
                else
                {
                    // Should we generate a default archive delay?
                    if (textMessage.ArchiveAfterUtc is null)
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Setting an archive delay of {ts} for message: {id}",
                            textMessage.ProcessAfterUtc,
                            textMessage.Id
                            );

                        // Generate a default archive delay for the message.
                        textMessage.ArchiveAfterUtc = DateTime.UtcNow.AddDays(7);
                    }
                }
            }

            // Sanity the processing delay.
            if (textMessage.ProcessAfterUtc > DateTime.UtcNow.AddDays(30))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Processing delay > 30 days. Limiting to 30 days for message: {id}",
                    textMessage.Id
                    );

                // Generate a default processing delay for the message.
                textMessage.ProcessAfterUtc = DateTime.UtcNow.AddDays(7);
            }

            // Sanity the archive delay.
            if (textMessage.ArchiveAfterUtc < DateTime.UtcNow.AddDays(7))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Archive delay < 7 days. Limiting to 7 days for message: {id}",
                    textMessage.Id
                    );

                // Generate a default archive delay for the message.
                textMessage.ArchiveAfterUtc = DateTime.UtcNow.AddDays(7);
            }

            // Sanity the archive delay.
            if (textMessage.ArchiveAfterUtc > DateTime.UtcNow.AddDays(365))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Archive delay > 365 days. Limiting to 365 days for message: {id}",
                    textMessage.Id
                    );

                // Generate a default archive delay for the message.
                textMessage.ArchiveAfterUtc = DateTime.UtcNow.AddDays(365);
            }

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(ITextMessageRepository.CreateAsync)
                );

            // Perform the operation.
            var result = await _textMessageRepository.CreateAsync(
                textMessage,
                cancellationToken
                ).ConfigureAwait(false);

            // Do we have an associated provider?
            if (textMessage.ProviderType is not null)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Decrypting the provider parameter values for message: {id}",
                    textMessage.Id
                    );

                // Provider parameters are encrypted, at rest, so we'll need
                //   to deal with those values now.
                foreach (var parameter in textMessage.ProviderType.Parameters)
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

            // Do we have an associated provider?
            if (textMessage.ProviderType is not null)
            {
                // Provider parameters are encrypted, at rest, so we'll need
                //   to deal with those values now.
                foreach (var parameter in textMessage.ProviderType.Parameters)
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Encrypting the provider parameter values for message: {id}",
                        textMessage.Id
                        );

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
                nameof(ITextMessageRepository.UpdateAsync)
                );

            // Perform the operation.
            var result = await _textMessageRepository.UpdateAsync(
                textMessage,
                cancellationToken
                ).ConfigureAwait(false);

            // Do we have an associated provider?
            if (textMessage.ProviderType is not null)
            {
                // Provider parameters are encrypted, at rest, so we'll need
                //   to deal with those values now.
                foreach (var parameter in textMessage.ProviderType.Parameters)
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Decrypting the provider parameter values for message: {id}",
                        textMessage.Id
                        );

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
