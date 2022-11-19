
namespace CG.Purple.Host.Options;

/// <summary>
/// This class contains configuration settings for message processing.
/// </summary>
internal class MessageProcessingOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property indicates how long to throttle the service between
    /// processing iterations.
    /// </summary>
    public TimeSpan? ThrottleDuration { get; set; }

    /// <summary>
    /// This property indicates how long to pause the service before 
    /// processing is allowed to begin.
    /// </summary>
    public TimeSpan? StartupDelay { get; set; }

    #endregion
}
