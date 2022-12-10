
namespace CG.Purple.Providers.Models;

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
    /// This property contains the date/time when the message was created.
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }

    /// <summary>
    /// This property contains the date/time when the message was sent.
    /// </summary>
    public DateTime? SentOnUtc { get; set; }

    /// <summary>
    /// This property contains the date/time when the message failed to send.
    /// </summary>
    public DateTime? FailedOnUtc { get; set; }

    /// <summary>
    /// This property contains the error when the message failed to send.
    /// </summary>
    public string? Error { get; set; }

    #endregion
}
