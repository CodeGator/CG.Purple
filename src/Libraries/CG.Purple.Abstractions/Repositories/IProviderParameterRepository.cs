
namespace CG.Purple.Repositories;

/// <summary>
/// This interface represents an object that manages the storage and retrieval
/// of <see cref="ProviderParameter"/> objects.
/// </summary>
public interface IProviderParameterRepository
{
    /// <summary>
    /// This method indicates whether there are any <see cref="ProviderParameter"/> objects
    /// in the underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns <c>true</c> if there
    /// are any <see cref="ProviderParameter"/> objects; <c>false</c> otherwise.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method counts the number of <see cref="ProviderParameter"/> objects in the 
    /// underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a count of the 
    /// number of <see cref="ProviderParameter"/> objects in the underlying storage.</returns>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<long> CountAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method creates a new <see cref="ProviderParameter"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="providerParameter">The model to create in the underlying storage.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly created
    /// <see cref="ProviderParameter"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<ProviderParameter> CreateAsync(
        ProviderParameter providerParameter,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method deletes an existing <see cref="ProviderParameter"/> object from the 
    /// underlying storage.
    /// </summary>
    /// <param name="providerParameter">The model to delete from the underlying storage.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task DeleteAsync(
        ProviderParameter providerParameter,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method updates an existing <see cref="ProviderParameter"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="providerParameter">The model to update in the underlying storage.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly updated
    /// <see cref="ProviderParameter"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="RepositoryException">This exception is thrown whenever the
    /// repository fails to complete the operation.</exception>
    Task<ProviderParameter> UpdateAsync(
        ProviderParameter providerParameter,
        CancellationToken cancellationToken = default
        );
}
