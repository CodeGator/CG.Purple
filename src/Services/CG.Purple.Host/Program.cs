
using MudBlazor.Extensions;

BootstrapLogger.LogLevelToDebug();

try
{
    // Log what we are about to do.
    BootstrapLogger.Instance().LogInformation(
        "Starting up {name}",
        AppDomain.CurrentDomain.FriendlyName
        );

    // Create an application builder.
    var builder = WebApplication.CreateBuilder(args);

    // Add the standard Blazor stuff.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();

    // Add MudBlazor stuff
    builder.Services.AddMudServices();

    // Add CodeGator stuff.
    builder.AddCryptographyWithSharedKeys(
        bootstrapLogger: BootstrapLogger.Instance()
        );
    builder.AddDataProtectionWithSharedKeys(
        bootstrapLogger: BootstrapLogger.Instance()
        );
    builder.AddBusinessLayer(
        bootstrapLogger: BootstrapLogger.Instance()
        );
    builder.AddDataAccessLayer(
        bootstrapLogger: BootstrapLogger.Instance()
        );
    builder.AddSeedingLayer(
        bootstrapLogger: BootstrapLogger.Instance()
        );

    // Add the controllers.
    builder.Services.AddControllers()
        .AddApplicationPart(Assembly.Load("CG.Purple.Controllers"));

    // Build the application.
    var app = builder.Build();

    // Setup the proper environment.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Use the standard Blazor stuff.
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    // Use the CodeGator stuff.
    app.UseDalStartup()
        .UseStartupSeeding();

    // Run the application.
    app.Run();
}
catch (Exception ex)
{
    // Log the error.
    BootstrapLogger.Instance().LogCritical(
        ex,
        "Unhandled exception: {msg}!",
        ex.GetBaseException().Message
        );
}
finally
{
    // Log what we are doing.
    BootstrapLogger.Instance().LogInformation(
        "Shutting down"
        );
}

