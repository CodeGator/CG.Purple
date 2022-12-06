namespace CG.Purple.Smtp.Providers;

/// <summary>
/// This class is a SMTP implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
internal class SmtpProvider :
    MessageProviderBase<SmtpProvider>,
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
    /// This constructor creates a new instance of the <see cref="SmtpProvider"/>
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
    public SmtpProvider(
        IMailMessageManager mailMessageManager,
        IMessageManager messageManager,
        IMessageLogManager processLogManager,
        IMessagePropertyManager messagePropertyManager,
        ILogger<SmtpProvider> logger
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
    {
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

            // Get the server url.
            var serverUrlProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "ServerUrl"
                );

            // Did we fail?
            if (serverUrlProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The 'ServerUrl' parameter is missing, or invalid!"
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the UserName parameter"
                );

            // Get the user name.
            var userNameProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "UserName"
                );

            // Did we fail?
            if (userNameProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The 'UserName' parameter is missing, or invalid!"
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the Password parameter"
                );

            // Get the password.
            var passwordProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "Password"
                );

            // Did we fail?
            if (passwordProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The 'Password' parameter is missing, or invalid!"
                    );
            }

            // =======
            // Step 2: Create the .NET mail client.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a .NET SMTP client"
                );

            // Create the SMTP client.
            using System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(
                serverUrlProperty.Value
                );

            // Set the credentials for the client.
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(
                userNameProperty.Value,
                passwordProperty.Value
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
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Creating a .NET wrapper for message: {id}",
                        message.Id
                        );

                    // Create the .NET model.
                    var msg = CreateDotNetMessage(
                        mailMessage
                        );

                    // =======
                    // Step 4: Send the email.
                    // =======

                    // Log what we are about to do.
                    _logger.LogTrace(
                        "Deferring to {method} for message: {id}",
                        nameof(System.Net.Mail.SmtpClient.Send),
                        mailMessage.Id
                        );

                    // Send the message.
                    client.Send(msg);

                    // Log what we did.
                    _logger.LogInformation(
                        "Message: {id} was sent.",
                        message.Id
                        );

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Transitioning message: {id} to the {state} state.",
                        mailMessage.Id,
                        MessageState.Sent
                        );

                    // Transition to the 'Sent' state.
                    await mailMessage.ToSentStateAsync(
                        _messageManager,
                        _processLogManager,
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Log what happened.
                    _logger.LogWarning(
                        "Message: {id} failed to send: {err}!",
                        message.Id,
                        ex.GetBaseException().Message
                        );

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Bumping the error count, for message: {id}",
                        message.Id
                        );

                    // Bump the error count on the message.
                    await mailMessage.BumpErrorCountAsync(
                        _messageManager,
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Transitioning message: {id} to the {state} state.",
                        mailMessage.Id,
                        MessageState.Failed
                        );

                    // Transition to the 'Failed' state.
                    await mailMessage.ToFailedStateAsync(
                        ex,
                        _messageManager,
                        _processLogManager,
                        providerType,
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);
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
        }
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method creates a new .NET wrapper for an email.
    /// </summary>
    /// <param name="mailMessage">The mail message to use for the operation.</param>
    /// <returns>An <see cref="System.Net.Mail.MailMessage"/> object.</returns>
    private System.Net.Mail.MailMessage CreateDotNetMessage(
        MailMessage mailMessage
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Creating a System.Net.Mail.MailMessage object"
            );

        var dotNetMessage = new System.Net.Mail.MailMessage()
        {
            From = new System.Net.Mail.MailAddress(mailMessage.From),
            Subject = mailMessage.Subject,
            Body = mailMessage.Body,
            IsBodyHtml = mailMessage.IsHtml
        };

        // Log what we are about to do.
        _logger.LogDebug(
            "Setting To address(es) on the mail object"
            );

        // Set the target address(es).
        foreach (var to in mailMessage.To.Split(';'))
        {
            if (!string.IsNullOrEmpty(to))
            {
                dotNetMessage.To.Add(to);
            }
        }

        // Was a CC supplied?
        if (!string.IsNullOrEmpty(mailMessage.CC))
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Setting CC address(es) on the mail object"
                );

            // Set the CC address(es).
            foreach (var cc in mailMessage.CC.Split(';'))
            {
                if (!string.IsNullOrEmpty(cc))
                {
                    dotNetMessage.CC.Add(cc);
                }
            }
        }

        // Was a BCC supplied?
        if (!string.IsNullOrEmpty(mailMessage.BCC))
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Setting BCC address(es) on the mail object"
                );

            // Set the BCC address(es).
            foreach (var bcc in mailMessage.BCC.Split(';'))
            {
                if (!string.IsNullOrEmpty(bcc))
                {
                    dotNetMessage.Bcc.Add(bcc);
                }
            }
        }

        // Log what we are about to do.
        _logger.LogDebug(
            "Setting any attachment(s) on the mail object"
            );

        // Were there attachments?
        foreach (var attachment in mailMessage.Attachments)
        {
            // Get the data and mime type.
            using var contentStream = new MemoryStream(attachment.Data);
            var mimeType = $"{attachment.MimeType.Type}/{attachment.MimeType.SubType}";

            // Set the attachment.
            dotNetMessage.Attachments.Add(
                    new System.Net.Mail.Attachment(
                        contentStream,
                        mimeType
                        )
                    );
        }

        // Log what we are about to do.
        _logger.LogDebug(
            "Returning a populated mail object"
            );

        // Return the results.
        return dotNetMessage;
    }

    // *******************************************************************

    /// <summary>
    /// This method resets the given message to a 'Pending' state and clears
    /// the error count on the message. 
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task ResetMessageAsync(
        Message message,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Removing the provider from message: {id}",
            message.Id
            );

        // Update the message.
        message.ProviderType = null;

        // Save the changes.
        _ = await _messageManager.UpdateAsync(
            message,
            "host",
            cancellationToken
            ).ConfigureAwait(false);

        // Transition back to the 'Pending' state.
        await message.ToPendingStateAsync(
            _messageManager,
            _processLogManager,
            "host",
            cancellationToken
            ).ConfigureAwait(false);
    }

    #endregion
}
