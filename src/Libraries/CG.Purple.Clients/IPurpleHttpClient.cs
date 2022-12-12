
namespace CG.Purple.Clients;

/// <summary>
/// This interface represents a REST API client for the <see cref="CG.Purple"/>
/// microservice.
/// </summary>
public interface IPurpleHttpClient
{
    /// <summary>
    /// This property contains a reference to the inner HTTP client.
    /// </summary>
    HttpClient HttpClient { get; }

    /// <summary>
    /// This method sends a request to the <see cref="CG.Purple"/> microservice,
    /// to store a mail message.
    /// </summary>
    /// <param name="request">The request to use for the operation./</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the response
    /// from the microservice, if one was sent, or <c>NULL</c> otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    Task<StorageResponse?> SendMailMessageAsync(
        MailStorageRequest request,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method sends a request to the <see cref="CG.Purple"/> microservice,
    /// to store a text message.
    /// </summary>
    /// <param name="request">The request to use for the operation./</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the response
    /// from the microservice, if one was sent, or <c>NULL</c> otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>/
    Task<StorageResponse?> SendTextMessageAsync(
        TextStorageRequest request,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method sends a request to the <see cref="CG.Purple"/> microservice,
    /// for the status of the given mail message.
    /// </summary>
    /// <param name="messageKey">The message key to use for the operation./</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the response
    /// from the microservice, if one was sent, or <c>NULL</c> otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    Task<StatusResponse?> GetMailStatusByKeyAsync(
        string messageKey,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method sends a request to the <see cref="CG.Purple"/> microservice,
    /// for the status of the given text message.
    /// </summary>
    /// <param name="messageKey">The message key to use for the operation./</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the response
    /// from the microservice, if one was sent, or <c>NULL</c> otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    Task<StatusResponse?> GetTextStatusByKeyAsync(
        string messageKey,
        CancellationToken cancellationToken = default
        );
}
