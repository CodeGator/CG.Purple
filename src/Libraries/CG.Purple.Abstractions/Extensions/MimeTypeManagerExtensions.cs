
namespace CG.Purple.Managers;

/// <summary>
/// This class contains extension methods related to the <see cref="IMimeTypeManager"/>
/// type.
/// </summary>
public static class MimeTypeManagerExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method searches for a matching <see cref="MimeType"/> object
    /// using the given canonical MIME type (ie, in the format: text/plain).
    /// </summary>
    /// <param name="mimeTypeManager">The mime type manager to use for the
    /// operation.</param>
    /// <param name="canonicalType">The canonical MIME type to use for the
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation that returns the matching 
    /// <see cref="MimeType"/> object, or NULL if no match was found.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="ManagerException">This exception is thrown whenever the
    /// manager fails to complete the operation.</exception>
    public static async Task<MimeType?> FindByCanonicalTypeAsync(
        this IMimeTypeManager mimeTypeManager,
        string canonicalType,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(mimeTypeManager, nameof(mimeTypeManager))
            .ThrowIfNullOrEmpty(canonicalType, nameof(canonicalType));

        // Split the parts.
        var parts = canonicalType.Split('/');

        // Look for the mime type.
        var mimeType = await mimeTypeManager.FindByTypeAsync(
            parts[0],
            parts[1],
            cancellationToken
            ).ConfigureAwait(false);

        // Return the results.
        return mimeType.FirstOrDefault();
    }

    #endregion
}
