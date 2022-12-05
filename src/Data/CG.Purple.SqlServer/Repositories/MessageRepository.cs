
namespace CG.Purple.SqlServer.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="IMessageRepository"/>
/// interface.
/// </summary>
internal class MessageRepository : IMessageRepository
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
    internal protected readonly ILogger<IMessageRepository> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessageRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContextFactory">The EFCORE data-context factory
    /// to use with this repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public MessageRepository(
        IDbContextFactory<PurpleDbContext> dbContextFactory,
        IMapper mapper,
        ILogger<IMessageRepository> logger
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
                "Searching for messages"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.Messages.AnyAsync(
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
                "Failed to search for messages!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for messages!",
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
                "Searching for messages"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.Messages.CountAsync(
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
                "Failed to count messages!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
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
                "deleting an {entity} instance from the {ctx} data-context",
                nameof(Message),
                nameof(PurpleDbContext)
                );

            // Delete from the data-store.
            await dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM [Purple].[Messages] WHERE [Id] = {0}",
                parameters: new object[] { message.Id },
                cancellationToken: cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a message!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to delete a message!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<Message>> FindAllAsync(
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
                "Searching messages."
                );

            // Perform the message search.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var Messages = await dbContext.Messages
                .Include(x => x.Attachments).ThenInclude(x => x.MimeType).ThenInclude(x => x.FileTypes)
                .Include(x => x.MessageProperties).ThenInclude(x => x.PropertyType)
                .Include(x => x.ProviderType).ThenInclude(x => x.Parameters).ThenInclude(x => x.ParameterType)
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Convert the entities to a models.
            var result = Messages.Select(x =>
                _mapper.Map<Message>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for messages!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a  " +
                "messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<Message?> FindByIdAsync(
        long id,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfZero(id, nameof(id));

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
                "Searching for a matching message."
                );

            // Perform the message search.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var message = await dbContext.Messages.Where(x => 
                x.Id == id
                ).Include(x => x.Attachments).ThenInclude(x => x.MimeType).ThenInclude(x => x.FileTypes)
                 .Include(x => x.MessageProperties).ThenInclude(x => x.PropertyType)
                 .Include(x => x.ProviderType).ThenInclude(x => x.Parameters).ThenInclude(x => x.ParameterType)
                 .FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Did we fail?
            if (message is null)
            {
                return null; // Nothing found!
            }

            // Convert the entity to a model.
            var result = _mapper.Map<Message>(
                message
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(Message)} entity to a model."
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
                "Failed to search for a message by id!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a  " +
                "message by id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<Message?> FindByKeyAsync(
        string messageKey,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(messageKey, nameof(messageKey));

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
                "Searching for a matching message."
                );

            // Perform the message search.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var message = await dbContext.Messages.Where(x =>
                x.MessageKey == messageKey.ToUpper()
                ).Include(x => x.Attachments).ThenInclude(x => x.MimeType).ThenInclude(x => x.FileTypes)
                 .Include(x => x.MessageProperties).ThenInclude(x => x.PropertyType)
                 .Include(x => x.ProviderType).ThenInclude(x => x.Parameters).ThenInclude(x => x.ParameterType)
                 .FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Did we fail?
            if (message is null) 
            {
                return null; // Nothing found!
            }

            // Convert the entity to a model.
            var result = _mapper.Map<Message>(
                message
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(Message)} entity to a model."
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
                "Failed to search for a message by key!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a  " +
                "message by key!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<Message>> FindReadyToArchiveAsync(
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
                "Searching for messages to archive."
                );

            // Perform the message search for:
            //  (A) messages in a terminal state (Sent or Failed)
            //  (B) messages whose archive after date is <= now.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var messages = await dbContext.Messages.Where(x =>
                x.ArchiveAfterUtc <= DateTime.UtcNow &&
                (x.MessageState == MessageState.Failed || x.MessageState == MessageState.Sent)
                ).Include(x => x.Attachments).ThenInclude(x => x.MimeType).ThenInclude(x => x.FileTypes)
                 .Include(x => x.MessageProperties).ThenInclude(x => x.PropertyType)
                 .Include(x => x.ProviderType).ThenInclude(x => x.Parameters).ThenInclude(x => x.ParameterType)
                 .OrderByDescending(x => x.CreatedBy).ThenBy(x => x.Priority)
                 .ToListAsync(
                    cancellationToken
                    ).ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Convert the entities to a models.
            var result = messages.Select(x =>
                _mapper.Map<Message>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for messages that are ready to archive!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for messages " +
                "that are ready to archive!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<Message>> FindReadyToProcessAsync(
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
                "Searching for messages to process."
                );

            // Perform the message search for:
            //  (A) messages that aren't in a terminal state
            //  (B) messages that aren't disabled.
            //  (C) messages whose process after date is null or <= now 
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var messages = await dbContext.Messages.Where(x =>
                x.IsDisabled == false && 
                x.MessageState != MessageState.Failed &&
                x.MessageState != MessageState.Sent &&
                (x.ProcessAfterUtc == null || x.ProcessAfterUtc <= DateTime.UtcNow)
                ).Include(x => x.Attachments).ThenInclude(x => x.MimeType).ThenInclude(x => x.FileTypes)
                 .Include(x => x.MessageProperties).ThenInclude(x => x.PropertyType)
                 .Include(x => x.ProviderType).ThenInclude(x => x.Parameters).ThenInclude(x => x.ParameterType)
                 .OrderByDescending(x => x.CreatedBy).ThenBy(x => x.Priority)
                 .ToListAsync(
                    cancellationToken
                    ).ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Convert the entities to a models.
            var result = messages.Select(x =>
                _mapper.Map<Message>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for messages that are ready to process!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for messages " +
                "that are ready to process!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<Message>> FindReadyToRetryAsync(
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
                "Searching for messages to retry."
                );

            // Perform the message search for:
            //  (A) messages that are in a failed state
            //  (B) messages that aren't disabled.
            //  (C) messages whose process after date is null or < now.
            //  (D) messages whose error count is < max errors
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var messages = await dbContext.Messages.Where(x =>
                x.IsDisabled == false &&
                x.MessageState == MessageState.Failed &&
                x.ErrorCount < x.MaxErrors &&
                (x.ProcessAfterUtc == null || x.ProcessAfterUtc <= DateTime.UtcNow)
                ).Include(x => x.Attachments).ThenInclude(x => x.MimeType).ThenInclude(x => x.FileTypes)
                 .Include(x => x.MessageProperties).ThenInclude(x => x.PropertyType)
                 .Include(x => x.ProviderType).ThenInclude(x => x.Parameters).ThenInclude(x => x.ParameterType)
                 .OrderByDescending(x => x.CreatedBy).ThenBy(x => x.Priority)
                 .ToListAsync(
                    cancellationToken
                    ).ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Convert the entities to a models.
            var result = messages.Select(x =>
                _mapper.Map<Message>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for messages that are ready to retry!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for messages " +
                "that are ready to retry!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<Message> UpdateAsync(
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
                "Converting a {entity} model to an entity",
                nameof(Message)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.Message>(
                message
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(Message)} model to an entity."
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
            dbContext.Entry(entity).Property(x => x.MessageKey).IsModified = false;
            dbContext.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            dbContext.Entry(entity).Property(x => x.CreatedOnUtc).IsModified = false;

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating a {entity} entity in the {ctx} data-context.",
                nameof(Message),
                nameof(PurpleDbContext)
                );

            // Start tracking the entity.
            dbContext.Messages.Attach(entity);

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
                nameof(Message)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<Message>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(Message)} entity to a model."
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
                "Failed to update a message!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to update a message!",
                innerException: ex
                );
        }
    }

    #endregion
}
