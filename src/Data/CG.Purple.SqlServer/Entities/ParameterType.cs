namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This class represents a provider parameter type entity.
/// </summary>
internal class ParameterType : EntityBase
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
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the description of the parameter type.
    /// </summary>
    public string? Description { get; set; }

    #endregion
}
