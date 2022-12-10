
namespace CG.Purple.Maui.ViewModels;

/// <summary>
/// This class represents a status notification for a message.
/// </summary>
public class StatusNotification
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the key for the message.
    /// </summary>
    public string MessageKey { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the messages was created.
    /// </summary>
    public bool Created { get; set; }

    /// <summary>
    /// This property indicates whether the message was sent.
    /// </summary>
    public bool Sent { get; set; }

    /// <summary>
    /// This property indicates whether the message failed to send.
    /// </summary>
    public bool Failed { get; set; }

    /// <summary>
    /// This property contains the error when the message failed to send.
    /// </summary>
    public string? Error { get; set; }

    #endregion
}
