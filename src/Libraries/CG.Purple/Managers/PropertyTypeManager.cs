
using System.Xml.Linq;

namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IPropertyTypeManager"/>
/// interface.
/// </summary>
internal class PropertyTypeManager : IPropertyTypeManager
{
    // *******************************************************************
    // Constants.
    // *******************************************************************

    #region Constants

    /// <summary>
    /// This constants contains the cache key for this manager.
    /// </summary>
    internal protected const string CACHE_KEY = "PropertyTypeManager";

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IPropertyTypeRepository _propertyTypeRepository = null!;

    /// <summary>
    /// This field contains the distributed cache for this manager.
    /// </summary>
    internal protected IDistributedCache _distributedCache = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IPropertyTypeManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PropertyTypeManager"/>
    /// class.
    /// </summary>
    /// <param name="propertyTypeRepository">The property type repository to use
    /// with this manager.</param>
    /// <param name="distributedCache">The distributed cache to use for 
    /// this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public PropertyTypeManager(
        IPropertyTypeRepository propertyTypeRepository,
        IDistributedCache distributedCache,
        ILogger<IPropertyTypeManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(propertyTypeRepository, nameof(propertyTypeRepository))
            .ThrowIfNull(distributedCache, nameof(distributedCache))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _propertyTypeRepository = propertyTypeRepository;
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
                nameof(IPropertyTypeRepository.AnyAsync)
                );

            // Perform the search.
            return await _propertyTypeRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for property types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for property types!",
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
                nameof(IPropertyTypeRepository.CountAsync)
                );

            // Perform the search.
            return await _propertyTypeRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count property types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count property types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<PropertyType> CreateAsync(
        PropertyType propertyType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(propertyType, nameof(propertyType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(PropertyType)
                );

            // Ensure the stats are correct.
            propertyType.CreatedOnUtc = DateTime.UtcNow;
            propertyType.CreatedBy = userName;
            propertyType.LastUpdatedBy = null;
            propertyType.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IPropertyTypeRepository.CreateAsync)
                );

            // Perform the operation.
            return await _propertyTypeRepository.CreateAsync(
                propertyType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new property type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new property type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        PropertyType propertyType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(propertyType, nameof(propertyType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(PropertyType)
                );

            // Ensure the stats are correct.
            propertyType.LastUpdatedOnUtc = DateTime.UtcNow;
            propertyType.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IPropertyTypeRepository.DeleteAsync)
                );

            // Perform the operation.
            await _propertyTypeRepository.DeleteAsync(
                propertyType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a property type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to delete a property type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<PropertyType>> FindAllAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IPropertyTypeRepository.FindAllAsync)
                );

            // Perform the operation.
            return await _propertyTypeRepository.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for property types!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for property " +
                "types!",
                innerException: ex
                );
        }

    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<PropertyType?> FindByNameAsync(
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
                nameof(IPropertyTypeRepository.FindByNameAsync)
                );

            // Perform the operation.
            return await _propertyTypeRepository.FindByNameAsync(
                name,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for property types by name!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for property " +
                "types by name!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<PropertyType> UpdateAsync(
        PropertyType propertyType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(propertyType, nameof(propertyType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(PropertyType)
                );

            // Ensure the stats are correct.
            propertyType.LastUpdatedOnUtc = DateTime.UtcNow;
            propertyType.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IPropertyTypeRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _propertyTypeRepository.UpdateAsync(
                propertyType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a property type!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a property type!",
                innerException: ex
                );
        }
    }

    #endregion
}
