
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
    /// This constructor creates a new instance of the <see cref="RetryDirector"/>
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
                // Log what we are about to do.
                _logger.LogDebug(
                    "No messages were ready to process."
                    );
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
            // Step 4: Assign a provider to messages.
            // =======

            // Assign a provider type to any messages that lack one.
            messages = await AssignProviderTypeAsync(
                messages,
                availableProviderTypes,
                providerPropertyType,
                cancellationToken
                ).ConfigureAwait(false);

            // =======
            // Step 5: Ensure messages are in a 'Processing' state.
            // =======

            // Make sure all messages reflect the proper state.
            messages = await FromPendingToProcessingAsync(
                messages,
                availableProviderTypes,
                providerPropertyType,
                cancellationToken
                ).ConfigureAwait(false);

            // =======
            // Step 6: Send messages to the provider.
            // =======

            try
            {
                // Attempt to send messages to their respective provider.
                await SendPendingMessagesToProvidersAsync(
                    messages,
                    providerPropertyType,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
            catch (ProviderException ex)
            {
                // Log what happened.
                _logger.LogError(
                    ex,
                    "Provider failed to process one or more messages!"
                    );

                // TODO : we need a recovery mechanism here.
            }
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
        // Step 1: Group the messages so we can process in batches.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Grouping messages by provider type."
            );

        // Group messages by the provider property.
        var messagesGroupedByProvider = from msg in pendingMessages
                from prop in msg.MessageProperties
                where prop.PropertyType.Id == providerPropertyType.Id
                group msg by prop.Value;

        // TODO : should order by provider type priority (how to do this??).

        // Log what we are about to do.
        _logger.LogDebug(
            "Looping through {count} message groups.",
            messagesGroupedByProvider.Count()
            );

        // Loop through the messages (grouped by provider type).
        foreach (var groupedMessages in messagesGroupedByProvider) 
        {
            // =======
            // Step 2: Get the provider type for this group.
            // =======

            // Get the provider type's name.
            var providerTypePropertyName = groupedMessages.Key;

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the provider type: {name}",
                providerTypePropertyName
                );

            // Get the associated provider type model.
            var assignedProviderType = await _providerTypeManager.FindByNameAsync(
                providerTypePropertyName,
                cancellationToken
                ).ConfigureAwait(false);

            // Should never happen, but, pffft, check it anyway.
            if (assignedProviderType is null)
            {
                // If we get here then the provider type property, on the group,
                //   contains a missing, or invalid value. Should never happen,
                //   but if it does, we can recover by (A) clearing the provider
                //   type on the messages in that group - thereby giving the
                //   pipeline another chance to get the assignment right, the
                //   next time through - and (B) logging the problem.

                // Clear the provider for the group.
                await ClearProviderOnGroup(
                    groupedMessages,
                    providerPropertyType,
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing else to do!
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
                assignedProviderType,
                cancellationToken
                ).ConfigureAwait(false);

            // Should never happen, but, pffft, check it anyway.
            if (messageProvider is null)
            {
                // If we get here then the provider type property, on the group,
                //   wasn't found in the database. Should never happen, but if
                //   it does, we can recover by (A) clearing the provider on the
                //   messages in that group - thereby giving the pipeline another
                //   chance to get the assignment right, the next time through,
                //   and (B) logging the problem.

                // Clear the provider for the group.
                await ClearProviderOnGroup(
                    groupedMessages,
                    providerPropertyType,
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing else to do!
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
                assignedProviderType,
                providerPropertyType,
                cancellationToken
                ).ConfigureAwait(false);
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method clears the assigned provider property type on all the
    /// messages in the given group.
    /// </summary>
    /// <param name="groupedMessages">The group to use for the operation.</param>
    /// <param name="providerPropertyType">The 'Provider' property type
    /// to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task ClearProviderOnGroup(
        IGrouping<string, Message> groupedMessages,
        PropertyType providerPropertyType,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Clearing the provider type property for {count} messages.",
            groupedMessages.Count()
            );

        // Loop through the messages in the group.
        foreach (var message in groupedMessages)
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the {name} provider type property",
                providerPropertyType.Name
                );

            // Look for the provider property type, on the message.
            var providerMessageProperty = message.MessageProperties.FirstOrDefault(
                x => x.PropertyType.Id == providerPropertyType.Id
                );

            // Should never happen, but, pffft, check it anyway.
            if (providerMessageProperty is null)
            {
                // Log what happened.
                _logger.LogInformation(
                    "Failed to find the assigned provider property on message: {id}!",
                    message.Id
                    );

                // Record what happened, in the log.
                await _processLogManager.LogErrorEventAsync(
                    "Failed to find the assigned provider property!",
                    "host",
                    cancellationToken
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Bumping the error count, for message: {id}",
                    message.Id
                    );

                // Bump the error count on the message.
                await message.BumpErrorCountAsync(
                    _messageManager,
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing more to do!
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Removing the {name} provider type property from message: {id}",
                providerPropertyType.Name,
                message.Id
                );

            // Update the local message.
            message.MessageProperties.Remove(
                providerMessageProperty
                );

            // Log what we are about to do.
            _logger.LogDebug(
                "Deleting the {name} provider type, for message: {id}",
                providerPropertyType.Name,
                message.Id
                );

            // Update the database.
            await _messagePropertyManager.DeleteAsync(
                providerMessageProperty,
                "host",
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Bumping the error count, for message: {id}",
                message.Id
                );

            // Bump the error count on the message.
            await message.BumpErrorCountAsync(
                _messageManager,
                "host",
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
    private async Task<IEnumerable<Message>> FromPendingToProcessingAsync(
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
            "Looking for messages in a '{state}' state.",
            MessageState.Pending
            );

        // Look for messages in a pending state.
        var pendingMessages = messages.Where(x =>
            x.MessageState == MessageState.Pending
            );

        // Did we fail?
        if (!pendingMessages.Any())
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "No messages in a '{state}' state were found",
                MessageState.Pending
                );
            return messages; // Done!
        }

        // =======
        // Step 2: Assign the 'Processing state to each message.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for messages that don't need to transition"
            );

        // Look for any messages that don't need to transition.
        var processingMessages = messages.Except(
            pendingMessages,
            MessageEqualityComparer.Instance()
            ).ToList();

        // Log what we are about to do.
        _logger.LogDebug(
            "Looping through {count} pending messages",
            pendingMessages.Count()
            );

        // Loop and transition messages.
        foreach (var message in pendingMessages)
        {
            // =======
            // Step 2B: Find the associated provider.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the provider type, for message: {id}",
                message.Id
                );

            // Look for the provider property type, on the message.
            var messageProviderProperty = message.MessageProperties.FirstOrDefault(x =>
                x.PropertyType.Id == providerPropertyType?.Id
                );

            // Should never happen, but, pffft, check it anyway.
            if (messageProviderProperty is null)
            {
                // Log what happened.
                _logger.LogInformation(
                    "Message: {id} didn't contain a provider property!",
                    message.Id
                    );

                // If we get here then the message didn't contain a provider
                //   type property. Should never happen, but if it does, we
                //   can recover by (A) leaving the message in the 'Pending'
                //   state and (B) logging the problem.

                // Record what happened, in the log.
                await _processLogManager.LogErrorEventAsync(
                    "The provider property type is missing, or invalid for the message!",
                    "host",
                    cancellationToken
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Bumping the error count, for message: {id}",
                    message.Id
                    );

                // Bump the error count on the message.
                await message.BumpErrorCountAsync(
                    _messageManager,
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing more to do!
            }

            // =======
            // Step 2C: Find the provider type.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the {name} provider type, for message: {id}",
                messageProviderProperty.Value,
                message.Id
                );

            // Get the provider type, based on the property from the message.
            var assignedProviderType  = await _providerTypeManager.FindByNameAsync(
                messageProviderProperty.Value,
                cancellationToken
                ).ConfigureAwait(false);

            // Did we fail?
            if (assignedProviderType is null)
            {
                // Log what happened.
                _logger.LogInformation(
                    "The provider type {pt} is missing, or invalid, for message: {id}!",
                    messageProviderProperty.Value,
                    message.Id
                    );

                // If we get here then the assigned provider type, on the message,
                //   has a missing, or invalid value. Should never happen, but if 
                //   it does, we can recover by: (A) removing the assigned provider
                //   property, from the message, (B) leaving the message in the
                //   'Pending' state and (C) logging the problem.

                // Record what happened, in the log.
                await _processLogManager.LogErrorEventAsync(
                    $"The provider type {messageProviderProperty.Value} " +
                        $"is missing, or invalid!",
                    "host",
                    cancellationToken
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Bumping the error count, for message: {id}",
                    message.Id
                    );

                // Bump the error count on the message.
                await message.BumpErrorCountAsync(
                    _messageManager,
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing more to do!
            }

            // =======
            // Step 2D: Transition the message state.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Transitioning message: {id} to state: {state}",
                message.Id,
                MessageState.Processing
                );

            // Transition to the 'Processing' state.
            await message.ToProcessingStateAsync(
                _messageManager,
                _processLogManager,
                assignedProviderType,
                "host",
                cancellationToken
                ).ConfigureAwait(false);

            // The message is now in a 'Processing' state.
            processingMessages.Add(message);
        }

        // =======
        // Step 3: return the results.
        // =======

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
            // Log what we are about to do.
            _logger.LogDebug(
                "No messages without an assigned provider were found"
                );
            return messages; // Done!
        }

        // =======
        // Step 2: Assign an appropriate provider to each message.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for messages that don't need to have a provider " +
            "type assigned to them"
            );

        // Look for any messages that don't need to have a provider type
        // assigned to them.
        var assignedMessages = messages.Except(
            unassignedMessages,
            MessageEqualityComparer.Instance()
            ).ToList();

        // Log what we are about to do.
        _logger.LogDebug(
            "Looping through {count} unassigned messages",
            unassignedMessages.Count()
            );

        // Loop and process the unassigned messages.
        foreach (var message in unassignedMessages)
        {
            // =======
            // Step 2B: Select the appropriate provider type.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Executing the provider selection algorithm, on message: {id}",
                message.Id
                );

            // For now, this will be our 'algorithm' for assigning a provider
            //   type to a message, based on the priority and capability of
            //   the provider, along with the message type.
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
                //   now there are none left to do the work. We throw an exception
                //   because there isn't a way to recover from this doomsday scenario.

                // Panic!!
                throw new InvalidDataException(
                    $"A provider type capable of processing {message.MessageType} " +
                    $"messages could not be assigned for message: {message.Id}!"
                    );
            }

            // =======
            // Step 2C: Save the changes.
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

            // Update our local copy.
            message.MessageProperties.Add(
                messageProperty
                );

            // =======
            // Step 2D: Transition the message state.
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
                _processLogManager,
                assignedProviderType,
                "host",
                cancellationToken
                ).ConfigureAwait(false);

            // Add the message to the collection.
            assignedMessages.Add(message);
        }

        // =======
        // Step 3: Return the results.
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
            // If we get here then there are no provider types, at all,
            //   in the database. We throw an exception because there
            //   isn't a way to recover from this doomsday scenario.

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
            // If we get here then there are no available (not disabled)
            //   provider types, at all, in the database. We throw an
            //   exception because there isn't a way to recover from
            //   this doomsday scenario.

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
            // If we get here then there isn't a 'Provider' property type,
            //   at all, in the database. We throw an exception because 
            //   there isn't a way to recover from this doomsday scenario.

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
