
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

    /// <summary>
    /// This property indicates whether this provider can process emails.
    /// </summary>
    public bool CanProcessEmails { get; set; }

    /// <summary>
    /// This property indicates whether this provider can process texts.
    /// </summary>
    public bool CanProcessTexts { get; set; }

    /// <summary>
    /// This property contains the relative priority for this provider.
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// This property indicates the provider has been disabled.
    /// </summary>
    public bool IsDisabled { get; set; }

    #endregion
}
