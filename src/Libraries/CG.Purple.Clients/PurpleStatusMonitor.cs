
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
    /// This property contains a reference to a hub for the monitor.
    /// </summary>
    internal protected readonly IHubConnection _hubConnection = null!;

    #endregion

    // *******************************************************************
    // Events.
    // *******************************************************************

    #region Events

    /// <summary>
    /// This event is fired when the microservice sends a status update.
    /// </summary>
    public event EventHandler<StatusNotification>? Status;

    /// <summary>
    /// This event is fired when an error is detected while processing a 
    /// status update.
    /// </summary>
    public event EventHandler<Exception>? Error;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property indicates whether or not the monitor is actively 
    /// connected to the microservice.
    /// </summary>
    public virtual bool IsConnected 
    {
        get { return _hubConnection.State == HubConnectionState.Connected; }
    }

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PurpleStatusMonitor"/>
    /// class.
    /// </summary>
    /// <param name="hubConnection">The hub to use with this monitor.</param>
    public PurpleStatusMonitor(
        IHubConnection hubConnection
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(hubConnection, nameof(hubConnection));

        // Save the reference(s).
        _hubConnection = hubConnection;

        // Wire up a back-channel handler.
        var foo = _hubConnection.On(
            "Status", 
            new[] { typeof(StatusNotification) },
            (arg1, arg2) => 
            {
                try
                {
                    // Do we have 1 argument?
                    if (arg1.Length == 1)
                    {
                        // Is the argument the right type?
                        if (arg1[0] is StatusNotification)
                        {
#pragma warning disable CS8604 // Possible null reference argument.
                            // Raise the event.
                            Status?.Invoke(this, arg1[0] as StatusNotification);
#pragma warning restore CS8604 // Possible null reference argument.
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    // Raise the event.
                    Error?.Invoke(this, ex);
                }

                // Return the task.
                return Task.CompletedTask; 
            }, 
            this
            );
    }

    #endregion
}
