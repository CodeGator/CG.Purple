
namespace CG.Purple.Host.Directors;

// TODO : obviously, this entire class needs to be refactored. I'm looking
//   at creating an IMessageManager type, and using that here to collapse
//   the mail/text processing paths. Next, I'll eventually be moving code
//   into private methods, to get rid of these crazy long methods that we
//   have now. But, for now, I'm still trying to work out the overall flow,
//   so, we'll have to deal with long methods for a bit longer.

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
    /// This field contains the mail message manager for this director.
    /// </summary>
    internal protected readonly IMailMessageManager _mailMessageManager = null!;

    /// <summary>
    /// This field contains the message property manager for this director.
    /// </summary>
    internal protected readonly IMessagePropertyManager _messagePropertyManager = null!;

    /// <summary>
    /// This field contains the text message manager for this director.
    /// </summary>
    internal protected readonly ITextMessageManager _textMessageManager = null!;

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
    /// <param name="mailMessageManager">The mail message manager to use
    /// with this director.</param>
    /// <param name="messagePropertyManager">The message property manager 
    /// to use with this director.</param>
    /// <param name="textMessageManager">The text message manager to use 
    /// with this director.</param>
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
        IMailMessageManager mailMessageManager,
        IMessagePropertyManager messagePropertyManager,
        ITextMessageManager textMessageManager, 
        IProviderLogManager providerLogManager,
        IProviderTypeManager providerTypeManager,
        IPropertyTypeManager propertyTypeManager,
        IMessageProviderFactory messageProviderFactory,
        ILogger<IProcessDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mailMessageManager, nameof(mailMessageManager))
            .ThrowIfNull(messagePropertyManager, nameof(messagePropertyManager))
            .ThrowIfNull(textMessageManager, nameof(textMessageManager))
            .ThrowIfNull(providerLogManager, nameof(providerLogManager))
            .ThrowIfNull(providerTypeManager, nameof(providerTypeManager))
            .ThrowIfNull(propertyTypeManager, nameof(propertyTypeManager))
            .ThrowIfNull(messageProviderFactory, nameof(messageProviderFactory))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _mailMessageManager = mailMessageManager;
        _messagePropertyManager = messagePropertyManager;   
        _textMessageManager = textMessageManager;
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
    public virtual async Task ProcessAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // =======
            // Step 1: Find the 'Provider' property type.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for a provider property on a message."
                );

            // Look for the provider property type.
            var providerTypeProperty = await _propertyTypeManager.FindByNameAsync(
                "Provider"
                ).ConfigureAwait(false);

            // Should never happen, but, pffft, check it anyway.
            if (providerTypeProperty is null ) 
            {
                // If we get here then the database doesn't have a 'Provider'
                //   property, which isn't really recoverable.

                // Panic!!
                throw new KeyNotFoundException(
                    "The 'Provider' property type was not found!"
                    );
            }

            // =======
            // Step 2: Process the pending mail messages.
            // =======

            try
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Looking for a list of available mail providers."
                    );

                // Look for the list of mail providers.
                var mailProviders = await _providerTypeManager.FindForEmailsAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should never happen, but, pffft, check it anyway.
                if (!mailProviders.Any())
                {
                    // If we get here then no providers for emails were found, which
                    //   might happen because (A): somebody deleted something important
                    //   in the database, or (B): somebody disabled all the mail providers,
                    //   or (C): the pipeline itself has disabled one or more providers
                    //   because it's having trouble creating them. Obviously, none of
                    //   these situations are normal. Unfortunately, they also aren't 
                    //   recoverable. Yikes!

                    // Log what happened.
                    _logger.LogError(
                        "No providers were found that are capable of " +
                        "processing mail messages!"
                        );

                    // Let's not hammer the poor log.
                    await Task.Delay(
                        TimeSpan.FromSeconds(30)
                        ).ConfigureAwait(false);
                }
                else
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Processing pending mail messages."
                        );

                    // Process mail messages.
                    await ProcessPendingMailMessagesAsync(
                        providerTypeProperty,
                        mailProviders,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                // If we get here then something died while processing a mail message.

                // Log what happened.
                _logger.LogError(
                    ex,
                    "Failed to process mail messages!"
                    );

                // Provider better context.
                throw new DirectorException(
                    message: $"The director failed to process mail messages!",
                    innerException: ex
                    );
            }

            // =======
            // Step 3: Process the pending text messages.
            // =======

            try
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Looking for a list of available text providers."
                    );

                // Look for the list of text providers.
                var textProviders = await _providerTypeManager.FindForTextsAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should never happen, but, pffft, check it anyway.
                if (!textProviders.Any())
                {
                    // If we get here then no providers for texts were found, which
                    //   might happen because (A): somebody deleted something important
                    //   in the database, or (B): somebody disabled all the text providers,
                    //   or (C): the pipeline itself has disabled one or more providers
                    //   because it's having trouble creating them. Obviously, none of
                    //   these situations are normal. Unfortunately, they also aren't 
                    //   recoverable. Yikes!

                    // Log what happened.
                    _logger.LogError(
                        "No providers were found that are capable of " +
                        "processing text messages!"
                        );

                    // Let's not hammer the poor log.
                    await Task.Delay(
                        TimeSpan.FromSeconds(30)
                        ).ConfigureAwait(false);
                }
                else
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Processing pending text messages."
                        );

                    // Process text messages.
                    await ProcessPendingTextMessagesAsync(
                        providerTypeProperty,
                        textProviders,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                // If we get here then something died while processing a text message.

                // Log what happened.
                _logger.LogError(
                    ex,
                    "Failed to process text messages!"
                    );

                // Provider better context.
                throw new DirectorException(
                    message: $"The director failed to process text messages!",
                    innerException: ex
                    );
            }
        }
        catch (Exception ex)
        {
            // If we get here then something died during processing.

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

    #endregion
}
