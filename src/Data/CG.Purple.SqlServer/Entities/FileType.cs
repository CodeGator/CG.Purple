namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This class represents a file type entity.
/// </summary>
internal class FileType : EntityBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the unique identifier for the file type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the identifier for the associated MIME type.
    /// </summary>
    public int MimeTypeId { get; set; }

    /// <summary>
    /// This property contains the associated MIME type.
    /// </summary>
    public virtual MimeType MimeType { get; set; } = null!;

    /// <summary>
    /// This property contains the extension for the file type.
    /// </summary>
    public string Extension { get; set; } = null!;

    #endregion
}
