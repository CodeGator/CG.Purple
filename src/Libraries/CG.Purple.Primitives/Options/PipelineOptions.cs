
namespace CG.Purple.Options;

/// <summary>
/// This class contains configuration settings for message processing.
/// </summary>
public class PipelineOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property indicates how long to pause the service before 
    /// processing is allowed to begin.
    /// </summary>
    public TimeSpan? StartupDelay { get; set; }

    /// <summary>
    /// This property indicates how long to pause the service between 
    /// processing cycles.
    /// </summary>
    public TimeSpan? ThrottleDelay { get; set; }

    #endregion
}
