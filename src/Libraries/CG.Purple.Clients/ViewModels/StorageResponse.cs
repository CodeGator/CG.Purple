
namespace CG.Purple.Clients.ViewModels;

/// <summary>
/// This class represents a response to a mail request.
/// </summary>
public class StorageResponse
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the key for the message.
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string MessageKey { get; set; } = null!;

    /// <summary>
    /// This property contains the creation date/time for the message.
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }

    #endregion
}
