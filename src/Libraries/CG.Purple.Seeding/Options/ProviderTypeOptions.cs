
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to provider type seeding.
/// </summary>
public class ProviderTypeOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This provider contains the name for the provider type.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// This provider contains the description for the provider type.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string? Description { get; set; }

    #endregion
}
