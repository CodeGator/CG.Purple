
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
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the random number generator for the provider.
    /// </summary>
    internal protected readonly RandomNumberGenerator _random = null!;

    #endregion

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
        // Create the randomizer.
        _random = RandomNumberGenerator.Create();  
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
            // ========
            // Step 1: Simulate the processing of messages.
            // ========

            // Log what we are about to do.
            _logger.LogDebug(
                "Looping through {count} messages",
                messages.Count()
                );

            // Loop through the messages.
            foreach (var message in messages)
            {
                // TODO : write the code for this.
                /*
                // Update the message and record the event.
                await MessageWasSentAsync(
                    message,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Update the message and record the event.
                await MessageFailedToSendAsync(
                    ex.GetBaseException().Message,
                    message,
                    cancellationToken
                    ).ConfigureAwait(false);
                */
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
