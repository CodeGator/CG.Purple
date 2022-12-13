
namespace CG.Purple.Clients.ViewModels;

/// <summary>
/// This class contains extension methods related to the <see cref="StatusNotification"/>
/// type.
/// </summary>
internal static class StatusNotificationExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns a safe error for the given <see cref="StatusNotification"/>
    /// object.
    /// </summary>
    /// <param name="status">The status notification to use for the 
    /// operation.</param>
    /// <returns>A rendering of the property that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeError(
        this StatusNotification status
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(status, nameof(status));

        // Return the full type.
        return !string.IsNullOrEmpty(status.Error)
            ? status.Error ?? "N/A"
            : "N/A";
    }

    #endregion
}
