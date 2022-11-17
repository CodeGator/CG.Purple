
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to mime type seeding.
/// </summary>
public class MimeTypeOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the type for the mime type.
    /// </summary>
    [Required]
    [MaxLength(127)]
    public string Type { get; set; } = null!;

    /// <summary>
    /// This property contains the sub-type for the mime type.
    /// </summary>
    [Required]
    [MaxLength(127)]
    public string SubType { get; set; } = null!;

    /// <summary>
    /// This property contains the associated file extensions.
    /// </summary>
    public List<string> Extensions { get; set; } = new();

    #endregion
}
