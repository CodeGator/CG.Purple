
namespace CG.Purple.Models;


/// <summary>
/// This class contains extension methods related to the <see cref="MessageLog"/>
/// type.
/// </summary>
internal static class ProcessLogExtensions001
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns a safe BeforeState for the given <see cref="MessageLog"/>
    /// object.
    /// </summary>
    /// <param name="processLog">The process log to use for the operation.</param>
    /// <returns>A rendering of the property that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeBeforeState(
        this MessageLog processLog
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog));

        // Is there a state?
        if (processLog.BeforeState.HasValue)
        {
            // Return the state.
            return Enum.GetName(processLog.BeforeState.Value) ?? "N/A";
        }

        // Return no state.
        return "N/A";        
    }

    // *******************************************************************

    /// <summary>
    /// This method returns a safe AfterState for the given <see cref="MessageLog"/>
    /// object.
    /// </summary>
    /// <param name="processLog">The process log to use for the operation.</param>
    /// <returns>A rendering of the property that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeAfterState(
        this MessageLog processLog
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog));

        // Is there a state?
        if (processLog.AfterState.HasValue)
        {
            // Return the state.
            return Enum.GetName(processLog.AfterState.Value) ?? "N/A";
        }

        // Return no state.
        return "N/A";
    }

    // *******************************************************************

    /// <summary>
    /// This method returns a safe ProviderType for the given <see cref="MessageLog"/>
    /// object.
    /// </summary>
    /// <param name="processLog">The process log to use for the operation.</param>
    /// <returns>A rendering of the property that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeProviderType(
        this MessageLog processLog
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog));

        // Is there a provider type?
        if (processLog.ProviderType is not null)
        {
            // Return the prover type.
            return processLog.ProviderType.Name;
        }

        // Return no state.
        return "N/A";
    }

    // *******************************************************************

    /// <summary>
    /// This method returns a safe Error for the given <see cref="MessageLog"/>
    /// object.
    /// </summary>
    /// <param name="processLog">The process log to use for the operation.</param>
    /// <returns>A rendering of the property that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeError(
        this MessageLog processLog
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog));

        // Is there an error?
        if (!string.IsNullOrEmpty(processLog.Error))
        {
            // Return the error.
            return processLog.Error;
        }

        // Return no error.
        return "N/A";
    }

    // *******************************************************************

    /// <summary>
    /// This method returns a safe Message for the given <see cref="MessageLog"/>
    /// object.
    /// </summary>
    /// <param name="processLog">The process log to use for the operation.</param>
    /// <returns>A rendering of the message that is safe to use in a
    /// Blazor page.</returns>
    public static string SafeMessage(
        this MessageLog processLog
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(processLog, nameof(processLog));

        // Return the message.
        return $"{processLog.Message?.Id}" ?? "N/A";
    }

    #endregion
}

