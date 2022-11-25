
using System.ComponentModel.DataAnnotations;

namespace CG.Purple.Models;

/// <summary>
/// This class represents a message attachment model.
/// </summary>
public class Attachment : ModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the unique identifier for the attachment.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// This property contains the original file name for the attachment.
    /// </summary>
    [MaxLength(260)]
    public string? OriginalFileName { get; set; }

    /// <summary>
    /// This property contains the associated message.
    /// </summary>
    [Required]
    public virtual Message Message { get; set; } = null!;

    /// <summary>
    /// This property contains the associated MIME type.
    /// </summary>
    [Required]
    public virtual MimeType MimeType { get; set; } = null!;

    /// <summary>
    /// This property contains the length, in bits, for the attachment.
    /// </summary>
    public long Length { get; set; }

    /// <summary>
    /// This property contains the bits for the attachment.
    /// </summary>
    public byte[] Data { get; set; } = null!;

    #endregion
}
