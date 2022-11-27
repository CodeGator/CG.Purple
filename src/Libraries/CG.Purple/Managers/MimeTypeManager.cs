
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IMimeTypeManager"/>
/// interface.
/// </summary>
internal class MimeTypeManager : IMimeTypeManager
{
    // *******************************************************************
    // Constants.
    // *******************************************************************

    #region Constants

    /// <summary>
    /// This constants contains the cache key for this manager.
    /// </summary>
    internal protected const string CACHE_KEY = "MimeTypeManager";

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the business logic layer options for this manager.
    /// </summary>
    internal protected readonly MimeTypeOptions? _mimeTypeOptions;

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IMimeTypeRepository _mimeTypeRepository = null!;

    /// <summary>
    /// This field contains the distributed cache for this manager.
    /// </summary>
    internal protected IDistributedCache _distributedCache = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IMimeTypeManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MimeTypeManager"/>
    /// class.
    /// </summary>
    /// <param name="bllOptions">The business logic layer options to use 
    /// with this manager.</param>
    /// <param name="mimeTypeRepository">The mime type repository to use
    /// with this manager.</param>
    /// <param name="distributedCache">The distributed cache to use for 
    /// this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public MimeTypeManager(
        IOptions<BllOptions> bllOptions,
        IMimeTypeRepository mimeTypeRepository,
        IDistributedCache distributedCache,
        ILogger<IMimeTypeManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(bllOptions, nameof(bllOptions))
            .ThrowIfNull(mimeTypeRepository, nameof(mimeTypeRepository))
            .ThrowIfNull(distributedCache, nameof(distributedCache))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _mimeTypeOptions = bllOptions.Value?.MimeTypes;
        _mimeTypeRepository = mimeTypeRepository;
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
                nameof(IMimeTypeRepository.AnyAsync)
                );

            // Perform the search.
            return await _mimeTypeRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for mime types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for mime types!",
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
                nameof(IMimeTypeRepository.CountAsync)
                );

            // Perform the search.
            return await _mimeTypeRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count mime types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count mime types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MimeType> CreateAsync(
        MimeType mimeType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mimeType, nameof(mimeType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MimeType)
                );

            // Ensure the stats are correct.
            mimeType.CreatedOnUtc = DateTime.UtcNow;
            mimeType.CreatedBy = userName;
            mimeType.LastUpdatedBy = null;
            mimeType.LastUpdatedOnUtc = null;

            // types are always lower case.
            mimeType.Type = mimeType.Type.ToLower().Trim();

            // sub-types are always lower case.
            mimeType.SubType = mimeType.SubType.ToLower().Trim();

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMimeTypeRepository.CreateAsync)
                );

            // Perform the operation.
            return await _mimeTypeRepository.CreateAsync(
                mimeType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new mime type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new mime type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        MimeType mimeType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mimeType, nameof(mimeType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MimeType)
                );

            // Ensure the stats are correct.
            mimeType.LastUpdatedOnUtc = DateTime.UtcNow;
            mimeType.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMimeTypeRepository.DeleteAsync)
                );

            // Perform the operation.
            await _mimeTypeRepository.DeleteAsync(
                mimeType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a mime type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a mime type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<MimeType>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMimeTypeRepository.FindAllAsync)
                );

            // Perform the operation.
            var mimeTypes = await _mimeTypeRepository.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return mimeTypes;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for mime types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for mime types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<MimeType>> FindByTypeAsync(
        string type,
        string subType,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMimeTypeRepository.FindByTypeAsync)
                );

            // Perform the operation.
            var mimeTypes = await _mimeTypeRepository.FindByTypeAsync(
                type,
                subType,
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return mimeTypes;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for mime types by type/subtype!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for mime " +
                "types by type/subtype!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MimeType?> FindByExtensionAsync(
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
                nameof(IMimeTypeRepository.FindByExtensionAsync)
                );

            // Perform the operation.
            var mimeType = await _mimeTypeRepository.FindByExtensionAsync(
                extension,
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return mimeType;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a matching mime type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for a matching " +
                "mime type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MimeType?> FindByIdAsync(
        int id,
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
                nameof(IMimeTypeRepository.FindByExtensionAsync)
                );

            // Perform the operation.
            var mimeType = await _mimeTypeRepository.FindByIdAsync(
                id,
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return mimeType;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a matching mime type by id!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for a matching " +
                "mime type by id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MimeType> UpdateAsync(
        MimeType mimeType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mimeType, nameof(mimeType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MimeType)
                );

            // Ensure the stats are correct.
            mimeType.LastUpdatedOnUtc = DateTime.UtcNow;
            mimeType.LastUpdatedBy = userName;

            // types are always lower case.
            mimeType.Type = mimeType.Type.ToLower().Trim();

            // sub-types are always lower case.
            mimeType.SubType = mimeType.SubType.ToLower().Trim();

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMimeTypeRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _mimeTypeRepository.UpdateAsync(
                mimeType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a mime type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a mime type!",
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
    private async Task<List<MimeType>> GetCachedDataAsync(
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogTrace(
            "Deferring to IDistributedCache.GetOrSetAsync"
            );

        // Get (set) the cached data for this manager.
        var mimeTypes = await _distributedCache.GetOrSetAsync<List<MimeType>>(
            CACHE_KEY,
        new DistributedCacheEntryOptions()
        {
            SlidingExpiration = _mimeTypeOptions?.DefaultCacheDuration
                ?? TimeSpan.FromHours(1)
        },
        () =>
        {
            return (_mimeTypeRepository.FindAllAsync(
                cancellationToken
                ).Result).ToList();
        },
        cancellationToken
        ).ConfigureAwait(false);

        // Return the results.
        return mimeTypes;
    }

    #endregion
}
