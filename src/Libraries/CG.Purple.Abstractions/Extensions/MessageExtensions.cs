
using System;

namespace CG.Purple.Providers;

/// <summary>
/// This class contains extension methods related to the <see cref="Message"/>
/// type.
/// </summary>
public static class MessageExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method transitions the given <see cref="Message"/> to a 
    /// <see cref="MessageState.Pending"/> state, clears any currently 
    /// assigned provider type, and records the event in the message log.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="messageManager">The message manager to use for the
    /// operation.</param>
    /// <param name="messageLogManager">The message log manager to use 
    /// for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task ToPendingStateAsync(
        this Message message,
        IMessageManager messageManager,
        IMessageLogManager messageLogManager,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Remember the previous state.
        var oldMessageState = message.MessageState;

        // The message is now in a 'Pending' state.
        message.MessageState = MessageState.Pending;

        // Clear the currently assigned provider type (if any).
        message.ProviderType = null;

        // Update the message.
        message = await messageManager.UpdateAsync(
            message,
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Record what we did, in the log.
        _ = await messageLogManager.LogResetEventAsync(
            message,
            oldMessageState,
            userName,
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method transitions the given <see cref="Message"/> to a 
    /// <see cref="MessageState.Processing"/> state, and records the event
    /// in the processing log.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="messageManager">The message manager to use for the
    /// operation.</param>
    /// <param name="messageLogManager">The message log manager to use 
    /// for the operation.</param>
    /// <param name="assignedProviderType">The assigned provider type to use
    /// for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task ToProcessingStateAsync(
        this Message message,
        IMessageManager messageManager,
        IMessageLogManager messageLogManager,
        ProviderType assignedProviderType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNull(assignedProviderType, nameof(assignedProviderType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Remember the previous state.
        var oldMessageState = message.MessageState;

        // The message is now in a 'Processing' state.
        message.MessageState = MessageState.Processing;

        // Update the message.
        message = await messageManager.UpdateAsync(
            message,
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Record what we did, in the log.
        _ = await messageLogManager.LogAssignedEventAsync(
            message,
            oldMessageState,
            assignedProviderType,
            userName,
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method transitions the given <see cref="Message"/> to a 
    /// <see cref="MessageState.Failed"/> state, and records the event
    /// in the processing log.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="messageManager">The message manager to use for the
    /// operation.</param>
    /// <param name="messageLogManager">The message log manager to use 
    /// for the operation.</param>
    /// <param name="errorMessage">The error message to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task ToFailedStateAsync(
        this Message message,
        IMessageManager messageManager,
        IMessageLogManager messageLogManager,
        string errorMessage,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Remember the previous state.
        var oldMessageState = message.MessageState;

        // The message is now in a 'Failed' state.
        message.MessageState = MessageState.Failed;

        // Bump up the error count.
        message.ErrorCount++;

        // Adjust the next process time by a random number of seconds.
        var randomSeconds = Random.Shared.Next(1, 60);
        message.ProcessAfterUtc = message.ProcessAfterUtc.HasValue
            ? message.ProcessAfterUtc.Value.AddSeconds(randomSeconds)
            : DateTime.UtcNow.AddSeconds(randomSeconds);

        // Adjust the archive time (if any) to account for any change
        //   we made to the processing time.
        message.ArchiveAfterUtc = message.ArchiveAfterUtc.HasValue
            ? message.ArchiveAfterUtc.Value.AddSeconds(randomSeconds)
            : null;

        // Update the message.
        message = await messageManager.UpdateAsync(
            message,
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Record what we did, in the log.
        _ = await messageLogManager.LogErrorEventAsync(
            message,
            oldMessageState,
            errorMessage,
            userName,
            cancellationToken
            ).ConfigureAwait(false);
    }

    #endregion
}
