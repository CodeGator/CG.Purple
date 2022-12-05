
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
    /// This property contains the file attachments for the message.
    /// </summary>
    public List<string> Attachments { get; set; } = new();

    /// <summary>
    /// This property contains the properties for the message.
    /// </summary>
    public List<MessagePropertyOptions> Properties { get; set; } = new();

    /// <summary>
    /// This property indicates whether the message is disabled, or not.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// This property contains the relative priority of the message.
    /// </summary>
    public int Priority { get; set; }

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
    /// This property indicates when the message should be archived.
    /// </summary>
    public DateTime? ArchiveAfterUtc { get; set; }

    /// <summary>
    /// This property contains an optional provider name, for pre-assigning
    /// a message to a specific provider.
    /// </summary>
    [MaxLength(64)]
    public string? ProviderTypeName { get; set; }

    #endregion
}
