
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
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the description of the property type.
    /// </summary>
    public string? Description { get; set; }

    #endregion
}
