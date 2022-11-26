
namespace CG.Purple.Models;

/// <summary>
/// This class contains extension methods related to the <see cref="MimeType"/>
/// type.
/// </summary>
public static class MimeTypeExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns the full type for the given <see cref="MimeType"/>
    /// object, in the format: type/subtype.
    /// </summary>
    /// <param name="mimeType">The mime type to use for the operation.</param>
    /// <returns>A rendering of the property that is safe to use in a
    /// Blazor page.</returns>
    public static string FullType(
        this MimeType mimeType
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(mimeType, nameof(mimeType));

        // Return the full type.
        return $"{mimeType.Type}/{mimeType.SubType}";
    }

    #endregion
}

