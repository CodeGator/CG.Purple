
namespace CG.Purple.Clients;

/// <summary>
/// This interface represents a status monitor for the <see cref="CG.Purple"/>
/// microservice.
/// </summary>
public interface IPurpleStatusMonitor
{
    /// <summary>
    /// This event is fired when the microservice sends a status update.
    /// </summary>
    event EventHandler<StatusNotification>? Status;

    /// <summary>
    /// This event is fired when an error is detected while processing a 
    /// status update.
    /// </summary>
    event EventHandler<Exception>? Error;

    /// <summary>
    /// This property indicates whether or not the monitor is actively 
    /// connected to the microservice.
    /// </summary>
    bool IsConnected { get; }
}
