
namespace Microsoft.Maui.Hosting;

/// <summary>
/// This class contains extension methods related to the <see cref="MauiAppBuilder"/>
/// type.
/// </summary>
public static class MauiAppBuilderExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds the MAUI based REST API clients for the <see cref="CG.Purple"/>
    /// microservice.
    /// </summary>
    /// <param name="mauiAppBuilder">The MAUI application builder to
    /// use for the operation.</param>
    /// <param name="optionDelegate">The options delegate to use for the
    /// operation.</param>
    /// <param name="bootstrapLogger">A bootstrap logger to use for the
    /// operation.</param>
    /// <returns>The value of the <paramref name="mauiAppBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static MauiAppBuilder AddPurpleClients(
        this MauiAppBuilder mauiAppBuilder,
        Action<PurpleClientOptions> optionDelegate,
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mauiAppBuilder, nameof(mauiAppBuilder))
            .ThrowIfNull(optionDelegate, nameof(optionDelegate));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Creating default purple client options"
            );

        // Give the caller a change to change the options.
        var options = new PurpleClientOptions();
        optionDelegate?.Invoke(options);

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up Microsoft options"
            );

        // This bit of silliness is required because Microsoft refuses
        //   to fix their implementation of AddOptions, and I don't feel
        //   like standing up extra classes for no reason and manually
        //   registering everything here.
        // https://github.com/dotnet/runtime/issues/38491
        mauiAppBuilder.Services.AddOptions<PurpleClientOptions>()
            .Configure(x =>
            {
                x.DefaultBaseAddress = options.DefaultBaseAddress;
            }).Validate(x => true);

        // Call the overload.
        return mauiAppBuilder.AddPurpleClients(
            bootstrapLogger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method adds the MAUI based REST API clients for the <see cref="CG.Purple"/>
    /// microservice.
    /// </summary>
    /// <param name="mauiAppBuilder">The MAUI application builder to
    /// use for the operation.</param>
    /// <param name="bootstrapLogger">A bootstrap logger to use for the
    /// operation.</param>
    /// <returns>The value of the <paramref name="mauiAppBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static MauiAppBuilder AddPurpleClients(
        this MauiAppBuilder mauiAppBuilder,
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mauiAppBuilder, nameof(mauiAppBuilder));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the Microsoft HTTP extensions"
            );

        // Add the Microsoft extensions.
        mauiAppBuilder.Services.AddHttpClient();

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the Purple HTTP client wrapper(s)"
            );

        // Add the purple clients.
        mauiAppBuilder.Services.AddSingleton<PurpleClientMonitor>();
        mauiAppBuilder.Services.AddSingleton<PurpleHttpClientFactory>();
        mauiAppBuilder.Services.AddScoped<PurpleHttpClient>();        

        // Return the app builder.
        return mauiAppBuilder;
    }

    #endregion
}
