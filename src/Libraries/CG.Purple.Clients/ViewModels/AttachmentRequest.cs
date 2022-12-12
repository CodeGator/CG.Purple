
namespace CG.Purple.Clients.ViewModels;

/// <summary>
/// This class represents a file attachment request.
/// </summary>
public class AttachmentRequest
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the file name for the attachment.
    /// </summary>
    [Required]
    [MaxLength(260)]
    public string FileName { get; set; } = null!;

    /// <summary>
    /// This property contains the MIME type for the attachment.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string MimeType { get; set; } = null!;

    /// <summary>
    /// This property contains the length for the attachment.
    /// </summary>
    public long Length { get; set; }

    /// <summary>
    /// This property contains the base64 encoded data for the attachment.
    /// </summary>
    public string Data { get; set; } = null!;

    #endregion
}
