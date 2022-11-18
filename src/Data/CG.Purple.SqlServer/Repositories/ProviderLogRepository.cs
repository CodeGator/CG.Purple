
namespace CG.Purple.SqlServer.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="IProviderLogRepository"/>
/// interface.
/// </summary>
internal class ProviderLogRepository : IProviderLogRepository
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the EFCORE data-context factory for this repository.
    /// </summary>
    internal protected readonly IDbContextFactory<PurpleDbContext> _dbContextFactory;

    /// <summary>
    /// This field contains the auto-mapper for this repository.
    /// </summary>
    internal protected readonly IMapper _mapper;

    /// <summary>
    /// This field contains the logger for this repository.
    /// </summary>
    internal protected readonly ILogger<IProviderLogRepository> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderLogRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContextFactory">The EFCORE data-context factory
    /// to use with this repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public ProviderLogRepository(
        IDbContextFactory<PurpleDbContext> dbContextFactory,
        IMapper mapper,
        ILogger<IProviderLogRepository> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(dbContextFactory, nameof(dbContextFactory))
            .ThrowIfNull(mapper, nameof(mapper))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
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
            _logger.LogDebug(
                "Creating a {ctx} data-context",
                nameof(PurpleDbContext)
                );

            // Create a database context.
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for provider logs"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.ProviderLogs.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return data;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for provider logs"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for provider logs!",
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
            _logger.LogDebug(
                "Creating a {ctx} data-context",
                nameof(PurpleDbContext)
                );

            // Create a database context.
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for provider logs"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.ProviderLogs.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return data;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count provider logs"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count provider logs!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderLog> CreateAsync(
        ProviderLog providerLog,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(ProviderLog)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.ProviderLog>(
                providerLog
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderLog)} model to an entity."
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a {ctx} data-context",
                nameof(PurpleDbContext)
                );

            // Create a database context.
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // We don't mess with associated entity types.
            dbContext.Entry(entity.Message).State = EntityState.Unchanged;
            if (entity.ProviderType is not null)
            {
                dbContext.Entry(entity.ProviderType).State = EntityState.Unchanged;
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the {entity} to the {ctx} data-context.",
                nameof(ProviderLog),
                nameof(PurpleDbContext)
                );

            // Add the entity to the data-store.
            _ = await dbContext.ProviderLogs.AddAsync(
                    entity,
                    cancellationToken
                    ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Saving changes to the {ctx} data-context",
                nameof(PurpleDbContext)
                );

            // Save the changes.
            await dbContext.SaveChangesAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} entity to a model",
                nameof(ProviderLog)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<ProviderLog>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderLog)} entity to a model."
                    );
            }

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a provider log"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create a provider log!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        ProviderLog model,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a {ctx} data-context",
                nameof(PurpleDbContext)
                );

            // Create a database context.
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "deleting an {entity} instance from the {ctx} data-context",
                nameof(ProviderLog),
                nameof(PurpleDbContext)
                );

            // Delete from the data-store.
            await dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM [Purple].[ProviderLogs] WHERE [Id] = {0}",
                parameters: new object[] { model.Id },
                cancellationToken: cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a provider log"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to delete a provider log!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<ProviderLog> UpdateAsync(
        ProviderLog providerLog,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(ProviderLog)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.ProviderLog>(
                providerLog
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderLog)} model to an entity."
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a {ctx} data-context",
                nameof(PurpleDbContext)
                );

            // Create a database context.
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // We don't change 'read only' properties.
            dbContext.Entry(entity.CreatedBy).State = EntityState.Unchanged;
            dbContext.Entry(entity.CreatedOnUtc).State = EntityState.Unchanged;
            dbContext.Entry(entity.Id).State = EntityState.Unchanged;

            // We don't mess with associated entity types.
            dbContext.Entry(entity.Message).State = EntityState.Unchanged;
            if (entity.ProviderType is not null)
            {
                dbContext.Entry(entity.ProviderType).State = EntityState.Unchanged;
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating a {entity} entity in the {ctx} data-context.",
                nameof(ProviderLog),
                nameof(PurpleDbContext)
                );

            // Update the data-store.
            _= dbContext.ProviderLogs.Update(
                entity
                );

            // Log what we are about to do.
            _logger.LogDebug(
                "Saving changes to the {ctx} data-context",
                nameof(PurpleDbContext)
                );

            // Save the changes.
            await dbContext.SaveChangesAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} entity to a model",
                nameof(ProviderLog)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<ProviderLog>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(ProviderLog)} entity to a model."
                    );
            }

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a provider log"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to update a provider log!",
                innerException: ex
                );
        }
    }

    #endregion
}
