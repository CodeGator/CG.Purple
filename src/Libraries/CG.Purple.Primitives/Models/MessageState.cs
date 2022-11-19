
namespace CG.Purple.Models;

/// <summary>
/// This enumeration represents the valid message states.
/// </summary>
public enum MessageState
{
    /// <summary>
    /// This enumeration represents a message that has been
    /// stored, but not yet picked up for processing.
    /// </summary>
    [Description("Message is pending")]
    Pending,

    /// <summary>
    /// This enumeration represents a message that has
    /// been picked up for processing, but not yet sent.
    /// </summary>
    [Description("Message is processing")]
    Processing,

    /// <summary>
    /// This enumeration represents a message that has
    /// been picked up for processing, has failed processing
    /// at least once, and is now retrying.
    /// </summary>
    [Description("Message is retrying")]
    Retrying,

    /// <summary>
    /// This enumeration represents a message that has
    /// been sent.
    /// </summary>
    [Description("Message was sent")]
    Sent,

    /// <summary>
    /// This enumeration represents a message that has
    /// failed to process.
    /// </summary>
    [Description("Message failed to send")]
    Failed
}


