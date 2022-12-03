
using CG.Purple.Managers;

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
    /// <param name="logger">The logger to use with this controller.</param>
    public MailController(
        IMailMessageManager mailMessageManager,
        ILogger<MailController> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mailMessageManager, nameof(mailMessageManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _mailMessageManager = mailMessageManager;
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
    [HttpGet("ByKey/{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetByKeyAsync(
        string messageKey
        )
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok();
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> PostAsync(
        [FromBody] MailRequest request
        )
    {
        try
        {
            // Sanity check the model.
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Create the mail message.
            var message = await _mailMessageManager.CreateAsync(
                new Models.MailMessage()
                {
                    From = request.From ?? "",
                    To = request.To,
                    CC = request.CC ?? "",
                    BCC = request.BCC ?? "",
                    Subject = request.Subject ?? "",  
                    Body = request.Body,
                    IsHtml = request.IsHtml,    
                },
                User?.Identity?.Name ?? "anonymous"
                ).ConfigureAwait(false);

            // Did we fail?
            if (message is null)
            {
                return NoContent();
            }
            
            // Create the response.
            var response = new MailResponse()
            {
                MessageKey = message.MessageKey,
                CreatedOnUtc = message.CreatedOnUtc
            };

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
