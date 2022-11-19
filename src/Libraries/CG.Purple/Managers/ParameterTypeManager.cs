
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IParameterTypeManager"/>
/// interface.
/// </summary>
internal class ParameterTypeManager : IParameterTypeManager
{
    // *******************************************************************
    // Constants.
    // *******************************************************************

    #region Constants

    /// <summary>
    /// This constants contains the cache key for this manager.
    /// </summary>
    internal protected const string CACHE_KEY = "ParameterTypeManager";

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IParameterTypeRepository _parameterTypeRepository = null!;

    /// <summary>
    /// This field contains the distributed cache for this manager.
    /// </summary>
    internal protected IDistributedCache _distributedCache = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IParameterTypeManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ParameterTypeManager"/>
    /// class.
    /// </summary>
    /// <param name="parameterTypeRepository">The parameter type repository to use
    /// with this manager.</param>
    /// <param name="distributedCache">The distributed cache to use for 
    /// this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public ParameterTypeManager(
        IParameterTypeRepository parameterTypeRepository,
        IDistributedCache distributedCache,
        ILogger<IParameterTypeManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(parameterTypeRepository, nameof(parameterTypeRepository))
            .ThrowIfNull(distributedCache, nameof(distributedCache))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _parameterTypeRepository = parameterTypeRepository;
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
                nameof(IParameterTypeRepository.AnyAsync)
                );

            // Perform the search.
            return await _parameterTypeRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for parameter types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for parameter types!",
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
                nameof(IParameterTypeRepository.CountAsync)
                );

            // Perform the search.
            return await _parameterTypeRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count parameter types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count parameter types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ParameterType> CreateAsync(
        ParameterType parameterType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(parameterType, nameof(parameterType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ParameterType)
                );

            // Ensure the stats are correct.
            parameterType.CreatedOnUtc = DateTime.UtcNow;
            parameterType.CreatedBy = userName;
            parameterType.LastUpdatedBy = null;
            parameterType.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IParameterTypeRepository.CreateAsync)
                );

            // Perform the operation.
            return await _parameterTypeRepository.CreateAsync(
                parameterType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new parameter type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new parameter type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        ParameterType parameterType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(parameterType, nameof(parameterType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ParameterType)
                );

            // Ensure the stats are correct.
            parameterType.LastUpdatedOnUtc = DateTime.UtcNow;
            parameterType.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IParameterTypeRepository.DeleteAsync)
                );

            // Perform the operation.
            await _parameterTypeRepository.DeleteAsync(
                parameterType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a parameter type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a parameter type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ParameterType?> FindByNameAsync(
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
                nameof(IParameterTypeRepository.FindByNameAsync)
                );

            // Perform the operation.
            return await _parameterTypeRepository.FindByNameAsync(
                name,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for parameter types by name!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for parameter " +
                "types by name!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ParameterType> UpdateAsync(
        ParameterType parameterType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(parameterType, nameof(parameterType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(ParameterType)
                );

            // Ensure the stats are correct.
            parameterType.LastUpdatedOnUtc = DateTime.UtcNow;
            parameterType.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IParameterTypeRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _parameterTypeRepository.UpdateAsync(
                parameterType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a parameter type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a parameter type!",
                innerException: ex
                );
        }
    }

    #endregion
}
