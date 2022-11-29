
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplication"/>
/// type.
/// </summary>
public static class WebApplicationExtensions006
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds pipeline logic related to the hosted services 
    /// layer (yeah I know it's not technically a 'layer'. Work with me 
    /// here ...) for the <see cref="CG.Purple"/> project.
    /// </summary>
    /// <param name="webApplication">The web application to use for the 
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplication"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplication UseServicesLayer(
        this WebApplication webApplication
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplication, nameof(webApplication));

        // Log what we are about to do.
        webApplication.Logger.LogDebug(
            "Wiring up SignalR status hub."
            );

        // Map the blazor hub.
        webApplication.MapBlazorHub();

        // Use SignalR stuff.
        webApplication.MapHub<StatusHub>("/_status");

        // Return the application.
        return webApplication;
    }

    #endregion
}
