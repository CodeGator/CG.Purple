
namespace CG.Purple.Host.Services.Options;

/// <summary>
/// This class contains configuration settings for the Purple pipeline service.
/// </summary>
public class PipelineServiceOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property indicates how long to pause the service,at startup,
    /// before processing is allowed to begin.
    /// </summary>
    public TimeSpan? StartupDelay { get; set; }

    /// <summary>
    /// This property indicates how long to pause, between sections, of 
    /// each pipeline processing cycle.
    /// </summary>
    public TimeSpan? SectionDelay { get; set; }

    #endregion
}
