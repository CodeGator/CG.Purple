
namespace CG.Purple.Directors;

/// <summary>
/// This interface represents an object that performs message archiving operations.
/// </summary>
public interface IArchiveDirector
{
    /// <summary>
    /// This method attempts to archive any terminal messages whose creation
    /// date exceeds the given threshold.
    /// </summary>
    /// <param name="maxDaysToLive">The maximum number of days to keep terminal 
    /// messages before archiving them.</param>
    /// <param name="userName">The user name of the person performing the
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task ArchiveMessagesAsync(
        int maxDaysToLive,
        string userName,
        CancellationToken cancellationToken = default
        );
}
