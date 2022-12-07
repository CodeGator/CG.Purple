
namespace CG.Purple.Managers;

/// <summary>
/// This class contains extension methods related to the <see cref="IMessageLogManager"/>
/// type.
/// </summary>
public static class PipelineLogManagerExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method writes an <see cref="MessageEvent.Assigned"/> event to
    /// the processing log, for an even that caused a state transition in
    /// the associated message.
    /// </summary>
    /// <param name="messageLogManager">The message log manager to use
    /// for the operation.</param>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="previousMessageState">The message state before the 
    /// event took place.</param>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogAssignedEventAsync(
        this IMessageLogManager messageLogManager,
        Message message,
        MessageState previousMessageState,
        ProviderType providerType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNull(message, nameof(message))
            .ThrowIfNull(providerType, nameof(providerType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await messageLogManager.CreateAsync(
            new MessageLog()
            {
                ProviderType = providerType,
                Message = message,
                MessageEvent = MessageEvent.Assigned,
                BeforeState = previousMessageState,
                AfterState = message.MessageState   
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    // *******************************************************************

    /// <summary>
    /// This method writes a <see cref="MessageEvent.Reset"/> event to
    /// the processing log, for an even that caused a state transition in
    /// the associated message.
    /// </summary>
    /// <param name="messageLogManager">The message log manager to use
    /// for the operation.</param>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="previousMessageState">The message state before the 
    /// event took place.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogResetEventAsync(
        this IMessageLogManager messageLogManager,
        Message message,
        MessageState previousMessageState,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await messageLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Reset,
                BeforeState = previousMessageState, 
                AfterState = message.MessageState
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    #endregion
}
