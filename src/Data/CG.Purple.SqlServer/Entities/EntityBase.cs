namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This class represents a base entity.
/// </summary>
internal class EntityBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the name of the person who created the entity.
    /// </summary>
    public string CreatedBy { get; set; } = "anonymous";

    /// <summary>
    /// This property contains the date/time when the entity was created.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// This property contains the name of the last person to update the entity.
    /// </summary>
    public string? LastUpdatedBy { get; set; } = null!;

    /// <summary>
    /// This property contains the date/time when the entity was last updated.
    /// </summary>
    public DateTime? LastUpdatedOnUtc { get; set; }

    #endregion
}
