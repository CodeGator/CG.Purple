
namespace CG.Purple.Providers;

/// <summary>
/// This interface represents an object that creates <see cref="IMessageProvider"/>
/// instances, at runtime.
/// </summary>
public interface IMessageProviderFactory
{
    /// <summary>
    /// This method creates an <see cref="IMessageProvider"/> instance 
    /// from the given <see cref="ProviderType"/> object.
    /// </summary>
    /// <param name="providerType">The provider type to use for the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a <see cref="IMessageProvider"/>
    /// instance, or NULL if the operation cannot be completed.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    Task<IMessageProvider?> CreateAsync(
        ProviderType providerType,
        CancellationToken cancellationToken = default
        );
}
