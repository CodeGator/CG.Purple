
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
    /// This method writes an <see cref="MessageEvent.Error"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="ex">The exception to use for the operation.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogErrorEventAsync(
        this IMessageLogManager processLogManager,
        Exception ex,        
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Call the overload.
        return await processLogManager.LogErrorEventAsync(
            ex.GetBaseException().Message,            
            userName,            
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="MessageEvent.Error"/> event to
    /// the processing log, for an even that did not cause a state transition
    /// in the associated message.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="message">The optional message to use for the operation.</param>
    /// <param name="ex">The exception to use for the operation.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogErrorEventAsync(
        this IMessageLogManager processLogManager,
        Message message,
        Exception ex,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Call the overload.
        return await processLogManager.LogErrorEventAsync(
            message,
            ex.GetBaseException().Message,
            userName,
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="MessageEvent.Error"/> event to
    /// the processing log, for an even that caused a state transition in 
    /// the associated message.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="message">The optional message to use for the operation.</param>
    /// <param name="previousMessageState">The message state before the 
    /// event took place.</param>
    /// <param name="ex">The exception to use for the operation.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogErrorEventAsync(
        this IMessageLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        Exception ex,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Call the overload.
        return await processLogManager.LogErrorEventAsync(
            message,
            previousMessageState,
            ex.GetBaseException().Message,            
            userName,
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="MessageEvent.Error"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="errorMessage">The error message to use for the operation.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogErrorEventAsync(
        this IMessageLogManager processLogManager,
        string errorMessage, 
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
            new MessageLog()
            {
                MessageEvent = MessageEvent.Error,
                Error = errorMessage
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="MessageEvent.Error"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="errorMessage">The error message to use for the operation.</param>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogErrorEventAsync(
        this IMessageLogManager processLogManager,
        string errorMessage,
        ProviderType providerType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
            new MessageLog()
            {
                MessageEvent = MessageEvent.Error,
                Error = errorMessage,
                ProviderType = providerType
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="MessageEvent.Error"/> event to
    /// the processing log, for an even that caused a state transition in 
    /// the associated message.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="previousMessageState">The message state before the 
    /// event took place.</param>
    /// <param name="errorMessage">The error message to use for the operation.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogErrorEventAsync(
        this IMessageLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        string errorMessage,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Error,
                Error = errorMessage,
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
    /// This method writes an <see cref="MessageEvent.Error"/> event to
    /// the processing log, for an even that caused a state transition in 
    /// the associated message.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="previousMessageState">The message state before the 
    /// event took place.</param>
    /// <param name="errorMessage">The error message to use for the operation.</param>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogErrorEventAsync(
        this IMessageLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        string errorMessage,
        ProviderType providerType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage))
            .ThrowIfNull(providerType, nameof(providerType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Error,
                Error = errorMessage,
                BeforeState = previousMessageState,
                AfterState = message.MessageState,
                ProviderType = providerType
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="MessageEvent.Error"/> event to
    /// the processing log, for an even that did not cause a state transition 
    /// in the associated message.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="errorMessage">The error message to use for the operation.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogErrorEventAsync(
        this IMessageLogManager processLogManager,
        Message message,
        string errorMessage,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNullOrEmpty(errorMessage, nameof(errorMessage))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Error,
                Error = errorMessage
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="MessageEvent.Stored"/> event to
    /// the processing log, for an even that did not cause a state transition 
    /// in the associated message.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="MessageLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<MessageLog> LogStoredEventAsync(
        this IMessageLogManager processLogManager,
        Message message,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Stored,
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
    /// This method writes an <see cref="MessageEvent.Assigned"/> event to
    /// the processing log, for an even that caused a state transition in
    /// the associated message.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
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
        this IMessageLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        ProviderType providerType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(message, nameof(message))
            .ThrowIfNull(providerType, nameof(providerType))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
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
    /// <param name="processLogManager">The process log manager to use
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
        this IMessageLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
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

    // *******************************************************************

    /// <summary>
    /// This method writes a <see cref="MessageEvent.Disabled"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
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
    public static async Task<MessageLog> LogDisabledEventAsync(
        this IMessageLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Disabled,
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
    /// This method writes an <see cref="MessageEvent.Enabled"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
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
    public static async Task<MessageLog> LogEnabledEventAsync(
        this IMessageLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Enabled,
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
    /// This method writes an <see cref="MessageEvent.Sent"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
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
    public static async Task<MessageLog> LogSentEventAsync(
        this IMessageLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                ProviderType = message.ProviderType,
                MessageEvent = MessageEvent.Sent,
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
    /// This method writes an <see cref="MessageEvent.Sent"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
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
    public static async Task<MessageLog> LogSentEventAsync(
        this IMessageLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        ProviderType? providerType,
        string userName,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(message, nameof(message))
            .ThrowIfNullOrEmpty(userName, nameof(userName));

        // Record what we did, in the log.
        var result = await processLogManager.CreateAsync(
            new MessageLog()
            {
                Message = message,
                MessageEvent = MessageEvent.Sent,
                BeforeState = previousMessageState,
                AfterState = message.MessageState,
                ProviderType = providerType
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    #endregion
}
