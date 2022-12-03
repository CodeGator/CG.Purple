
namespace CG.Purple.Host.ViewModels;

/// <summary>
/// This class represents a mail request.
/// </summary>
public class MailRequest
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
    public string Body { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the <see cref="MailRequest.Body"/>
    /// property contains formatted HTML, or not.
    /// </summary>
    public bool IsHtml { get; set; }

    #endregion
}
