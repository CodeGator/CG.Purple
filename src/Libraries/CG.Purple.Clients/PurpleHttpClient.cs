
namespace CG.Purple.Clients;

/// <summary>
/// This class is a default implementation of the <see cref="IPurpleHttpClient"/>
/// interface.
/// </summary>
internal class PurpleHttpClient : IPurpleHttpClient
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
    // Properties.
    // *******************************************************************

    #region Properties

    /// <inheritdoc/>
    public virtual HttpClient HttpClient => _httpClient;

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

    /// <inheritdoc />
    public virtual async Task<StorageResponse?> SendMailMessageAsync(
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

    /// <inheritdoc />
    public virtual async Task<StorageResponse?> SendTextMessageAsync(
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

    /// <inheritdoc />
    public virtual async Task<StatusResponse?> GetMailStatusByKeyAsync(
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

    /// <inheritdoc />
    public virtual async Task<StatusResponse?> GetTextStatusByKeyAsync(
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
