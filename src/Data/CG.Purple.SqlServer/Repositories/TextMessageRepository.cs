﻿
namespace CG.Purple.SqlServer.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="ITextMessageRepository"/>
/// interface.
/// </summary>
internal class TextMessageRepository : ITextMessageRepository
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
    internal protected readonly ILogger<ITextMessageRepository> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="TextMessageRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContextFactory">The EFCORE data-context factory
    /// to use with this repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public TextMessageRepository(
        IDbContextFactory<PurpleDbContext> dbContextFactory,
        IMapper mapper,
        ILogger<ITextMessageRepository> logger
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
                "Searching for text messages"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.TextMessages.AnyAsync(
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
                "Failed to search for text messages!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for text messages!",
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
                "Searching for text messages"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.TextMessages.CountAsync(
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
                "Failed to count text messages!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count text messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<TextMessage> CreateAsync(
        TextMessage textMessage,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(textMessage, nameof(textMessage));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(TextMessage)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.TextMessage>(
                textMessage
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(TextMessage)} model to an entity."
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

            // We always want the key stored in upper case.
            entity.MessageKey = entity.MessageKey.ToUpper();

            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the {entity} to the {ctx} data-context.",
                nameof(TextMessage),
                nameof(PurpleDbContext)
                );

            // Add the entity to the data-store.
            dbContext.TextMessages.Attach(entity);

            // Mark the entity as added so EFCORE will insert it.
            dbContext.Entry(entity).State = EntityState.Added;

            // Is there an associated provider type ?
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
                nameof(TextMessage)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<TextMessage>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(TextMessage)} entity to a model."
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
                "Failed to create a text message!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create a text message!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TextMessage>> FindAllAsync(
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
                "Searching text messages."
                );

            // Perform the text message search.
            var textMessages = await dbContext.TextMessages
                .Include(x => x.Attachments).ThenInclude(x => x.MimeType).ThenInclude(x => x.FileTypes)
                .Include(x => x.MessageProperties).ThenInclude(x => x.PropertyType)
                .AsNoTracking()
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var models = textMessages.Select(x =>
                _mapper.Map<TextMessage>(x)
                );

            // Return the results.
            return models;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for text messages!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a text " +
                "messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<TextMessage?> FindByIdAsync(
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
                "Searching for a matching text message."
                );

            // Perform the text message search.
            var textMessage = await dbContext.TextMessages.Where(x => 
                x.Id == id
                ).Include(x => x.Attachments).ThenInclude(x => x.MimeType).ThenInclude(x => x.FileTypes)
                .Include(x => x.MessageProperties).ThenInclude(x => x.PropertyType)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Convert the entity to a model.
            var model = _mapper.Map<TextMessage>(
                textMessage
                );

            // Did we fail?
            if (textMessage is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(TextMessage)} entity to a model."
                    );
            }

            // Return the results.
            return model;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a text message by id!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a text " +
                "message by id!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<TextMessage?> FindByKeyAsync(
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
                "Searching for a matching text message."
                );

            // Perform the text message search.
            var textMessage = await dbContext.TextMessages.Where(x =>
                x.MessageKey == messageKey.ToUpper()
                ).Include(x => x.Attachments).ThenInclude(x => x.MimeType).ThenInclude(x => x.FileTypes)
                .Include(x => x.MessageProperties).ThenInclude(x => x.PropertyType)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Convert the entity to a model.
            var model = _mapper.Map<TextMessage>(
                textMessage
                );

            // Did we fail?
            if (textMessage is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(TextMessage)} entity to a model."
                    );
            }

            // Return the results.
            return model;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for a text message by key!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a text " +
                "message by key!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<TextMessage> UpdateAsync(
        TextMessage textMessage,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(textMessage, nameof(textMessage));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(TextMessage)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.TextMessage>(
                textMessage
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(TextMessage)} model to an entity."
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
                "Updating a {entity} entity in the {ctx} data-context.",
                nameof(TextMessage),
                nameof(PurpleDbContext)
                );

            // Start tracking the entity.
            dbContext.TextMessages.Attach(entity);

            // Mark the entity as modified so EFCORE will update it.
            dbContext.Entry(entity).State = EntityState.Modified;

            // We never change these 'read only' properties.
            dbContext.Entry(entity).Property(x => x.Id).IsModified = false;
            dbContext.Entry(entity).Property(x => x.MessageKey).IsModified = false;
            dbContext.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            dbContext.Entry(entity).Property(x => x.CreatedOnUtc).IsModified = false;
            dbContext.Entry(entity).Property(x => x.ProcessAfterUtc).IsModified = false;
            dbContext.Entry(entity).Property(x => x.ArchiveAfterUtc).IsModified = false;

            // Is there an associated provider type ?
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

            // Find the message again so we pick up any attachments, and/or
            //   properties that we removed, on the entity, earlier (see above).
            var result = await FindByIdAsync(
                entity.Id,
                cancellationToken
                ).ConfigureAwait(false);

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new InvalidOperationException(
                    $"Failed to find the message: {entity.Id}!"
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
                "Failed to update a text message!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to update a text message!",
                innerException: ex
                );
        }
    }

    #endregion
}
