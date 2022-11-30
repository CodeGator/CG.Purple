
namespace CG.Purple.Models;

/// <summary>
/// This class is a custom equality comparer for the <see cref="ParameterType"/>
/// type.
/// </summary>
public class ParameterTypeEqualityComparer : IEqualityComparer<ParameterType>
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the shared instance.
    /// </summary>
    private static ParameterTypeEqualityComparer? _instance;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ParameterTypeEqualityComparer"/>
    /// class.
    /// </summary>
    [DebuggerStepThrough]
    private ParameterTypeEqualityComparer() { }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns the singleton <see cref="ParameterTypeEqualityComparer"/>
    /// instance.
    /// </summary>
    /// <returns>An <see cref="ParameterTypeEqualityComparer"/> instance.</returns>
    [DebuggerStepThrough]
    public static ParameterTypeEqualityComparer Instance()
    {
        // Should we create the instance?
        if (_instance is null)
        {
            _instance = new ParameterTypeEqualityComparer();
        }

        // Return the instance.
        return _instance;
    }

    // *******************************************************************

    /// <inheritdoc/>
    public bool Equals(ParameterType? x, ParameterType? y)
    {
        // If anything is null it's not equal.
        if (x is null || y is null)
        {
            return false;
        }

        // Return the equality of the ids.
        return x.Id == y.Id;
    }

    // *******************************************************************

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] ParameterType obj)
    {
        // Return the id's hashcode.
        return obj.Id.GetHashCode();
    }

    #endregion
}
