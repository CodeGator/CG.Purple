
namespace CG.Purple.Models;

/// <summary>
/// This class is a custom equality comparer for the <see cref="FileType"/>
/// type.
/// </summary>
public class FileTypeEqualityComparer : IEqualityComparer<FileType>
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the shared instance.
    /// </summary>
    private static FileTypeEqualityComparer? _instance;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="FileTypeEqualityComparer"/>
    /// class.
    /// </summary>
    [DebuggerStepThrough]
    private FileTypeEqualityComparer() { }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns the singleton <see cref="FileTypeEqualityComparer"/>
    /// instance.
    /// </summary>
    /// <returns>An <see cref="FileTypeEqualityComparer"/> instance.</returns>
    [DebuggerStepThrough]
    public static FileTypeEqualityComparer Instance()
    {
        // Should we create the instance?
        if (_instance is null)
        {
            _instance = new FileTypeEqualityComparer();
        }

        // Return the instance.
        return _instance;
    }

    // *******************************************************************

    /// <inheritdoc/>
    public bool Equals(FileType? x, FileType? y)
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
    public int GetHashCode([DisallowNull] FileType obj)
    {
        // Return the id's hashcode.
        return obj.Id.GetHashCode();
    }

    #endregion
}
