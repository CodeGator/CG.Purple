
namespace CG.Purple.Host.ViewModels;

/// <summary>
/// This class is an implementation of the <see cref="IBrowserFile"/>
/// interface that adapts an <see cref="Attachment"/> for display in 
/// a browser.
/// </summary>
class AttachmentBrowserFile : IBrowserFile
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the attachment for this view-model.
    /// </summary>
    protected readonly Attachment _attachment = null!;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the file name for the view-model's file.
    /// </summary>
    public string Name => _attachment.SafeOriginalFileName();

    /// <summary>
    /// This property contains the modified date for the view-model's
    /// file.
    /// </summary>
    public DateTimeOffset LastModified => new DateTimeOffset(
        _attachment.LastUpdatedOnUtc ?? DateTime.UtcNow
        );

    /// <summary>
    /// This property contains the size for the view-model's file.
    /// </summary>
    public long Size => _attachment.Length;

    /// <summary>
    /// This property contains the MIME type for the view-model's file.
    /// </summary>
    public string ContentType => _attachment.MimeType.FullType();

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="AttachmentBrowserFile"/>
    /// class.
    /// </summary>
    /// <param name="attachment">The attachment to use with this view-model.</param>
    public AttachmentBrowserFile(
        Attachment attachment
        )
    {
        // Validate the arguments before attempting to use them.
        Guard.Instance().ThrowIfNull(attachment, nameof(attachment));

        // Save the reference(s).
        _attachment = attachment;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns the data for the view-model's file.
    /// </summary>
    /// <param name="maxAllowedSize">The maximum size allowed for this 
    /// operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A <see cref="Stream"/> containing the bytes for the 
    /// view-model's associated file.</returns>
    public Stream OpenReadStream(
        long maxAllowedSize = 512000,
        CancellationToken cancellationToken = default
        )
    {
        // Sanity check the size.
        if (maxAllowedSize < _attachment.Length)
        {
            throw new InvalidOperationException(
                "The file is too big!"
                );
        }

        // Return the data wrapped as a stream.
        return new MemoryStream(
            _attachment.Data
            );
    }

    #endregion

}
