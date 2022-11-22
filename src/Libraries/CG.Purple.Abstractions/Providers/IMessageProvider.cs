
namespace CG.Purple.Providers;

/// <summary>
/// This interface represents an object that integrates with third-party 
/// message providers, such as SMTP, or SendGrid.
/// </summary>
public interface IMessageProvider
{
    /// <summary>
    /// This method sends messages to an external provider.
    /// </summary>
    /// <param name="messages">The messages to use for the operation.</param>
    /// <param name="parameters">The parameters to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ProviderException">This exception is thrown whenever 
    /// the provider fails to complete the operation.</exception>
    Task ProcessMessagesAsync(
        IEnumerable<Message> messages,
        IEnumerable<ProviderParameter> parameters,
        CancellationToken cancellationToken = default
        );
}
    
