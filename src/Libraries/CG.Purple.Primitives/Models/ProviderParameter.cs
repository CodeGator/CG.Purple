
namespace CG.Purple.Models;

/// <summary>
/// This class represents a provider parameter model.
/// </summary>
public class ProviderParameter : ModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the associate provider type.
    /// </summary>
    [Required]
    public virtual ProviderType ProviderType { get; set; } = null!;

    /// <summary>
    /// This property contains the associate parameter type.
    /// </summary>
    [Required]
    public virtual ParameterType ParameterType { get; set; } = null!;

    /// <summary>
    /// This property contains the value for the provider parameter.
    /// </summary>
    public string Value { get; set; } = null!;

    #endregion
}
