
namespace CG.Purple.SqlServer.Repositories;

/// <summary>
/// This class is an EFCORE implementation of the <see cref="IMimeTypeRepository"/>
/// interface.
/// </summary>
internal class MimeTypeRepository : IMimeTypeRepository
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
    internal protected readonly ILogger<IMimeTypeRepository> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MimeTypeRepository"/>
    /// class.
    /// </summary>
    /// <param name="dbContextFactory">The EFCORE data-context factory
    /// to use with this repository.</param>
    /// <param name="mapper">The auto-mapper to use with this repository.</param>
    /// <param name="logger">The logger to use with this repository.</param>
    public MimeTypeRepository(
        IDbContextFactory<PurpleDbContext> dbContextFactory,
        IMapper mapper,
        ILogger<IMimeTypeRepository> logger
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
                "Searching for mime types"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.MimeTypes.AnyAsync(
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
                "Failed to search for mime types"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for mime types!",
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
                "Searching for mime types"
                );

            // Search for any entities in the data-store.
            var data = await dbContext.MimeTypes.CountAsync(
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
                "Failed to count mime types"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to count mime types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MimeType> CreateAsync(
        MimeType mimeType,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(MimeType)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.MimeType>(
                mimeType
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MimeType)} model to an entity."
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
                nameof(MimeType),
                nameof(PurpleDbContext)
                );

            // Add the entity to the data-store.
            _ = await dbContext.MimeTypes.AddAsync(
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
                nameof(MimeType)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<MimeType>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MimeType)} entity to a model."
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
                "Failed to create a mime type"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to create a mime type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        MimeType model,
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
                nameof(MimeType),
                nameof(PurpleDbContext)
                );

            // Delete from the data-store.
            await dbContext.Database.ExecuteSqlRawAsync(
                "DELETE FROM [Purple].[MimeTypes] WHERE [Id] = {0}",
                parameters: new object[] { model.Id },
                cancellationToken: cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to delete a mime type"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to delete a mime type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<MimeType>> FindByTypeAsync(
        string type,
        string subType,        
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

            List<Entities.MimeType> data = new();

            // Log what we are about to do.
            _logger.LogDebug(
                "Searching for matching mime type(s)."
                );            

            // Search for all mime types.
            if (string.IsNullOrEmpty(type) && string.IsNullOrEmpty(subType)) 
            {
                data = await dbContext.MimeTypes
                    .OrderBy(x => x.Type)
                    .ThenBy(x => x.SubType)
                    .Include(x => x.FileTypes)
                    .ToListAsync(
                        cancellationToken
                        ).ConfigureAwait(false);
            }

            // Search for mime types that match the type.
            else if (!string.IsNullOrEmpty(type) && string.IsNullOrEmpty(subType))
            {
                data = await dbContext.MimeTypes.Where(x => 
                    x.Type == type
                    ).OrderBy(x => x.Type)
                    .ThenBy(x => x.SubType)
                    .Include(x => x.FileTypes)
                    .ToListAsync(
                        cancellationToken
                        ).ConfigureAwait(false);
            }

            // Search for mime types that match the sub-type.
            else if (string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(subType))
            {
                data = await dbContext.MimeTypes.Where(x => 
                    x.SubType == subType
                    ).OrderBy(x => x.Type)
                    .ThenBy(x => x.SubType)
                    .Include(x => x.FileTypes)
                    .ToListAsync(
                        cancellationToken
                        ).ConfigureAwait(false);
            }

            // Search for mime types that match the type and sub-type.
            else
            {
                data = await dbContext.MimeTypes.Where(x => 
                    x.Type == type && 
                    x.SubType == subType
                    ).OrderBy(x => x.Type)
                    .ThenBy(x => x.SubType)
                    .Include(x => x.FileTypes)
                    .ToListAsync(
                        cancellationToken
                        ).ConfigureAwait(false);
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting the {entity} entities to models",
                nameof(MimeType)
                );

            // Convert the entities to models.
            var models = data.Select(x => 
                _mapper.Map<MimeType>(x)
                );

            // Return the result.
            return models;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for mime types by type and/or sub-type"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for mime " +
                "types by type and/or sub-type!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MimeType?> FindByExtensionAsync(
        string extension,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(extension, nameof(extension));

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
                "Searching for a matching file type."
                );

            // Perform the file type search.
            var fileType = await dbContext.FileTypes.Where(x => 
                x.Extension == extension
                ).FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Did we fail?
            if (fileType is null)
            {
                return null; // Not found!
            }

            // Perform the mime type search.
            var entity = await dbContext.MimeTypes.Where(x =>
                x.Id == fileType.MimeTypeId
                ).Include(x => x.FileTypes)
                .FirstOrDefaultAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

            // Convert the entity to a model.
            var model = _mapper.Map<MimeType>(
                entity
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MimeType)} entity to a model."
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
                "Failed to search for a mime type by extension"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to search for a mime " +
                "type by extension",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MimeType> UpdateAsync(
        MimeType mimeType,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Converting a {entity} model to an entity",
                nameof(MimeType)
                );

            // Convert the model to an entity.
            var entity = _mapper.Map<Entities.MimeType>(
                mimeType
                );

            // Did we fail?
            if (entity is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MimeType)} model to an entity."
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

            // We update entity properties but not any associated entities.
            dbContext.Entry(entity).State = EntityState.Modified;
            dbContext.Entry(entity.FileTypes).State = EntityState.Unchanged;

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating a {entity} entity in the {ctx} data-context.",
                nameof(MimeType),
                nameof(PurpleDbContext)
                );

            // Update the data-store.
            _= dbContext.MimeTypes.Update(
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
                nameof(MimeType)
                );

            // Convert the entity to a model.
            var result = _mapper.Map<MimeType>(
                entity
                );

            // Did we fail?
            if (result is null)
            {
                // Panic!!
                throw new AutoMapperMappingException(
                    $"Failed to map the {nameof(MimeType)} entity to a model."
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
                "Failed to update a mime type"
                );

            // Provider better context.
            throw new RepositoryException(
                message: $"The repository failed to update a mime type!",
                innerException: ex
                );
        }
    }

    #endregion
}
