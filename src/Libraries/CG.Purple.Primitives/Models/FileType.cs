
namespace CG.Purple.Models;

/// <summary>
/// This class represents a file type model.
/// </summary>
public class FileType : ModelBase
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
    /// This property contains the associated MIME type.
    /// </summary>
    [Required]
    public virtual MimeType MimeType { get; set; } = null!;

    /// <summary>
    /// This property contains the extension for the file type.
    /// </summary>
    [Required]
    [MaxLength(260)]
    public string Extension { get; set; } = null!;

    #endregion
}
