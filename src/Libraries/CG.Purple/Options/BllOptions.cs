
namespace CG.Purple.Options;

/// <summary>
/// This class contains configuration settings for the business logic layer.
/// </summary>
internal class BllOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains configuration settings for the <see cref="IAttachmentManager"/>
    /// type.
    /// </summary>
    public AttachmentOptions? Attachments { get; set; }

    /// <summary>
    /// This property contains configuration settings for the <see cref="IFileTypeManager"/>
    /// type.
    /// </summary>
    public FileTypeOptions? FileTypes { get; set; }

    /// <summary>
    /// This property contains configuration settings for the <see cref="IMimeTypeManager"/>
    /// type.
    /// </summary>
    public MimeTypeOptions? MimeTypes { get; set; }

    /// <summary>
    /// This property contains configuration settings for the <see cref="IMailMessageManager"/>
    /// type.
    /// </summary>
    public MailMessageOptions? MailMessages { get; set; }

    /// <summary>
    /// This property contains configuration settings for the <see cref="ITextMessageManager"/>
    /// type.
    /// </summary>
    public TextMessageOptions? TextMessages { get; set; }

    #endregion
}
