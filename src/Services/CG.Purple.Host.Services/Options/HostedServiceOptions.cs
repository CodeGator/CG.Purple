
namespace CG.Purple.Host.Services.Options;

/// <summary>
/// This class contains configuration settings for Purple hosted services.
/// </summary>
public class HostedServiceOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the settings for the Purple pipeline service.
    /// </summary>
    public PipelineServiceOptions? PipelineService { get; set; }

    #endregion
}
