
namespace CG.Purple.Models;


/// <summary>
/// This class contains extension methods related to the <see cref="PropertyType"/>
/// type.
/// </summary>
internal static class PropertyTypeExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns a safe description for the given <see cref="PropertyType"/>
    /// object.
    /// </summary>
    /// <param name="propertyType">The property type to use for the operation.</param>
    /// <returns>A rendering of the description that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeDescription(
        this PropertyType propertyType
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(propertyType, nameof(propertyType));

        // Is there a description?
        if (!string.IsNullOrEmpty(propertyType.Description))
        {
            // Return the description.
            return propertyType.Description;
        }

        // Return no description.
        return "";        
    }

    #endregion
}

