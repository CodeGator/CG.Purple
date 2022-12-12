
namespace CG.Purple.Clients.ViewModels;

/// <summary>
/// This class represents a message property request.
/// </summary>
public class MessagePropertyRequest
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the name of the message property.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string PropertyName { get; set; } = null!;

    /// <summary>
    /// This property contains the value of the message property.
    /// </summary>
    [Required]
    public string Value { get; set; } = null!;

    #endregion
}
