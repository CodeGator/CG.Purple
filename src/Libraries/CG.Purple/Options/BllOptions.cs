
namespace CG.Purple.Options;

/// <summary>
/// This class contains configuration settings for the business logic layer.
/// </summary>
internal class BllOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains configuration settings for the <see cref="IFileTypeManager"/>
    /// type.
    /// </summary>
    public FileTypeOptions? FileTypes { get; set; }

    #endregion
}
