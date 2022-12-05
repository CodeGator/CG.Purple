
namespace CG.Purple.Managers;

/// <summary>
/// This interface represents an object that manages operations related to
/// <see cref="Message"/> objects.
/// </summary>
public interface IMessageManager
{
    /// <summary>
    /// This method indicates whether there are any <see cref="Message"/> objects
    /// in the underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns <c>true</c> if there
    /// are any <see cref="Message"/> objects; <c>false</c> otherwise.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method counts the number of <see cref="Message"/> objects in the 
    /// underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a count of the 
    /// number of <see cref="Message"/> objects in the underlying storage.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<long> CountAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method deletes an existing <see cref="Message"/> object from the 
    /// underlying storage.
    /// </summary>
    /// <param name="message">The model to delete from the underlying storage.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task DeleteAsync(
        Message message,
        string userName,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a sequence of <see cref="Message"/> objects.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of <see cref="Message"/> 
    /// objects.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<IEnumerable<Message>> FindAllAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a single matching <see cref="Message"/> object using
    /// the given identifier.
    /// </summary>
    /// <param name="id">The message identifier to use for the search.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a matching <see cref="Message"/> 
    /// object, if one was found, or NULL otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<Message?> FindByIdAsync(
        long id,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a single matching <see cref="Message"/> object using
    /// the given message key.
    /// </summary>
    /// <param name="messageKey">The message key to use for the search.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a matching <see cref="Message"/> 
    /// object, if one was found, or NULL otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<Message?> FindByKeyAsync(
        string messageKey,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a sequence of <see cref="Message"/> objects
    /// that are older than the given number of days.
    /// </summary>
    /// <param name="maxDaysToLive">The maximum number of days a message can
    /// live in a terminal state before we archive it.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of matching
    /// <see cref="Message"/> objects.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<IEnumerable<Message>> FindReadyToArchiveAsync(
        int maxDaysToLive,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a sequence of <see cref="Message"/> objects
    /// that are not disabled, or sent, or processed.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of matching
    /// <see cref="Message"/> objects.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<IEnumerable<Message>> FindReadyToProcessAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a sequence of <see cref="Message"/> objects
    /// that are in a failed state with an error count that is less than the
    /// given maximum.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of matching
    /// <see cref="Message"/> objects.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<IEnumerable<Message>> FindReadyToRetryAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method updates an existing <see cref="Message"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="message">The model to update in the underlying storage.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly updated
    /// <see cref="Message"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<Message> UpdateAsync(
        Message message,
        string userName,
        CancellationToken cancellationToken = default
        );
}
