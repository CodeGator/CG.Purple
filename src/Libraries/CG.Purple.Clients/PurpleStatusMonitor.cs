
namespace CG.Purple.Clients;

/// <summary>
/// This class is a default implementation of the <see cref="IPurpleStatusMonitor"/>
/// interface.
/// </summary>
internal class PurpleStatusMonitor : IPurpleStatusMonitor
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the client options.
    /// </summary>
    internal protected readonly IOptions<PurpleClientOptions> _options;

    /// <summary>
    /// This property contains an optional reference to a signalR hub.
    /// </summary>
    internal protected HubConnection? _statusHub { get; set; }

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains a delegate for receiving continuous status 
    /// updates from the microservice.
    /// </summary>
    public virtual Action<StatusNotification>? Status { get; set; }

    /// <summary>
    /// This property indicates whether or not the monitor is actively 
    /// connected to the microservice.
    /// </summary>
    public virtual bool IsConnected { get; private set; }

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PurpleStatusMonitor"/>
    /// class.
    /// </summary>
    /// <param name="options">The client options to use with this class.</param>
    public PurpleStatusMonitor(
        IOptions<PurpleClientOptions> options
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(options, nameof(options));

        // Save the reference(s).
        _options = options;

        // Stand up the back channel.
        EnsureBackchannel();
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method stand up a back channel for status updates from the 
    /// microservice.
    /// </summary>
    private void EnsureBackchannel()
    {
        // Do we need a hub?
        if (_statusHub is null)
        {
            // Get the base address.
            var url = $"{_options.Value.DefaultBaseAddress ?? 
                "https://localhost:7134"}";

            // Ensure it ends with a trailing '/'
            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            // Create a signalR hub builder.
            var builder = new HubConnectionBuilder()
                .WithUrl($"{url}_status")
                .WithAutomaticReconnect();

            // Create the signalR hub.
            _statusHub = builder.Build();

            // Wire up a back-channel handler.
            _statusHub.On(
                "Status",
                (StatusNotification statusUpdate) =>
                {
                    try
                    {
                        // Call the delegate.
                        Status?.Invoke(statusUpdate);
                    }
                    catch (Exception)
                    {
                        // TODO : decide what to do here.
                    }
                });

            // Try a few times.
            for (var x = 0; x < 3; x++)
            {
                try
                {
                    // Start the back channel.
                    _statusHub.StartAsync().Wait();

                    // Stop trying if we succeed.
                    break;
                }
                catch (AggregateException ex)
                {
                    // We might be running before the service is ready ...
                    if (ex.GetBaseException() is HttpRequestException)
                    {
                        // Wait a bit.
                        Task.Delay(500).Wait();
                    }                    
                }
            }

            // Tell the world what happened.
            IsConnected = _statusHub.State == HubConnectionState.Connected;
        }
    }

    #endregion
}
