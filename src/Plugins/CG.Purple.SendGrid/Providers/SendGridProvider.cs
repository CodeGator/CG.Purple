namespace CG.Purple.SendGrid.Providers;

/// <summary>
/// This class is a SendGrid implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
internal class SendGridProvider :
    MessageProviderBase<SendGridProvider>,
    IMessageProvider
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the mail manager for this provider.
    /// </summary>
    internal protected readonly IMailMessageManager _mailMessageManager = null!;

    /// <summary>
    /// This field contains the message manager for this provider.
    /// </summary>
    internal protected readonly IMessageManager _messageManager = null!;

    /// <summary>
    /// This field contains the message property manager for this director.
    /// </summary>
    internal protected readonly IMessagePropertyManager _messagePropertyManager = null!;

    /// <summary>
    /// This field contains the process log manager for this director.
    /// </summary>
    internal protected readonly IMessageLogManager _processLogManager = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SendGridProvider"/>
    /// class.
    /// </summary>
    /// <param name="mailMessageManager">The mail message manager to use
    /// with this provider.</param>
    /// <param name="messageManager">The message manager to use with this 
    /// provider.</param>
    /// <param name="messagePropertyManager">The message property manager 
    /// to use with this provider.</param>
    /// <param name="processLogManager">The process log manager to use
    /// with this provider.</param>
    /// <param name="logger">The logger to use with this provider.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public SendGridProvider(
        IMailMessageManager mailMessageManager,
        IMessageManager messageManager,
        IMessageLogManager processLogManager,
        IMessagePropertyManager messagePropertyManager,
        ILogger<SendGridProvider> logger
        ) : base(logger)
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mailMessageManager, nameof(mailMessageManager))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(messagePropertyManager, nameof(messagePropertyManager));

        // Save the reference(s).
        _mailMessageManager = mailMessageManager;
        _messageManager = messageManager;
        _messagePropertyManager = messagePropertyManager;
        _processLogManager = processLogManager;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public override async Task ProcessMessagesAsync(
        IEnumerable<Message> messages,
        ProviderType providerType,
        CancellationToken cancellationToken = default
        )
    {/*
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messages, nameof(messages))
            .ThrowIfNull(providerType, nameof(providerType));

        try
        {
            // =======
            // Step 1: Find the parameters we'll need.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the ServerUrl parameter"
                );

            // Get the API key.
            var apiKeyParameter = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "ApiKey"
                );

            // Did we fail?
            if (apiKeyParameter is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The 'ApiKey' parameter is missing, or invalid!"
                    );
            }

            // =======
            // Step 2: Create the client.
            // =======

            // Create the SendGrid client.
            var client = new SendGridClient(
                apiKeyParameter.Value
                );

            // =======
            // Step 3: Process the individual messages.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looping through {count} messages",
                messages.Count()
                );

            // Loop through the messages.
            foreach (var message in messages)
            {
                // =======
                // Step 3A: Validate the message type.
                // =======

                // Log what we are about to do.
                _logger.LogDebug(
                    "Ensuring the message type is correct"
                    );

                // Should never happen, but, pffft, check it anyway.
                if (message.MessageType != MessageType.Mail)
                {
                    // Record what happened, in the log.
                    _ = await _processLogManager.LogErrorEventAsync(
                        "Message isn't an email!",
                        providerType,
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Bumping the error count, for message: {id}",
                        message.Id
                        );

                    // Bump the error count for the message.
                    await message.BumpErrorCountAsync(
                        _messageManager,
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Resetting the provider, for message: {id}",
                        message.Id
                        );

                    // Since the provider can't process this message, we'll reset
                    //   it. This away the pipeline can (hopefully) assign it to a
                    //   provider that can process it.
                    await ResetMessageAsync(
                        message,
                        cancellationToken
                        ).ConfigureAwait(false);
                        
                    continue; // Nothing left to do!
                }

                // Log what we are about to do.
                _logger.LogDebug(
                    "Fetching the rest of the email message"
                    );

                // Get the mail portion of the message.
                var mailMessage = await _mailMessageManager.FindByIdAsync(
                    message.Id,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should never happen, but, pffft, check it anyway.
                if (mailMessage is null)
                {
                    // Record what happened, in the log.
                    _ = await _processLogManager.LogErrorEventAsync(
                        "Unable to find the email for processing!",
                        providerType,
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Bumping the error count, for message: {id}",
                        message.Id
                        );

                    // Bump the error count for the message.
                    await message.BumpErrorCountAsync(
                        _messageManager,
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Resetting the provider, for message: {id}",
                        message.Id
                        );

                    // Since the provider can't process this message, we'll reset
                    //   it. This away the pipeline can (hopefully) assign it to a
                    //   provider that can process it.
                    await ResetMessageAsync(
                        message,
                        cancellationToken
                        ).ConfigureAwait(false);

                    continue; // Nothing left to do!
                }

                try 
                {
                    var from = new EmailAddress(message.From);
                    var to = new EmailAddress(message.);
                    var plainTextContent = "and easy to do anywhere, even with C#";
                    var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = await client.SendEmailAsync(msg);
                }
                catch (Exception ex)
                {
                    // =======
                    // Step 3D: Mark the message as failed.
                    // =======

                    // TODO : write the code for this.
                }
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
            throw new ProviderException(
                relatedProvider: providerType,
                message: $"The provider failed to process one or more messages!",
                innerException: ex
                );
        }*/
    }

    #endregion
}
