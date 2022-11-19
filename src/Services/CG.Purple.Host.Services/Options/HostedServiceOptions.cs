
namespace CG.Purple.Host.Options;

/// <summary>
/// This class contains configuration settings for hosted services.
/// </summary>
internal class HostedServiceOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains message processing options.
    /// </summary>
    public MessageProcessingOptions? MessageProcessing { get; set; }

    #endregion
}
