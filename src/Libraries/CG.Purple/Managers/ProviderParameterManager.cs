
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IProviderParameterManager"/>
/// interface.
/// </summary>
internal class ProviderParameterManager : IProviderParameterManager
{
    // *******************************************************************
    // Constants.
    // *******************************************************************

    #region Constants

    /// <summary>
    /// This constants contains the cache key for this manager.
    /// </summary>
    internal protected const string CACHE_KEY = "ProviderParameterManager";

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IProviderParameterRepository _providerParameterRepository = null!;

    /// <summary>
    /// This field contains the distributed cache for this manager.
    /// </summary>
    internal protected IDistributedCache _distributedCache;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IProviderParameterManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderParameterManager"/>
    /// class.
    /// </summary>
    /// <param name="providerParameterRepository">The provider parameter repository to use
    /// with this manager.</param>
    /// <param name="distributedCache">The distributed cache to use for 
    /// this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public ProviderParameterManager(
        IProviderParameterRepository providerParameterRepository,
        IDistributedCache distributedCache,
        ILogger<IProviderParameterManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(providerParameterRepository, nameof(providerParameterRepository))
            .ThrowIfNull(distributedCache, nameof(distributedCache))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _providerParameterRepository = providerParameterRepository;
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
                nameof(IProviderParameterRepository.AnyAsync)
                );

            // Perform the search.
            return await _providerParameterRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to search for provider parameters!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for provider parameters!",
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
                nameof(IProviderParameterRepository.CountAsync)
                );

            // Perform the search.
            return await _providerParameterRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to count provider parameters!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count provider parameters!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderParameter> CreateAsync(
        ProviderParameter providerParameter,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerParameter, nameof(providerParameter))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderParameter)
                );

            // Ensure the stats are correct.
            providerParameter.CreatedOnUtc = DateTime.UtcNow;
            providerParameter.CreatedBy = userName;
            providerParameter.LastUpdatedBy = null;
            providerParameter.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderParameterRepository.CreateAsync)
                );

            // Perform the operation.
            return await _providerParameterRepository.CreateAsync(
                providerParameter,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to create a new provider parameter!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new provider parameter!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        ProviderParameter providerParameter,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerParameter, nameof(providerParameter))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderParameter)
                );

            // Ensure the stats are correct.
            providerParameter.LastUpdatedOnUtc = DateTime.UtcNow;
            providerParameter.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderParameterRepository.DeleteAsync)
                );

            // Perform the operation.
            await _providerParameterRepository.DeleteAsync(
                providerParameter,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to delete a provider parameter!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a provider parameter!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderParameter> UpdateAsync(
        ProviderParameter providerParameter,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerParameter, nameof(providerParameter))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderParameter)
                );

            // Ensure the stats are correct.
            providerParameter.LastUpdatedOnUtc = DateTime.UtcNow;
            providerParameter.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderParameterRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _providerParameterRepository.UpdateAsync(
                providerParameter,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to update a provider parameter!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a provider parameter!",
                innerException: ex
                );
        }
    }

    #endregion
}
