
namespace CG.Purple.Models;

/// <summary>
/// This class represents a property type model.
/// </summary>
public class PropertyType : ModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the unique identifier for the property 
    /// type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the name of the property type.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the description of the property type.
    /// </summary>
    [MaxLength(128)]
    public string? Description { get; set; }

    /// <summary>
    /// This property indicates the property type is for system use.
    /// </summary>
    public bool IsSystem { get; set; }

    #endregion
}
