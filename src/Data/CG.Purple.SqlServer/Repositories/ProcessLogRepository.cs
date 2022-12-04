
namespace CG.Purple.SqlServer.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="IProcessLogRepository"/>
/// interface.
/// </summary>
internal class ProcessLogRepository : IProcessLogRepository
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
    internal protected readonly ILogger<IProcessLogRepository> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProcessLogRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContextFactory">The EFCORE data-context factory
    /// to use with this repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public ProcessLogRepository(
        IDbContextFactory<PurpleDbContext> dbContextFactory,
        IMapper mapper,
        ILogger<IProcessLogRepository> logger
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
                "Searching for process logs"
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
                "Failed to search for process logs!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for process logs!",
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
                "Searching for process logs"
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
                "Failed to count process logs!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count process logs!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<PipelineLog> CreateAsync(
        PipelineLog processLog,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(PipelineLog)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.PipelineLog>(
                processLog
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(PipelineLog)} model to an entity."
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

            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the {entity} to the {ctx} data-context.",
                nameof(PipelineLog),
                nameof(PurpleDbContext)
                );

            // Add the entity to the data-store.
            dbContext.ProviderLogs.Attach(entity);

            // Mark the entity as added so EFCORE will insert it.
            dbContext.Entry(entity).State = EntityState.Added;

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
                nameof(PipelineLog)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<PipelineLog>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(PipelineLog)} entity to a model."
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
                "Failed to create a process log!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create a process log!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<PipelineLog>> FindAllAsync(
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
                "Searching process logs."
                );

            // Perform the log search.
            var processLogs = await dbContext.PipelineLogs
                .Include(x => x.Message)
                .Include(x => x.ProviderType)
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var result = processLogs.Select(x =>
                _mapper.Map<PipelineLog>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for process logs!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for process logs!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<PipelineLog>> FindByMessageAsync(
        Message message,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message));

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
                "Searching process logs."
                );

            // Perform the log search.
            var processLogs = await dbContext.PipelineLogs.Where(x =>
                x.MessageId == message.Id
                ).Include(x => x.Message)
                .Include(x => x.ProviderType)
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var result = processLogs.Select(x =>
                _mapper.Map<PipelineLog>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for process logs by message!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for process " +
                "logs by message!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        PipelineLog processLog,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog));

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
                nameof(PipelineLog),
                nameof(PurpleDbContext)
                );

            // Delete from the data-store.
            await dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM [Purple].[ProviderLogs] WHERE [Id] = {0}",
                parameters: new object[] { processLog.Id },
                cancellationToken: cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a process log!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to delete a process log!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<PipelineLog> UpdateAsync(
        PipelineLog processLog,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(PipelineLog)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.PipelineLog>(
                processLog
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(PipelineLog)} model to an entity."
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

            // We never change these 'read only' properties.
            dbContext.Entry(entity).Property(x => x.Id).IsModified = false;
            dbContext.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            dbContext.Entry(entity).Property(x => x.CreatedOnUtc).IsModified = false;

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating a {entity} entity in the {ctx} data-context.",
                nameof(PipelineLog),
                nameof(PurpleDbContext)
                );

            // Start tracking the entity.
            dbContext.ProviderLogs.Attach(entity);

            // Mark the entity as modified so EFCORE will update it.
            dbContext.Entry(entity).State = EntityState.Modified;

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
                nameof(PipelineLog)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<PipelineLog>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(PipelineLog)} entity to a model."
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
                "Failed to update a process log!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to update a process log!",
                innerException: ex
                );
        }
    }

    #endregion
}
