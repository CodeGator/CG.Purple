
namespace CG.Purple.Providers.DoNothing;

/// <summary>
/// This class is a "do nothing" implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
internal class DoNothingProvider :
    MessageProviderBase<DoNothingProvider>,
    IMessageProvider
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="DoNothingProvider"/>
    /// class.
    /// </summary>
    /// <param name="messageManager">The message manager to use with this 
    /// provider.</param>
    /// <param name="processLogManager">The process log manager to use
    /// with this provider.</param>
    /// <param name="logger">The logger to use with this provider.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public DoNothingProvider(
        IMessageManager messageManager,
        IMessageLogManager processLogManager,
        ILogger<DoNothingProvider> logger
        ) : base(
            messageManager,
            processLogManager,
            logger
            )
    {
        
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

        // The purpose of this provider is to provide a way to simulate
        //   the sending of messages, for test purposes. 

        try
        {
            // =======
            // Step 1: Find the parameters we'll need.
            // =======

            // Get the max messages per day.
            var maxMessagesPerDay = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "MaxMessagesPerDay"
                );

            // ========
            // Step 2: Simulate the processing of messages.
            // ========

            // Log what we are about to do.
            _logger.LogDebug(
                "Looping through {count} messages",
                messages.Count()
                );

            // Loop through the messages.
            foreach (var message in messages)
            {
                // Get a random number.
                var bytes = new byte[2];
                _random.GetNonZeroBytes(bytes);

                // Should we simulate success, or failure?
                if (bytes[0] > bytes[1]) 
                {
                    // Log what we are about to do.
                    _logger.LogInformation(
                        "Simulating a send for message: {id} using provider: {name}",
                        message.Id,
                        nameof(DoNothingProvider)
                        );

                    // Update the message and record the event.
                    await MessageWasSentAsync(
                        message,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
                else
                {
                    // Has this message already failed at least once? 
                    if (message.ErrorCount > 0)
                    {
                        // Should we simulate success, or failure?
                        if (bytes[0] < 127)
                        {
                            // Log what we are about to do.
                            _logger.LogInformation(
                                "Simulating a fail for message: {id} using provider: {name}",
                                message.Id,
                                nameof(DoNothingProvider)
                                );

                            // Update the message and record the event.
                            await MessageFailedToSendAsync(
                                "Simulated message send failure",
                                message,
                                cancellationToken
                                ).ConfigureAwait(false);
                        }
                        else
                        {
                            // Log what we are about to do.
                            _logger.LogInformation(
                                "Simulating a send for message: {id} using provider: {name}",
                                message.Id,
                                nameof(DoNothingProvider)
                                );

                            // Update the message and record the event.
                            await MessageWasSentAsync(
                                message,
                                cancellationToken
                                ).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        // Should we simulate success, or failure?
                        if (bytes[0] < 85)
                        {
                            // Log what we are about to do.
                            _logger.LogInformation(
                                "Simulating a fail for message: {id} using provider: {name}",
                                message.Id,
                                nameof(DoNothingProvider)
                                );

                            // Update the message and record the event.
                            await MessageFailedToSendAsync(
                                "Simulated message send failure",
                                message,
                                cancellationToken
                                ).ConfigureAwait(false);
                        }
                        else
                        {
                            // Log what we are about to do.
                            _logger.LogInformation(
                                "Simulating a send for message: {id} using provider: {name}",
                                message.Id,
                                nameof(DoNothingProvider)
                                );

                            // Update the message and record the event.
                            await MessageWasSentAsync(
                                message,
                                cancellationToken
                                ).ConfigureAwait(false);
                        }
                    }
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
