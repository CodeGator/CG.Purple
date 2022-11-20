
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
public static partial class WebApplicationBuilderExtensions_Purple_Seeding
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds the data seeding layer for the <see cref="CG.Purple"/> 
    /// project.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="sectionName">The configuration section to use for the
    /// operation. Defaults to: <c>Seeding</c></param>
    /// <param name="bootstrapLogger">The bootstrap logger to use for the
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplicationBuilder AddSeedingLayer(
        this WebApplicationBuilder webApplicationBuilder,
        string sectionName = "Seeding",
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder))
            .ThrowIfNullOrEmpty(sectionName, nameof(sectionName));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Configuring seeding startup options from the {section} section",
            sectionName
            );

        // Configure the shared seeding options.
        webApplicationBuilder.Services.ConfigureOptions<SeedStartupOptions>(
            webApplicationBuilder.Configuration.GetSection(sectionName),
            out var seedStartupOptions
            );

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the seed directors"
            );

        // Add the directors.
        webApplicationBuilder.Services.AddScoped<ISeedDirector, SeedDirector>();

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
