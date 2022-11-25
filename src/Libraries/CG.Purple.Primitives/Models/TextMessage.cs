
namespace CG.Purple.Models;

/// <summary>
/// This class represents a notification text model.
/// </summary>
public class TextMessage : Message
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the to address for the message.
    /// </summary>
    [Required]
    [MaxLength(1024)]
    public string To { get; set; } = null!;

    /// <summary>
    /// This property contains the body for the message.
    /// </summary>
    [Required]
    public string Body { get; set; } = null!;

    #endregion
}
