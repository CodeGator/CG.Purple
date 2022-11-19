
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IFileTypeManager"/>
/// interface.
/// </summary>
internal class FileTypeManager : IFileTypeManager
{
    // *******************************************************************
    // Constants.
    // *******************************************************************

    #region Constants

    /// <summary>
    /// This constants contains the cache key for this manager.
    /// </summary>
    internal protected const string CACHE_KEY = "FileTypeManager";

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the business logic layer options for this manager.
    /// </summary>
    internal protected readonly FileTypeOptions? _fileTypeOptions;

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IFileTypeRepository _fileTypeRepository = null!;

    /// <summary>
    /// This field contains the distributed cache for this manager.
    /// </summary>
    internal protected IDistributedCache _distributedCache =  null!;

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
    /// <param name="bllOptions">The business logic layer options to use 
    /// with this manager.</param>
    /// <param name="fileTypeRepository">The file type repository to use
    /// with this manager.</param>
    /// <param name="distributedCache">The distributed cache to use for 
    /// this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public FileTypeManager(
        IOptions<BllOptions> bllOptions,
        IFileTypeRepository fileTypeRepository,
        IDistributedCache distributedCache,
        ILogger<IFileTypeManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(bllOptions, nameof(bllOptions))
            .ThrowIfNull(fileTypeRepository, nameof(fileTypeRepository))
            .ThrowIfNull(distributedCache, nameof(distributedCache))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _fileTypeOptions = bllOptions.Value?.FileTypes;
        _fileTypeRepository = fileTypeRepository;
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
            // Try to search using cached data. If that fails then
            //   defer to the repository and do it the old school way.

            bool result = false;
            try
            {
                // Check the cache for data.
                var fileTypes = await GetCachedDataAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to IList.Count"
                    );

                // Return the results.
                result = fileTypes.Any();
            }
            catch (Exception ex)
            {
                // Log what happened.
                _logger.LogWarning(
                    ex,
                    "Cache check failed!"
                    );

                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(IFileTypeRepository.AnyAsync)
                    );

                // Check the repository for the data.
                result = await _fileTypeRepository.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);
            }

            // Return the results,
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
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
            // Try to count using cached data. If that fails then
            //   defer to the repository and do it the old school way.

            var result = 0L;
            try
            {
                // Check the cache for data.
                var fileTypes = await GetCachedDataAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to IList.Count"
                    );

                // Return the results.
                result = fileTypes.LongCount();
            }
            catch (Exception ex)
            {
                // Log what happened.
                _logger.LogWarning(
                    ex,
                    "Cache check failed!"
                    );

                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(IFileTypeRepository.CountAsync)
                    );

                // Perform the search.
                result = await _fileTypeRepository.CountAsync(
                    cancellationToken
                    ).ConfigureAwait(false);
            }

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
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
            var result = await _fileTypeRepository.CreateAsync(
                fileType,
                cancellationToken
                ).ConfigureAwait(false);

            // Update the cache.
            await _distributedCache.RefreshAsync(
                CACHE_KEY,
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

            // Update the cache.
            await _distributedCache.RefreshAsync(
                CACHE_KEY,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
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

        // Try to search using cached data. If that fails then
        //   defer to the repository and do it the old school way.
        
        try
        {
            FileType? result = null;
            try
            {
                // Check the cache for data.
                var fileTypes = await GetCachedDataAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to IList.Count"
                    );

                // Perform the operation.
                result = fileTypes.FirstOrDefault(x => 
                    x.Extension == extension
                    );
            }
            catch (Exception ex)
            {
                // Log what happened.
                _logger.LogWarning(
                    ex,
                    "Cache check failed!"
                    );

                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(IFileTypeRepository.FindByExtensionAsync)
                    );

                // Perform the operation.
                result = await _fileTypeRepository.FindByExtensionAsync(
                    extension,
                    cancellationToken
                    ).ConfigureAwait(false);
            }

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a file type by extension!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for a file type " +
                "by extension!",
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
            var result = await _fileTypeRepository.UpdateAsync(
                fileType,
                cancellationToken
                ).ConfigureAwait(false);

            // Update the cache.
            await _distributedCache.RefreshAsync(
                CACHE_KEY,
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

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method gets the cached data for this manager.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the cached 
    /// data for this manager.</returns>
    private async Task<List<FileType>> GetCachedDataAsync(
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogTrace(
            "Deferring to IDistributedCache.GetOrSetAsync"
            );

        // Get (set) the cached data for this manager.
        var fileTypes = await _distributedCache.GetOrSetAsync<List<FileType>>(
            CACHE_KEY,
            new DistributedCacheEntryOptions() 
            { 
                SlidingExpiration = _fileTypeOptions?.DefaultCacheDuration 
                    ?? TimeSpan.FromHours(1)
            },
            () =>
            {
                return (_fileTypeRepository.FindAllAsync(
                    cancellationToken
                    ).Result).ToList();
            },
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return fileTypes;
    }

    #endregion
}
