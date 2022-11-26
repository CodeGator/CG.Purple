
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to property type seeding.
/// </summary>
public class PropertyTypeOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the name for the property type.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the description for the property type.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string? Description { get; set; }

    /// <summary>
    /// This property indicates the property type is for system use.
    /// </summary>
    public bool IsSystem { get; set; }

    #endregion
}
