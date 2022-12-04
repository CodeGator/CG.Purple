
namespace CG.Purple.Models;

/// <summary>
/// This class represents a notification message model.
/// </summary>
public class Message : ModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the unique identifier for the message.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// This property contains a unique key for the message.
    /// </summary>
    public string MessageKey { get; set; } = null!;

    /// <summary>
    /// This property contains the origin of the message.
    /// </summary>
    public string From { get; set; } = null!;

    /// <summary>
    /// This property contains the message type.
    /// </summary>
    public MessageType MessageType { get; set; }

    /// <summary>
    /// This property contains the message state.
    /// </summary>
    public MessageState MessageState { get; set; }

    /// <summary>
    /// This property indicates the message has been disabled.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// This property contains the relative priority of the message.
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// This property indicates how many errors have occurred during
    /// the processing of this message.
    /// </summary>
    public int ErrorCount { get; set; }

    /// <summary>
    /// This property indicates the point at which we stop trying to 
    /// process this message.
    /// </summary>
    public int MaxErrors { get; set; }

    /// <summary>
    /// This property indicates when the message should be sent through the
    /// pipeline for processing.
    /// </summary>
    public DateTime? ProcessAfterUtc { get; set; }

    /// <summary>
    /// This property contains an associated provider type.
    /// </summary>
    public virtual ProviderType? ProviderType { get; set; }

    /// <summary>
    /// This property contains the associated message properties.
    /// </summary>
    public virtual ICollection<MessageProperty> MessageProperties { get; set; }
        = new HashSet<MessageProperty>();

    /// <summary>
    /// This property contains the associated file attachments.
    /// </summary>
    public virtual ICollection<Attachment> Attachments { get; set; }
        = new HashSet<Attachment>();

    #endregion
}
