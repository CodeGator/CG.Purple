namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This class represents a message provider type entity.
/// </summary>
internal class ProviderType : EntityBase
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

    /// <summary>
    /// This property indicates whether this provider type can process
    /// emails.
    /// </summary>
    public bool CanProcessEmails { get; set; }

    /// <summary>
    /// This property indicates whether this provider type can process 
    /// texts.
    /// </summary>
    public bool CanProcessTexts { get; set; }

    /// <summary>
    /// This property contains the relative priority for this provider.
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// This property indicates the provider type has been disabled.
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// This property contains the .NET type for the associated provider
    /// type.
    /// </summary>
    public string FactoryType { get; set; } = null!;

    /// <summary>
    /// This property contains the associated parameters.
    /// </summary>
    public virtual ICollection<ProviderParameter> Parameters { get; set; }
        = new HashSet<ProviderParameter>();

    #endregion
}
