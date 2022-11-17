
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IProviderTypeManager"/>
/// interface.
/// </summary>
internal class ProviderTypeManager : IProviderTypeManager
{
    // *******************************************************************
    // Constants.
    // *******************************************************************

    #region Constants

    /// <summary>
    /// This constants contains the cache key for this manager.
    /// </summary>
    internal protected const string CACHE_KEY = "ProviderTypeManager";

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IProviderTypeRepository _providerTypeRepository = null!;

    /// <summary>
    /// This field contains the distributed cache for this manager.
    /// </summary>
    internal protected IDistributedCache _distributedCache;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IProviderTypeManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderTypeManager"/>
    /// class.
    /// </summary>
    /// <param name="providerTypeRepository">The provider type repository to use
    /// with this manager.</param>
    /// <param name="distributedCache">The distributed cache to use for 
    /// this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public ProviderTypeManager(
        IProviderTypeRepository providerTypeRepository,
        IDistributedCache distributedCache,
        ILogger<IProviderTypeManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(providerTypeRepository, nameof(providerTypeRepository))
            .ThrowIfNull(distributedCache, nameof(distributedCache))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _providerTypeRepository = providerTypeRepository;
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
                nameof(IProviderTypeRepository.AnyAsync)
                );

            // Perform the search.
            return await _providerTypeRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to search for provider types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for provider types!",
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
                nameof(IProviderTypeRepository.CountAsync)
                );

            // Perform the search.
            return await _providerTypeRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to count provider types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count provider types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderType> CreateAsync(
        ProviderType providerType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerType, nameof(providerType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderType)
                );

            // Ensure the stats are correct.
            providerType.CreatedOnUtc = DateTime.UtcNow;
            providerType.CreatedBy = userName;
            providerType.LastUpdatedBy = null;
            providerType.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderTypeRepository.CreateAsync)
                );

            // Perform the operation.
            return await _providerTypeRepository.CreateAsync(
                providerType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to create a new provider type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new provider type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        ProviderType providerType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerType, nameof(providerType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderType)
                );

            // Ensure the stats are correct.
            providerType.LastUpdatedOnUtc = DateTime.UtcNow;
            providerType.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderTypeRepository.DeleteAsync)
                );

            // Perform the operation.
            await _providerTypeRepository.DeleteAsync(
                providerType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to delete a provider type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a provider type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderType?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(name, nameof(name));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderTypeRepository.FindByNameAsync)
                );

            // Perform the operation.
            return await _providerTypeRepository.FindByNameAsync(
                name,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to search for provider types by name!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for provider " +
                "types by name!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderType> UpdateAsync(
        ProviderType providerType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerType, nameof(providerType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderType)
                );

            // Ensure the stats are correct.
            providerType.LastUpdatedOnUtc = DateTime.UtcNow;
            providerType.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderTypeRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _providerTypeRepository.UpdateAsync(
                providerType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to update a provider type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a provider type!",
                innerException: ex
                );
        }
    }

    #endregion
}
