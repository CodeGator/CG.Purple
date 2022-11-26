
namespace CG.Purple.Models;

/// <summary>
/// This class contains extension methods related to the <see cref="MailMessage"/>
/// type.
/// </summary>
internal static class MailMessageExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns a safe subject for the given <see cref="MailMessage"/>
    /// object.
    /// </summary>
    /// <param name="mailMessage">The mail message to use for the operation.</param>
    /// <returns>A rendering of the property that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeSubject(
        this MailMessage mailMessage
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(mailMessage, nameof(mailMessage));

        // Return the full type.
        return !string.IsNullOrEmpty(mailMessage.Subject)
            ? mailMessage.Subject ?? "N/A"
            : "N/A";
    }

    #endregion
}
