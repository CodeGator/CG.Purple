
namespace CG.Purple.Models;

/// <summary>
/// This class represents a provider parameter type model.
/// </summary>
public class ParameterType : ModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the unique identifier for the parameter type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the name of the parameter type.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the description of the parameter type.
    /// </summary>
    [MaxLength(128)]
    public string? Description { get; set; }

    #endregion
}
