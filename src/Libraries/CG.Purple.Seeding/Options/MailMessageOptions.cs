
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to mail message seeding.
/// </summary>
public class MailMessageOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

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
    /// This property contains the subject for the message.
    /// </summary>
    [Required]
    public string Body { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the <see cref="MailMessageOptions.Body"/>
    /// property contains HTML formatted text, or not.
    /// </summary>
    public bool IsHtml { get; set; }

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

    #endregion
}
