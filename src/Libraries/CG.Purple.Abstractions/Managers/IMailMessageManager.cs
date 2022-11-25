
namespace CG.Purple.Managers;

/// <summary>
/// This interface represents an object that manages operations related to
/// <see cref="MailMessage"/> objects.
/// </summary>
public interface IMailMessageManager
{
    /// <summary>
    /// This method indicates whether there are any <see cref="MailMessage"/> objects
    /// in the underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns <c>true</c> if there
    /// are any <see cref="MailMessage"/> objects; <c>false</c> otherwise.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<bool> AnyAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method counts the number of <see cref="MailMessage"/> objects in the 
    /// underlying storage.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a count of the 
    /// number of <see cref="MailMessage"/> objects in the underlying storage.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<long> CountAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method creates a new <see cref="MailMessage"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="mailMessage">The model to create in the underlying storage.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly created
    /// <see cref="MailMessage"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<MailMessage> CreateAsync(
        MailMessage mailMessage,
        string userName,
        CancellationToken cancellationToken = default
        );
    
    /// <summary>
    /// This method searches for a sequence of <see cref="MailMessage"/> objects.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of <see cref="MailMessage"/> 
    /// objects.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<IEnumerable<MailMessage>> FindAllAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a single matching <see cref="MailMessage"/> object using
    /// the given identifier.
    /// </summary>
    /// <param name="id">The message identifier to use for the search.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a matching <see cref="MailMessage"/> 
    /// object, if one was found, or NULL otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<MailMessage?> FindByIdAsync(
        long id,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a single matching <see cref="MailMessage"/> object using
    /// the given message key.
    /// </summary>
    /// <param name="messageKey">The message key to use for the search.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a matching <see cref="MailMessage"/> 
    /// object, if one was found, or NULL otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<MailMessage?> FindByKeyAsync(
        string messageKey,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method searches for a sequence of <see cref="MailMessage"/> objects
    /// that are not disabled, or sent, or processed.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns a sequence of matching
    /// <see cref="MailMessage"/> objects.</returns>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<IEnumerable<MailMessage>> FindPendingAsync(
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method updates an existing <see cref="MailMessage"/> object in the 
    /// underlying storage.
    /// </summary>
    /// <param name="mailMessage">The model to update in the underlying storage.</param>
    /// <param name="userName">The user name of the person performing the 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the newly updated
    /// <see cref="MailMessage"/> object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    Task<MailMessage> UpdateAsync(
        MailMessage mailMessage,
        string userName,
        CancellationToken cancellationToken = default
        );
}
