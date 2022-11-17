
namespace CG.Purple.Models;

/// <summary>
/// This class represents a message provider type model.
/// </summary>
public class ProviderType : ModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the unique identifier for the provider type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// This property contains the name of the provider type.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the description of the provider type.
    /// </summary>
    public string? Description { get; set; }

    #endregion
}
