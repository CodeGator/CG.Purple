
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

    /// <summary>
    /// This property indicates the maximum number of errors a message
    /// can accrue before we give up and stop trying to process it.
    /// </summary>
    public int? MaxErrorCount { get; set; }

    #endregion
}
