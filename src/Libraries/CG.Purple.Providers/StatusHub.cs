
namespace CG.Purple.Providers;

/// <summary>
/// This class is a SignalR hub for the status back channel.
/// </summary>
public class StatusHub : Hub
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the service provider for this hub.
    /// </summary>
    internal protected readonly IServiceProvider _serviceProvider;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="StatusHub"/>
    /// class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use with
    /// this hub.</param>
    public StatusHub(
        IServiceProvider serviceProvider
        ) 
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceProvider, nameof(serviceProvider));

        // Save the reference(s).
        _serviceProvider = serviceProvider;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method sends a status notification for the given message.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task to perform the operation.</returns>
    public async virtual Task OnStatusAsync(
        Message message,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message));

        // Do we have any clients to notify?
        if (Clients is not null)
        {
            // Create a DI scope.
            using var scope = _serviceProvider.CreateScope();

            // Create a message log manager.
            var messageLogManager = scope.ServiceProvider.GetRequiredService<IMessageLogManager>(); 

            // Look for the associated logs (oldest first).
            var logs = (await messageLogManager.FindByMessageAsync(
                message
                ).ConfigureAwait(false))
                .OrderByDescending(x => x.CreatedOnUtc);

            // Create status for the notification.
            var status = new StatusNotification()
            {
                MessageKey = message.MessageKey,
                Sent = logs.Any(x => x.MessageEvent == MessageEvent.Sent)
            };

            // Should we look for failure information?
            if (status.Sent is false)
            {
                var log = logs.FirstOrDefault(x => x.MessageEvent == MessageEvent.Error);
                status.Error = log?.Error;
            }

            // Send the notification to the clients.
            await Clients.All.SendAsync(
                "Status",
                status,
                cancellationToken
                );
        }
    }

    #endregion
}
