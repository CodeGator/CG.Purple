
namespace CG.Purple.Providers;

/// <summary>
/// This interface represents an object that integrates with third-party 
/// processing providers, such as Twillio, or SendGridr.
/// </summary>
public interface IMessageProvider
{
    /// <summary>
    /// This method sends a mail message to an external provider.
    /// </summary>
    /// <param name="mailMessage">The message to use for the operation.</param>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ProviderException">This exception is thrown whenever 
    /// the provider fails to complete the operation.</exception>
    Task SendMailAsync(
        MailMessage mailMessage,
        ProviderType providerType,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method sends a text message to an external provider.
    /// </summary>
    /// <param name="textMessage">The message to use for the operation.</param>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ProviderException">This exception is thrown whenever 
    /// the provider fails to complete the operation.</exception>
    Task SendTextAsync(
        TextMessage textMessage,
        ProviderType providerType,
        CancellationToken cancellationToken = default
        );
}
