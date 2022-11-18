
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
