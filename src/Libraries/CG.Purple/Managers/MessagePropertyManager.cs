
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IMessagePropertyManager"/>
/// interface.
/// </summary>
internal class MessagePropertyManager : IMessagePropertyManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IMessagePropertyRepository _messagePropertyRepository = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IMessagePropertyManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessagePropertyManager"/>
    /// class.
    /// </summary>
    /// <param name="messagePropertyRepository">The messageProperty repository to use
    /// with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public MessagePropertyManager(
        IMessagePropertyRepository messagePropertyRepository,
        ILogger<IMessagePropertyManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(messagePropertyRepository, nameof(messagePropertyRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _messagePropertyRepository = messagePropertyRepository;
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
                nameof(IMessagePropertyRepository.AnyAsync)
                );

            // Perform the search.
            return await _messagePropertyRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for messagePropertys!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for messagePropertys!",
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
                nameof(IMessagePropertyRepository.CountAsync)
                );

            // Perform the search.
            return await _messagePropertyRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count messagePropertys!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count messagePropertys!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MessageProperty> CreateAsync(
        MessageProperty messageProperty,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messageProperty, nameof(messageProperty))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MessageProperty)
                );

            // Ensure the stats are correct.
            messageProperty.CreatedOnUtc = DateTime.UtcNow;
            messageProperty.CreatedBy = userName;
            messageProperty.LastUpdatedBy = null;
            messageProperty.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessagePropertyRepository.CreateAsync)
                );

            // Perform the operation.
            return await _messagePropertyRepository.CreateAsync(
                messageProperty,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new messageProperty!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new messageProperty!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        MessageProperty messageProperty,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messageProperty, nameof(messageProperty))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MessageProperty)
                );

            // Ensure the stats are correct.
            messageProperty.LastUpdatedOnUtc = DateTime.UtcNow;
            messageProperty.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessagePropertyRepository.DeleteAsync)
                );

            // Perform the operation.
            await _messagePropertyRepository.DeleteAsync(
                messageProperty,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a messageProperty!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a messageProperty!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MessageProperty> UpdateAsync(
        MessageProperty messageProperty,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messageProperty, nameof(messageProperty))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MessageProperty)
                );

            // Ensure the stats are correct.
            messageProperty.LastUpdatedOnUtc = DateTime.UtcNow;
            messageProperty.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMessagePropertyRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _messagePropertyRepository.UpdateAsync(
                messageProperty,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a messageProperty!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a messageProperty!",
                innerException: ex
                );
        }
    }

    #endregion
}
