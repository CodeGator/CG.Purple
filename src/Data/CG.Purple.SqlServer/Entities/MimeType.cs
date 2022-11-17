namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This class represents a MIME type entity.
/// </summary>
internal class MimeType : EntityBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the unique identifier for the MIME type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the MIME type.
    /// </summary>
    public string Type { get; set; } = null!;

    /// <summary>
    /// This property contains the MIME sub-type.
    /// </summary>
    public string SubType { get; set; } = null!;

    /// <summary>
    /// This property contains the associated file types.
    /// </summary>
    public virtual ICollection<FileType> FileTypes { get; set; }
        = new HashSet<FileType>();

    #endregion
}
