
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

        // Add the status monitor.
        webApplicationBuilder.Services.AddSingleton<IPurpleStatusMonitor, PurpleStatusMonitor>(serviceProvider =>
        {
            // Get the client options.
            var options = serviceProvider.GetRequiredService<IOptions<PurpleClientOptions>>();

            // Get the base address.
            var url = $"{options.Value.DefaultBaseAddress ?? "https://localhost:7134"}";

            // Ensure it ends with a trailing '/'
            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            // Create a signalR hub builder.
            var builder = new HubConnectionBuilder()
                .WithUrl($"{url}_status")
                .WithAutomaticReconnect();

            // Create the signalR hub.
            var hubConnection = new HubConnectionWrapper(builder.Build());

            // Try a few times.
            for (var x = 0; x < 3; x++)
            {
                try
                {
                    // Start the hub.
                    hubConnection.StartAsync().Wait();

                    // Stop trying if we succeed.
                    break;
                }
                catch (AggregateException ex)
                {
                    // We might be running before the service is ready ...
                    if (ex.GetBaseException() is HttpRequestException)
                    {
                        // Wait a bit.
                        Task.Delay(500).Wait();
                    }
                }
            }

            // Create the status monitor.
            var monitor = new PurpleStatusMonitor(
                hubConnection
                );

            // Return the results.
            return monitor;

        });

        // Add the client factory.
        webApplicationBuilder.Services.AddSingleton<IPurpleHttpClientFactory, PurpleHttpClientFactory>();

        // Add the client.
        webApplicationBuilder.Services.AddScoped<IPurpleHttpClient, PurpleHttpClient>();        

        // Return the app builder.
        return webApplicationBuilder;
    }

    #endregion
}
