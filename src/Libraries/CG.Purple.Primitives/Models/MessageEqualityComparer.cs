
namespace CG.Purple.Models;

/// <summary>
/// This class is a custom equality comparer for the <see cref="Message"/>
/// type.
/// </summary>
public class MessageEqualityComparer : IEqualityComparer<Message>
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the shared instance.
    /// </summary>
    private static MessageEqualityComparer? _instance;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessageEqualityComparer"/>
    /// class.
    /// </summary>
    [DebuggerStepThrough]
    private MessageEqualityComparer() { }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns the singleton <see cref="MessageEqualityComparer"/>
    /// instance.
    /// </summary>
    /// <returns>An <see cref="MessageEqualityComparer"/> instance.</returns>
    [DebuggerStepThrough]
    public static MessageEqualityComparer Instance()
    {
        // Should we create the instance?
        if (_instance is null)
        {
            _instance = new MessageEqualityComparer();
        }

        // Return the instance.
        return _instance;
    }

    // *******************************************************************

    /// <inheritdoc/>
    public bool Equals(Message? x, Message? y)
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
    public int GetHashCode([DisallowNull] Message obj)
    {
        // Return the id's hashcode.
        return obj.Id.GetHashCode();
    }

    #endregion
}
