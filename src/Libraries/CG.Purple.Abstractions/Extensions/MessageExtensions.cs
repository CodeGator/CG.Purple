
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
    /// <see cref="MessageState.Sent"/> state, and records the event
    /// in the processing log.
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
    public static async Task ToSentStateAsync(
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

        // The message is now in a 'Sent' state.
        message.MessageState = MessageState.Sent;

        // Update the message.
        _ = await messageManager.UpdateAsync(
            message,
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Record what we did, in the log.
        _ = await messageLogManager.LogSentEventAsync(
            message,
            oldMessageState,
            userName,
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method transitions the given <see cref="Message"/> to a 
    /// <see cref="MessageState.Pending"/> state, and records the event
    /// in the processing log.
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

        // Update the message.
        _ = await messageManager.UpdateAsync(
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
    /// <see cref="MessageState.Failed"/> state, and records the event
    /// in the processing log.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="ex">The exception to use for the operation.</param>
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
    public static async Task ToFailedStateAsync(
        this Message message,
        Exception ex,
        IMessageManager messageManager,
        IMessageLogManager messageLogManager,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Call the overload.
        await message.ToFailedStateAsync(
            ex.GetBaseException().Message,
            messageManager,
            messageLogManager,
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
    /// <param name="ex">The exception to use for the operation.</param>
    /// <param name="messageManager">The message manager to use for the
    /// operation.</param>
    /// <param name="messageLogManager">The message log manager to use 
    /// for the operation.</param>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task ToFailedStateAsync(
        this Message message,
        Exception ex,
        IMessageManager messageManager,
        IMessageLogManager messageLogManager,
        ProviderType providerType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Call the overload.
        await message.ToFailedStateAsync(
            ex.GetBaseException().Message,
            messageManager,
            messageLogManager,
            providerType,
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
    /// <param name="errorMessage">The error message to use for the operation.</param>
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
    public static async Task ToFailedStateAsync(
        this Message message,
        string errorMessage,
        IMessageManager messageManager,
        IMessageLogManager messageLogManager,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Remember the previous state.
        var oldMessageState = message.MessageState;

        // The message is now in a 'Failed' state.
        message.MessageState = MessageState.Failed;

        // Update the message.
        _ = await messageManager.UpdateAsync(
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

    // *******************************************************************

    /// <summary>
    /// This method transitions the given <see cref="Message"/> to a 
    /// <see cref="MessageState.Failed"/> state, and records the event
    /// in the processing log.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="errorMessage">The error message to use for the operation.</param>
    /// <param name="messageManager">The message manager to use for the
    /// operation.</param>
    /// <param name="messageLogManager">The message log manager to use 
    /// for the operation.</param>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task ToFailedStateAsync(
        this Message message,
        string errorMessage,
        IMessageManager messageManager,
        IMessageLogManager messageLogManager,
        ProviderType providerType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNull(providerType, nameof(providerType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Remember the previous state.
        var oldMessageState = message.MessageState;

        // The message is now in a 'Failed' state.
        message.MessageState = MessageState.Failed;

        // Update the message.
        _ = await messageManager.UpdateAsync(
            message,
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Record what we did, in the log.
        _ = await messageLogManager.LogErrorEventAsync(
            message,
            oldMessageState,
            errorMessage,
            providerType,
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
        _ = await messageManager.UpdateAsync(
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
    /// This method resets the error count on the message.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="messageManager">The message manager to use for the 
    /// operation.</param>
    /// <param name="userName">The user name of the person performing the
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task ClearErrorCountAsync(
        this Message message,
        IMessageManager messageManager,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Clear the error count on the message.
        message.ErrorCount = 0;
        await messageManager.UpdateAsync(
            message,
            userName,
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method increments the error count on the message, by 1.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="messageManager">The message manager to use for the 
    /// operation.</param>
    /// <param name="userName">The user name of the person performing the
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task BumpErrorCountAsync(
        this Message message,
        IMessageManager messageManager,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Bump the error count on the message.
        message.ErrorCount += 1;
        await messageManager.UpdateAsync(
            message,
            userName,
            cancellationToken
            ).ConfigureAwait(false);
    }

    #endregion
}
