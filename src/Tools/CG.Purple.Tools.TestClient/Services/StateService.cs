
namespace CG.Purple.Tools.TestClient.Services;

/// <summary>
/// This class is a simple state service.
/// </summary>
internal class StateService : Dictionary<string, object>
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the attachment requests.
    /// </summary>
    public List<AttachmentRequest> Attachments 
    {
        get => this["attachments"] as List<AttachmentRequest> 
                ?? Array.Empty<AttachmentRequest>().ToList(); 

        set => this["attachments"] = value
            ?? Array.Empty<AttachmentRequest>().ToList();  
    }

    /// <summary>
    /// This property contains the property requests.
    /// </summary>
    public List<MessagePropertyRequest> Properties
    {
        get => this["properties"] as List<MessagePropertyRequest>
                ?? Array.Empty<MessagePropertyRequest>().ToList();

        set => this["properties"] = value
            ?? Array.Empty<MessagePropertyRequest>().ToList();
    }

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="StateService"/>
    /// class
    /// </summary>
    public StateService()
    {
        // Create the initial state.
        this["attachments"] = Array.Empty<AttachmentRequest>().ToList();
        this["properties"] = Array.Empty<MessagePropertyRequest>().ToList();
    }

    #endregion
}
