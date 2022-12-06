
using CG.Purple.DoNothing.Providers;

namespace CG.Purple.DoNothing;

/// <summary>
/// This class contains the plugin module's startup logic.
/// </summary>
public class Module : ModuleBase
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public override void ConfigureServices(
        WebApplicationBuilder webApplicationBuilder,
        IConfiguration configuration,
        ILogger? bootstrapLogger
        )
    {
        // Log what we are about to do.
        bootstrapLogger?.LogDebug(
            "Registering the {name} provider with the DI container.",
            nameof(DoNothingProvider)
            );

        // Add the concrete provider to the DI container.
        webApplicationBuilder.Services.AddScoped<DoNothingProvider>();
    }

    // *******************************************************************

    /// <inheritdoc/>
    public override void Configure(
        WebApplication webApplication
        )
    {
        
    }

    #endregion
}
