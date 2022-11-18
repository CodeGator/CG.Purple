
namespace CG.Purple.Models;

/// <summary>
/// This class represents a base model.
/// </summary>
public class ModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the name of the person who created the model.
    /// </summary>
    public string CreatedBy { get; set; } = "anonymous";

    /// <summary>
    /// This property contains the date/time when the model was created.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// This property contains the name of the last person to update the model.
    /// </summary>
    public string? LastUpdatedBy { get; set; } = null!;

    /// <summary>
    /// This property contains the date/time when the model was last updated.
    /// </summary>
    public DateTime? LastUpdatedOnUtc { get; set; }

    #endregion
}
