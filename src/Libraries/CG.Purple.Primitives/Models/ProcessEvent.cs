
namespace CG.Purple.Models;

/// <summary>
/// This enumeration represents the valid process events.
/// </summary>
public enum ProcessEvent
{
    /// <summary>
    /// This enumeration represents a message storage event.
    /// </summary>
    [Description("The message was stored")]
    Stored,

    /// <summary>
    /// This enumeration represents an assign processor event.
    /// </summary>
    [Description("The message was assigned a processor")]
    Assigned,

    /// <summary>
    /// This enumeration represents a reset message event.
    /// </summary>
    [Description("The message was reset to the 'Pending' state")]
    Reset,

    /// <summary>
    /// This enumeration represents a message disable event.
    /// </summary>
    [Description("The message was disabled")]
    Disabled,

    /// <summary>
    /// This enumeration represents an message enable event.
    /// </summary>
    [Description("The message was enabled")]
    Enabled,

    /// <summary>
    /// This enumeration represents a successful message send event.
    /// </summary>
    [Description("The message was sent")]
    Sent,

    /// <summary>
    /// This enumeration represents a processed related error event.
    /// </summary>
    [Description("An error occurred while sending the message")]
    Error
}


