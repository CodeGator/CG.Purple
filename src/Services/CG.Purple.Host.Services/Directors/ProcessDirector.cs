
namespace CG.Purple.Host.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IProcessDirector"/>
/// </summary>
internal class ProcessDirector : IProcessDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the message manager for this director.
    /// </summary>
    internal protected readonly IMessageManager _messageManager = null!;

    /// <summary>
    /// This field contains the message provider factory for this director.
    /// </summary>
    internal protected readonly IMessageProviderFactory _messageProviderFactory = null!;

    /// <summary>
    /// This field contains the message property manager for this director.
    /// </summary>
    internal protected readonly IMessagePropertyManager _messagePropertyManager = null!;

    /// <summary>
    /// This field contains the process log manager for this director.
    /// </summary>
    internal protected readonly IProcessLogManager _processLogManager = null!;

    /// <summary>
    /// This field contains the property type manager for this director.
    /// </summary>
    internal protected readonly IPropertyTypeManager _propertyTypeManager = null!;

    /// <summary>
    /// This field contains the provider type manager for this director.
    /// </summary>
    internal protected readonly IProviderTypeManager _providerTypeManager = null!;

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<IProcessDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProcessDirector"/>
    /// class.
    /// </summary>
    /// <param name="messageManager">The message manager to use with this 
    /// director.</param>
    /// <param name="messageProviderFactory">The message provider factory to 
    /// use with this director.</param>
    /// <param name="messagePropertyManager">The message property manager 
    /// to use with this director.</param>
    /// <param name="providerTypeManager">The provider type manager to use
    /// with this director.</param>
    /// <param name="processLogManager">The process log manager to use
    /// with this director.</param>
    /// <param name="propertyTypeManager">The property type manager to use
    /// with this director.</param>

    /// <param name="logger">The logger to use with this director.</param>
    public ProcessDirector(
        IMessageManager messageManager,
        IMessageProviderFactory messageProviderFactory,
        IMessagePropertyManager messagePropertyManager,
        IProcessLogManager processLogManager,
        IProviderTypeManager providerTypeManager,
        IPropertyTypeManager propertyTypeManager,        
        ILogger<IProcessDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(messageProviderFactory, nameof(messageProviderFactory))
            .ThrowIfNull(messagePropertyManager, nameof(messagePropertyManager))
            .ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(providerTypeManager, nameof(providerTypeManager))
            .ThrowIfNull(propertyTypeManager, nameof(propertyTypeManager))            
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _messageManager = messageManager;
        _messageProviderFactory = messageProviderFactory;
        _messagePropertyManager = messagePropertyManager;   
        _providerTypeManager = providerTypeManager;
        _processLogManager = processLogManager;
        _propertyTypeManager = propertyTypeManager;        
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task ProcessMessagesAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // =======
            // Step 1: Find messages ready to process (if any).
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for messages that are ready to process."
                );

            // Get any messages that are ready to process.
            var messages = await _messageManager.FindReadyToProcessAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Are we done?
            if (!messages.Any())
            {
                return; // Done!
            }

            // =======
            // Step 2: Find available provider types (if any).
            // =======

            // Get the list of available provider types here because we'll
            //   need this information throughout the processing
            var availableProviderTypes = await FindAvailableProviderTypesAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // =======
            // Step 3: Find the 'Provider' property type.
            // =======

            // Get the 'Provider' property type here because we'll need
            //   this information throughout the processing
            var providerPropertyType = await FindProviderPropertyTypeAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // =======
            // Step 4: Assign a provider to any messages that need one.
            // =======

            // Assign a provider type to any message that lacks one.
            messages = await AssignProviderTypeAsync(
                messages,
                availableProviderTypes,
                providerPropertyType,
                cancellationToken
                ).ConfigureAwait(false);

            // =======
            // Step 5: Ensure any 'Pending' messages now have a 'Processing state.
            // =======

            // Make sure all messages reflect the proper state.
            messages = await TransitionFromPendingToProcessingAsync(
                messages,
                availableProviderTypes,
                providerPropertyType,
                cancellationToken
                ).ConfigureAwait(false);

            // =======
            // Step 6: Send messages to the appropriate provider.
            // =======

            // Attempt to send messages to their respective provider.
            await SendPendingMessagesToProvidersAsync(
                messages,
                providerPropertyType,
                cancellationToken
                ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to process one or more messages!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to process one or more messages!",
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
    /// This method sends the each message in the given collection to the
    /// associated provider, for processing.
    /// </summary>
    /// <param name="pendingMessages">The list of pending messages to 
    /// use for the operation.</param>
    /// <param name="providerPropertyType">The 'Provider' property type
    /// to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task SendPendingMessagesToProvidersAsync(
        IEnumerable<Message> pendingMessages,
        PropertyType providerPropertyType,
        CancellationToken cancellationToken = default
        )
    {
        // =======
        // Step 1: Group the messages so we can batch the processing.
        // =======

        // Group messages by the provider property.
        var messagesGroupedByProvider = from msg in pendingMessages
                from prop in msg.MessageProperties
                where prop.PropertyType.Id == providerPropertyType.Id
                group msg by prop.Value;

        // TODO : should order by provider priority.

        // Loop through the grouped messages.
        foreach (var groupedMessages in messagesGroupedByProvider) 
        {
            // =======
            // Step 2: Get the provider type for this batch.
            // =======

            // Get the assigned provider type's name.
            var providerTypePropertyName = groupedMessages.Key;

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the provider type: {name}",
                providerTypePropertyName
                );

            // Get the associated provider type.
            var providerType = await _providerTypeManager.FindByNameAsync(
                providerTypePropertyName,
                cancellationToken
                ).ConfigureAwait(false);

            // Should never happen, but, pffft, check it anyway.
            if (providerType is null)
            {
                // Panic!!
                throw new InvalidDataException(
                    $"Provider type {providerTypePropertyName} is missing, or invalid!"
                    );
            }

            // =======
            // Step 3: Create the actual provider from the type.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating an instance of provider: {name}",
                providerTypePropertyName
                );

            // Stand up an instance of the provider.
            var messageProvider = await _messageProviderFactory.CreateAsync(
                providerType,
                cancellationToken
                ).ConfigureAwait(false);

            // Should never happen, but, pffft, check it anyway.
            if (messageProvider is null)
            {
                // Panic!!
                throw new InvalidDataException(
                    $"Failed to create an instance of provider: {providerTypePropertyName}!"
                    );
            }

            // =======
            // Step 4: Send the messages to the provider.
            // =======

            // Log what we are about to do.
            _logger.LogTrace(
                "Deferring {count} messages to provider: {name}",
                groupedMessages.Count(),
                providerTypePropertyName
                );

            // Pass the message(s) to the provider.
            await messageProvider.ProcessMessagesAsync(
                groupedMessages.AsEnumerable(),
                providerType.Parameters,
                providerPropertyType,
                cancellationToken
                ).ConfigureAwait(false);
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method transitions the given messages from the 'Pending' state
    /// to the 'Processing' state, then returns the modified list of messages 
    /// to the caller.
    /// </summary>
    /// <param name="messages">The list of messages to use for the operation.</param>
    /// <param name="availableProviderTypes">The list of available provider
    /// types to use for the operation.</param>
    /// <param name="providerPropertyType">The 'Provider' property type
    /// to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence 
    /// of <see cref="Message"/> objects.</returns>
    private async Task<IEnumerable<Message>> TransitionFromPendingToProcessingAsync(
        IEnumerable<Message> messages,
        IEnumerable<ProviderType> availableProviderTypes,
        PropertyType providerPropertyType,
        CancellationToken cancellationToken = default
        )
    {
        // =======
        // Step 1: Find messages in a 'Pending' state.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for messages in a 'Pending' state."
            );

        // Look for messages in a pending state.
        var pendingMessages = messages.Where(x =>
            x.MessageState == MessageState.Pending
            );

        // Did we fail?
        if (!pendingMessages.Any())
        {
            return messages; // Done!
        }

        // =======
        // Step 2: Assign the 'Processing state to each message.
        // =======

        // Look for any messages that don't need to transition.
        var processingMessages = messages.Except(
            pendingMessages,
            MessageEqualityComparer.Instance()
            ).ToList();

        // Loop and transition messages.
        foreach (var message in pendingMessages)
        {
            // =======
            // Step 3: Find the associated provider.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the provider type for message: {id}",
                message.Id
                );

            // Look for the provider property type, on the message.
            var messageProviderProperty = message.MessageProperties.FirstOrDefault(x =>
                x.PropertyType.Id == providerPropertyType?.Id
                );

            // Should never happen, but, pffft, check it anyway.
            if (messageProviderProperty is null)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating a log entry for message: {id}",
                    message.Id
                    );

                // Record what we did, in the log.
                await _processLogManager.CreateAsync(
                    new ProcessLog()
                    {
                        Message = message,
                        Event = ProcessEvent.Error,
                        Error = "The provider property type is missing, or invalid for the message!"
                    },
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing more to do!
            }

            // =======
            // Step 4: Find the provider type.
            // =======

            // Get the provider type, based on the property from the message.
            var messageProvider  = await _providerTypeManager.FindByNameAsync(
                messageProviderProperty.Value,
                cancellationToken
                ).ConfigureAwait(false);

            // Did we fail?
            if (messageProvider is null)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating a log entry for message: {id}",
                    message.Id
                    );

                // Record what we did, in the log.
                await _processLogManager.CreateAsync(
                    new ProcessLog()
                    {
                        Message = message,
                        Event = ProcessEvent.Error,
                        Error = $"The provider type {messageProviderProperty.Value} is invalid!"
                    },
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing more to do!
            }

            // =======
            // Step 5: Transition the message.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Transitioning message: {id} to state: {state}",
                message.Id,
                MessageState.Processing
                );

            // Remember the previous state.
            var oldMessageState = message.MessageState;

            // The message is now in a processing state.
            message.MessageState = MessageState.Processing;

            // Update the message.
            _ = await _messageManager.UpdateAsync(
                message,
                "host",
                cancellationToken
                ).ConfigureAwait(false);

            // =======
            // Step 6: Log the changes.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a log entry for message: {id}",
                message.Id
                );

            // Record what we did, in the log.
            await _processLogManager.CreateAsync(
                new ProcessLog()
                {
                    Message = message,
                    BeforeState = oldMessageState,
                    AfterState = message.MessageState,
                    Event = ProcessEvent.Assigned,
                    ProviderType = messageProvider
                },
                "host",
                cancellationToken
                ).ConfigureAwait(false);

            // The message is now in a 'Processing' state.
            processingMessages.Add(message);
        }

        // Return the results.
        return processingMessages;
    }

    // *******************************************************************

    /// <summary>
    /// This method assigns one of the given available provider types 
    /// to each of the pending messages, then returns the modified list
    /// of messages to the caller.
    /// </summary>
    /// <param name="messages">The list of messages to use for the operation.</param>
    /// <param name="availableProviderTypes">The list of available provider
    /// types to use for the operation.</param>
    /// <param name="providerPropertyType">The 'Provider' property type
    /// to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence 
    /// of <see cref="Message"/> objects.</returns>
    private async Task<IEnumerable<Message>> AssignProviderTypeAsync(
        IEnumerable<Message> messages,
        IEnumerable<ProviderType> availableProviderTypes,
        PropertyType providerPropertyType,
        CancellationToken cancellationToken = default
        )
    {
        // =======
        // Step 1: Find messages that need a provider type.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for messages without an assigned provider."
            );

        // Look for messages that haven't been assigned a provider, yet,
        //   which would be evident as a 'Provider' property type, added
        //   to the message's properties.
        var unassignedMessages = messages.Where(x =>
            !x.MessageProperties.Any(y =>
                y.PropertyType.Id == providerPropertyType.Id
                ));

        // Did we fail?
        if (!unassignedMessages.Any() ) 
        { 
            return messages; // Done!
        }

        // =======
        // Step 2: Assign an appropriate provider to each message.
        // =======

        // Look for any messages that don't need to have a provider type
        // assigned to them.
        var assignedMessages = messages.Except(
            unassignedMessages,
            MessageEqualityComparer.Instance()
            ).ToList();

        // Loop and process the unassigned messages.
        foreach (var message in unassignedMessages)
        {
            // =======
            // Step 3: Select the appropriate provider type.
            // =======

            // For now, this will be our 'algorithm' for assigning a provider
            //   to a message, based on the priority and capability of the provider,
            //   along with the message type (email or text).
            ProviderType? assignedProviderType = null;
            if (message.MessageType == MessageType.Mail)
            {
                assignedProviderType = availableProviderTypes.Where(x =>
                    x.CanProcessEmails
                    ).OrderBy(x => x.Priority)
                    .FirstOrDefault();
            }
            else
            {
                assignedProviderType = availableProviderTypes.Where(x =>
                    x.CanProcessTexts
                    ).OrderBy(x => x.Priority)
                    .FirstOrDefault();
            }                                

            // Did we fail?
            if (assignedProviderType is null)
            {
                // If we get here then we didn't find an available provider whose
                //   capabilities match the message type. This might happen if (A)
                //   someone has deleted something from the database, or (B) the
                //   pipeline itself has disabled one or more provider types, and
                //   now there are none left to do the work.

                // Panic!!
                throw new InvalidDataException(
                    $"A provider type capable of processing {message.MessageType} " +
                    "messages was not found!"
                    );
            }

            // =======
            // Step 4: Save the changes.
            // =======

            // Add the 'provider' property type, to the message, which effectively
            //   'assigns' the provider to the message.
            var messageProperty = await _messagePropertyManager.CreateAsync(
                new MessageProperty()
                {
                    Message = message,
                    PropertyType = providerPropertyType,
                    Value = assignedProviderType.Name,
                },
                "host",
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Updating the local copy of message {id}",
                message.Id
                );

            // Update our local copy so we don't have to do a round trip back
            //   to the database.
            message.MessageProperties.Add(
                messageProperty
                );

            // =======
            // Step 5: Transition the message state.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Transitioning message: {id} to state: {state}",
                message.Id,
                MessageState.Processing
                );

            // Remember the previous state.
            var oldMessageState = message.MessageState;

            // The message is now in a processing state.
            message.MessageState = MessageState.Processing;

            // Update the message.
            _ = await _messageManager.UpdateAsync(
                message,
                "host",
                cancellationToken
                ).ConfigureAwait(false);

            // =======
            // Step 6: Log the changes.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a log entry for message: {id}",
                message.Id
                );

            // Record what we did, in the log.
            await _processLogManager.CreateAsync(
                new ProcessLog()
                {
                    Message = message,
                    BeforeState = oldMessageState,
                    AfterState = message.MessageState,
                    Event = ProcessEvent.Assigned,
                    ProviderType = assignedProviderType
                },
                "host",
                cancellationToken
                ).ConfigureAwait(false);

            // The message is now assigned an in a 'Processing' state.
            assignedMessages.Add(message);
        }

        // =======
        // Step 7: Return the results.
        // =======

        // Return the results.
        return assignedMessages;
    }

    // *******************************************************************

    /// <summary>
    /// This method returns a sequence of all the available message provider
    /// types.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of 
    /// <see cref="ProviderType"/> objects.</returns>
    /// <exception cref="InvalidDataException">This exception is thrown in the
    /// very rare case that no provider types were found at all, or no available 
    /// provider types were found.</exception>
    private async Task<IEnumerable<ProviderType>> FindAvailableProviderTypesAsync(
        CancellationToken cancellationToken = default
        )
    {
        // =======
        // Step 1: Get all the provider types.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for a list of provider types."
            );

        // Look for the list of available provider types.
        var providerTypes = await _providerTypeManager.FindAllAsync(
            cancellationToken
            ).ConfigureAwait(false);

        // Should never happen, but, pffft, check it anyway.
        if (!providerTypes.Any())
        {
            // Panic!!
            throw new InvalidDataException(
                "No provider types were found!"
                );
        }

        // =======
        // Step 2: Remove any that are disabled.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for a list of available provider types."
            );

        // Now look for available provider types.
        var availableProviders = providerTypes.Where(x =>
            x.IsDisabled == false
            );

        // Should never happen, but, pffft, check it anyway.
        if (!availableProviders.Any())
        {
            // Panic!!
            throw new InvalidDataException(
                "No available provider types were found!"
                );
        }

        // =======
        // Step 3: Return the results.
        // =======

        // Return the results.
        return availableProviders;
    }

    // *******************************************************************

    /// <summary>
    /// This method locates the <see cref="PropertyType"/> designated for
    /// assigning a provider to a message.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a <see cref="PropertyType"/>
    /// object.</returns>
    /// <exception cref="InvalidDataException">This exception is thrown in the
    /// very rare case that the 'Provider' property type isn't found.</exception>
    private async Task<PropertyType> FindProviderPropertyTypeAsync(
        CancellationToken cancellationToken = default
        )
    {
        // =======
        // Step 1: Find the 'Provider' property type.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for the 'provider' property type."
            );

        // Look for the 'provider' property type.
        var providerPropertyType = await _propertyTypeManager.FindByNameAsync(
            "Provider",
            cancellationToken
            ).ConfigureAwait(false);

        // Should never happen, but, pffft, check it anyway.
        if (providerPropertyType is null)
        {
            // Panic!!
            throw new InvalidDataException(
                "The 'Provider' property type was not found!"
                );
        }

        // =======
        // Step 2: Return the results.
        // =======

        // Return the results
        return providerPropertyType;
    }

    #endregion
}
