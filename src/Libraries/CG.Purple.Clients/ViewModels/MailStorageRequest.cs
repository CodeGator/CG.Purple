
namespace CG.Purple.Clients.ViewModels;

/// <summary>
/// This class represents a mail storage request.
/// </summary>
public class MailStorageRequest
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains a optional key for the message. When specified,
    /// this property must be unique or the request will fail. A unique
    /// key is generated, for the message, if one is not specified here.
    /// </summary>
    [MaxLength(32)]
    public string? MessageKey { get; set; }

    /// <summary>
    /// This property contains the 'From' address for the message.
    /// </summary>
    [Required]
    [MaxLength(320)]
    [MinLength(1)]
    public string From { get; set; } = null!;

    /// <summary>
    /// This property contains the 'To' address for the message.
    /// </summary>
    [Required]
    [MaxLength(1024)]
    public string To { get; set; } = null!;

    /// <summary>
    /// This property contains the 'CC' address for the message.
    /// </summary>
    [MaxLength(1024)]
    public string? CC { get; set; }

    /// <summary>
    /// This property contains the 'BCC' address for the message.
    /// </summary>
    [MaxLength(1024)]
    public string? BCC { get; set; }

    /// <summary>
    /// This property contains the subject for the message.
    /// </summary>
    [MaxLength(998)]
    public string? Subject { get; set; }

    /// <summary>
    /// This property contains the body for the message.
    /// </summary>
    [Required]
    public string Body { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the <see cref="MailStorageRequest.Body"/>
    /// property contains formatted HTML, or not.
    /// </summary>
    public bool IsHtml { get; set; }

    /// <summary>
    /// This property contains the associated attachments, 
    /// for the message.
    /// </summary>
    [Required]
    public List<AttachmentRequest> Attachments { get; set; } = null!;

    /// <summary>
    /// This property contains the associated properties, for the message.
    /// </summary>
    [Required]
    public List<MessagePropertyRequest> Properties { get; set; } = null!;

    /// <summary>
    /// This property contains an optional disabled flag.
    /// </summary>
    public bool? IsDisabled { get; set; }

    /// <summary>
    /// This property contains an optional priority, for the message.
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// This property contains an optional provider type, for the message.
    /// </summary>
    [MaxLength(64)]
    public string? ProviderType { get; set; }

    /// <summary>
    /// This property indicates the point at which messages processing
    /// should stop.
    /// </summary>
    public int? MaxErrors { get; set; }

    /// <summary>
    /// This property contains an optional date for delaying the start of 
    /// process, for this message.
    /// </summary>
    public DateTime? ProcessAfterUtc { get; set; }

    /// <summary>
    /// This property contains an optional date for delaying the archiving, 
    /// for this message.
    /// </summary>
    public DateTime? ArchiveAfterUtc { get; set; }

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MailStorageRequest"/>
    /// class.
    /// </summary>
    public MailStorageRequest()
    {
        Attachments = new List<AttachmentRequest>();
        Properties = new List<MessagePropertyRequest>();
    }

    #endregion
}
