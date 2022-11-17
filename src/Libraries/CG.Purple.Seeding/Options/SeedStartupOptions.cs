
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to startup data seeding.
/// </summary>
internal class SeedStartupOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property indicates whether to perform a data seeding operation
    /// on startup.
    /// </summary>
    public bool SeedOnStartup { get; set; }

    /// <summary>
    /// This property indicates whether to force the seeding operation, which
    /// has the effect of possibly seeding tables with existing data.
    /// </summary>
    public bool Force { get; set; }

    /// <summary>
    /// This property contains a list of external JSON file names, each
    /// one containing settings for a seeding operation.
    /// </summary>
    [MaxLength(260)]
    public List<string>? FileNames { get; set; }

    #endregion
}
