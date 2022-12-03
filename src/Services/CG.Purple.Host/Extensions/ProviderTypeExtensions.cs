
namespace CG.Purple.Models;


/// <summary>
/// This class contains extension methods related to the <see cref="ProviderType"/>
/// type.
/// </summary>
internal static class ProviderTypeExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns a safe description for the given <see cref="ProviderType"/>
    /// object.
    /// </summary>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <returns>A rendering of the description that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeDescription(
        this ProviderType providerType
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(providerType, nameof(providerType));

        // Is there a description?
        if (!string.IsNullOrEmpty(providerType.Description))
        {
            // Return the description.
            return providerType.Description;
        }

        // Return no description.
        return "";        
    }

    #endregion
}

