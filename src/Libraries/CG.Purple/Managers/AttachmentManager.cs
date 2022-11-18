
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IAttachmentManager"/>
/// interface.
/// </summary>
internal class AttachmentManager : IAttachmentManager
{
    // *******************************************************************
    // Constants.
    // *******************************************************************

    #region Constants

    /// <summary>
    /// This constants contains the cache key for this manager.
    /// </summary>
    internal protected const string CACHE_KEY = "AttachmentManager";

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IAttachmentRepository _attachmentRepository = null!;

    /// <summary>
    /// This field contains the distributed cache for this manager.
    /// </summary>
    internal protected IDistributedCache _distributedCache;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IAttachmentManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="AttachmentManager"/>
    /// class.
    /// </summary>
    /// <param name="attachmentRepository">The attachment repository to use
    /// with this manager.</param>
    /// <param name="distributedCache">The distributed cache to use for 
    /// this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public AttachmentManager(
        IAttachmentRepository attachmentRepository,
        IDistributedCache distributedCache,
        ILogger<IAttachmentManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(attachmentRepository, nameof(attachmentRepository))
            .ThrowIfNull(distributedCache, nameof(distributedCache))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _attachmentRepository = attachmentRepository;
        _distributedCache = distributedCache;
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
                nameof(IAttachmentRepository.AnyAsync)
                );

            // Perform the search.
            return await _attachmentRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to search for attachments!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for attachments!",
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
                nameof(IAttachmentRepository.CountAsync)
                );

            // Perform the search.
            return await _attachmentRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to count attachments!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count attachments!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<Attachment> CreateAsync(
        Attachment attachment,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(attachment, nameof(attachment))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(Attachment)
                );

            // Ensure the stats are correct.
            attachment.CreatedOnUtc = DateTime.UtcNow;
            attachment.CreatedBy = userName;
            attachment.LastUpdatedBy = null;
            attachment.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IAttachmentRepository.CreateAsync)
                );

            // Perform the operation.
            return await _attachmentRepository.CreateAsync(
                attachment,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to create a new attachment!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new attachment!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        Attachment attachment,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(attachment, nameof(attachment))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(Attachment)
                );

            // Ensure the stats are correct.
            attachment.LastUpdatedOnUtc = DateTime.UtcNow;
            attachment.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IAttachmentRepository.DeleteAsync)
                );

            // Perform the operation.
            await _attachmentRepository.DeleteAsync(
                attachment,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to delete a attachment!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a attachment!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<Attachment> UpdateAsync(
        Attachment attachment,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(attachment, nameof(attachment))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(Attachment)
                );

            // Ensure the stats are correct.
            attachment.LastUpdatedOnUtc = DateTime.UtcNow;
            attachment.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IAttachmentRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _attachmentRepository.UpdateAsync(
                attachment,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to update a attachment!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a attachment!",
                innerException: ex
                );
        }
    }

    #endregion
}
