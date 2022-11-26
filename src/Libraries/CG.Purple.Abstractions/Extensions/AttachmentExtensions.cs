
namespace CG.Purple.Models;


/// <summary>
/// This class contains extension methods related to the <see cref="Attachment"/>
/// type.
/// </summary>
public static class AttachmentExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns a non-null file name for the given <see cref="Attachment"/>
    /// object.
    /// </summary>
    /// <param name="attachment">The attachment to use for the operation.</param>
    /// <returns>A rendering of the property that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeOriginalFileName(
        this Attachment attachment
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(attachment, nameof(attachment));

        // Return the full type.
        return attachment.OriginalFileName ?? "N/A";
    }

    #endregion
}

