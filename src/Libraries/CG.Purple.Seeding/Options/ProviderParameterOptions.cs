
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to provider parameter seeding.
/// </summary>
public class ProviderParameterOptions
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
    public string ProviderTypeName { get; set; } = null!;

    /// <summary>
    /// This provider contains the name for the parameter type.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string ParameterTypeName { get; set; } = null!;

    /// <summary>
    /// This provider contains the value for the parameter.
    /// </summary>
    public string Value { get; set; } = null!;

    #endregion
}
