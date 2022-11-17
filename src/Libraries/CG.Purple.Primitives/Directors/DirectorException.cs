
namespace CG.Purple.Directors;

/// <summary>
/// This class represents a director related exception.
/// </summary>
[Serializable]
public class DirectorException : Exception
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="DirectorException"/>
    /// class.
    /// </summary>
    public DirectorException()
    {

    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="DirectorException"/>
    /// class.
    /// </summary>
    /// <param name="message">The message to use for the exception.</param>
    /// <param name="innerException">An optional inner exception reference.</param>
    public DirectorException(
        string message,
        Exception innerException
        ) : base(message, innerException)
    {

    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="DirectorException"/>
    /// class.
    /// </summary>
    /// <param name="message">The message to use for the exception.</param>
    public DirectorException(
        string message
        ) : base(message)
    {

    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="DirectorException"/>
    /// class.
    /// </summary>
    /// <param name="info">The serialization info to use for the exception.</param>
    /// <param name="context">The streaming context to use for the exception.</param>
    public DirectorException(
        SerializationInfo info,
        StreamingContext context
        ) : base(info, context)
    {

    }

    #endregion
}
