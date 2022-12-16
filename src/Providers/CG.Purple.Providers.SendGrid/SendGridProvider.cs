
namespace CG.Purple.Providers.SendGrid;

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
    /// This field contains the SendGrid client for this provider.
    /// </summary>
    internal protected readonly ISendGridClient _sendGridClient;

    /// <summary>
    /// This field contains the mail manager for this provider.
    /// </summary>
    internal protected readonly IMailMessageManager _mailMessageManager = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SendGridProvider"/>
    /// class.
    /// </summary>
    /// <param name="sendGridClient">The SendGrid client to use with this 
    /// provider.</param>
    /// <param name="statusHub">The SignalR status hub to use with this 
    /// provider.</param>
    /// <param name="mailMessageManager">The mail message manager to use
    /// with this provider.</param>
    /// <param name="messageManager">The message manager to use with this 
    /// provider.</param>
    /// <param name="processLogManager">The process log manager to use with 
    /// this provider.</param>
    /// <param name="logger">The logger to use with this provider.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public SendGridProvider(
        ISendGridClient sendGridClient,
        StatusHub statusHub,
        IMailMessageManager mailMessageManager,
        IMessageManager messageManager,
        IMessageLogManager processLogManager,
        ILogger<SendGridProvider> logger
        ) : base(
            statusHub,
            messageManager,
            processLogManager,
            logger
            )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(sendGridClient, nameof(sendGridClient))
            .ThrowIfNull(mailMessageManager, nameof(mailMessageManager));

        // Save the reference(s).
        _sendGridClient = sendGridClient;
        _mailMessageManager = mailMessageManager;
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
            // Step 1: Process the individual messages.
            // =======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looping through {count} messages",
                messages.Count()
                );

            // Loop through the messages.
            foreach (var message in messages)
            {
                try
                {
                    // ========
                    // Step 1A: Validate the message type.
                    // ========

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Ensuring type is correct for message: {id}",
                        message.Id
                        );

                    // Should never happen, but, pffft, check it anyway.
                    if (message.MessageType != MessageType.Mail)
                    {
                        // Update the message and record the event.
                        await MessageIsWrongTypeAsync(
                            message,
                            cancellationToken
                            ).ConfigureAwait(false);

                        continue; // Nothing left to do!
                    }

                    // ========
                    // Step 1B: Get the complete message.
                    // ========

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Fetching the email part of message: {id}",
                        message.Id
                        );

                    // Get the mail portion of the message.
                    var mailMessage = await _mailMessageManager.FindByIdAsync(
                        message.Id,
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Should never happen, but, pffft, check it anyway.
                    if (mailMessage is null)
                    {
                        // Update the message and record the event.
                        await UnableToFindMailMessageAsync(
                            message,
                            cancellationToken
                            ).ConfigureAwait(false);

                        continue; // Nothing left to do!
                    }

                    // ========
                    // Step 1C: Wrap the message.
                    // ========

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Creating a .NET wrapper for message: {id}",
                        message.Id
                        );

                    // Create a .NET model for the message.
                    var msg = CreateDotNetMessage(mailMessage);

                    // ========
                    // Step 1D: Send the message.
                    // ========

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Sending the message: {id}",
                        mailMessage.Id
                        );

                    // Send the message.
                    var response = await _sendGridClient.SendEmailAsync(
                        msg,
                        cancellationToken
                        ).ConfigureAwait(false);

                    // ========
                    // Step 1E: Transition the message.
                    // ========

                    // Did we fail?
                    if (!response.IsSuccessStatusCode)
                    {
                        // Log what happened.
                        _logger.LogWarning(
                            "Message: {id} failed to send. StatusCode: {code} using provider: {name}!",
                            message.Id,
                            response.StatusCode,
                            nameof(SendGridProvider)
                            );

                        // Update the message and record the event.
                        await MessageFailedToSendAsync(
                            $"StatusCode: {response.StatusCode}",
                            message,
                            cancellationToken
                            ).ConfigureAwait(false);
                    }
                    else
                    {
                        // Log what we did.
                        _logger.LogInformation(
                            "Message: {id} was sent using provider: {name}.",
                            message.Id,
                            nameof(SendGridProvider)
                            );

                        // Update the message and record the event.
                        await MessageWasSentAsync(
                            message,
                            cancellationToken
                            ).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    // Log what happened.
                    _logger.LogInformation(
                        "Message: {id} failed to send: {err} using provider: {name}!",
                        message.Id,
                        ex.GetBaseException().Message,
                        nameof(SendGridProvider)
                        );

                    // Update the message and record the event.
                    await MessageFailedToSendAsync(
                        ex.GetBaseException().Message,
                        message,
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
    /// <returns>An <see cref="SendGridMessage"/> object.</returns>
    private SendGridMessage CreateDotNetMessage(
        MailMessage mailMessage
        )
    {
        // Log what we are about to do.
        _logger.LogDebug(
            "Creating a SendGridMessage object"
            );

        // Create the .NET model.
        var msg = MailHelper.CreateSingleEmailToMultipleRecipients(
            MailHelper.StringToEmailAddress(mailMessage.From),
            mailMessage.To.Split(';').Select(x => MailHelper.StringToEmailAddress(x)).ToList(),
            mailMessage.Subject,
            mailMessage.IsHtml ? "" : mailMessage.Body,
            mailMessage.IsHtml ? mailMessage.Body : ""
            );

        // Is there a CC address?
        if (!string.IsNullOrEmpty(mailMessage.CC))
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the CC address"
                );

            // Add the CC addresses.
            msg.AddCcs(
                mailMessage.CC.Split(';').Select(x => MailHelper.StringToEmailAddress(x)).ToList()
                );
        }

        // Is there a BCC address?
        if (!string.IsNullOrEmpty(mailMessage.BCC))
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the BCC address"
                );

            // Add the BCC addresses.
            msg.AddBccs(
                mailMessage.BCC.Split(';').Select(x => MailHelper.StringToEmailAddress(x)).ToList()
                );
        }

        // Any attachments?
        if (mailMessage.Attachments.Any())
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Adding the attachments"
                );

            // Add the attachments.
            msg.AddAttachments(mailMessage.Attachments.Select(x =>
                new global::SendGrid.Helpers.Mail.Attachment()
                {
                    Content = Convert.ToBase64String(x.Data),
                    ContentId = $"{x.Id}",
                    Filename = x.OriginalFileName,
                    Disposition = "attachment",
                    Type = $"{x.MimeType.Type}/{x.MimeType.SubType}"
                }));
        }

        // Return the results.
        return msg;
    }

    #endregion
}
