
namespace CG.Purple.Models;

/// <summary>
/// This class is a custom equality comparer for the <see cref="MessageProperty"/>
/// type.
/// </summary>
public class MessagePropertyEqualityComparer : IEqualityComparer<MessageProperty>
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the shared instance.
    /// </summary>
    private static MessagePropertyEqualityComparer? _instance;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessagePropertyEqualityComparer"/>
    /// class.
    /// </summary>
    [DebuggerStepThrough]
    private MessagePropertyEqualityComparer() { }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns the singleton <see cref="MessagePropertyEqualityComparer"/>
    /// instance.
    /// </summary>
    /// <returns>An <see cref="MessagePropertyEqualityComparer"/> instance.</returns>
    [DebuggerStepThrough]
    public static MessagePropertyEqualityComparer Instance()
    {
        // Should we create the instance?
        if (_instance is null)
        {
            _instance = new MessagePropertyEqualityComparer();
        }

        // Return the instance.
        return _instance;
    }

    // *******************************************************************

    /// <inheritdoc/>
    public bool Equals(MessageProperty? x, MessageProperty? y)
    {
        // If anything is null it's not equal.
        if (x is null || y is null)
        {
            return false;
        }

        // If the message is missing it's not equal.
        if (x.Message is null || y.Message is null)
        {
            return false;
        }

        // If the property type is missing it's not equal.
        if (x.PropertyType is null || y.PropertyType is null)
        {
            return false;
        }

        // Return the equality of the ids.
        return x.Message.Id == y.Message.Id &&
            x.PropertyType.Id == y.PropertyType.Id;
    }

    // *******************************************************************

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] MessageProperty obj)
    {
        // If the message is missing return the object hashcode.
        if (obj.Message is null)
        {
            return obj.GetHashCode(); 
        }

        // If the property type is missing return the object hashcode.
        if (obj.PropertyType is null)
        {
            return obj.GetHashCode();
        }

        // Return the id's hashcode.
        return obj.Message.Id.GetHashCode() +
            obj.PropertyType.Id.GetHashCode();
    }

    #endregion
}
