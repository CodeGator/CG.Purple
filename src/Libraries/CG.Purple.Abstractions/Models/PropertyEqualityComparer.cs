
namespace CG.Purple.Models;

/// <summary>
/// This class is a custom equality comparer for the <see cref="PropertyType"/>
/// type.
/// </summary>
public class PropertyTypeEqualityComparer : IEqualityComparer<PropertyType>
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the shared instance.
    /// </summary>
    private static PropertyTypeEqualityComparer? _instance;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PropertyTypeEqualityComparer"/>
    /// class.
    /// </summary>
    [DebuggerStepThrough]
    private PropertyTypeEqualityComparer() { }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns the singleton <see cref="PropertyTypeEqualityComparer"/>
    /// instance.
    /// </summary>
    /// <returns>An <see cref="PropertyTypeEqualityComparer"/> instance.</returns>
    [DebuggerStepThrough]
    public static PropertyTypeEqualityComparer Instance()
    {
        // Should we create the instance?
        if (_instance is null)
        {
            _instance = new PropertyTypeEqualityComparer();
        }

        // Return the instance.
        return _instance;
    }

    // *******************************************************************

    /// <inheritdoc/>
    public bool Equals(PropertyType? x, PropertyType? y)
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
    public int GetHashCode([DisallowNull] PropertyType obj)
    {
        // Return the id's hashcode.
        return obj.Id.GetHashCode();
    }

    #endregion
}
