

namespace CG.Purple.Tools.TestClient.Pages;

/// <summary>
/// This class is the code-behind for the <see cref="Index"/> page.
/// </summary>
public partial class Index
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the model for the text form
    /// </summary>
    internal protected TextStorageRequest _textModel = new()
    {
        From = "test1@codegator.com",
        To = "test1@codegator.com",
        Body = "this is a test text"
    };

    /// <summary>
    /// This field contains the model for the mail form
    /// </summary>
    internal protected MailStorageRequest _mailModel = new()
    {
        From = "test1@codegator.com",
        To = "test1@codegator.com",
        Subject = "test email",
        Body = "this is a test email"
    };

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the HTTP client factory for the page.
    /// </summary>
    [Inject]
    public PurpleHttpClientFactory Factory { get; set; } = null!;

    /// <summary>
    /// This property contains the monitor for the page.
    /// </summary>
    [Inject]
    public PurpleClientMonitor Monitor { get; set; } = null!;

    /// <summary>
    /// This property contains the snackbar service for this page.
    /// </summary>
    [Inject]
    protected ISnackbar SnackbarService { get; set; } = null!;

    /// <summary>
    /// This property contains the client options for this page.
    /// </summary>
    [Inject]
    public IOptions<PurpleClientOptions> Options { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    protected override void OnInitialized()
    {
        // Wire up a handler for status notifications.
        Monitor.Status += (status) =>
        {
            SnackbarService.Add(
                $"<b>Status update!</b>",
                Severity.Normal,
                options => options.CloseAfterNavigation = true
                );
        };

        // Give the base class a chance.
        base.OnInitialized();
    }

    /// <summary>
    /// This method sends a mail message to the microservice.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnValidMailSubmitAsync()
    {
        try
        {
            // Create the HTTP client.
            var client = Factory.CreateClient();

            // Send the email.
            var response = await client.SendMailMessageAsync(
                _mailModel
                );

            // Did we succeed?
            if (response is not null)
            {
                // Tell the world what happened.
                SnackbarService.Add(
                    $"<b>Sent the mail message!</b> " +
                    $"<ul><li><b>CreatedOn: {response.CreatedOnUtc}</b></li></ul>" +
                    $"<ul><li>MessageKey: {response.MessageKey}</li></ul>",
                    Severity.Success,
                    options => options.CloseAfterNavigation = true
                    );
            }
            else
            {
                // Tell the world what happened.
                SnackbarService.Add(
                    $"<b>Failed to send the mail message!</b>",
                    Severity.Error,
                    options => options.CloseAfterNavigation = true
                    );
            }
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Something broke!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }        
    }

    // *******************************************************************

    /// <summary>
    /// This method sends a text message to the microservice.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnValidTextSubmitAsync()
    {
        try
        {
            // Create the HTTP client.
            var client = Factory.CreateClient();

            // Send the text.
            var response = await client.SendTextMessageAsync(
                _textModel
                );

            // Did we succeed?
            if (response is not null)
            {
                // Tell the world what happened.
                SnackbarService.Add(
                    $"<b>Sent the text message!</b> " +
                    $"<ul><li><b>CreatedOn: {response.CreatedOnUtc}</b></li></ul>" +
                    $"<ul><li>MessageKey: {response.MessageKey}</li></ul>",
                    Severity.Success,
                    options => options.CloseAfterNavigation = true
                    );
            }
            else
            {
                // Tell the world what happened.
                SnackbarService.Add(
                    $"<b>Failed to send the text message!</b>",
                    Severity.Error,
                    options => options.CloseAfterNavigation = true
                    );
            }
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Something broke!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method displays the text attributes dialog
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnTextAttachmentsAsync()
    {
        try
        {
            // TODO : write the code for this.   
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Something broke!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method displays the text properties dialog
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnTextPropertiesAsync()
    {
        try
        {
            // TODO : write the code for this.   
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Something broke!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method displays the mail attributes dialog
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnMailAttachmentsAsync()
    {
        try
        {
            // TODO : write the code for this.   
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Something broke!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method displays the mail properties dialog
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnMailPropertiesAsync()
    {
        try
        {
            // TODO : write the code for this.   
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Something broke!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    #endregion
}
