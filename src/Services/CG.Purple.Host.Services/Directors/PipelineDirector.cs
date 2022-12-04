
using CG.Purple.Providers;

namespace CG.Purple.Host.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IPipelineDirector"/>
/// </summary>
internal class PipelineDirector : IPipelineDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the options for this director.
    /// </summary>
    internal protected readonly PipelineOptions? _pipelineOptions;

    /// <summary>
    /// This field contains the message manager for this director.
    /// </summary>
    internal protected readonly IMessageManager _messageManager;

    /// <summary>
    /// This field contains the message log manager for this director.
    /// </summary>
    internal protected readonly IMessageLogManager _messageLogManager;

    /// <summary>
    /// This field contains the provider type manager for this director.
    /// </summary>
    internal protected readonly IProviderTypeManager _providerTypeManager;

    /// <summary>
    /// This field contains the message provider factory for this director.
    /// </summary>
    internal protected readonly IMessageProviderFactory _messageProviderFactory = null!;

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<IPipelineDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PipelineDirector"/>
    /// class.
    /// </summary>
    /// <param name="hostedServiceOptions">The hosted service options to use
    /// with this director.</param>
    /// <param name="messageManager">The message manager to use with this
    /// director.</param>
    /// <param name="messageLogManager">The message log manager to use with 
    /// this director.</param>
    /// <param name="providerTypeManager">The provider type manager to 
    /// use with this director.</param>
    /// <param name="messageProviderFactory">The message provider factory to
    /// use with this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public PipelineDirector(
        IOptions<HostedServiceOptions> hostedServiceOptions,
        IMessageManager messageManager,
        IMessageLogManager messageLogManager,
        IProviderTypeManager providerTypeManager,
        IMessageProviderFactory messageProviderFactory,
        ILogger<IPipelineDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(hostedServiceOptions, nameof(hostedServiceOptions))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNull(providerTypeManager, nameof(providerTypeManager))
            .ThrowIfNull(messageProviderFactory, nameof(messageProviderFactory))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _pipelineOptions = hostedServiceOptions.Value.Pipeline;
        _messageManager = messageManager;
        _messageLogManager = messageLogManager;
        _providerTypeManager = providerTypeManager;
        _messageProviderFactory = messageProviderFactory;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task ProcessAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Processing messages."
                );

            // Process messages.
            await ProcessMessagesAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to process messages!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to process messages!",
                innerException: ex
                );
        }

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Retrying messages."
                );

            // Retry messages.
            await RetryMessagesAsync(
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to retry messages!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to retry messages!",
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
    /// This method assigns a provider type to any messages that currently
    /// lack one.
    /// </summary>
    /// <param name="messages">The messages to use for the operation.</param>
    /// <param name="providerTypes">The provider types to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the collection
    /// of messages that are now ready to be processed.</returns>
    private async Task<IEnumerable<Message>> AssignProviderMessagesAsync(
        IEnumerable<Message> messages,
        IEnumerable<ProviderType> providerTypes,
        CancellationToken cancellationToken = default
        )
    {
        // =======
        // Step 1: Find pending messages.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for pending messages."
            );

        // Look for messages that haven't been assigned a provider, yet.
        var pendingMessages = messages.Where(x => 
            x.ProviderType == null
            );

        // Did we fail?
        if (!pendingMessages.Any())
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "No pending messages were found"
                );
            return messages; // Done!
        }

        // =======
        // Step 2: Find processing messages.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for processing messages."
            );

        // Look for messages that are already processing.
        var processingMessages = messages.Except(
            pendingMessages,
            MessageEqualityComparer.Instance()
            ).ToList(); 

        // =======
        // Step 3: Assign a provider type to any pending messages.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looping through {count} pending messages",
            pendingMessages.Count()
            );

        // Loop and process pending messages.
        foreach (var message in pendingMessages)
        {
            // =======
            // Step 3B: Select the appropriate provider type.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Selecting a provider type for message: {id}",
                message.Id
                );

            ProviderType? assignedProviderType = null;

            // Assign a provider that can process emails.
            if (message.MessageType == MessageType.Mail)
            {
                assignedProviderType = providerTypes.Where(x =>
                    x.CanProcessEmails
                    ).OrderBy(x => x.Priority)
                    .FirstOrDefault();
            }

            // Assign a provider that can process texts.
            if (message.MessageType == MessageType.Text)
            {
                assignedProviderType = providerTypes.Where(x =>
                    x.CanProcessTexts
                    ).OrderBy(x => x.Priority)
                    .FirstOrDefault();
            }

            // Did we fail?
            if (assignedProviderType is null)
            {
                // Panic!!
                throw new InvalidDataException(
                    $"A provider type capable of processing {message.MessageType} " +
                    $"messages could not be assigned for message: {message.Id}!"
                    );
            }

            // =======
            // Step 3C: Save the changes to the message.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Assigning provider: {id1} to message: {id2}",
                assignedProviderType.Id,
                message.Id
                );

            // Assign the provider.
            message.ProviderType = assignedProviderType;

            // Save the changes.
            _ = await _messageManager.UpdateAsync(
                message,
                "host",
                cancellationToken
                );

            // =======
            // Step 3D: Transition the message to the 'Processing' state.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Transitioning message: {id} to state: {state}",
                message.Id,
                MessageState.Processing
                );

            // The message is now in a 'Processing' state.
            await message.ToProcessingStateAsync(
                _messageManager,
                _messageLogManager,
                assignedProviderType,
                "host",
                cancellationToken
                ).ConfigureAwait(false);

            // Add the message to the collection.
            processingMessages.Add(message);
        }

        // =======
        // Step 3: Return the results.
        // =======

        // Return the results.
        return processingMessages;
    }

    // *******************************************************************

    /// <summary>
    /// This method fetches a sequence of enabled provider types.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of 
    /// <see cref="ProviderType"/> objects.</returns>
    /// <exception cref="InvalidDataException">The exception is thrown if
    /// there are no provider types, or there are no enabled provider types.
    /// </exception>
    private async Task<IEnumerable<ProviderType>> GetProviderTypesAsync(
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Fetching all the provider types."
            );

        // Get the list of all provider types.
        var providerTypes = await _providerTypeManager.FindAllAsync()
            .ConfigureAwait(false);

        // Did we fail?
        if (!providerTypes.Any())
        {
            // Panic!!
            throw new InvalidDataException(
                "No provider types were found!"
                );
        }

        // Filter out any disabled provider types.
        providerTypes = providerTypes.Where(x => !x.IsDisabled);

        // Did we fail?
        if (!providerTypes.Any())
        {
            // Panic!!
            throw new InvalidDataException(
                "No enabled provider types were found!"
                );
        }

        // Return the results.
        return providerTypes;
    }

    // *******************************************************************

    /// <summary>
    /// This method looks for any messages that can be processed, and sends
    /// those messages to the appropriate provider.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task ProcessMessagesAsync(
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Fetching all the messages that are ready to process."
            );

        // Get the list of messages that are ready to process.
        var messages = await _messageManager.FindReadyToProcessAsync()
            .ConfigureAwait(false);

        // Did we fail?
        if (!messages.Any())
        {
            // If we get here then there's no work to do, so, we'll wait
            //   a bit before we continue the next stage of the pipeline
            //   in order to throttle the overall idle load.
            await Task.Delay(
                1000,
                cancellationToken
                ).ConfigureAwait(false);
            return; // We're done!
        }

        // Get the list of all (enabled) provider types.
        var providerTypes = await GetProviderTypesAsync()
            .ConfigureAwait(false);

        // =======
        // Step 1: Assign a provider to messages, as needed.
        // =======

        // Assign providers to messages.
        messages = await AssignProviderMessagesAsync(
            messages,
            providerTypes
            ).ConfigureAwait(false);

        // =======
        // Step 2: Ensure all messages with a provider are now in
        //   a 'Processing' state.
        // =======

        // Look for any messages that are still in a pending state.
        var pendingMessages = messages.Where(x =>
            x.MessageState == MessageState.Pending
            );

        // Log what we are about to do.
        _logger.LogDebug(
            "Looping through {count} pending messages.",
            pendingMessages.Count()
            );

        // Loop through the pending messages.
        foreach (var message in pendingMessages) 
        {
            // Should never happen, but, pffft, check it anyway.
            if (message.ProviderType is not null)
            {
                // Transition the message state.
                await message.ToProcessingStateAsync(
                    _messageManager,
                    _messageLogManager,
                    message.ProviderType,
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);
            }            
        }

        // =======
        // Step 3: Group messages by provider type.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Grouping messages by provider type."
            );

        // Group messages that are in a 'Processing' state by their
        //   assigned provider, then order the groups by the
        //   provider's relative priority.
        var messagesGroupedByProvider = messages.Where(x => 
            x.MessageState == MessageState.Processing
            ).GroupBy(x =>
                x.ProviderType
                ).OrderBy(y => y.Key?.Priority);

        // Log what we are about to do.
        _logger.LogDebug(
            "Looping through {count} message groups.",
            messagesGroupedByProvider.Count()
            );

        // Loop through the messages (grouped by provider type).
        foreach (var groupedMessages in messagesGroupedByProvider)
        {
            // Should never happen, but, pffft, check it anyway.
            if (groupedMessages.Key is null)
            {
                // Log what happened.
                _logger.LogWarning(
                    "Encountered a NULL provider type in the grouped messages!",
                    groupedMessages.Key
                    );
                continue; // Nothing else to do!
            }

            // =======
            // Step 3B: Create the provider instance.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating an instance of provider: {name}",
                groupedMessages?.Key?.Name ?? "unknown"
                );

            // Stand up an instance of the provider.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var messageProvider = await _messageProviderFactory.CreateAsync(
                groupedMessages.Key,
                cancellationToken
                ).ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Should never happen, but, pffft, check it anyway.
            if (messageProvider is null)
            {
                // Log what happened.
                _logger.LogWarning(
                    "Failed to create a {name} provider instance!",
                    groupedMessages.Key
                    );
                continue; // Nothing more to do!
            }

            // =======
            // Step 3C: Send messages to the corresponding provider.
            // =======

            try
            { 
                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring {count} messages to provider: {name}",
                    groupedMessages.Count(),
                    groupedMessages.Key.Name
                    );

                // Pass the message(s) to the provider.
                await messageProvider.ProcessMessagesAsync(
                    groupedMessages.AsEnumerable(),
                    groupedMessages.Key,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Log what happened.
                _logger.LogError(
                    ex,
                    "Failed to process messages for provider: {name}!",
                    groupedMessages.Key
                    );
                continue; // Nothing more to do!
            }
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method looks for any messages that can be retried, and sets
    /// them back to a pending state.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task RetryMessagesAsync(
        CancellationToken cancellationToken = default
        )
    {
        // TODO: write the code for this.
    }

    #endregion
}
