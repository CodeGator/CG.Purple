
namespace CG.Purple.Directors;

/// <summary>
/// This interface represents an object that selects a provider for processing 
/// a message.
/// </summary>
public interface IAssignmentDirector
{
    /// <summary>
    /// This method selects a providers for the given message.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <param name="availableProviders">The list of available provider types
    /// to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task<ProviderType> SelectProviderAsync(
        Message message,
        IEnumerable<ProviderType> availableProviders,
        CancellationToken cancellationToken = default
        );
}
