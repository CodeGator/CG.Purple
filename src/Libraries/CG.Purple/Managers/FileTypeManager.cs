
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IFileTypeManager"/>
/// interface.
/// </summary>
internal class FileTypeManager : IFileTypeManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IFileTypeRepository _fileTypeRepository = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IFileTypeManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="FileTypeManager"/>
    /// class.
    /// </summary>
    /// <param name="fileTypeRepository">The file type repository to use
    /// with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public FileTypeManager(
        IFileTypeRepository fileTypeRepository,
        ILogger<IFileTypeManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(fileTypeRepository, nameof(fileTypeRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _fileTypeRepository = fileTypeRepository;
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
                nameof(IFileTypeRepository.AnyAsync)
                );

            // Perform the search.
            return await _fileTypeRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to search for file types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for file types!",
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
                nameof(IFileTypeRepository.CountAsync)
                );

            // Perform the search.
            return await _fileTypeRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to count file types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count file types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<FileType> CreateAsync(
        FileType fileType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(fileType, nameof(fileType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(FileType)
                );

            // Ensure the stats are correct.
            fileType.CreatedOnUtc = DateTime.UtcNow;
            fileType.CreatedBy = userName;
            fileType.LastUpdatedBy = null;
            fileType.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IFileTypeRepository.CreateAsync)
                );

            // Perform the operation.
            return await _fileTypeRepository.CreateAsync(
                fileType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to create a new file type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new file type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        FileType fileType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(fileType, nameof(fileType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(FileType)
                );

            // Ensure the stats are correct.
            fileType.LastUpdatedOnUtc = DateTime.UtcNow;
            fileType.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IFileTypeRepository.DeleteAsync)
                );

            // Perform the operation.
            await _fileTypeRepository.DeleteAsync(
                fileType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to delete a file type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a file type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<FileType?> FindByExtensionAsync(
        string extension,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(extension, nameof(extension));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IFileTypeRepository.FindByExtensionAsync)
                );

            // Perform the operation.
            return await _fileTypeRepository.FindByExtensionAsync(
                extension,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to search for file types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for file types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<FileType> UpdateAsync(
        FileType fileType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(fileType, nameof(fileType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(FileType)
                );

            // Ensure the stats are correct.
            fileType.LastUpdatedOnUtc = DateTime.UtcNow;
            fileType.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IFileTypeRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _fileTypeRepository.UpdateAsync(
                fileType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to update a file type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a file type!",
                innerException: ex
                );
        }
    }

    #endregion
}
