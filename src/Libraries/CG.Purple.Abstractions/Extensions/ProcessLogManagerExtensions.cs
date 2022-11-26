
namespace CG.Purple.Managers;

/// <summary>
/// This class contains extension methods related to the <see cref="IProcessLogManager"/>
/// type.
/// </summary>
public static class ProcessLogManagerExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
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
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
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
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
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
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
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
            new ProcessLog()
            {
                Event = ProcessEvent.Error,
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
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="errorMessage">The error message to use for the operation.</param>
    /// <param name="data">Extra data associated with the event.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
        string errorMessage,
        string? data,
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
            new ProcessLog()
            {
                Event = ProcessEvent.Error,
                Error = errorMessage,
                Data = data
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
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
            new ProcessLog()
            {
                Event = ProcessEvent.Error,
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
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="errorMessage">The error message to use for the operation.</param>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <param name="data">Extra data related to the event.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
        string errorMessage,
        ProviderType providerType,
        string? data,
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
            new ProcessLog()
            {
                Event = ProcessEvent.Error,
                Error = errorMessage,
                ProviderType = providerType,
                Data = data
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Error,
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
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
    /// the processing log, for an even that caused a state transition in 
    /// the associated message.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="previousMessageState">The message state before the 
    /// event took place.</param>
    /// <param name="errorMessage">The error message to use for the operation.</param>
    /// <param name="data">Extra data related to the event.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        string errorMessage,
        string? data,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Error,
                Error = errorMessage,
                BeforeState = previousMessageState,
                AfterState = message.MessageState,
                Data = data
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
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
    /// <param name="data">Extra data related to the event.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        string errorMessage,
        ProviderType providerType,
        string? data,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Error,
                Error = errorMessage,
                BeforeState = previousMessageState,
                AfterState = message.MessageState,
                Data = data,
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
    /// This method writes an <see cref="ProcessEvent.Error"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogErrorEventAsync(
        this IProcessLogManager processLogManager,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Error,
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
    /// This method writes an <see cref="ProcessEvent.Stored"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogStoredEventAsync(
        this IProcessLogManager processLogManager,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Stored,
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
    /// This method writes an <see cref="ProcessEvent.Assigned"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogAssignedEventAsync(
        this IProcessLogManager processLogManager,
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
            new ProcessLog()
            {
                ProviderType = providerType,
                Message = message,
                Event = ProcessEvent.Assigned,
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
    /// This method writes a <see cref="ProcessEvent.Reset"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogResetEventAsync(
        this IProcessLogManager processLogManager,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Assigned,
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
    /// This method writes a <see cref="ProcessEvent.Disabled"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogDisabledEventAsync(
        this IProcessLogManager processLogManager,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Disabled,
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
    /// This method writes an <see cref="ProcessEvent.Enabled"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogEnabledEventAsync(
        this IProcessLogManager processLogManager,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Enabled,
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
    /// This method writes an <see cref="ProcessEvent.Sent"/> event to
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
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogSentEventAsync(
        this IProcessLogManager processLogManager,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Sent,
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
    /// This method writes an <see cref="ProcessEvent.Sent"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="previousMessageState">The message state before the 
    /// event took place.</param>
    /// <param name="data">Extra data associated with the event.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogSentEventAsync(
        this IProcessLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        string? data,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Sent,
                BeforeState = previousMessageState,
                AfterState = message.MessageState,
                Data = data
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    // *******************************************************************

    /// <summary>
    /// This method writes an <see cref="ProcessEvent.Sent"/> event to
    /// the processing log.
    /// </summary>
    /// <param name="processLogManager">The process log manager to use
    /// for the operation.</param>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="previousMessageState">The message state before the 
    /// event took place.</param>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <param name="data">Extra data associated with the event.</param>
    /// <param name="userName">The user name of the perform performing
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly 
    /// created <see cref="ProcessLog"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    public static async Task<ProcessLog> LogSentEventAsync(
        this IProcessLogManager processLogManager,
        Message message,
        MessageState previousMessageState,
        ProviderType providerType,
        string? data,
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
            new ProcessLog()
            {
                Message = message,
                Event = ProcessEvent.Sent,
                BeforeState = previousMessageState,
                AfterState = message.MessageState,
                ProviderType = providerType,
                Data = data
            },
            userName,
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return result;
    }

    #endregion
}
