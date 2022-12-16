
namespace CG.Purple.Providers.Twillio;

/// <summary>
/// This class is a Twillio implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
internal class TwillioProvider :
    MessageProviderBase<TwillioProvider>,
    IMessageProvider
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the text manager for this provider.
    /// </summary>
    internal protected readonly ITextMessageManager _textMessageManager = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="TwillioProvider"/>
    /// class.
    /// </summary>
    /// <param name="statusHub">The SignalR status context to use with this 
    /// provider.</param>
    /// <param name="textMessageManager">The text message manager to use
    /// with this provider.</param>
    /// <param name="messageManager">The message manager to use with this 
    /// provider.</param>
    /// <param name="processLogManager">The process log manager to use
    /// with this provider.</param>
    /// <param name="logger">The logger to use with this provider.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public TwillioProvider(
        StatusHub statusHub,
        ITextMessageManager textMessageManager,
        IMessageManager messageManager,
        IMessageLogManager processLogManager,
        ILogger<TwillioProvider> logger
        ) : base(
            statusHub,
            messageManager,
            processLogManager,
            logger
            )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(textMessageManager, nameof(textMessageManager));

        // Save the reference(s).
        _textMessageManager = textMessageManager;
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
            // ========
            // Step 1: Get the parameters we'll need.
            // ========

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the AccountSid parameter"
                );

            // Get the account SID.
            var accountSidProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "AccountSid"
                );

            // Did we fail?
            if (accountSidProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The 'AccountSid' parameter is missing, or invalid!"
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for the AuthToken parameter"
                );

            // Get the user name.
            var authTokenProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "AuthToken"
                );

            // Did we fail?
            if (authTokenProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The 'AuthToken' parameter is missing, or invalid!"
                    );
            }

            // ========
            // Step 2: Initialize the Twillio client.
            // ========

            // Log what we are about to do.
            _logger.LogDebug(
                "Initializing the Twillio client"
                );

            // Initialize the client.
            TwilioClient.Init(
                accountSidProperty.Value,
                authTokenProperty.Value
                );

            // ========
            // Step 3: Process the messages
            // ========

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
                    // Step 3A: Validate the message type.
                    // ========

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Ensuring type is correct for message: {id}",
                        message.Id
                        );

                    // Should never happen, but, pffft, check it anyway.
                    if (message.MessageType != MessageType.Text)
                    {
                        // Update the message and record the event.
                        await MessageIsWrongTypeAsync(
                            message,
                            cancellationToken
                            ).ConfigureAwait(false);

                        continue; // Nothing left to do!
                    }

                    // ========
                    // Step 3B: Get the complete message.
                    // ========

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Fetching the text part of message: {id}",
                        message.Id
                        );

                    // Get the text portion of the message.
                    var textMessage = await _textMessageManager.FindByIdAsync(
                        message.Id,
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Should never happen, but, pffft, check it anyway.
                    if (textMessage is null)
                    {
                        // Update the message and record the event.
                        await UnableToFindMailMessageAsync(
                            message,
                            cancellationToken
                            ).ConfigureAwait(false);

                        continue; // Nothing left to do!
                    }

                    // ========
                    // Step 3C: Send the message.
                    // ========

                    // TODO : look into ways to support the mediaUrl parameter,
                    //   on the Twillio client, which requires that all media
                    //   be publicly accessible through a url. 

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Sending message: {id}",
                        message.Id
                        );

                    // Send the message.
                    var response = MessageResource.Create(
                        body: textMessage.Body,
                        from: new Twilio.Types.PhoneNumber(textMessage.From),
                        to: new Twilio.Types.PhoneNumber(textMessage.To)
                        );

                    // Did we succeed?
                    if (response.Status == MessageResource.StatusEnum.Queued ||
                        response.Status == MessageResource.StatusEnum.Sent)
                    {
                        // Log what we did.
                        _logger.LogInformation(
                            "Message: {id} was sent using provider: {name}.",
                            message.Id,
                            nameof(TwillioProvider)
                            );

                        // ========
                        // Step 3D: Transition the message.
                        // ========

                        // Update the message and record the event.
                        await MessageWasSentAsync(
                            message,
                            cancellationToken
                            ).ConfigureAwait(false);
                    }
                    else
                    {
                        // Log what happened.
                        _logger.LogWarning(
                            "Message: {id} failed to send. ErrorCode: {code} ErrorMessage: {msg} using provider: {name}!",
                            message.Id,
                            response.ErrorCode,
                            response.ErrorMessage,
                            nameof(TwillioProvider)
                            );

                        // ========
                        // Step 3D: Transition the message.
                        // ========

                        // Update the message and record the event.
                        await MessageFailedToSendAsync(
                            $"ErrorCode: {response.ErrorCode}, ErrorMessage: {response.ErrorMessage}",
                            message,
                            cancellationToken
                            ).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    // Log what happened.
                    _logger.LogWarning(
                        "Message: {id} failed to send: {err} using provider: {name}!",
                        message.Id,
                        ex.GetBaseException().Message,
                        nameof(TwillioProvider)
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
}
