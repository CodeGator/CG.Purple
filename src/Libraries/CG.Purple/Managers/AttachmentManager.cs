﻿
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IAttachmentManager"/>
/// interface.
/// </summary>
internal class AttachmentManager : IAttachmentManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IAttachmentRepository _attachmentRepository = null!;

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
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public AttachmentManager(
        IAttachmentRepository attachmentRepository,
        ILogger<IAttachmentManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(attachmentRepository, nameof(attachmentRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _attachmentRepository = attachmentRepository;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc cref="IAttachmentManager.AnyAsync(CancellationToken)" />
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
            // Log what happened.
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

    /// <inheritdoc cref="IAttachmentManager.CountAsync(CancellationToken)"/>
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
            // Log what happened.
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

    /// <inheritdoc cref="IAttachmentManager.CreateAsync(Attachment, string, CancellationToken)"/>
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
            // Log what happened.
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

    /// <inheritdoc cref="IAttachmentManager.DeleteAsync(Attachment, string, CancellationToken)"/>
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
            // Log what happened.
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

    /// <inheritdoc cref="IAttachmentManager.FindAllAsync(CancellationToken)"/>
    public virtual async Task<IEnumerable<Attachment>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IAttachmentRepository.FindAllAsync)
                );

            // Perform the operation.
            var result = await _attachmentRepository.FindAllAsync(
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

    /// <inheritdoc cref="IAttachmentManager.UpdateAsync(Attachment, string, CancellationToken)"/>
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
            // Log what happened.
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
