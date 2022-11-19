
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IProviderLogManager"/>
/// interface.
/// </summary>
internal class ProviderLogManager : IProviderLogManager
{
    // *******************************************************************
    // Constants.
    // *******************************************************************

    #region Constants

    /// <summary>
    /// This constants contains the cache key for this manager.
    /// </summary>
    internal protected const string CACHE_KEY = "ProviderLogManager";

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IProviderLogRepository _providerLogRepository = null!;

    /// <summary>
    /// This field contains the distributed cache for this manager.
    /// </summary>
    internal protected IDistributedCache _distributedCache = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IProviderLogManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderLogManager"/>
    /// class.
    /// </summary>
    /// <param name="providerLogRepository">The provider log repository to use
    /// with this manager.</param>
    /// <param name="distributedCache">The distributed cache to use for 
    /// this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public ProviderLogManager(
        IProviderLogRepository providerLogRepository,
        IDistributedCache distributedCache,
        ILogger<IProviderLogManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(providerLogRepository, nameof(providerLogRepository))
            .ThrowIfNull(distributedCache, nameof(distributedCache))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _providerLogRepository = providerLogRepository;
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
                nameof(IProviderLogRepository.AnyAsync)
                );

            // Perform the search.
            return await _providerLogRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for provider logs!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for provider logs!",
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
                nameof(IProviderLogRepository.CountAsync)
                );

            // Perform the search.
            return await _providerLogRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count provider logs!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count provider logs!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderLog> CreateAsync(
        ProviderLog providerLog,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerLog, nameof(providerLog))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderLog)
                );

            // Ensure the stats are correct.
            providerLog.CreatedOnUtc = DateTime.UtcNow;
            providerLog.CreatedBy = userName;
            providerLog.LastUpdatedBy = null;
            providerLog.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderLogRepository.CreateAsync)
                );

            // Perform the operation.
            return await _providerLogRepository.CreateAsync(
                providerLog,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new provider log!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new provider log!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        ProviderLog providerLog,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerLog, nameof(providerLog))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderLog)
                );

            // Ensure the stats are correct.
            providerLog.LastUpdatedOnUtc = DateTime.UtcNow;
            providerLog.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderLogRepository.DeleteAsync)
                );

            // Perform the operation.
            await _providerLogRepository.DeleteAsync(
                providerLog,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a provider log!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a provider log!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderLog> UpdateAsync(
        ProviderLog providerLog,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(providerLog, nameof(providerLog))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ProviderLog)
                );

            // Ensure the stats are correct.
            providerLog.LastUpdatedOnUtc = DateTime.UtcNow;
            providerLog.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProviderLogRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _providerLogRepository.UpdateAsync(
                providerLog,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a provider log!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a provider log!",
                innerException: ex
                );
        }
    }

    #endregion
}
