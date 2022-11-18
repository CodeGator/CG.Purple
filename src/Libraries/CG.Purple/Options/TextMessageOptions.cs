
namespace CG.Purple.Options;

/// <summary>
/// This class contains configuration settings for the <see cref="ITextMessageManager"/> 
/// type.
/// </summary>
internal class TextMessageOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties
       
    /// <summary>
    /// This property contains the default cache duration for <see cref="TextMessage"/>
    /// objects.
    /// </summary>
    public TimeSpan? DefaultCacheDuration { get; set; }

    #endregion
}
