
namespace CG.Purple.Options;

/// <summary>
/// This class contains configuration settings for the <see cref="IAttachmentManager"/> 
/// type.
/// </summary>
internal class AttachmentOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the default cache duration for <see cref="Attachment"/>
    /// objects.
    /// </summary>
    public TimeSpan? DefaultCacheDuration { get; set; }

    #endregion
}
