
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
public static class WebApplicationBuilderExtensions003
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds the "SendGrid" provider for the <see cref="CG.Purple"/>
    /// project.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="bootstrapLogger">A bootstrap logger to use for the
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplicationBuilder AddSendGridProvider(
        this WebApplicationBuilder webApplicationBuilder,
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Registering the '{name}' provider",
            nameof(SendGridProvider)
            );

        // Add the SendGrid client.
        webApplicationBuilder.Services.AddScoped<ISendGridClient>(serviceProvider =>
        {
            // We need to use the provider parameters to collect the required
            //   setup for the SendGrid Client.

            // Get the provider type manager.
            var providerTypeManager = serviceProvider.GetRequiredService<
                IProviderTypeManager
                >();

            // Get the provider type.
            var providerType = providerTypeManager.FindByNameAsync(
                "SendGrid"
                ).Result;

            // Did we fail?
            if (providerType is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    "The SendGrid provider type was not found!"
                    );
            }

            // Get the api key.
            var apiKeyParameter = providerType.Parameters.FirstOrDefault(x =>
                x.ParameterType.Name == "ApiKey"
                );

            // Did we fail?
            if (apiKeyParameter is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    "The ApiKey parameter was not found!"
                    );
            }

            // Create the SendGrid client.
            var client = new SendGridClient(
                new SendGridClientOptions()
                {
                    ApiKey = apiKeyParameter.Value,
                    ReliabilitySettings = new ReliabilitySettings(
                        2,
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(10),
                        TimeSpan.FromSeconds(3)
                        )
                });

            // Return the results.
            return client;
        });

        // Add the provider.
        webApplicationBuilder.Services.AddScoped<SendGridProvider>();

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
