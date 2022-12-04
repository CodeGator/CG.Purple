
namespace CG.Purple.Options;

/// <summary>
/// This class contains configuration settings for hosted services.
/// </summary>
public class HostedServiceOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains pipeline processing options.
    /// </summary>
    public PipelineOptions? Pipeline { get; set; }

    #endregion
}
