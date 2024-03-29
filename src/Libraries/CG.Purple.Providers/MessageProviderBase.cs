﻿
namespace CG.Purple.Providers;

/// <summary>
/// This class is a base implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
public abstract class MessageProviderBase<T> : IMessageProvider
    where T : MessageProviderBase<T>
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the SignalR status hub for this provider.
    /// </summary>
    internal protected readonly StatusHub _statusHub;

    /// <summary>
    /// This field contains the message manager for this provider.
    /// </summary>
    internal protected readonly IMessageManager _messageManager = null!;

    /// <summary>
    /// This field contains the message log manager for this provider.
    /// </summary>
    internal protected readonly IMessageLogManager _messageLogManager = null!;

    /// <summary>
    /// This field contains the logger for this provider.
    /// </summary>
    internal protected readonly ILogger<T> _logger;

    /// <summary>
    /// This field contains the random number generator for this provider.
    /// </summary>
    internal protected readonly RandomNumberGenerator _random = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor create a new instance of the <see cref="MessageProviderBase{T}"/>
    /// class.
    /// </summary>
    /// <param name="statusHub">The SignalR status hub to use with this 
    /// provider.</param>
    /// <param name="messageManager">The message manager to use with this 
    /// provider.</param>
    /// <param name="messageLogManager">The message log manager to use
    /// with this provider.</param>
    /// <param name="logger">The logger to use with this provider.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    protected MessageProviderBase(
        StatusHub statusHub,
        IMessageManager messageManager,
        IMessageLogManager messageLogManager,
        ILogger<T> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(statusHub, nameof(statusHub))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _statusHub = statusHub;
        _messageManager = messageManager;
        _messageLogManager = messageLogManager;
        _logger = logger;
        _random = RandomNumberGenerator.Create();
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public abstract Task ProcessMessagesAsync(
        IEnumerable<Message> messages,
        ProviderType providerType,
        CancellationToken cancellationToken = default
        );

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method sets the given message to a 'Failed' state, resets any
    /// associated provider type, increments the error count on the message,
    /// and logs the error.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    protected virtual async Task MessageIsWrongTypeAsync(
        Message message,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Setting message: {id} to a failed state, clearing any " +
            "provider, and bumping the error count",
            message.Id
            );

        // Remember any assigned provider type.
        var oldProviderType = message.ProviderType;

        // Update the message.
        message.ProviderType = null;
        message.ErrorCount++;
        message.MessageState = MessageState.Failed;

        // Save the changes.
        _ = await _messageManager.UpdateAsync(
            message,
            "host",
            cancellationToken
            ).ConfigureAwait(false);

        // Log what we are about to do.
        _logger.LogDebug(
            "Writing error event to the message log for message: {id}",
            message.Id
            );

        // Record what we did, in the log.
        var result = await _messageLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Error,
                Error = "Provider is incapable of processing messages of this type!",
                ProviderType = oldProviderType
            },
            "host",
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method sets the given message to a 'Failed' state, resets any
    /// associated provider type, increments the error count on the message,
    /// and logs the error.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    protected virtual async Task UnableToFindMailMessageAsync(
        Message message,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Setting message: {id} to a 'Failed' state, clearing any " +
            "provider, and bumping the error count",
            message.Id
            );

        // Remember any assigned provider type.
        var oldProviderType = message.ProviderType;

        // Update the message.
        message.ProviderType = null;
        message.ErrorCount++;
        message.MessageState = MessageState.Failed;

        // Save the changes.
        _ = await _messageManager.UpdateAsync(
            message,
            "host",
            cancellationToken
            ).ConfigureAwait(false);

        // Log what we are about to do.
        _logger.LogDebug(
            "Writing error event to the message log for message: {id}",
            message.Id
            );

        // Record what we did, in the log.
        var result = await _messageLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Error,
                Error = "Unable to find the mail part of the message!",
                ProviderType = oldProviderType
            },
            "host",
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method sets the given message to a 'Failed' state, resets any
    /// associated provider type, increments the error count on the message,
    /// and logs the error.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    protected virtual async Task UnableToFindTextMessageAsync(
        Message message,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Setting message: {id} to a 'Failed' state, clearing any " +
            "provider, and bumping the error count",
            message.Id
            );

        // Remember any assigned provider type.
        var oldProviderType = message.ProviderType;

        // Update the message.
        message.ProviderType = null;
        message.ErrorCount++;
        message.MessageState = MessageState.Failed;

        // Save the changes.
        _ = await _messageManager.UpdateAsync(
            message,
            "host",
            cancellationToken
            ).ConfigureAwait(false);

        // Log what we are about to do.
        _logger.LogDebug(
            "Writing error event to the message log for message: {id}",
            message.Id
            );

        // Record what we did, in the log.
        var result = await _messageLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Error,
                Error = "Unable to find the text part of the message!",
                ProviderType = oldProviderType
            },
            "host",
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method sets the given message to a 'Sent' state and logs the 
    /// event.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    protected virtual async Task MessageWasSentAsync(
        Message message,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Setting message: {id} to a 'Sent' state",
            message.Id
            );

        // Remember the previous state.
        var oldMessageState = message.MessageState;

        // Update the message.
        message.MessageState = MessageState.Sent;

        // Save the changes.
        _ = await _messageManager.UpdateAsync(
            message,
            "host",
            cancellationToken
            ).ConfigureAwait(false);

        // Log what we are about to do.
        _logger.LogDebug(
            "Logging that the message: {id} was sent.",
            message.Id
            );

        // Record what we did, in the log.
        var result = await _messageLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                ProviderType = message.ProviderType,
                MessageEvent = MessageEvent.Sent,
                BeforeState = oldMessageState,
                AfterState = message.MessageState
            },
            "host",
            cancellationToken
            ).ConfigureAwait(false);

        // Log what we are about to do.
        _logger.LogDebug(
            "Sending a SignalR status update for message: {id}.",
            message.Id
            );

        // Send the status update.
        await _statusHub.OnStatusAsync(
            message,
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method sets the given message to a 'Failed' state, bumps the
    /// error count, clears any previously assigned provider, and logs the 
    /// event.
    /// </summary>
    /// <param name="providerMessage">The provider message to use for the 
    /// operation.</param>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    protected virtual async Task MessageFailedToSendAsync(
        string providerMessage,
        Message message,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Setting message: {id} to a 'Failed' state",
            message.Id
            );

        // Remember any assigned provider type.
        var oldProviderType = message.ProviderType;

        // Clear the provider.
        message.ProviderType = null;

        // Bump up the error count.
        message.ErrorCount++;

        // Set the state to failed.
        message.MessageState = MessageState.Failed;

        // Adjust the next process time by a random number of seconds,
        //   in case the pipeline is just temporarily clogged.
        var randomSeconds = _random.Next(1, 60);
        message.ProcessAfterUtc = message.ProcessAfterUtc.HasValue
            ? message.ProcessAfterUtc.Value.AddSeconds(randomSeconds)
            : DateTime.UtcNow.AddSeconds(randomSeconds);

        // Adjust the archive time (if any) to account for any change
        //   we made to the processing time.
        message.ArchiveAfterUtc = message.ArchiveAfterUtc.HasValue
            ? message.ArchiveAfterUtc.Value.AddSeconds(randomSeconds)
            : null;

        // Save the changes.
        _ = await _messageManager.UpdateAsync(
            message,
            "host",
            cancellationToken
            ).ConfigureAwait(false);

        // Log what we are about to do.
        _logger.LogDebug(
            "Logging that the message: {id} failed to send.",
            message.Id
            );

        // Record what we did, in the log.
        var result = await _messageLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Error,
                Error = providerMessage,
                ProviderType = oldProviderType
            },
            "host",
            cancellationToken
            ).ConfigureAwait(false);

        // Log what we are about to do.
        _logger.LogDebug(
            "Sending a SignalR status update for message: {id}.",
            message.Id
            );

        // Send the status update.
        await _statusHub.OnStatusAsync(
            message,
            cancellationToken
            ).ConfigureAwait(false);
    }

    #endregion
}
