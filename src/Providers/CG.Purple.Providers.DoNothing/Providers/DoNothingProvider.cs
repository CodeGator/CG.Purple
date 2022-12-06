using CG.Purple.Managers;

namespace CG.Purple.DoNothing.Providers;

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
    public override Task ProcessMessagesAsync(
        IEnumerable<Message> messages,
        ProviderType providerType,
        CancellationToken cancellationToken = default
        )
    {
        return Task.CompletedTask;
    }

    #endregion
}
