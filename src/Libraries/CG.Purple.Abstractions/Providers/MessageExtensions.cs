
namespace CG.Purple.Providers;

/// <summary>
/// This class contains extension methods related to the <see cref="Message"/>
/// type.
/// </summary>
public static partial class MessageExtensions
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
    /// <param name="processLogManager">The process log manager to use 
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
        IProcessLogManager processLogManager,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(processLogManager, nameof(processLogManager))
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
        await processLogManager.CreateAsync(
            new ProcessLog()
            {
                Message = message,
                BeforeState = oldMessageState,
                AfterState = message.MessageState,
                Event = ProcessEvent.Sent,
            },
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
    /// <param name="processLogManager">The process log manager to use 
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
        IProcessLogManager processLogManager,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(processLogManager, nameof(processLogManager))
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
        await processLogManager.CreateAsync(
            new ProcessLog()
            {
                Message = message,
                BeforeState = oldMessageState,
                AfterState = message.MessageState,
                Event = ProcessEvent.Reset,
            },
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
    /// <param name="processLogManager">The process log manager to use 
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
        IProcessLogManager processLogManager,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNull(ex, nameof(ex))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(processLogManager, nameof(processLogManager))
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
        await processLogManager.CreateAsync(
            new ProcessLog()
            {
                Message = message,
                BeforeState = oldMessageState,
                AfterState = message.MessageState,
                Event = ProcessEvent.Reset,
                Error = ex.GetBaseException().Message
            },
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
    /// <param name="processLogManager">The process log manager to use 
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
        IProcessLogManager processLogManager,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(processLogManager, nameof(processLogManager))
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
        await processLogManager.CreateAsync(
            new ProcessLog()
            {
                Message = message,
                BeforeState = oldMessageState,
                AfterState = message.MessageState,
                Event = ProcessEvent.Assigned,
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);
    }

    #endregion
}
