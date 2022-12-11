
using CG.Purple.Tools.TestClient.Services;

namespace CG.Purple.Tools.TestClient;

/// <summary>
/// This class is the main entry point for the application.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// This method is used to create the app instance.
    /// </summary>
    /// <returns>The app instance.</returns>
    public static MauiApp CreateMauiApp()
    {
        // Create the builder.
        var builder = MauiApp.CreateBuilder();

        // Configure the builder.
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Add the Blazor stuff.
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();
        
        // Add the purple client.
        builder.AddPurpleClients(options =>
        {
            options.DefaultBaseAddress = "https://localhost:7134";
        });

        // Add the state service.
        builder.Services.AddSingleton<StateService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		    builder.Logging.AddDebug();
#endif      

        // Build the app and return it.
        return builder.Build();
    }
}