
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to process log seeding.
/// </summary>
public class ProcessLogOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the key for the associated message.
    /// </summary>
    [MaxLength(32)]
    public string MessageKey { get; set; } = null!;

    /// <summary>
    /// This property contains the type of the associated message.
    /// </summary>
    [MaxLength(30)]
    public string MessageType { get; set; } = null!;

    /// <summary>
    /// This property contains the name of the associated provider type.
    /// </summary>
    [MaxLength(64)]
    public string? ProviderTypeName { get; set; }

    /// <summary>
    /// This property contains the associated processing event.
    /// </summary>
    [MaxLength(30)]
    public string Event { get; set; } = null!;

    /// <summary>
    /// This property contains the state of the message before the 
    /// event took place.
    /// </summary>
    [MaxLength(30)]
    public string? BeforeState { get; set; }

    /// <summary>
    /// This property contains the state of the message after the
    /// event took place.
    /// </summary>
    [MaxLength(30)]
    public string? AfterState { get; set; }

    /// <summary>
    /// This property contains extra data associated with the event.
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// This property contains error data associated with the event.
    /// </summary>
    public string? Error { get; set; }

    #endregion
}
