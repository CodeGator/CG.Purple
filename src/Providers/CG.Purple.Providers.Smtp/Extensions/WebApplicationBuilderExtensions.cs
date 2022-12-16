
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
    /// This method adds the "SMTP" provider for the <see cref="CG.Purple"/>
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
    public static WebApplicationBuilder AddSmtpProvider(
        this WebApplicationBuilder webApplicationBuilder,
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Registering the '{name}' provider",
            nameof(SmtpProvider)
            );

        // Add the SMTP client.
        webApplicationBuilder.Services.AddScoped<ISmtpClient>(serviceProvider =>
        {
            // We need to use the provider parameters to collect the required
            //   setup for the SMTP Client.

            // Get the provider type manager.
            var providerTypeManager = serviceProvider.GetRequiredService<
                IProviderTypeManager
                >();

            // Get the provider type.
            var providerType = providerTypeManager.FindByNameAsync(
                "Smtp"
                ).Result;

            // Did we fail?
            if (providerType is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    "The SMTP provider type was not found!"
                    );
            }

            // Get the server url.
            var serverUrlProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "ServerUrl"
                );

            // Did we fail?
            if (serverUrlProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The 'ServerUrl' parameter is missing, or invalid!"
                    );
            }

            // Get the user name.
            var userNameProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "UserName"
                );

            // Did we fail?
            if (userNameProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The 'UserName' parameter is missing, or invalid!"
                    );
            }

            // Get the password.
            var passwordProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "Password"
                );

            // Did we fail?
            if (passwordProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The 'Password' parameter is missing, or invalid!"
                    );
            }

            // Create the SMTP client.
            var smtpClient = new System.Net.Mail.SmtpClient(
                serverUrlProperty.Value
                );

            // Set the credentials for the client.
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(
               userNameProperty.Value,
               passwordProperty.Value
               );

            // Return the results.
            return new SmtpClientWrapper(smtpClient);
        });

        // Add the provider.
        webApplicationBuilder.Services.AddScoped<SmtpProvider>();

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
