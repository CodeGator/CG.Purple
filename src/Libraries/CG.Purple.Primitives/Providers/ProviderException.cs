
namespace CG.Purple.Providers;

/// <summary>
/// This class represents a message provider related exception.
/// </summary>
[Serializable]
public class ProviderException : Exception
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the associated provider type.
    /// </summary>
    public ProviderType? RelatedProvider { get; set; }

    /// <summary>
    /// This property contains the associated message.
    /// </summary>
    public Message? RelatedMessage { get; set; }

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderException"/>
    /// class.
    /// </summary>
    public ProviderException()
    {

    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderException"/>
    /// class.
    /// </summary>
    /// <param name="message">The message to use for the exception.</param>
    /// <param name="innerException">An optional inner exception reference.</param>
    public ProviderException(
        string message,
        Exception innerException
        ) : base(message, innerException)
    {

    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderException"/>
    /// class.
    /// </summary>
    /// <param name="relatedProvider">The related provider to use for 
    /// the exception.</param>
    /// <param name="message">The message to use for the exception.</param>
    /// <param name="innerException">An optional inner exception reference.</param>
    public ProviderException(
        ProviderType relatedProvider,
        string message,
        Exception innerException
        ) : base(message, innerException)
    {
        // Save the reference(s).
        RelatedProvider = relatedProvider;
    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderException"/>
    /// class.
    /// </summary>
    /// <param name="relatedMessage">The related message to use for the 
    /// exception.</param>
    /// <param name="relatedProvider">The related provider to use for 
    /// the exception.</param>
    /// <param name="message">The message to use for the exception.</param>
    /// <param name="innerException">An optional inner exception reference.</param>
    public ProviderException(
        Message relatedMessage,
        ProviderType relatedProvider,
        string message,
        Exception innerException
        ) : base(message, innerException)
    {
        // Save the reference(s).
        RelatedMessage = relatedMessage;    
        RelatedProvider = relatedProvider;  
    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderException"/>
    /// class.
    /// </summary>
    /// <param name="relatedProvider">The related provider to use for 
    /// the exception.</param>
    /// <param name="message">The message to use for the exception.</param>
    public ProviderException(
        ProviderType relatedProvider,
        string message
        ) : base(message)
    {
        // Save the reference(s).
        RelatedProvider = relatedProvider;
    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderException"/>
    /// class.
    /// </summary>
    /// <param name="relatedMessage">The related message to use for the 
    /// exception.</param>
    /// <param name="relatedProvider">The related provider to use for 
    /// the exception.</param>
    /// <param name="message">The message to use for the exception.</param>
    public ProviderException(
        Message relatedMessage,
        ProviderType relatedProvider,
        string message
        ) : base(message)
    {
        // Save the reference(s).
        RelatedMessage = relatedMessage;
        RelatedProvider = relatedProvider;
    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderException"/>
    /// class.
    /// </summary>
    /// <param name="info">The serialization info to use for the exception.</param>
    /// <param name="context">The streaming context to use for the exception.</param>
    public ProviderException(
        SerializationInfo info,
        StreamingContext context
        ) : base(info, context)
    {
        // Save the reference(s).
        RelatedMessage = info.GetValue(
            "RelatedMessage", 
            typeof(Message)
            ) as Message;
        RelatedProvider = info.GetValue(
            "RelatedProvider",
            typeof(ProviderType)
            ) as ProviderType;
    }

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method sets the <see cref="SerializationInfo"/> with information
    /// about the exception.
    /// </summary>
    /// <param name="info">The serialization information to use for the
    /// operation.</param>
    /// <param name="context">The serialization context to use for the
    /// operation.</param>
    public override void GetObjectData(
        SerializationInfo info, 
        StreamingContext context
        )
    {
        // Should we save the message?
        if (RelatedMessage is not null)
        {
            // Save the value.
            info.AddValue("RelatedMessage", RelatedMessage);
        }

        // Should we save the message?
        if (RelatedProvider is not null)
        {
            // Save the value.
            info.AddValue("RelatedProvider", RelatedProvider);
        }

        // Give the base class a chance.
        base.GetObjectData(info, context);
    }

    #endregion
}
