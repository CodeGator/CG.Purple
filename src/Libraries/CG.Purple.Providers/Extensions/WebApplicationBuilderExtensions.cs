﻿
using CG.Purple.Providers;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
public static class WebApplicationBuilderExtensions006
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds the SignalR types for the provider layer (yeah I 
    /// know it's not technically a 'layer'. Work with me here ...) for the 
    /// <see cref="CG.Purple"/> project.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="bootstrapLogger">The bootstrap logger to use for the 
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplicationBuilder AddProviderLayer(
        this WebApplicationBuilder webApplicationBuilder,
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up SignalR support"
            );

        // Add SignalR stuff.
        webApplicationBuilder.Services.AddSignalR(options =>
        {
            // Is this a development machine?
            if (webApplicationBuilder.Environment.IsDevelopment())
            {
                // Tell the world what we are about to do.
                bootstrapLogger?.LogDebug(
                    "Enabling detailed error, for SignalR, since this is a development environment"
                    );

                // Enable detailed errors.
                options.EnableDetailedErrors = true;
            }
        });

        // Add our SignalR hub.
        webApplicationBuilder.Services.AddSingleton<StatusHub>();

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
