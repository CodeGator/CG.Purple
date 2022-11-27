﻿
using CG.Purple.Options;

namespace CG.Purple.Managers;

/// <summary>
/// This class is a default implementation of the <see cref="IMailMessageManager"/>
/// interface.
/// </summary>
internal class MailMessageManager : IMailMessageManager
{
    // *******************************************************************
    // Constants.
    // *******************************************************************

    #region Constants

    /// <summary>
    /// This constants contains the cache key for this manager.
    /// </summary>
    internal protected const string CACHE_KEY = "MailMessageManager";

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the business logic layer options for this manager.
    /// </summary>
    internal protected readonly MailMessageOptions? _mailMessageOptions;

    /// <summary>
    /// This field contains the repository for this manager.
    /// </summary>
    internal protected readonly IMailMessageRepository _mailMessageRepository = null!;

    /// <summary>
    /// This field contains the distributed cache for this manager.
    /// </summary>
    internal protected IDistributedCache _distributedCache = null!;

    /// <summary>
    /// This field contains the logger for this manager.
    /// </summary>
    internal protected readonly ILogger<IMailMessageManager> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MailMessageManager"/>
    /// class.
    /// </summary>
    /// <param name="bllOptions">The business logic layer options to use 
    /// with this manager.</param>
    /// <param name="mailMessageRepository">The mail message repository to use
    /// with this manager.</param>
    /// <param name="distributedCache">The distributed cache to use for 
    /// this manager.</param>
    /// <param name="logger">The logger to use with this manager.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public MailMessageManager(
        IOptions<BllOptions> bllOptions,
        IMailMessageRepository mailMessageRepository,
        IDistributedCache distributedCache,
        ILogger<IMailMessageManager> logger
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(bllOptions, nameof(bllOptions))
            .ThrowIfNull(mailMessageRepository, nameof(mailMessageRepository))
            .ThrowIfNull(distributedCache, nameof(distributedCache))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s)
        _mailMessageOptions = bllOptions.Value?.MailMessages;
        _mailMessageRepository = mailMessageRepository;
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
                nameof(IMailMessageRepository.AnyAsync)
                );

            // Perform the search.
            return await _mailMessageRepository.AnyAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to search for mail messages!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for mail messages!",
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
                nameof(IMailMessageRepository.CountAsync)
                );

            // Perform the search.
            var result = await _mailMessageRepository.CountAsync(
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
                "Failed to count mail messages!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to count mail messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MailMessage> CreateAsync(
        MailMessage mailMessage,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mailMessage, nameof(mailMessage))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MailMessage)
                );

            // Ensure the stats are correct.
            mailMessage.CreatedOnUtc = DateTime.UtcNow;
            mailMessage.CreatedBy = userName;
            mailMessage.LastUpdatedBy = null;
            mailMessage.LastUpdatedOnUtc = null;

            // Nothing else makes any sense for this type.
            mailMessage.MessageType = MessageType.Mail;

            // Always create messages in this state.
            mailMessage.MessageState = MessageState.Pending;

            // Should we generate a message key?
            if (string.IsNullOrEmpty(mailMessage.MessageKey))
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Generating a unique key for the message."
                    );

                // Generate the unique message key.
                mailMessage.MessageKey = $"{Guid.NewGuid()}";
            }            

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMailMessageRepository.CreateAsync)
                );

            // Perform the operation.
            var result = await _mailMessageRepository.CreateAsync(
                mailMessage,
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
                "Failed to create a new mail message!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to create a new mail message!",
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
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMailMessageRepository.FindAllAsync)
                );

            // Perform the operation.
            var result = await _mailMessageRepository.FindAllAsync(
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
                "Failed to search for mail messages!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for mail " +
                "messages!",
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
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfZero(id, nameof(id));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMailMessageRepository.FindByIdAsync)
                );

            // Perform the operation.
            var result = await _mailMessageRepository.FindByIdAsync(
                id,
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
                "Failed to search for a mail message by id!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for a mail " +
                "message by id!",
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
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNullOrEmpty(messageKey, nameof(messageKey));

        try
        {
            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMailMessageRepository.FindByKeyAsync)
                );

            // Perform the operation.
            var result = await _mailMessageRepository.FindByKeyAsync(
                messageKey,
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
                "Failed to search for a mail message by key!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to search for a mail " +
                "message by key!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task<MailMessage> UpdateAsync(
        MailMessage mailMessage,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mailMessage, nameof(mailMessage))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the {name} model stats",
                nameof(MailMessage)
                );

            // Ensure the stats are correct.
            mailMessage.LastUpdatedOnUtc = DateTime.UtcNow;
            mailMessage.LastUpdatedBy = userName;

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring to {name}",
                nameof(IMailMessageRepository.UpdateAsync)
                );

            // Perform the operation.
            var result = await _mailMessageRepository.UpdateAsync(
                mailMessage,
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
                "Failed to update a mail message!"
                );

            // Provider better context.
            throw new ManagerException(
                message: $"The manager failed to update a mail message!",
                innerException: ex
                );
        }
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method gets the cached data for this manager.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the cached 
    /// data for this manager.</returns>
    private async Task<List<MailMessage>> GetCachedDataAsync(
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogTrace(
            "Deferring to IDistributedCache.GetOrSetAsync"
            );

        // Get (set) the cached data for this manager.
        var mailMessages = await _distributedCache.GetOrSetAsync<List<MailMessage>>(
            CACHE_KEY,
        new DistributedCacheEntryOptions()
        {
            SlidingExpiration = _mailMessageOptions?.DefaultCacheDuration
                ?? TimeSpan.FromHours(1)
        },
        () =>
        {
            return (_mailMessageRepository.FindAllAsync(
                cancellationToken
                ).Result).ToList();
        },
        cancellationToken
        ).ConfigureAwait(false);

        // Return the results.
        return mailMessages;
    }

    #endregion
}
