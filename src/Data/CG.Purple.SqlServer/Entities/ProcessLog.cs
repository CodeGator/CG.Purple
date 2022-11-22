namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This class represents a process log entity.
/// </summary>
internal class ProcessLog : EntityBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the unique identifier for the entry.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// This property contains the unique identifier for the associated 
    /// message.
    /// </summary>
    public long? MessageId { get; set; }

    /// <summary>
    /// This property contains the associate message.
    /// </summary>
    public virtual Message? Message { get; set; }

    /// <summary>
    /// This property contains the associated provider type.
    /// </summary>
    public int? ProviderTypeId { get; set; }

    /// <summary>
    /// This property contains the associated provider type.
    /// </summary>
    public virtual ProviderType? ProviderType { get; set; }

    /// <summary>
    /// This property contains the associate processing event.
    /// </summary>
    public ProcessEvent Event { get; set; }

    /// <summary>
    /// This property contains the state of the message before the 
    /// event took place.
    /// </summary>
    public MessageState? BeforeState { get; set; }

    /// <summary>
    /// This property contains the state of the message after the
    /// event took place.
    /// </summary>
    public MessageState? AfterState { get; set; }

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
