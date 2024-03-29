﻿
namespace CG.Purple.SqlServer.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="IAttachmentRepository"/>
/// interface.
/// </summary>
internal class AttachmentRepository : IAttachmentRepository
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
    internal protected readonly ILogger<IAttachmentRepository> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="AttachmentRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContextFactory">The EFCORE data-context factory
    /// to use with this repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public AttachmentRepository(
        IDbContextFactory<PurpleDbContext> dbContextFactory,
        IMapper mapper,
        ILogger<IAttachmentRepository> logger
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
                "Searching for attachments"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.Attachments.AnyAsync(
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
                "Failed to search for attachments!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for attachments!",
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
                "Searching for attachments"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.Attachments.CountAsync(
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
                "Failed to count attachments!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count attachments!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<Attachment> CreateAsync(
        Attachment attachment,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(attachment, nameof(attachment));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(Attachment)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.Attachment>(
                attachment
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(Attachment)} model to an entity."
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
                nameof(Attachment),
                nameof(PurpleDbContext)
                );

            // Add the entity to the data-store.
            dbContext.Attachments.Attach(entity);

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
                nameof(Attachment)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<Attachment>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(Attachment)} entity to a model."
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
                "Failed to create an attachment!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create an attachment!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        Attachment attachment,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(attachment, nameof(attachment));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(Attachment)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.Attachment>(
                attachment
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(Attachment)} model to an entity."
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
                nameof(Attachment),
                nameof(PurpleDbContext)
                );

            // Find the tracked entity (if any).
            var trackedEntry = await dbContext.Attachments.FindAsync(
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
                nameof(Attachment),
                nameof(PurpleDbContext)
                );

            // Delete from the data-store.
            dbContext.Attachments.Remove(
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
                "Failed to delete an attachment!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to delete an attachment!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<Attachment>> FindAllAsync(
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
                "Searching for attachments."
                );

            // Perform the attachment search.
            var attachments = await dbContext.Attachments
                .Include(x => x.Message)
                .Include(x => x.MimeType)
                .AsNoTracking()
                .ToListAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Convert the entities to a models.
            var result = attachments.Select(x =>
                _mapper.Map<Attachment>(x)
                );

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for attachments!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for " +
                "attachments!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<Attachment> UpdateAsync(
        Attachment attachment,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(attachment, nameof(attachment));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(Attachment)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.Attachment>(
                attachment
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(Attachment)} model to an entity."
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
                nameof(Attachment),
                nameof(PurpleDbContext)
                );

            // Start tracking the entity.
            dbContext.Attachments.Attach(entity);

            // Mark the entity as modified so EFCORE will update it.
            dbContext.Entry(entity).State = EntityState.Modified;

            // We never change these 'read only' properties.
            dbContext.Entry(entity).Property(x => x.Id).IsModified = false;
            dbContext.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
            dbContext.Entry(entity).Property(x => x.CreatedOnUtc).IsModified = false;

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
                nameof(Attachment)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<Attachment>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(Attachment)} entity to a model."
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
                "Failed to update an attachment!"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to update an attachment!",
                innerException: ex
                );
        }
    }

    #endregion
}
