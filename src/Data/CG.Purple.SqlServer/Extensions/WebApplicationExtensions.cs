
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplication"/>
/// type.
/// </summary>
public static partial class WebApplicationExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method runs the DAL startup logic for the <see cref="CG.Purple"/> 
    /// project.
    /// </summary>
    /// <param name="webApplication">The web application to use for the 
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplication"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplication UseDalStartup(
        this WebApplication webApplication
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplication, nameof(webApplication));

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Checking the application's environment for DAL startup."
            );

        // We only touch the database in a development environment.
        if (webApplication.Environment.IsDevelopment())
        {
            // Log what we are about to do.
            webApplication.Logger.LogDebug(
                "Fetching the DAL startup options from the DI container."
                );

            // Get the DAL startup options.
            var dalStartOptions = webApplication.Services.GetRequiredService<
                IOptions<DalOptions>
                >();

            // Should we drop the underlying database?
            if (dalStartOptions.Value.DropDatabaseOnStartup)
            {
                // Drop (and re-create) the databases.
                webApplication.DropAndRecreateDatabaseAsync().Wait();
            }
            else
            {
                // Log what we didn't do.
                webApplication.Logger.LogWarning(
                    "Skipping drop and recreate of databases because " +
                    "the '{flag}' flag is either false, or missing.",
                    nameof(dalStartOptions.Value.DropDatabaseOnStartup)
                    );

                // Should we apply any pending migrations?
                if (dalStartOptions.Value.MigrateDatabaseOnStartup)
                {
                    // Migrate the databases.
                    webApplication.MigrateDatabaseAsync().Wait();
                }
                else
                {
                    // Log what we didn't do.
                    webApplication.Logger.LogWarning(
                        "Skipping migration because the '{flag}' flag is either " +
                        "false, or missing.",
                        nameof(dalStartOptions.Value.MigrateDatabaseOnStartup)
                        );
                }                
            }            
        }
        else
        {
            // Log what we didn't do.
            webApplication.Logger.LogWarning(
                "Ignoring DAL startup because we aren't in a development " +
                "environment."
                );
        }

        // Return the application.
        return webApplication;
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method drops and recreates the database.
    /// </summary>
    /// <param name="webApplication">The web application to use for the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// throughout the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    private static async Task DropAndRecreateDatabaseAsync(
        this WebApplication webApplication,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        webApplication.Logger.LogInformation(
            "Dropping and recreating the database"
            );

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Creating a DI scope."
            );

        // Create a DI scope.
        using var scope = webApplication.Services.CreateScope();

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Creating a {ctx} instance.",
            nameof(PurpleDbContext)
            );

        // Create a data-context.
        var purpleDbContext = scope.ServiceProvider.GetRequiredService<
            PurpleDbContext
            >();

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Dropping the database '{db}', on server '{srv}'",
            purpleDbContext.Database.GetDatabaseName(),
            purpleDbContext.Database.GetServerName()
            );

        // Drop any existing database.
        await purpleDbContext.Database.EnsureDeletedAsync(
            cancellationToken
            ).ConfigureAwait(false);

        // Migrate the database (and create if it doesn't exist).
        await webApplication.MigrateDatabaseAsync(
            cancellationToken
            ).ConfigureAwait(false);
    }

    // *******************************************************************

    /// <summary>
    /// This method migrates the database.
    /// project.
    /// </summary>
    /// <param name="webApplication">The web application to use for the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the life of the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    private static async Task MigrateDatabaseAsync(
        this WebApplication webApplication,
        CancellationToken cancellationToken = default
        )
    {
        // Log what we are about to do.
        webApplication.Logger.LogInformation(
            "Migrating the database."
            );

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Creating a DI scope."
            );

        // Create a DI scope.
        using var scope = webApplication.Services.CreateScope();

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Creating a {ctx} instance.",
            nameof(PurpleDbContext)
            );

        // Create a data-context.
        var configurationDbContext = scope.ServiceProvider.GetRequiredService<
            PurpleDbContext
            >();

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Migrating the {ctx} on database '{db}', on server '{srv}'",
            nameof(PurpleDbContext),
            configurationDbContext.Database.GetDatabaseName(),
            configurationDbContext.Database.GetServerName()
            );

        // Migrate the data-context.
        await configurationDbContext.Database.MigrateAsync()
            .ConfigureAwait(false);
    }

    #endregion
}
