
namespace CG.Purple.SqlServer.Factories;

/// <summary>
/// This class is a default implementation of the <see cref="IDesignTimeDbContextFactory{TContext}"/>
/// interface.
/// </summary>
internal class PurpleDbContextDesignTimeFactory 
    : IDesignTimeDbContextFactory<PurpleDbContext>
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method creates a new <see cref="PurpleDbContext"/> instance.
    /// </summary>
    /// <param name="args">Optional arguments.</param>
    /// <returns>A <see cref="PurpleDbContext"/> instance.</returns>
    public PurpleDbContext CreateDbContext(string[] args)
    {
        // Create the configuration.
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("appSettings.json");
        var configuration = configBuilder.Build();

        var connectionString = configuration["DAL:ConnectionString"];
        if (string.IsNullOrEmpty(connectionString))
        {
            // Panic!!
            throw new ArgumentException(
                message: "The connection string at DAL:ConnectionString, " +
                "in the appSettings, json file is required for migrations, " +
                "but is currently missing, or empty!"
                );
        }

        // Create the options builder.
        var optionsBuilder = new DbContextOptionsBuilder<PurpleDbContext>();

        // Set the connection string.
        optionsBuilder.UseSqlServer(connectionString);

        // Create the and return the data-context.
        var dataContext = new PurpleDbContext(optionsBuilder.Options);

        // Return the results.
        return dataContext;
    }

    #endregion
}
