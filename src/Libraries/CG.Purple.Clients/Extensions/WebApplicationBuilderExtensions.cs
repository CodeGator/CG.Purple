
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
public static class WebApplicationBuilderExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds the web based REST API clients for the <see cref="CG.Purple"/>
    /// microservice.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="optionDelegate">The options delegate to use for the
    /// operation.</param>
    /// <param name="bootstrapLogger">A bootstrap logger to use for the
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplicationBuilder AddPurpleClients(
        this WebApplicationBuilder webApplicationBuilder,
        Action<PurpleClientOptions> optionDelegate,
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder))
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
        webApplicationBuilder.Services.AddOptions<PurpleClientOptions>()
            .Configure(x =>
            {
                x.DefaultBaseAddress = options.DefaultBaseAddress;
            }).Validate(x => true);

        // Call the overload.
        return webApplicationBuilder.AddPurpleClients(
            bootstrapLogger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method adds the web based REST API clients for the <see cref="CG.Purple"/>
    /// microservice.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="bootstrapLogger">A bootstrap logger to use for the
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplicationBuilder AddPurpleClients(
        this WebApplicationBuilder webApplicationBuilder,
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the Microsoft HTTP extensions"
            );

        // Add the Microsoft extensions.
        webApplicationBuilder.Services.AddHttpClient();

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the Purple HTTP client wrapper(s)"
            );

        // Add the purple clients.
        webApplicationBuilder.Services.AddSingleton<PurpleClientMonitor>();
        webApplicationBuilder.Services.AddSingleton<PurpleHttpClientFactory>();
        webApplicationBuilder.Services.AddScoped<PurpleHttpClient>();        

        // Return the app builder.
        return webApplicationBuilder;
    }

    #endregion
}
