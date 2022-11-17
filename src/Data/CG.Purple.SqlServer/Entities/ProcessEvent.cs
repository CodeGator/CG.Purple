namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This enumeration represents the valid process events.
/// </summary>
internal enum ProcessEvent
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
    /// This enumeration represents an error event.
    /// </summary>
    [Description("An error occurred")]
    Error
}


