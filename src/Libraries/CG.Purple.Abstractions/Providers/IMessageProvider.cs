
namespace CG.Purple.Providers;

/// <summary>
/// This interface represents an object that integrates with
/// a third-party messaging provider.
/// </summary>
public interface IMessageProvider
{
    /// <summary>
    /// This method performs a message processing operation.
    /// </summary>
    /// <typeparam name="TMessage">The type of associated message.</typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ProviderException">This exception is thrown whenever 
    /// the provider fails to complete the operation.</exception>
    Task<ProviderResponse<TMessage>> ProcessAsync<TMessage>(
        ProviderRequest<TMessage> request,
        CancellationToken cancellationToken = default
        ) where TMessage : Message;
}
