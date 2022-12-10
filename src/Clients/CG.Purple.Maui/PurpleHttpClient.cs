
namespace CG.Purple.Maui;

/// <summary>
/// This class is a REST API client for the <see cref="CG.Purple"/> microservice.
/// </summary>
public class PurpleHttpClient : HttpClient
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the internal HTTP client for this client.
    /// </summary>
    internal protected readonly HttpClient _httpClient;
    
    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PurpleHttpClient"/>
    /// class.
    /// </summary>
    /// <param name="httpClient">The HTTP client to use with this client.</param>
    /// <param name="options">The configuration options to use with this 
    /// client.</param>
    public PurpleHttpClient(
        HttpClient httpClient,
        IOptions<PurpleClientOptions> options
        ) 
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(httpClient, nameof(httpClient))
            .ThrowIfNull(options, nameof(options));

        // Save the reference(s).
        _httpClient = httpClient;

        // Was a base address specified?
        if (!string.IsNullOrEmpty(options.Value.DefaultBaseAddress))
        {
            // Set the base address.
            _httpClient.BaseAddress = new Uri(
                options.Value.DefaultBaseAddress
                );
        }
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

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
    /// or more arguments are missing, or invalid.</exception>/// <returns></returns>
    public async Task<StorageResponse?> SendMailMessageAsync(
        MailStorageRequest request,
        CancellationToken cancellationToken = default
        )
    {
        // Send the POST.
        var result = await _httpClient.PostAsync(
            "/api/Mail",
            JsonContent.Create(request)
            );

        try
        {
            // Throw if we failed.
            result.EnsureSuccessStatusCode();

            // Read the response from the microservice.
            var response = await result.Content.ReadFromJsonAsync<StorageResponse>(
                cancellationToken: cancellationToken
                );

            // Return the response.
            return response;
        }
        catch (Exception ex)
        {
            // Look for an error message in the body.
            var error = await result.Content.ReadAsStringAsync(
                cancellationToken
                );

            // Did we find anything?
            if (!string.IsNullOrEmpty(error))
            {
                // Add better context.
                throw new HttpRequestException(
                    message: error,
                    inner: ex
                    );
            }            
            else
            {
                throw;
            }
        }
    }

    // *******************************************************************

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
    /// or more arguments are missing, or invalid.</exception>/// <returns></returns>
    public async Task<StorageResponse?> SendTextMessageAsync(
        TextStorageRequest request,
        CancellationToken cancellationToken = default
        )
    {
        // Send the POST.
        var result = await _httpClient.PostAsync(
            "/api/Text",
            JsonContent.Create(request)
            );

        try
        {
            // Throw if we failed.
            result.EnsureSuccessStatusCode();

            // Read the response from the microservice.
            var response = await result.Content.ReadFromJsonAsync<StorageResponse>(
                cancellationToken: cancellationToken
                );

            // Return the response.
            return response;
        }
        catch (Exception ex)
        {
            // Look for an error message in the body.
            var error = await result.Content.ReadAsStringAsync(
                cancellationToken
                );

            // Did we find anything?
            if (!string.IsNullOrEmpty(error))
            {
                // Add better context.
                throw new HttpRequestException(
                    message: error,
                    inner: ex
                    );
            }
            else
            {
                throw;
            }
        }
    }

    // *******************************************************************

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
    /// or more arguments are missing, or invalid.</exception>/// <returns></returns>
    public async Task<StatusResponse?> GetMailStatusByKeyAsync(
        string messageKey,
        CancellationToken cancellationToken = default
        )
    {
        // Send the GET.
        var result = await _httpClient.GetAsync(
            $"api/Mail/ByKey/{messageKey}",
            cancellationToken
            );

        // Throw if we failed.
        result.EnsureSuccessStatusCode();

        // Read the response from the microservice.
        var response = await result.Content.ReadFromJsonAsync<StatusResponse>(
            cancellationToken: cancellationToken
            );

        // Return the response.
        return response;
    }

    // *******************************************************************

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
    /// or more arguments are missing, or invalid.</exception>/// <returns></returns>
    public async Task<StatusResponse?> GetTextStatusByKeyAsync(
        string messageKey,
        CancellationToken cancellationToken = default
        )
    {
        // Send the GET.
        var result = await _httpClient.GetAsync(
            $"api/Text/ByKey/{messageKey}",
            cancellationToken
            );

        // Throw if we failed.
        result.EnsureSuccessStatusCode();

        // Read the response from the microservice.
        var response = await result.Content.ReadFromJsonAsync<StatusResponse>(
            cancellationToken: cancellationToken
            );

        // Return the response.
        return response;
    }

    #endregion
}
