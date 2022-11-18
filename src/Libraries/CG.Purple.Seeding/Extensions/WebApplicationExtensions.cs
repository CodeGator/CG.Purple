
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
    /// This method runs the startup seeding logic for the <see cref="CG.Purple"/> 
    /// project.
    /// </summary>
    /// <param name="webApplication">The web application to use for the 
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplication"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplication UseStartupSeeding(
        this WebApplication webApplication
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplication, nameof(webApplication));

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Checking the application's environment for seeding startup."
            );

        // We only touch the database in a development environment.
        if (webApplication.Environment.IsDevelopment())
        {
            // Log what we are about to do.
            webApplication.Logger.LogDebug(
                "Fetching the seed startup options from the DI container."
                );

            // Get the DAL startup options.
            var seedStartupOptions = webApplication.Services.GetRequiredService<
                IOptions<SeedStartupOptions>
                >();

            // Are there any files to process?
            if (seedStartupOptions.Value.FileNames is not null &&
                seedStartupOptions.Value.FileNames.Any())
            {
                // Seed the database.
                webApplication.PerformSeeding(
                    seedStartupOptions.Value.FileNames,
                    "seed",
                    seedStartupOptions.Value
                    );
            }
            else
            {
                // Log what we didn't do.
                webApplication.Logger.LogWarning(
                    "Ignoring seeding startup because the FileNames collection " +
                    "is empty, or missing."
                    );
            }
        }
        else
        {
            // Log what we didn't do.
            webApplication.Logger.LogWarning(
                "Ignoring seeding startup because we aren't in a development " +
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
    /// This method iterates through the given list of file names and 
    /// performs a seeding operation on each one.
    /// </summary>
    /// <param name="webApplication">The web application to use for the
    /// operation.</param>
    /// <param name="fileNames">The list of file names to use for the 
    /// operation.</param>
    /// <param name="userName">The name of the user performing the 
    /// operation.</param>
    /// <param name="seedStartupOptions">The seed startup options to use
    /// for the operation.</param>
    private static void PerformSeeding(
        this WebApplication webApplication,
        List<string> fileNames,
        string userName,
        SeedStartupOptions seedStartupOptions
        )
    {
        // Loop through the files.
        foreach (var fileName in fileNames)
        {
            // Read the configuration settings.
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(fileName, false);
            var configuration = builder.Build();

            // Perform the seeding for this type.
            webApplication.PerformSeeding(
                configuration,
                fileName,
                userName,
                seedStartupOptions
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method iterates through the given list of file names and 
    /// performs a seeding operation on each one.
    /// </summary>
    /// <param name="webApplication">The web application to use for the
    /// operation.</param>
    /// <param name="configuration">The configuration to use for the seeding 
    /// operation.</param>
    /// <param name="fileName">The file name to use for the operation.</param>
    /// <param name="userName">The name of the user performing the 
    /// operation.</param>
    /// <param name="seedStartupOptions">The seed startup options to use
    /// for the operation.</param>
    private static void PerformSeeding(
        this WebApplication webApplication,
        IConfiguration configuration,
        string fileName,
        string userName,
        SeedStartupOptions seedStartupOptions
        )
    {
        // We have a convention that says the root value of this JSON
        //   file has the name of the type we're seeding.
        var rootValue = configuration.GetChildren().FirstOrDefault();

        // Did we fail?
        if (string.IsNullOrEmpty(rootValue?.Key))
        {
            // Panic!!
            throw new ArgumentException(
                $"The root value is missing, or empty, in file: {fileName}"
                );
        }

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Creating a DI scope."
            );

        // Create a DI scope.
        using var scope = webApplication.Services.CreateScope();

        // Create a seed director.
        var seedDirector = scope.ServiceProvider.GetRequiredService<
            ISeedDirector
            >();

        // What type of seeding are we doing?
        switch (rootValue.Key)
        {
            case "MailMessages":
                seedDirector.SeedMailMessagesAsync(
                    configuration,
                    userName,
                    seedStartupOptions.Force
                    ).Wait();
                break;
            case "MimeTypes":
                seedDirector.SeedMimeTypesAsync(
                    configuration,
                    userName,
                    seedStartupOptions.Force
                    ).Wait();
                break;
            case "ParameterTypes":
                seedDirector.SeedParameterTypesAsync(
                    configuration,
                    userName,
                    seedStartupOptions.Force
                    ).Wait();
                break;
            case "PropertyTypes":
                seedDirector.SeedPropertyTypesAsync(
                    configuration,
                    userName,
                    seedStartupOptions.Force
                    ).Wait();
                break;
            case "ProviderParameters":
                seedDirector.SeedProviderParametersAsync(
                    configuration,
                    userName,
                    seedStartupOptions.Force
                    ).Wait();
                break;
            case "ProviderTypes":
                seedDirector.SeedProviderTypesAsync(
                    configuration,
                    userName,
                    seedStartupOptions.Force
                    ).Wait();
                break;
            case "TextMessages":
                seedDirector.SeedTextMessagesAsync(
                    configuration,
                    userName,
                    seedStartupOptions.Force
                    ).Wait();
                break;
            default:
                throw new ArgumentException(
                    $"Don't know how to seed '{rootValue.Key}' types!"
                    );
        }
    }

    #endregion
}
