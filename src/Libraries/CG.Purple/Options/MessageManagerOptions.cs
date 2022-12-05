
namespace CG.Purple.Options;

/// <summary>
/// This class contains configuration settings that are common to all 
/// message managers.
/// </summary>
public abstract class MessageManagerOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the default processing delay that may be 
    /// added to all new messages, as they are created.
    /// </summary>
    public TimeSpan? DefaultProcessDelay { get; set; }

    /// <summary>
    /// This property contains the default archiving delay that is added 
    /// to all new messages, as they are created.
    /// </summary>
    public TimeSpan? DefaultArchiveDelay { get; set; }

    #endregion
}
