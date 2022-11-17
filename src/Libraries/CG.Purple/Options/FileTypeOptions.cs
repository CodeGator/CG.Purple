
namespace CG.Purple.Options;

/// <summary>
/// This class contains configuration settings for the <see cref="IFileTypeManager"/> 
/// type.
/// </summary>
internal class FileTypeOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties
       
    /// <summary>
    /// This property contains the default cache duration for <see cref="FileType"/>
    /// objects.
    /// </summary>
    public TimeSpan? DefaultCacheDuration { get; set; }

    #endregion
}
