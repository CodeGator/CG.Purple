namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This class represents a property type entity.
/// </summary>
internal class PropertyType : EntityBase
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

    /// <summary>
    /// This property indicates the property type is for system use.
    /// </summary>
    public bool IsSystem { get; set; }

    #endregion
}
