
namespace CG.Purple.SqlServer.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="IMailMessageRepository"/>
/// interface.
/// </summary>
internal class MailMessageRepository : IMailMessageRepository
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
    internal protected readonly ILogger<IMailMessageRepository> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MailMessageRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContextFactory">The EFCORE data-context factory
    /// to use with this repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public MailMessageRepository(
        IDbContextFactory<PurpleDbContext> dbContextFactory,
        IMapper mapper,
        ILogger<IMailMessageRepository> logger
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
                "Searching for mail messages"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.MailMessages.AnyAsync(
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
                "Failed to search for mail messages"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for mail messages!",
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
                "Searching for mail messages"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.MailMessages.CountAsync(
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
                "Failed to count mail messages"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count mail messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MailMessage> CreateAsync(
        MailMessage mailMessage,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(MailMessage)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.MailMessage>(
                mailMessage
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MailMessage)} model to an entity."
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
                nameof(MailMessage),
                nameof(PurpleDbContext)
                );            

            // Add the entity to the data-store.
            _ = await dbContext.MailMessages.AddAsync(
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
                nameof(MailMessage)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<MailMessage>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MailMessage)} entity to a model."
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
                "Failed to create a mail message"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create a mail message!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        MailMessage model,
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
                nameof(MailMessage),
                nameof(PurpleDbContext)
                );

            // Delete from the data-store.
            await dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM [Purple].[MailMessages] WHERE [Id] = {0}",
                parameters: new object[] { model.Id },
                cancellationToken: cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a mail message"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to delete a mail message!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<MailMessage>> FindAllAsync(
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
                "Searching mail messages."
                );

            // Perform the mail message search.
            var mailMessages = await dbContext.MailMessages
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var result = mailMessages.Select(x =>
                _mapper.Map<MailMessage>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for mail messages"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a mail messages",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MailMessage?> FindByIdAsync(
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
                "Searching for a matching mail message."
                );

            // Perform the mail message search.
            var mailMessage = await dbContext.MailMessages.Where(x => 
                x.Id == id
                ).FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Did we fail?
            if (mailMessage is null)
            {
                return null; // Nothing found!
            }

            // Convert the entity to a model.
            var result = _mapper.Map<MailMessage>(
                mailMessage
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MailMessage)} entity to a model."
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
                "Failed to search for a mail message by id"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a mail " +
                "message by id",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MailMessage?> FindByKeyAsync(
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
                "Searching for a matching mail message."
                );

            // Perform the mail message search.
            var mailMessage = await dbContext.MailMessages.Where(x =>
                x.MessageKey == messageKey.ToUpper()
                ).FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Did we fail?
            if (mailMessage is null) 
            {
                return null; // Nothing found!
            }

            // Convert the entity to a model.
            var result = _mapper.Map<MailMessage>(
                mailMessage
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MailMessage)} entity to a model."
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
                "Failed to search for a mail message by key"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a mail " +
                "message by key",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MailMessage> UpdateAsync(
        MailMessage mailMessage,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(MailMessage)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.MailMessage>(
                mailMessage
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MailMessage)} model to an entity."
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
            dbContext.Entry(entity.MessageKey).State = EntityState.Unchanged;
            dbContext.Entry(entity.CreatedBy).State = EntityState.Unchanged;
            dbContext.Entry(entity.CreatedOnUtc).State = EntityState.Unchanged;
            dbContext.Entry(entity.Id).State = EntityState.Unchanged;
            dbContext.Entry(entity.MessageKey).State = EntityState.Unchanged;

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating a {entity} entity in the {ctx} data-context.",
                nameof(MailMessage),
                nameof(PurpleDbContext)
                );

            // Update the data-store.
            _= dbContext.MailMessages.Update(
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
                nameof(MailMessage)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<MailMessage>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MailMessage)} entity to a model."
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
                "Failed to update a mail message"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to update a mail message!",
                innerException: ex
                );
        }
    }

    #endregion
}
