
namespace CG.Purple.Host.Controllers;

/// <summary>
/// This class is a REST controller for operations related to email resources.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the mail message manager for this controller.
    /// </summary>
    internal protected readonly IMailMessageManager _mailMessageManager;

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
    /// This field contains the provider type manager for this controller.
    /// </summary>
    internal protected readonly IProviderTypeManager _providerTypeManager;

    /// <summary>
    /// This field contains the logger for this controller.
    /// </summary>
    internal protected readonly ILogger<MailController> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MailController"/>
    /// class.
    /// </summary>
    /// <param name="mailMessageManager">The mail message manager to use 
    /// with this controller.</param>
    /// <param name="messageLogManager">The message log manager to use 
    /// with this controller.</param>
    /// <param name="mimeTypeManager">The mime type manager to use with 
    /// this controller.</param>
    /// <param name="propertyTypeManager">The property type manager to
    /// use with this controller.</param>
    /// <param name="providerTypeManager">The provider type manager to
    /// use with this controller.</param>
    /// <param name="logger">The logger to use with this controller.</param>
    public MailController(
        IMailMessageManager mailMessageManager,
        IMessageLogManager messageLogManager,
        IMimeTypeManager mimeTypeManager,
        IPropertyTypeManager propertyTypeManager,
        IProviderTypeManager providerTypeManager,
        ILogger<MailController> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mailMessageManager, nameof(mailMessageManager))
            .ThrowIfNull(messageLogManager, nameof(messageLogManager))
            .ThrowIfNull(mimeTypeManager, nameof(mimeTypeManager))
            .ThrowIfNull(propertyTypeManager, nameof(propertyTypeManager))
            .ThrowIfNull(providerTypeManager, nameof(providerTypeManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _mailMessageManager = mailMessageManager;
        _messageLogManager = messageLogManager;
        _mimeTypeManager = mimeTypeManager;
        _propertyTypeManager = propertyTypeManager;
        _providerTypeManager = providerTypeManager;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method searches for a mail message by key.
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
            var mailMessage = await _mailMessageManager.FindByKeyAsync(
                messageKey
                ).ConfigureAwait(false);

            // Did we fail?
            if (mailMessage is null)
            {
                return NotFound("The message key is invalid!");
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
                ).ConfigureAwait(false))
                .OrderByDescending(x => x.CreatedOnUtc);

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
                "Failed to search for a mail message, by key!"
                );

            // Return an overview of the problem.
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "The controller failed to search for a mail " +
                "message, by key!"
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method stores a new email.
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
        [FromBody] MailStorageRequest request
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

            // Should we validate the provider type?
            if (!string.IsNullOrEmpty(request.ProviderType))
            {
                // Look for the given provider type.
                var providerType = await _providerTypeManager.FindByNameAsync(
                    request.ProviderType 
                    ).ConfigureAwait(false);

                // Did we fail?
                if (providerType is null)
                {
                    return BadRequest("The ProviderType field value is invalid!");
                }
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

            // Create the mail message.
            var message = await _mailMessageManager.CreateAsync(
                new MailMessage()
                {
                    From = request.From ?? "",
                    To = request.To,
                    CC = request.CC ?? "",
                    BCC = request.BCC ?? "",
                    Subject = request.Subject ?? "",  
                    Body = request.Body,
                    IsHtml = request.IsHtml,
                    Attachments = attachments,
                    MessageProperties = properties
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
                "Failed to store a mail message!"
                );

            // Return an overview of the problem.
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "The controller failed to store a mail message!"
                );
        }
    }

    #endregion
}
