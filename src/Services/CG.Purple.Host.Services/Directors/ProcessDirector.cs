
using CG.Purple.Models;

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
    /// This field contains the message property manager for this director.
    /// </summary>
    internal protected readonly IMessagePropertyManager _messagePropertyManager = null!;

    /// <summary>
    /// This field contains the provider log manager for this director.
    /// </summary>
    internal protected readonly IProviderLogManager _providerLogManager = null!;

    /// <summary>
    /// This field contains the property type manager for this director.
    /// </summary>
    internal protected readonly IPropertyTypeManager _propertyTypeManager = null!;

    /// <summary>
    /// This field contains the provider type manager for this director.
    /// </summary>
    internal protected readonly IProviderTypeManager _providerTypeManager = null!;

    /// <summary>
    /// This field contains the message provider factory for this director.
    /// </summary>
    internal protected readonly IMessageProviderFactory _messageProviderFactory = null!;

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
    /// <param name="messagePropertyManager">The message property manager 
    /// to use with this director.</param>
    /// <param name="providerTypeManager">The provider type manager to use
    /// with this director.</param>
    /// <param name="providerLogManager">The provider log manager to use
    /// with this director.</param>
    /// <param name="propertyTypeManager">The property type manager to use
    /// with this director.</param>
    /// <param name="messageProviderFactory">The message provider factory to 
    /// use with this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public ProcessDirector(
        IMessageManager messageManager,
        IMessagePropertyManager messagePropertyManager,
        IProviderLogManager providerLogManager,
        IProviderTypeManager providerTypeManager,
        IPropertyTypeManager propertyTypeManager,
        IMessageProviderFactory messageProviderFactory,
        ILogger<IProcessDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(messagePropertyManager, nameof(messagePropertyManager))
            .ThrowIfNull(providerLogManager, nameof(providerLogManager))
            .ThrowIfNull(providerTypeManager, nameof(providerTypeManager))
            .ThrowIfNull(propertyTypeManager, nameof(propertyTypeManager))
            .ThrowIfNull(messageProviderFactory, nameof(messageProviderFactory))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _messageManager = messageManager;
        _messagePropertyManager = messagePropertyManager;   
        _providerTypeManager = providerTypeManager;
        _providerLogManager = providerLogManager;
        _propertyTypeManager = propertyTypeManager;
        _messageProviderFactory = messageProviderFactory;   
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
            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for pending messages."
                );

            // Get any pending messages.
            var pendingMessages = await _messageManager.FindPendingAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Did we fail?
            if (!pendingMessages.Any())
            {
                // If we get here it means there are no messages waiting
                //   to be processed.
                return; // Done!
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for available provider types."
                );

            // Get the list of available provider types here so we don't
            //   repeat this call, later.
            var availableProviderTypes = await FindAvailableProviderTypesAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the 'Provider' property type."
                );

            // Get the 'Provider' property type here so we don't repeat
            //   this call, later.
            var providerPropertyType = await FindProviderPropertyTypeAsync(
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Attempting to assign providers to messages."
                );

            // Assign a provider to any pending message that needs one.
            pendingMessages = await AssignProviderToMessages(
                pendingMessages,
                availableProviderTypes,
                providerPropertyType,
                cancellationToken
                ).ConfigureAwait(false);

            // Log what we are about to do.
            _logger.LogDebug(
                "Attempting to send messages to providers."
                );

            // Attempt to send messages to their assigned provider.
            await SendPendingMessagesToProvidersAsync(
                pendingMessages,
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
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    private async Task SendPendingMessagesToProvidersAsync(
        IEnumerable<Message> pendingMessages,
        CancellationToken cancellationToken = default
        )
    {
        // TODO : write the code for this.
    }

    // *******************************************************************

    /// <summary>
    /// This method assigns one of the given available provider types 
    /// to each of the pending messages, then returns the modified list
    /// of messages to the caller.
    /// </summary>
    /// <param name="pendingMessages">The list of pending messages to 
    /// use for the operation.</param>
    /// <param name="availableProviderTypes">The list of available provider
    /// types to use for the operation.</param>
    /// <param name="providerPropertyType">The 'Provider' property type
    /// to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence 
    /// of <see cref="Message"/> objects.</returns>
    private async Task<IEnumerable<Message>> AssignProviderToMessages(
        IEnumerable<Message> pendingMessages,
        IEnumerable<ProviderType> availableProviderTypes,
        PropertyType providerPropertyType,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for messages without an assigned provider."
            );

        // Look for messages that haven't been assigned a provider, yet,
        //   which would be evident as a 'Provider' property type, added
        //   to the message's properties.
        var unassignedMessages = pendingMessages.Where(x =>
            !x.MessageProperties.Any(y =>
                y.PropertyType.Id == providerPropertyType.Id
                ));

        // Did we fail?
        if (!unassignedMessages.Any() ) 
        { 
            // If we get here it means there are no unassigned messages
            //   waiting to be processed.
            return pendingMessages; // Done!
        }

        // Look for any messages that don't need to have a provider type
        // assigned to them.
        var assignedMessages = pendingMessages.Except(
            unassignedMessages
            ).ToList();

        // Loop and process the unassigned messages.
        foreach (var message in unassignedMessages)
        {
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
                //   pipeline itself has disabled one or more provider types and
                //   now we have none left to do the work. Either way, this isn't
                //   a recoverable scenario.

                // Panic!!
                throw new InvalidDataException(
                    $"A provider type capable of processing {message.MessageType} " +
                    "messages was not found!"
                    );
            }

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

            // Log what we are about to do.
            _logger.LogDebug(
                "Adding message: {id} to the collection of assigned messages",
                message.Id
                );

            // Log what we are about to do.
            _logger.LogInformation(
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

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a log entry for message: {id}",
                message.Id
                );

            // Record what we did, in the log.
            await _providerLogManager.CreateAsync(
                new ProviderLog()
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
        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for a list of provider types."
            );

        // Look for the list of available providers.
        var providers = await _providerTypeManager.FindAllAsync(
            cancellationToken
            ).ConfigureAwait(false);

        // Should never happen, but, pffft, check it anyway.
        if (!providers.Any())
        {
            // If we get here then the database doesn't have any message
            //   providers, for some reason, which really should never
            //   happen, and isn't recoverable.

            // Panic!!
            throw new InvalidDataException(
                "No provider types were found!"
                );
        }

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for a list of available provider types."
            );

        // Now look for available provider types.
        var availableProviders = providers.Where(x =>
            x.IsDisabled == false
            );

        // Should never happen, but, pffft, check it anyway.
        if (!availableProviders.Any())
        {
            // If we get here then: (A) somebody deleted something 
            //   important from the database, or (B) the processing
            //   pipeline itself has disabled all the available
            //   provider types, for some reason (should never happen,
            //   but, hey, technically it could).

            // Panic!!
            throw new InvalidDataException(
                "No available provider types were found!"
                );
        }

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
            // If we get here then the database doesn't have a 'Provider'
            //   property type, for some reason, which really should
            //   never happen, and isn't recoverable, so Yikes!!

            // Panic!!
            throw new InvalidDataException(
                "The 'Provider' property type was not found!"
                );
        }

        // Return the results
        return providerPropertyType;
    }

    /*
    #region Mail Processing

    /// <summary>
    /// This method triages and processes pending mail messages.
    /// </summary>
    /// <param name="providerTypeProperty">The provider property type to
    /// use for this operation.</param>
    /// <param name="availableProviders">The available processors to use 
    /// for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task ProcessPendingMailMessagesAsync(
        PropertyType providerTypeProperty,
        IEnumerable<ProviderType> availableProviders,
        CancellationToken cancellationToken = default
        )
    {
        // =======
        // Step 1: Get all pending mail messages.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for pending mail messages."
            );

        // Get the pending mail messages.
        var messages = await _mailMessageManager.FindPendingAsync(
            cancellationToken
            ).ConfigureAwait(false);

        // =======
        // Step 2: Ensure all messages have a provider.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Ensuring pending mail messages have a provider."
            );

        // Ensure every mail message has a provider.
        messages = await AssignProviderAsync(
            messages,
            providerTypeProperty,
            availableProviders,
            cancellationToken
            ).ConfigureAwait(false);

        // =======
        // Step 3: Process all the pending messages.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Attempting to send pending mail messages."
            );

        // Attempt to send the pending mail messages.
        await SendMailMessages(
            messages,
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method assigns a provider to any mail messages that lack one.
    /// </summary>
    /// <param name="messages">The mail messages to use for the operation.</param>
    /// <param name="providerTypeProperty">The provider property type to
    /// use for this operation.</param>
    /// <param name="availableProviders">The available processors to use 
    /// for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of
    /// messages.</returns>
    private async Task<IEnumerable<MailMessage>> AssignProviderAsync(
        IEnumerable<MailMessage> messages,
        PropertyType providerTypeProperty,
        IEnumerable<ProviderType> availableProviders,
        CancellationToken cancellationToken = default
        )
    {
        // =======
        // Step 1: Look for messages without a provider.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for unassigned mail messages."
            );

        // Look for messages that haven't been assigned a provider.
        var unassignedMessages = messages.Where(x =>
            !x.MessageProperties.Any(y =>
                y.PropertyType.Id == providerTypeProperty.Id
                )).ToList();

        var assignedMessages = new List<MailMessage>();

        // Did we find any?
        if (unassignedMessages.Any())
        {
            // If we get here it means we have messages that need a provider.

            // Log what we are about to do.
            _logger.LogDebug(
                "Looping through {count} unassigned mail messages.",
                unassignedMessages.Count
                );

            // Loop and assign a provider.
            foreach (var message in unassignedMessages)
            {
                try
                {
                    // =======
                    // Step 2: Find an available provider.
                    // =======

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Looking for an available mail provider."
                        );

                    // For now, this will be our provider selection 'algorithm'.
                    var availableProvider = availableProviders
                        .OrderBy(x => x.Priority)
                        .FirstOrDefault();

                    // Should never happen, but, pffft, check it anyway.
                    if (availableProvider is null)
                    {
                        // If we get here then no providers for emails were found, which
                        //   might happen because (A): somebody deleted something important
                        //   in the database, or (B): somebody disabled all the mail providers,
                        //   or (C): the pipeline itself has disabled one or more providers
                        //   because it's having trouble creating them. Obviously, none of
                        //   these situations are normal. Unfortunately, they also aren't 
                        //   recoverable. Yikes!

                        // Panic!!
                        throw new KeyNotFoundException(
                            "A provider capable of processing mail messages wasn't found!"
                            );
                    }

                    // =======
                    // Step 3: Assign the provider to the message.
                    // =======

                    // Log what we are about to do.
                    _logger.LogInformation(
                        "Assigning the provider {prov} to message: {id}.",
                        availableProvider.Name,
                        message.Id
                        );

                    // Add the provider property to the message.
                    var messageProperty = await _messagePropertyManager.CreateAsync(
                        new MessageProperty()
                        {
                            Message = message,
                            PropertyType = providerTypeProperty,
                            Value = availableProvider.Name,
                        },
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Adding the provider property to message: {id}",
                        message.Id
                        );

                    // Add the message property to the message.
                    message.MessageProperties.Add(messageProperty);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Adding the assigned message: {id} to a temporary collection",
                        message.Id
                        );

                    // Since 'message' is a foreach iterator variable, we can't 
                    //   just add the message property and have it be reflected
                    //   in the original collection. So, we have to add it to 
                    //   another collection instead. Isn't C# fun?
                    assignedMessages.Add(message);

                    // =======
                    // Step 4: Transition the message state.
                    // =======

                    // Log what we are about to do.
                    _logger.LogInformation(
                        "Transitioning message: {id} to state: {state}",
                        message.Id,
                        MessageState.Processing
                        );

                    // Remember the previous state.
                    var oldMessageState = message.MessageState;

                    // The message is now in a processing state.
                    message.MessageState = MessageState.Processing;

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Updating the message: {id}",
                        message.Id
                        );

                    // Update the message.
                    _ = await _mailMessageManager.UpdateAsync(
                        message,
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Creating a log entry for message: {id}",
                        message.Id
                        );

                    // Record what we did, in the log.
                    await _providerLogManager.CreateAsync(
                        new ProviderLog()
                        {
                            Message = message,
                            BeforeState = oldMessageState,
                            AfterState = message.MessageState,
                            Event = ProcessEvent.Assigned,
                            ProviderType = availableProvider
                        },
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // If we get here it means an individual message failed to process.

                    // Log what happened.
                    _logger.LogError(
                        ex,
                        "Failed to assign a provider!"
                        );

                    // Record what happened, in the log.
                    await _providerLogManager.CreateAsync(
                        new ProviderLog()
                        {
                            Message = message,
                            Event = ProcessEvent.Assigned,
                            Error = ex.GetBaseException().Message
                        },
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }
        }
        else
        {
            // If we get here it means all messages have a provider.

            // Log what we are about to do.
            _logger.LogDebug(
                "No unassigned text messages were found."
                );

            // Return the results.
            return messages;
        }

        // Log what we are about to do.
        _logger.LogDebug(
            "{count} text messages were assigned.",
            assignedMessages.Count
            );

        // Return the results.
        return assignedMessages;
    }

    // *******************************************************************

    /// <summary>
    /// This method attempts to send the given pending mail messages.
    /// </summary>
    /// <param name="messages">The mail messages to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task SendMailMessages(
        IEnumerable<MailMessage> messages,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Attempting to send {count} messages.",
            messages.Count()
            );

        // Loop through the mail messages (ignore disabled ones).
        foreach (var message in messages.Where(x => x.IsDisabled == false))
        {
            // =======
            // Step 1: find the 'assigned provider' property, on the message.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the 'Provider' property, on message: {id}",
                message.Id
                );

            // Look for the 'Provider' property, on the message.
            var providerProperty = message.MessageProperties.FirstOrDefault(x =>
                x.PropertyType.Name == "Provider"
                );

            // Should never happen, but, pffft, check it anyway.
            if (providerProperty is null)
            {
                // If we get here, the message somehow got to this point in 
                //   the pipeline without an assigned provider, which is never
                //   supposed to happen.

                // Log what we didn't find.
                _logger.LogError(
                    "The mail message: {id} does not have an assigned provider!",
                    message.Id
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Logging a process error on mail message: {id}",
                    message.Id
                    );

                // Write the error to the provider log.
                _ = await _providerLogManager.CreateAsync(
                    new ProviderLog()
                    {
                        Event = ProcessEvent.ProcessError,
                        Message = message,
                        BeforeState = message.MessageState,
                        AfterState = message.MessageState,
                        Error = "The mail message didn't have an assigned provider!"
                    },
                    "host",
                    cancellationToken
                    );

                // We don't need to do anything here, really, since the message
                //   will get picked up, and hopefully have another provider
                //   assigned to it, next time around.

                continue; // Nothing more to do!
            }

            // =======
            // Step 2: convert the property to the actual 'ProviderType' model.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the mail provider type: {name}",
                providerProperty.Value
                );

            // Get the provider type that was assigned to the message.
            var assignedProvider = await _providerTypeManager.FindByNameAsync(
                providerProperty.Value,
                cancellationToken
                ).ConfigureAwait(false);

            // Should never happen, but, pffft, check it anyway.
            if (assignedProvider is null)
            {
                // If we get here, the provider that was assigned to the message
                //   is, somehow, invalid. Obviously, this should never happen.

                // Log what we didn't find.
                _logger.LogError(
                    "The mail provider type: {name} wasn't found, for mail message: {id}!",
                    providerProperty.Value,
                    message.Id
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Logging a process error on mail message: {id}",
                    message.Id
                    );

                // Write the error to the provider log.
                _ = await _providerLogManager.CreateAsync(
                    new ProviderLog()
                    {
                        Event = ProcessEvent.ProcessError,
                        Message = message,
                        BeforeState = message.MessageState,
                        AfterState = message.MessageState,
                        Error = $"The mail provider type: {providerProperty.Value} wasn't found!"
                    },
                    "host",
                    cancellationToken
                    );

                // Let's try to save the overall processing by de-assigning the
                //   current provider, for the message, so the algorithm can
                //   pick another one, next time around.

                // Let's delete the 'Provider' property, on the message.
                message.MessageProperties.Remove(providerProperty);

                // Let' remove that property from the underlying storage.
                await _messagePropertyManager.DeleteAsync(
                    providerProperty,
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing more to do!
            }

            // Ensure the provider isn't disabled.
            if (assignedProvider.IsDisabled)
            {
                // Log what we are about to do.
                _logger.LogWarning(
                    "The provider {name} was disabled while processing message: {id}!",
                    assignedProvider.Name,
                    message.Id
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Logging a process error on mail message: {id}",
                    message.Id
                    );

                // Write the error to the provider log.
                _ = await _providerLogManager.CreateAsync(
                    new ProviderLog()
                    {
                        Event = ProcessEvent.ProcessError,
                        Message = message,
                        BeforeState = message.MessageState,
                        AfterState = message.MessageState,
                        Error = $"The provider {assignedProvider.Name} was " +
                        "disabled while processing message!"
                    },
                    "host",
                    cancellationToken
                    );

                continue; // Nothing more to do!
            }

            // =======
            // Step 3: create the actual provider instance.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating an instance of mail provider: {type}",
                assignedProvider.FactoryType
                );

            // Stand up an instance of the provider.
            var messageProvider = await _messageProviderFactory.CreateAsync(
                assignedProvider,
                cancellationToken
                ).ConfigureAwait(false);

            // Should never happen, but, pffft, check it anyway.
            if (messageProvider is null)
            {
                // If we get here, the provider was found, for this message,
                //   and we tried to create it, but that attempt failed, for
                //   some reason.

                // Log what we are about to do.
                _logger.LogError(
                    "Unable to create the provider: {name} for message: {id}",
                    assignedProvider.Name,
                    message.Id
                    );

                // Write the error to the provider log.
                _ = await _providerLogManager.CreateAsync(
                    new ProviderLog()
                    {
                        Event = ProcessEvent.ProcessError,
                        Message = message,
                        BeforeState = message.MessageState,
                        AfterState = message.MessageState,
                        Error = $"Unable to create provider factory type: {assignedProvider.FactoryType}!"
                    },
                    "host",
                    cancellationToken
                    );

                // At this point, the provider itself is not usable, since
                //   we can't create instances of it. So, the best thing to
                //   do is disable the provider so the algorithm can pick
                //   another one, next time around.

                // Log what we are about to do.
                _logger.LogWarning(
                    "Disabling provider: {name} since we can't create instances of it",
                    assignedProvider.Name
                    );

                // Update the model.
                assignedProvider.IsDisabled = true;

                // Update the underlying storage.
                _ = await _providerTypeManager.UpdateAsync(
                    assignedProvider,
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing more to do!
            }

            try
            {
                // =======
                // Step 4: pass the message to the provider, for processing.
                // =======

                // Defer to the provider.
                await messageProvider.SendMailAsync(
                    message,
                    assignedProvider,
                    cancellationToken
                    ).ConfigureAwait(false);

                // If we get here then we can safely assume the message state
                //   and the logs both reflect whatever the provider did, or
                //   didn't do, with the message - unless it threw an exception,
                //   in which case we deal with it below, in the catch block.
            }
            catch (Exception ex)
            {
                // If we get here there was a critical provider (or message) error.

                // Log what happened.
                _logger.LogError(
                    ex,
                    "The provider failed to process mail message: {id}!",
                    message.Id
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Logging a provider error on mail message: {id}",
                    message.Id
                    );

                // Write the error to the provider log.
                _ = await _providerLogManager.CreateAsync(
                    new ProviderLog()
                    {
                        Event = ProcessEvent.ProviderError,
                        Message = message,
                        BeforeState = message.MessageState,
                        AfterState = message.MessageState,
                        Error = ex.GetBaseException().Message
                    },
                    "host",
                    cancellationToken
                    );

                // TODO : figure out if we can recover from this.
            }
        }
    }

    #endregion

    #region Text Processing
    
    /// <summary>
    /// This method triages and processes pending text messages.
    /// </summary>
    /// <param name="providerTypeProperty">The provider property type to
    /// use for this operation.</param>
    /// <param name="availableProviders">The available processors to use 
    /// for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task ProcessPendingTextMessagesAsync(
        PropertyType providerTypeProperty,
        IEnumerable<ProviderType> availableProviders,
        CancellationToken cancellationToken = default
        )
    {
        // =======
        // Step 1: Get all pending text messages.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for pending text messages."
            );

        // Get the pending text messages.
        var messages = await _textMessageManager.FindPendingAsync(
            cancellationToken
            ).ConfigureAwait(false);

        // =======
        // Step 2: Ensure all messages have a provider.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Ensuring pending text messages have a provider."
            );

        // Ensure every text message has a provider.
        await AssignProviderAsync(
            messages,
            providerTypeProperty,
            availableProviders,
            cancellationToken
            ).ConfigureAwait(false);

        // =======
        // Step 3: Process all the pending messages.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Attempting to send pending text messages."
            );

        // Attempt to send the pending text messages.
        await SendTextMessages(
            messages,
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method assigns a provider to any text messages that lack one.
    /// </summary>
    /// <param name="messages">The text messages to use for the operation.</param>
    /// <param name="providerTypeProperty">The provider property type to
    /// use for this operation.</param>
    /// <param name="availableProviders">The available processors to use 
    /// for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of
    /// messages.</returns>
    private async Task<IEnumerable<TextMessage>> AssignProviderAsync(
        IEnumerable<TextMessage> messages,
        PropertyType providerTypeProperty,
        IEnumerable<ProviderType> availableProviders,
        CancellationToken cancellationToken = default
        )
    {
        // =======
        // Step 1: Look for messages without a provider.
        // =======

        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for unassigned text messages."
            );

        // Look for messages that haven't been assigned a provider.
        var unassignedMessages = messages.Where(x =>
            !x.MessageProperties.Any(y =>
                y.PropertyType.Id == providerTypeProperty.Id
                )).ToList();

        var assignedMessages = new List<TextMessage>();

        // Did we find any?
        if (unassignedMessages.Any())
        {
            // If we get here it means we have messages that need a provider.

            // Loop and assign a provider.
            foreach (var message in unassignedMessages)
            {
                try
                {
                    // =======
                    // Step 2: Find an available provider.
                    // =======

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Looking for an available text provider."
                        );

                    // For now, this will be our provider selection 'algorithm'.
                    var availableProvider = availableProviders
                        .OrderBy(x => x.Priority)
                        .FirstOrDefault();

                    // Should never happen, but, pffft, check it anyway.
                    if (availableProvider is null)
                    {
                        // If we get here then no providers for text were found, which
                        //   might happen because (A): somebody deleted something important
                        //   in the database, or (B): somebody disabled all the text providers,
                        //   or (C): the pipeline itself has disabled one or more providers
                        //   because it's having trouble creating them. Obviously, none of
                        //   these situations are normal. Unfortunately, they also aren't 
                        //   recoverable. Yikes!

                        // Panic!!
                        throw new KeyNotFoundException(
                            "A provider capable of processing text messages wasn't found!"
                            );
                    }

                    // =======
                    // Step 3: Assign the provider to the message.
                    // =======

                    // Log what we are about to do.
                    _logger.LogInformation(
                        "Assigning the provider {prov} to message: {id}.",
                        availableProvider.Name,
                        message.Id
                        );

                    // Add the provider property to the message.
                    var messageProperty = await _messagePropertyManager.CreateAsync(
                        new MessageProperty()
                        {
                            Message = message,
                            PropertyType = providerTypeProperty,
                            Value = availableProvider.Name,
                        },
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Adding the provider property to message: {id}",
                        message.Id
                        );

                    // Add the message property to the message.
                    message.MessageProperties.Add(messageProperty);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Adding the assigned message: {id} to a temporary collection",
                        message.Id
                        );

                    // Since 'message' is a foreach iterator variable, we can't 
                    //   just add the message property and have it be reflected
                    //   in the original collection. So, we have to add it to 
                    //   another collection instead. Isn't C# fun?
                    assignedMessages.Add(message);

                    // =======
                    // Step 4: Transition the message state.
                    // =======

                    // Log what we are about to do.
                    _logger.LogInformation(
                        "Transitioning message: {id} to state: {state}",
                        message.Id,
                        MessageState.Processing
                        );

                    // Remember the previous state.
                    var oldMessageState = message.MessageState;

                    // The message is now in a processing state.
                    message.MessageState = MessageState.Processing;

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Updating the message: {id}",
                        message.Id
                        );

                    // Update the message.
                    _ = await _textMessageManager.UpdateAsync(
                        message,
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Creating a log entry for message: {id}",
                        message.Id
                        );

                    // Record what we did, in the log.
                    await _providerLogManager.CreateAsync(
                        new ProviderLog()
                        {
                            Message = message,
                            BeforeState = message.MessageState,
                            AfterState = MessageState.Processing,
                            Event = ProcessEvent.Assigned,
                            ProviderType = availableProvider
                        },
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // If we get here it means an individual message failed to process.

                    // Log what happened.
                    _logger.LogError(
                        ex,
                        "Failed to assign a provider!"
                        );

                    // Record what happened, in the log.
                    await _providerLogManager.CreateAsync(
                        new ProviderLog()
                        {
                            Message = message,
                            Event = ProcessEvent.Assigned,
                            Error = ex.GetBaseException().Message
                        },
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }
        }
        else
        {
            // If we get here it means all messages have a provider.

            // Log what we are about to do.
            _logger.LogDebug(
                "No unassigned text messages were found."
                );

            // Return the results.
            return messages;
        }

        // Log what we are about to do.
        _logger.LogDebug(
            "{count} text messages were assigned.",
            assignedMessages.Count
            );

        // Return the results.
        return assignedMessages;
    }

    // *******************************************************************

    /// <summary>
    /// This method attempts to send the given pending text messages.
    /// </summary>
    /// <param name="messages">The text messages to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task SendTextMessages(
        IEnumerable<TextMessage> messages,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Attempting to send {count} messages.",
            messages.Count()
            );

        // Loop through the text messages (ignore disabled ones).
        foreach (var message in messages.Where(x => x.IsDisabled == false))
        {
            // =======
            // Step 1: find the 'assigned provider' property, on the message.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the 'Provider' property, on message: {id}",
                message.Id
                );

            // Look for the 'Provider' property, on the message.
            var providerProperty = message.MessageProperties.FirstOrDefault(x =>
                x.PropertyType.Name == "Provider"
                );

            // Should never happen, but, pffft, check it anyway.
            if (providerProperty is null)
            {
                // If we get here, the message somehow got to this point in 
                //   the pipeline without an assigned provider, which is never
                //   supposed to happen.

                // Log what we didn't find.
                _logger.LogError(
                    "The text message: {id} does not have an assigned provider!",
                    message.Id
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Logging a process error on text message: {id}",
                    message.Id
                    );

                // Write the error to the provider log.
                _ = await _providerLogManager.CreateAsync(
                    new ProviderLog()
                    {
                        Event = ProcessEvent.ProcessError,
                        Message = message,
                        BeforeState = message.MessageState,
                        AfterState = message.MessageState,
                        Error = "The text message didn't have an assigned provider!"
                    },
                    "host",
                    cancellationToken
                    );

                // We don't need to do anything here, really, since the message
                //   will get picked up, and hopefully have another provider
                //   assigned to it, next time around.

                continue; // Nothing more to do!
            }

            // =======
            // Step 2: convert the property to the actual 'ProviderType' model.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the text provider type: {name}",
                providerProperty.Value
                );

            // Get the provider type that was assigned to the message.
            var assignedProvider = await _providerTypeManager.FindByNameAsync(
                providerProperty.Value,
                cancellationToken
                ).ConfigureAwait(false);

            // Should never happen, but, pffft, check it anyway.
            if (assignedProvider is null)
            {
                // If we get here, the provider that was assigned to the message
                //   is, somehow, invalid. Obviously, this should never happen.

                // Log what we didn't find.
                _logger.LogError(
                    "The text provider type: {name} wasn't found, for text message: {id}!",
                    providerProperty.Value,
                    message.Id
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Logging a process error on text message: {id}",
                    message.Id
                    );

                // Write the error to the provider log.
                _ = await _providerLogManager.CreateAsync(
                    new ProviderLog()
                    {
                        Event = ProcessEvent.ProcessError,
                        Message = message,
                        BeforeState = message.MessageState,
                        AfterState = message.MessageState,
                        Error = $"The text provider type: {providerProperty.Value} wasn't found!"
                    },
                    "host",
                    cancellationToken
                    );

                // Let's try to save the overall processing by de-assigning the
                //   current provider, for the message, so the algorithm can
                //   pick another one, next time around.

                // Let's delete the 'Provider' property, on the message.
                message.MessageProperties.Remove(providerProperty);

                // Let' remove that property from the underlying storage.
                await _messagePropertyManager.DeleteAsync(
                    providerProperty,
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing more to do!
            }

            // Ensure the provider isn't disabled.
            if (assignedProvider.IsDisabled)
            {
                // Log what we are about to do.
                _logger.LogWarning(
                    "The provider {name} was disabled while processing message: {id}!",
                    assignedProvider.Name,
                    message.Id
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Logging a process error on text message: {id}",
                    message.Id
                    );

                // Write the error to the provider log.
                _ = await _providerLogManager.CreateAsync(
                    new ProviderLog()
                    {
                        Event = ProcessEvent.ProcessError,
                        Message = message,
                        BeforeState = message.MessageState,
                        AfterState = message.MessageState,
                        Error = $"The provider {assignedProvider.Name} was " +
                        "disabled while processing message!"
                    },
                    "host",
                    cancellationToken
                    );

                continue; // Nothing more to do!
            }

            // =======
            // Step 3: create the actual provider instance.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating an instance of text provider: {type}",
                assignedProvider.FactoryType
                );

            // Stand up an instance of the provider.
            var messageProvider = await _messageProviderFactory.CreateAsync(
                assignedProvider,
                cancellationToken
                ).ConfigureAwait(false);

            // Should never happen, but, pffft, check it anyway.
            if (messageProvider is null)
            {
                // If we get here, the provider was found, for this message,
                //   and we tried to create it, but that attempt failed, for
                //   some reason.

                // Log what we are about to do.
                _logger.LogError(
                    "Unable to create the provider: {name} for message: {id}",
                    assignedProvider.Name,
                    message.Id
                    );

                // Write the error to the provider log.
                _ = await _providerLogManager.CreateAsync(
                    new ProviderLog()
                    {
                        Event = ProcessEvent.ProcessError,
                        Message = message,
                        BeforeState = message.MessageState,
                        AfterState = message.MessageState,
                        Error = $"Unable to create provider factory type: {assignedProvider.FactoryType}!"
                    },
                    "host",
                    cancellationToken
                    );

                // At this point, the provider itself is not usable, since
                //   we can't create instances of it. So, the best thing to
                //   do is disable the provider so the algorithm can pick
                //   another one, next time around.

                // Log what we are about to do.
                _logger.LogWarning(
                    "Disabling provider: {name} since we can't create instances of it",
                    assignedProvider.Name
                    );

                // Update the model.
                assignedProvider.IsDisabled = true;

                // Update the underlying storage.
                _ = await _providerTypeManager.UpdateAsync(
                    assignedProvider,
                    "host",
                    cancellationToken
                    ).ConfigureAwait(false);

                continue; // Nothing more to do!
            }

            try
            {
                // =======
                // Step 4: pass the message to the provider, for processing.
                // =======

                // Defer to the provider.
                await messageProvider.SendTextAsync(
                    message,
                    assignedProvider,
                    cancellationToken
                    ).ConfigureAwait(false);

                // If we get here then we can safely assume the message state
                //   and the logs both reflect whatever the provider did, or
                //   didn't do, with the message - unless it threw an exception,
                //   in which case we deal with it below, in the catch block.
            }
            catch (Exception ex)
            {
                // If we get here there was a critical provider (or message) error.

                // Log what happened.
                _logger.LogError(
                    ex,
                    "The provider failed to process text message: {id}!",
                    message.Id
                    );

                // Log what we are about to do.
                _logger.LogDebug(
                    "Logging a provider error on text message: {id}",
                    message.Id
                    );

                // Write the error to the provider log.
                _ = await _providerLogManager.CreateAsync(
                    new ProviderLog()
                    {
                        Event = ProcessEvent.ProviderError,
                        Message = message,
                        BeforeState = message.MessageState,
                        AfterState = message.MessageState,
                        Error = ex.GetBaseException().Message
                    },
                    "host",
                    cancellationToken
                    );

                // TODO : figure out if we can recover from this.
            }
        }
    }

    #endregion
    */
    #endregion
}
