
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to parameter type seeding.
/// </summary>
public class ParameterTypeOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This parameter contains the name for the parameter type.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// This parameter contains the description for the parameter type.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string? Description { get; set; }

    #endregion
}
