
namespace CG.Purple.Models;

/// <summary>
/// This enumeration represents the valid message types.
/// </summary>
public enum MessageType
{
    /// <summary>
    /// This enumeration represents a mail message.
    /// </summary>
    [Description("Message is an email")]
    Mail,

    /// <summary>
    /// This enumeration represents a text message.
    /// </summary>
    [Description("Message is a text")]
    Text
}
