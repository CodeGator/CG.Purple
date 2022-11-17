
namespace CG.Purple.SqlServer.Options;

/// <summary>
/// This class contains configuration settings for the data access layer.
/// </summary>
public class DalOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the connection string for the DAL.
    /// </summary>
    [Required]
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// This property directs the DAL to drop the underlying database on 
    /// startup.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is ignored in production.
    /// </para>
    /// </remarks>
    public bool DropDatabaseOnStartup { get; set; }

    /// <summary>
    /// This property directs the DAL to migrate the underlying database 
    /// on startup.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property is ignored in production.
    /// </para>
    /// <para>
    /// If the <see cref="DalOptions.DropDatabaseOnStartup"/> property is set to 
    /// true then migrations are always applied, since the database is 
    /// then dropped and re-created. This property allows migrations to 
    /// be applied without dropping and re-creating the database.
    /// </para>
    /// </remarks>
    public bool MigrateDatabaseOnStartup { get; set; }

    #endregion
}
