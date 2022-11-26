namespace System;

/// <summary>
/// This class contains extension methods related to the <see cref="String"/>
/// type.
/// </summary>
internal static class StringExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method trims the length of the given string to, at most,
    /// <paramref name="maxLength"/> characters. A trailing ellipse is 
    /// added to the end of the string, if characters are trimmed.
    /// </summary>
    /// <param name="value">The string to use for the operation.</param>
    /// <param name="maxLength">The maximum number of character to allow
    /// in the return string.</param>
    /// <returns>A trimmed version of <paramref name="value"/>.</returns>
    public static string Truncate(
        this string value,
        int maxLength
        )
    {
        if (value.Length < maxLength) 
        {
            return value;
        }

        return $"{value.Substring( 0, maxLength )} ...";
    }

    #endregion
}
