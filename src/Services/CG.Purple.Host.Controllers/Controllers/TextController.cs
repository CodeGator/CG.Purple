
namespace CG.Purple.Host.Controllers;

/// <summary>
/// This class is a REST controller for operations related to text resources.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TextController : ControllerBase
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the text message manager for this controller.
    /// </summary>
    internal protected readonly ITextMessageManager _textMessageManager;

    /// <summary>
    /// This field contains the message log manager for this controller.
    /// </summary>
    internal protected readonly IMessageLogManager _messageLogManager;

    /// <summary>
    /// This field contains the mime type manager for this controller.
    /// </summary>
    internal protected readonly IMimeTypeManager _mimeTypeManager;

    /// <summary>
    /// This field contains the property type manager for this controller.
    /// </summary>
    internal protected readonly IPropertyTypeManager _propertyTypeManager;

    /// <summary>
    /// This field contains the logger for this controller.
    /// </summary>
    internal protected readonly ILogger<TextController> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="TextController"/>
    /// class.
    /// </summary>
    /// <param name="textMessageManager">The text message manager to use 
    /// with this controller.</param>
    /// <param name="messageLogManager">The message log manager to use 
    /// with this controller.</param>
    /// <param name="mimeTypeManager">The mime type manager to use with 
    /// this controller.</param>
    /// <param name="propertyTypeManager">The property type manager to
    /// use with this controller.</param>
    /// <param name="logger">The logger to use with this controller.</param>
    public TextController(
        ITextMessageManager textMessageManager,
        IMessageLogManager messageLogManager,
        IMimeTypeManager mimeTypeManager,
        IPropertyTypeManager propertyTypeManager,
        ILogger<TextController> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(textMessageManager, nameof(textMessageManager))
            .ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNull(mimeTypeManager, nameof(mimeTypeManager))
            .ThrowIfNull(propertyTypeManager, nameof(propertyTypeManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _textMessageManager = textMessageManager;
        _messageLogManager = messageLogManager;
        _mimeTypeManager = mimeTypeManager;
        _propertyTypeManager = propertyTypeManager;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method searches for a text message by key.
    /// </summary>
    /// <param name="messageKey">The message key to use for the operation.</param>
    /// <returns>A task to perform the operation that returns an <see cref="IActionResult"/>
    /// object.</returns>
    [HttpGet("{messageKey}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public virtual async Task<IActionResult> GetByKeyAsync(
        string messageKey
        )
    {
        try
        {
            // ======
            // Step 1: Check the incoming model.
            // ======

            // Sanity check the model state.
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // ======
            // Step 2: Look for the message.
            // ======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for message: {key}",
                messageKey
                );

            // Look for the given message.
            var mailMessage = await _textMessageManager.FindByKeyAsync(
                messageKey
                ).ConfigureAwait(false);

            // Did we fail?
            if (mailMessage is null)
            {
                return NotFound();
            }

            // ======
            // Step 3: Look for the logs.
            // ======

            // Log what we are about to do.
            _logger.LogDebug(
                "Looking for logs associated with message: {key}",
                messageKey
                );

            // Look for the associated logs (oldest first).
            var logs = (await _messageLogManager.FindByMessageAsync(
                mailMessage
                ).ConfigureAwait(false)).OrderByDescending(x => x.CreatedOnUtc);

            // ======
            // Step 4: Create a response.
            // ======

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a response for message: {key}",
                messageKey
                );

            // Create the status response.
            var status = new StatusResponse()
            {
                MessageKey = mailMessage.MessageKey,
                CreatedOnUtc = mailMessage.CreatedOnUtc,
                SentOnUtc = logs.FirstOrDefault(x => x.MessageEvent == MessageEvent.Sent)?.CreatedOnUtc
            };

            // Should we look for failure information?
            if (status.SentOnUtc is null)
            {
                var log = logs.FirstOrDefault(x => x.MessageEvent == MessageEvent.Error);
                status.FailedOnUtc = log?.CreatedOnUtc;
                status.Error = log?.Error;  
            }

            // Return the result.
            return Ok(status);
        }
        catch (Exception ex)
        {
            // Log the error in detail.
            _logger.LogError(
                ex,
                "Failed to search for a text message, by key!"
                );

            // Return an overview of the problem.
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "The controller failed to search for a text " +
                "message, by key!"
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method stores a new text.
    /// </summary>
    /// <param name="request">The request to use for the operation.</param>
    /// <returns>A task to perform the operation that returns an <see cref="IActionResult"/>
    /// object.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> PostAsync(
        [FromBody] TextStorageRequest request
        )
    {
        try
        {
            // ======
            // Step 1: Check the incoming model.
            // ======

            // Sanity check the model.
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // ======
            // Step 2: Convert types for storage.
            // ======

            // Loop and convert the attachments for the request.
            var attachments = new List<Attachment>();
            if (request.Attachments.Any())
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Converting {count} attachments for the request.",
                    request.Attachments.Count()
                    );

                // Loop through the attachments.
                foreach (var attachment in request.Attachments)
                {
                    // Look for the mime type.
                    var mimeType = await _mimeTypeManager.FindByCanonicalTypeAsync(
                        attachment.MimeType
                        ).ConfigureAwait(false);

                    // Did we fail?
                    if (mimeType is null)
                    {
                        // Panic!!
                        throw new KeyNotFoundException(
                            $"Mime type: {attachment.MimeType} was not found!"
                            );
                    }

                    // Add the attachment.
                    attachments.Add(new Attachment()
                    {
                        MimeType = mimeType,
                        OriginalFileName = attachment.FileName,
                        Length = attachment.Length,
                        Data = Convert.FromBase64String(attachment.Data)
                    });
                }
            }

            // Loop and convert the properties for the request.
            var properties = new List<MessageProperty>();
            if (request.Properties.Any())
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Converting {count} properties for the request.",
                    request.Properties.Count()
                    );

                // Loop through the properties.
                foreach (var property in request.Properties)
                {
                    // Look  for the property type.
                    var propertyType = await _propertyTypeManager.FindByNameAsync(
                        property.PropertyName
                        ).ConfigureAwait(false);

                    // Did we fail?
                    if (propertyType is null)
                    {
                        // Panic!!
                        throw new KeyNotFoundException(
                            $"property type: {property.PropertyName} not found!"
                            );
                    }

                    // Add the message property.
                    properties.Add(new MessageProperty()
                    {
                        PropertyType = propertyType,
                        Value = property.Value
                    });
                }
            }

            // ======
            // Step 3: Store the message.
            // ======

            // Log what we are about to do.
            _logger.LogDebug(
                "Storing a mail message"
                );

            // Create the text message.
            var message = await _textMessageManager.CreateAsync(
                new TextMessage()
                {
                    From = request.From ?? "",
                    To = request.To,
                    Body = request.Body
                },
                User?.Identity?.Name ?? "anonymous"
                ).ConfigureAwait(false);

            // ======
            // Step 4: Create the response.
            // ======

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a response"
                );

            // Create the response.
            var response = new StorageResponse();

            // Should never happen, but, pffft, check it anyway.
            if (message is null)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Message failed to store"
                    );

                return Ok(response);
            }

            // Fill out the response.
            response.MessageKey = message.MessageKey;
            response.CreatedOnUtc = message.CreatedOnUtc;

            // Log what we are about to do.
            _logger.LogDebug(
                "Returning link and results"
                );

            // Return the results.
            return Created($"ByKey/{response.MessageKey}", response);
        }
        catch (Exception ex)
        {
            // Log the error in detail.
            _logger.LogError(
                ex,
                "Failed to store a text message!"
                );

            // Return an overview of the problem.
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "The controller failed to store a text message!"
                );
        }
    }

    #endregion
}
