
namespace CG.Purple.Repositories;

/// <summary>
/// This class represents a repository related exception.
/// </summary>
[Serializable]
public class RepositoryException : Exception
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="RepositoryException"/>
    /// class.
    /// </summary>
    public RepositoryException()
    {

    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="RepositoryException"/>
    /// class.
    /// </summary>
    /// <param name="message">The message to use for the exception.</param>
    /// <param name="innerException">An optional inner exception reference.</param>
    public RepositoryException(
        string message,
        Exception innerException
        ) : base(message, innerException)
    {

    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="RepositoryException"/>
    /// class.
    /// </summary>
    /// <param name="message">The message to use for the exception.</param>
    public RepositoryException(
        string message
        ) : base(message)
    {

    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="RepositoryException"/>
    /// class.
    /// </summary>
    /// <param name="info">The serialization info to use for the exception.</param>
    /// <param name="context">The streaming context to use for the exception.</param>
    public RepositoryException(
        SerializationInfo info,
        StreamingContext context
        ) : base(info, context)
    {

    }

    #endregion
}
