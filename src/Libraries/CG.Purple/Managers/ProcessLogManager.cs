﻿
namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IProcessLogManager"/>
/// interface.
/// </summary>
internal class ProcessLogManager : IProcessLogManager
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IProcessLogRepository _processLogRepository = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IProcessLogManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProcessLogManager"/>
    /// class.
    /// </summary>
    /// <param name="processLogRepository">The process log repository to use
    /// with this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public ProcessLogManager(
        IProcessLogRepository processLogRepository,
        ILogger<IProcessLogManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogRepository, nameof(processLogRepository))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _processLogRepository = processLogRepository;
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
                nameof(IProcessLogRepository.AnyAsync)
                );

            // Perform the search.
            return await _processLogRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for process logs!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for process logs!",
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
                nameof(IProcessLogRepository.CountAsync)
                );

            // Perform the search.
            return await _processLogRepository.CountAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to count process logs!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count process logs!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<PipelineLog> CreateAsync(
        PipelineLog processLog,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(PipelineLog)
                );

            // Ensure the stats are correct.
            processLog.CreatedOnUtc = DateTime.UtcNow;
            processLog.CreatedBy = userName;
            processLog.LastUpdatedBy = null;
            processLog.LastUpdatedOnUtc = null;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProcessLogRepository.CreateAsync)
                );

            // Perform the operation.
            return await _processLogRepository.CreateAsync(
                processLog,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to create a new process log!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new process log!",
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
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProcessLogRepository.FindAllAsync)
                );

            // Perform the operation.
            var result = await _processLogRepository.FindAllAsync(
                cancellationToken
                ).ConfigureAwait(false);

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
            throw new ManagerException(
                message: $"The manager failed to search for  " +
                "process logs!",
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
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProcessLogRepository.FindByMessageAsync)
                );

            // Perform the operation.
            var result = await _processLogRepository.FindByMessageAsync(
                message,
                cancellationToken
                ).ConfigureAwait(false);

            // Return the results.
            return result;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for process logs for a message!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for  " +
                "process logs for a message!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(
        PipelineLog processLog,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(PipelineLog)
                );

            // Ensure the stats are correct.
            processLog.LastUpdatedOnUtc = DateTime.UtcNow;
            processLog.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProcessLogRepository.DeleteAsync)
                );

            // Perform the operation.
            await _processLogRepository.DeleteAsync(
                processLog,
                cancellationToken
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
            throw new ManagerException(
                message: $"The manager failed to delete a process log!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<PipelineLog> UpdateAsync(
        PipelineLog processLog,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(PipelineLog)
                );

            // Ensure the stats are correct.
            processLog.LastUpdatedOnUtc = DateTime.UtcNow;
            processLog.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IProcessLogRepository.UpdateAsync)
                );

            // Perform the operation.
            return await _processLogRepository.UpdateAsync(
                processLog,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to update a process log!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a process log!",
                innerException: ex
                );
        }
    }

    #endregion
}
