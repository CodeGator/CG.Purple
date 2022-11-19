
namespace CG.Purple.Directors;

/// <summary>
/// This interface represents an object that processes messages.
/// </summary>
public interface IProcessDirector
{
    /// <summary>
    /// This method processes pending messages.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task ProcessAsync(
        CancellationToken cancellationToken = default
        );
}
