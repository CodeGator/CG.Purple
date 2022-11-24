
try
{
    // Log what we are about to do.
    BootstrapLogger.Instance().LogInformation(
        "Starting up {name}",
        AppDomain.CurrentDomain.FriendlyName
        );

    // Create an application builder.
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog stuff.
    builder.Host.UseSerilog((ctx, lc) =>
    {
        lc.ReadFrom.Configuration(ctx.Configuration);
    });

    // Add Blazor stuff.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddHttpContextAccessor();

    // Add MudBlazor stuff
    builder.Services.AddMudServices();
    builder.Services.AddMudExtensions();

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
    builder.AddControllerLayer(
        bootstrapLogger: BootstrapLogger.Instance()
        );
    builder.AddServicesLayer(
        bootstrapLogger: BootstrapLogger.Instance()
        );
    builder.AddSmtpProvider(
        bootstrapLogger: BootstrapLogger.Instance()
        );
    builder.AddTwillioProvider(
        bootstrapLogger: BootstrapLogger.Instance()
        );
    builder.AddSendGridProvider(
        bootstrapLogger: BootstrapLogger.Instance()
        );
    builder.AddSeedingLayer(
        bootstrapLogger: BootstrapLogger.Instance()
        );
        
    // Add SignalR stuff.
    builder.Services.AddSignalR(options =>
    {
        options.EnableDetailedErrors = true;
    });

    // Build the application.
    var app = builder.Build();

    // Setup the proper environment.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Use Blazor stuff.
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    // Use SignalR stuff.
    app.MapHub<StatusHub>("/_status");

    // Use CodeGator stuff.
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

