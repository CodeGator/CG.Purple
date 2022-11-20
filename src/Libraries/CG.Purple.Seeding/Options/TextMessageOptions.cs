
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to text message seeding.
/// </summary>
public class TextMessageOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the key for the message.
    /// </summary>
    [Required]
    [MaxLength(36)]
    public string MessageKey { get; set; } = null!;

    /// <summary>
    /// This property contains the origin of the message.
    /// </summary>
    [Required]
    [MaxLength(1024)]
    public string From { get; set; } = null!;

    /// <summary>
    /// This property contains the 'To' phone number for the message.
    /// </summary>
    [Required]
    [MaxLength(1024)]
    public string To { get; set; } = null!;

    /// <summary>
    /// This property contains the subject for the message.
    /// </summary>
    [Required]
    public string Body { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the message is disabled, or not.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// This property contains the properties for the message.
    /// </summary>
    public List<MessagePropertyOptions> Properties { get; set; } = new();

    #endregion
}
