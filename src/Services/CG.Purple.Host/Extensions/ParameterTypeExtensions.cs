
using Nextended.Core.Extensions;

namespace CG.Purple.Models;


/// <summary>
/// This class contains extension methods related to the <see cref="ParameterType"/>
/// type.
/// </summary>
internal static class ParameterTypeExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns a safe description for the given <see cref="ParameterType"/>
    /// object.
    /// </summary>
    /// <param name="parameterType">The parameter type to use for the operation.</param>
    /// <returns>A rendering of the description that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeDescription(
        this ParameterType parameterType
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(parameterType, nameof(parameterType));

        // Is there a description?
        if (!string.IsNullOrEmpty(parameterType.Description))
        {
            // Return the description.
            return parameterType.Description;
        }

        // Return no description.
        return "";        
    }

    #endregion
}

