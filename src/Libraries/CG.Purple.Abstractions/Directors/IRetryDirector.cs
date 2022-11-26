
namespace CG.Purple.Directors;

/// <summary>
/// This interface represents an object that performs message retry operations.
/// </summary>
public interface IRetryDirector
{
    /// <summary>
    /// This method attempts to retry processing for any messages in a 'Failed' 
    /// state, whose error count is below the given threshold.
    /// </summary>
    /// <param name="maxErrorCount">The maximum number of errors a message can
    /// have before we stop trying to process it.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task RetryMessagesAsync(
        int maxErrorCount,
        CancellationToken cancellationToken = default
        );
}
