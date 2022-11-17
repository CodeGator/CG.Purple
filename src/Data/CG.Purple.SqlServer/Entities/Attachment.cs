namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This class represents a message attachment entity.
/// </summary>
internal class Attachment : EntityBase
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
    /// This property contains the identifier for the associated message.
    /// </summary>
    public long MessageId { get; set; }

    /// <summary>
    /// This property contains the original file name for the attachment.
    /// </summary>
    public string? OriginalFileName { get; set; }

    /// <summary>
    /// This property contains the associated message.
    /// </summary>
    public virtual Message Message { get; set; } = null!;

    /// <summary>
    /// This property contains the identifier for the associated MIME type.
    /// </summary>
    public int MimeTypeId { get; set; }

    /// <summary>
    /// This property contains the associated MIME type.
    /// </summary>
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
