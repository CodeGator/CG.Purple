
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
public static partial class WebApplicationBuilderExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds the hosted services layer (yeah I know it's not
    /// technically a 'layer'. Work with me here ...) for the <see cref="CG.Purple"/> 
    /// project.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="bootstrapLogger">The bootstrap logger to use for the 
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplicationBuilder AddServicesLayer(
        this WebApplicationBuilder webApplicationBuilder,
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the hosted services"
            );

        // Add the services.
        webApplicationBuilder.Services.AddHostedService<MessageProcessingService>();

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the directors"
            );

        // Add the directors.
        webApplicationBuilder.Services.AddScoped<IProcessDirector, ProcessDirector>();

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
