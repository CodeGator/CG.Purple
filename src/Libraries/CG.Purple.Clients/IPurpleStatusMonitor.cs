
namespace CG.Purple.Clients;

/// <summary>
/// This interface represents a status monitor for the <see cref="CG.Purple"/>
/// microservice.
/// </summary>
public interface IPurpleStatusMonitor
{
    /// <summary>
    /// This property contains a delegate for receiving continuous status 
    /// updates from the microservice.
    /// </summary>
    Action<StatusNotification>? Status { get; set; }

    /// <summary>
    /// This property indicates whether or not the monitor is actively 
    /// connected to the microservice.
    /// </summary>
    bool IsConnected { get; }
}
