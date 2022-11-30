

namespace CG.Purple.SendGrid;

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
        IConfiguration configuration
        )
    {
        // Add the provider.
        webApplicationBuilder.Services.AddScoped<SendGridProvider>();
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
