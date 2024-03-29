﻿
namespace CG.Purple.SqlServer.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="IMessageLogRepository"/>
/// interface.
/// </summary>
internal class MessageLogRepository : IMessageLogRepository
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
    internal protected readonly ILogger<IMessageLogRepository> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessageLogRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContextFactory">The EFCORE data-context factory
    /// to use with this repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public MessageLogRepository(
        IDbContextFactory<PurpleDbContext> dbContextFactory,
        IMapper mapper,
        ILogger<IMessageLogRepository> logger
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
                "Searching for message logs"
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
                "Failed to search for message logs!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for message logs!",
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
                "Searching for message logs"
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
                "Failed to count message logs!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count message logs!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MessageLog> CreateAsync(
        MessageLog messageLog,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messageLog, nameof(messageLog));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(MessageLog)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.MessageLog>(
                messageLog
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MessageLog)} model to an entity."
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
                nameof(MessageLog),
                nameof(PurpleDbContext)
                );

            // Add the entity to the data-store.
            dbContext.ProviderLogs.Attach(entity);

            // Mark the entity as added so EFCORE will insert it.
            dbContext.Entry(entity).State = EntityState.Added;

            // Is there an associated provider type?
            if (entity.ProviderType is not null)
            {
                // Loop through the parameters.
                foreach (var parameter in entity.ProviderType.Parameters)
                {
                    // Mark the parameter as detached so EFCORE won't mess with it.
                    dbContext.Entry(parameter).State = EntityState.Detached;
                }
            }

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
                nameof(MessageLog)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<MessageLog>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MessageLog)} entity to a model."
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
                "Failed to create a message log!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create a message log!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<MessageLog>> FindAllAsync(
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
                "Searching message logs."
                );

            // Perform the log search.
            var messageLogs = await dbContext.MessageLogs
                .Include(x => x.Message)
                .Include(x => x.ProviderType)
                .AsNoTracking()
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var result = messageLogs.Select(x =>
                _mapper.Map<MessageLog>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for message logs!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for message logs!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<MessageLog>> FindByMessageAsync(
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
                "Searching message logs."
                );

            // Perform the log search.
            var messageLogs = await dbContext.MessageLogs.Where(x =>
                x.MessageId == message.Id
                ).Include(x => x.Message)
                .Include(x => x.ProviderType)
                .AsNoTracking()
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var result = messageLogs.Select(x =>
                _mapper.Map<MessageLog>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for message logs by message!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for message " +
                "logs by message!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        MessageLog messageLog,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messageLog, nameof(messageLog));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(MessageLog)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.MessageLog>(
                messageLog
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MessageLog)} model to an entity."
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
                "looking for the tracked {entity} instance from the {ctx} data-context",
                nameof(MessageLog),
                nameof(PurpleDbContext)
                );

            // Find the tracked entity (if any).
            var trackedEntry = await dbContext.MessageLogs.FindAsync(
                entity.Id,
                cancellationToken
                );

            // Did we fail?
            if (trackedEntry is null)
            {
                return; // Nothing to do!
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "deleting an {entity} instance from the {ctx} data-context",
                nameof(MessageLog),
                nameof(PurpleDbContext)
                );

            // Delete from the data-store.
            dbContext.MessageLogs.Remove(
                trackedEntry
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
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a message log!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to delete a message log!",
                innerException: ex
                );
        }
    }

    #endregion
}
