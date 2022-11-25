
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
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// This property contains the description of the provider type.
    /// </summary>
    [MaxLength(64)]
    public string? Description { get; set; }

    /// <summary>
    /// This property indicates whether this provider can process emails.
    /// </summary>
    public bool CanProcessEmails { get; set; }

    /// <summary>
    /// This property indicates whether this provider can process texts.
    /// </summary>
    public bool CanProcessTexts { get; set; }

    /// <summary>
    /// This property contains the relative priority for this provider.
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// This property indicates the provider has been disabled.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// This property contains the .NET type for the associated provider.
    /// </summary>
    [Required]
    public string FactoryType { get; set; } = null!;

    /// <summary>
    /// This property contains the associated provider parameters.
    /// </summary>
    public virtual ICollection<ProviderParameter> Parameters { get; set; }
        = new HashSet<ProviderParameter>();

    #endregion
}
