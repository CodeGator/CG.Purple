
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to message property seeding.
/// </summary>
public class MessagePropertyOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the name of the associated property type.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string PropertyTypeName { get; set; } = null!;

    /// <summary>
    /// This property contains the value of the property.
    /// </summary>
    [Required]
    public string Value { get; set; } = null!;

    #endregion
}
