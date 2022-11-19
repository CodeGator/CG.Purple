
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
    /// <param name="logger">The logger to use with this director.</param>
    public ProcessDirector(
        IMailMessageManager mailMessageManager,
        IMessagePropertyManager messagePropertyManager,
        ITextMessageManager textMessageManager, 
        IProviderLogManager providerLogManager,
        IProviderTypeManager providerTypeManager,
        IPropertyTypeManager propertyTypeManager,
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
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _mailMessageManager = mailMessageManager;
        _messagePropertyManager = messagePropertyManager;   
        _textMessageManager = textMessageManager;
        _providerTypeManager = providerTypeManager;
        _providerLogManager = providerLogManager;
        _propertyTypeManager = propertyTypeManager;
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
            // Look for the provider property type.
            var providerTypeProperty = await _propertyTypeManager.FindByNameAsync(
                "Provider"
                ).ConfigureAwait(false);

            // Did we fail?
            if (providerTypeProperty is null ) 
            {
                // Panic!!
                throw new KeyNotFoundException(
                    "The 'Provider' property type was not found!"
                    );
            }

            try
            {
                // Look for the list of mail providers.
                var mailProviders = await _providerTypeManager.FindForEmailsAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Sanity check the providers.
                if (!mailProviders.Any())
                {
                    // Log what happened.
                    _logger.LogWarning(
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

            try
            {
                // Look for the list of text providers.
                var textProviders = await _providerTypeManager.FindForTextsAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Sanity check the providers.
                if (!textProviders.Any())
                {
                    // Log what happened.
                    _logger.LogWarning(
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
        // Get the pending mail messages.
        var messages = await _mailMessageManager.FindPendingAsync(
            cancellationToken
            ).ConfigureAwait(false);

        // Ensure every mail message has a provider.
        messages = await AssignProviderAsync(
            messages,
            providerTypeProperty,
            availableProviders,
            cancellationToken
            ).ConfigureAwait(false);

        // TODO : write the rest of this code.
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
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Looking for an available mail provider."
                        );

                    // For now, this will be our provider selection strategy.
                    var availableProvider = availableProviders
                        .OrderBy(x => x.Priority)
                        .FirstOrDefault();

                    // Should never happed, but, pffft, check it anyway.
                    if (availableProvider is null)
                    {
                        // Panic!!
                        throw new KeyNotFoundException(
                            "A provider capable of processing mail messages wasn't found!"
                            );
                    }

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
                    //   another collection, instead. Isn't C# fun?
                    assignedMessages.Add(message);

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
            // Return the results.
            return messages;
        }

        // Return the results.
        return assignedMessages;
    }

    // *******************************************************************

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
        // Get the pending text messages.
        var messages = await _textMessageManager.FindPendingAsync(
            cancellationToken
            ).ConfigureAwait(false);

        // Ensure every text message has a provider.
        await AssignProviderAsync(
            messages,
            providerTypeProperty,
            availableProviders,
            cancellationToken
            ).ConfigureAwait(false);

        // TODO : write the rest of this code.
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
            // Loop and assign a provider.
            foreach (var message in unassignedMessages)
            {
                try
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Looking for an available text provider."
                        );

                    // For now, this will be our provider selection strategy.
                    var availableProvider = availableProviders
                        .OrderBy(x => x.Priority)
                        .FirstOrDefault();

                    // Should never happed, but, pffft, check it anyway.
                    if (availableProvider is null)
                    {
                        // Panic!!
                        throw new KeyNotFoundException(
                            "A provider capable of processing text messages wasn't found!"
                            );
                    }

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
                    //   another collection, instead. Isn't C# fun?
                    assignedMessages.Add(message);

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
            // Return the results.
            return messages;
        }

        // Return the results.
        return assignedMessages;
    }

    #endregion
}
