
namespace CG.Purple.Clients.ViewModels;

/// <summary>
/// This class represents a response to a status request.
/// </summary>
public class StatusResponse
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the key for the message.
    /// </summary>
    [Required]
    [MaxLength(32)]
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
