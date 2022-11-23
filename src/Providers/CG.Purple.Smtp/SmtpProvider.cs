
namespace CG.Purple.Smtp;

/// <summary>
/// This class is a SMTP implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
internal class SmtpProvider : IMessageProvider
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
    internal protected readonly IProcessLogManager _processLogManager = null!;

    /// <summary>
    /// This field contains the logger for this provider.
    /// </summary>
    internal protected readonly ILogger<IMessageProvider> _logger = null!;

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
    public SmtpProvider(
        IMailMessageManager mailMessageManager,
        IMessageManager messageManager,
        IProcessLogManager processLogManager,
        IMessagePropertyManager messagePropertyManager,
        ILogger<IMessageProvider> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mailMessageManager, nameof(mailMessageManager))
            .ThrowIfNull(messageManager, nameof(messageManager))
            .ThrowIfNull(processLogManager, nameof(processLogManager))
            .ThrowIfNull(messagePropertyManager, nameof(messagePropertyManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _mailMessageManager = mailMessageManager;
        _messageManager = messageManager;
        _messagePropertyManager = messagePropertyManager;
        _processLogManager = processLogManager;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task ProcessMessagesAsync(
        IEnumerable<Message> messages,
        IEnumerable<ProviderParameter> parameters,
        PropertyType providerPropertyType,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(messages, nameof(messages))
            .ThrowIfNull(providerPropertyType, nameof(providerPropertyType))
            .ThrowIfNull(parameters, nameof(parameters));

        try
        {
            // =======
            // Step 1: Find the parameters we'll need.
            // =======

            // Get the server url.
            var serverUrlProperty = parameters.FirstOrDefault(
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

            // Get the user name.
            var userNameProperty = parameters.FirstOrDefault(
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

            // Get the password.
            var passwordProperty = parameters.FirstOrDefault(
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

            // Loop through the messages.
            foreach (var message in messages)
            {
                // Should never happen, but, pffft, check it anyway.
                if (message.MessageType != MessageType.Mail)
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
                            Error = "Message isn't an email!"
                        },
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Since this provider can't process this message, we'll
                    //   reset the message to give the pipeline another chance
                    //   to process it.
                    await ResetMessageAsync(
                        message,
                        providerPropertyType,
                        cancellationToken
                        ).ConfigureAwait(false);

                    continue; // Nothing left to do!
                }

                // Get the mail portion of the message.
                var mailMessage = await _mailMessageManager.FindByIdAsync(
                    message.Id,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should never happen, but, pffft, check it anyway.
                if (mailMessage is null)
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
                            Error = "Unable to find the email for processing!"
                        },
                        "host",
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Since this provider can't process this message, we'll
                    //   reset the message to give the pipeline another chance
                    //   to process it.
                    await ResetMessageAsync(
                        message,
                        providerPropertyType,
                        cancellationToken
                        ).ConfigureAwait(false);

                    continue; // Nothing left to do!
                }

                // Create the .NET model.
                var msg = CreateDotNetMessage(
                    mailMessage
                    );

                // =======
                // Step 4: Send the email.
                // =======

                try
                {
                    // Log what we are about to do.
                    _logger.LogTrace(
                        "Deferring to {method} for message: {id}",
                        nameof(System.Net.Mail.SmtpClient.Send),
                        mailMessage.Id
                        );

                    // Send the message.
                    client.Send(msg);

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
                    _logger.LogError(
                        ex,
                        "Failed to send an email!"
                        );

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
                "Failed to process messages!"
                );

            // Provider better context.
            throw new ProviderException(
                message: $"The provider failed to process messages!",
                innerException: ex
                );
        }
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    private System.Net.Mail.MailMessage CreateDotNetMessage(
        MailMessage mailMessage
        )
    {
        var dotNetMessage = new System.Net.Mail.MailMessage()
        {
            From = new System.Net.Mail.MailAddress(mailMessage.From),
            Subject = mailMessage.Subject,
            Body = mailMessage.Body,
            IsBodyHtml = mailMessage.IsHtml
        };

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
            // Set the BCC address(es).
            foreach (var bcc in mailMessage.BCC.Split(';'))
            {
                if (!string.IsNullOrEmpty(bcc))
                {
                    dotNetMessage.Bcc.Add(bcc);
                }
            }
        }

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

        // Return the results.
        return dotNetMessage;
    }

    // *******************************************************************

    /// <summary>
    /// This method resets the given message to a 'Pending' state. 
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="providerPropertyType">The provider property type to
    /// use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private async Task ResetMessageAsync(
        Message message,
        PropertyType providerPropertyType,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Looking for provider property on message: {id}",
            message.Id 
            );

        // Find the corresponding provider property, for the message.
        var assignedProviderProperty = message.MessageProperties.FirstOrDefault(
            x => x.PropertyType.Id == providerPropertyType.Id
            );

        // Should never happen, but, pffft, check it anyway.
        if (assignedProviderProperty is null) 
        {
            // Log what we are about to do.
            _logger.LogError(
                "Failed to find the assigned provider property on message: {id}!",
                message.Id    
                );

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
                    Error = "Failed to find the assigned provider property!"
                },
                "host",
                cancellationToken
                ).ConfigureAwait(false);

            return; // Nothing to do!
        }

        // Log what we are about to do.
        _logger.LogDebug(
            "Creating a log entry for message: {id}",
            message.Id
            );

        // Update the local model.
        message.MessageProperties.Remove(
            assignedProviderProperty
            );

        // Update the database.
        await _messagePropertyManager.DeleteAsync(
            assignedProviderProperty,
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
