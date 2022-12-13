
namespace CG.Purple.Samples.QuickStart.Pages.Home;

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
    /// This field contains a reference to breadcrumbs for the view.
    /// </summary>
    internal protected readonly List<BreadcrumbItem> _crumbs = new()
    {
        new BreadcrumbItem("Home", href: "/")
    };

    /// <summary>
    /// This field contains the model for the text form
    /// </summary>
    internal protected TextStorageRequest _textModel = new()
    {
        From = "+1 918 867 5309",
        To = "+1 918 867 5309",
        Body = "this is a test text",
        Attachments = new List<AttachmentRequest>()
        {
            new AttachmentRequest()
            {
                MimeType = "application/octet-stream",
                FileName = "test1.bin",
                Length = 4,
                Data = Convert.ToBase64String(new byte[] { 0, 1, 2, 3 })
            }
        },
        Properties = new List<MessagePropertyRequest>()
        { 
            new MessagePropertyRequest()
            {
                PropertyName = "Test 1",
                Value = "value 1"
            }
        }
    };

    /// <summary>
    /// This field contains the model for the mail form
    /// </summary>
    internal protected MailStorageRequest _mailModel = new()
    {
        From = "test1@codegator.com",
        To = "test1@codegator.com",
        Subject = "test email",
        Body = "this is a test email",
        Attachments = new List<AttachmentRequest>()
        {
            new AttachmentRequest()
            {
                MimeType = "application/octet-stream",
                FileName = "test1.bin",
                Length = 4,
                Data = Convert.ToBase64String(new byte[] { 0, 1, 2, 3 })
            }
        },
        Properties = new List<MessagePropertyRequest>()
        {
            new MessagePropertyRequest()
            {
                PropertyName = "Test 1",
                Value = "value 1"
            }
        }
    };

    /// <summary>
    /// This field contains a disabled flag, for messages.
    /// </summary>
    internal protected string? _messageKey;

    /// <summary>
    /// This field contains a disabled flag, for messages.
    /// </summary>
    internal protected bool? _isDisabled;

    /// <summary>
    /// This field contains a disabled flag, for messages.
    /// </summary>
    internal protected int? _maxErrors;

    /// <summary>
    /// This field contains a default priority, for messages.
    /// </summary>
    internal protected int? _priority;

    /// <summary>
    /// This field contains a default process after date, for messages.
    /// </summary>
    internal protected DateTime? _processAfter;

    /// <summary>
    /// This field contains a default archive after date, for messages.
    /// </summary>
    internal protected DateTime? _archiveAfter;

    /// <summary>
    /// This field contains a default providerType, for messages.
    /// </summary>
    internal protected string? _providerType;

    /// <summary>
    /// This field contains a list of status notifications.
    /// </summary>
    internal protected List<StatusNotification> _status = new();

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the HTTP client factory for the page.
    /// </summary>
    [Inject]
    public IPurpleHttpClientFactory Factory { get; set; } = null!;

    /// <summary>
    /// This property contains the monitor for the page.
    /// </summary>
    [Inject]
    public IPurpleStatusMonitor Monitor { get; set; } = null!;

    /// <summary>
    /// This property contains the snackbar service for this page.
    /// </summary>
    [Inject]
    protected ISnackbar SnackbarService { get; set; } = null!;

    /// <summary>
    /// This property contains the dialog service for this page.
    /// </summary>
    [Inject]
    protected IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// This property contains the navigation manager for this page.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

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

    /// <summary>
    /// This method is called by Blazor to initialize the page.
    /// </summary>
    protected override void OnInitialized()
    {
        // Wire up a handler for status notifications.
        Monitor.Status += (status) =>
        {
            // Remember the notification.
            _status.Add(status);
        };

        // Give the base class a chance.
        base.OnInitialized();
    }

    // *******************************************************************

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

            // Copy the message, in case the advanced properties changed.
            var message = new MailStorageRequest()
            {
                MessageKey = _messageKey,
                IsDisabled = _isDisabled,
                Priority = _priority,
                ProviderType = _providerType,
                MaxErrors = _maxErrors,
                ProcessAfterUtc = _processAfter,
                ArchiveAfterUtc = _archiveAfter,
                From = _mailModel.From,
                To = _mailModel.To, 
                CC = _mailModel.CC,
                BCC = _mailModel.BCC,
                Subject = _mailModel.Subject,
                Body = _mailModel.Body,
                IsHtml = _mailModel.IsHtml, 
                Attachments = _mailModel.Attachments,   
                Properties = _mailModel.Properties,
            };

            // Send the email.
            var response = await client.SendMailMessageAsync(
                message
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
                $"<ul><li>{ex.Message}</li></ul>" +
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

            // Copy the message, in case the advanced properties changed.
            var message = new TextStorageRequest()
            {
                MessageKey = _messageKey,
                IsDisabled = _isDisabled,
                Priority = _priority,
                ProviderType = _providerType,
                MaxErrors = _maxErrors,
                ProcessAfterUtc = _processAfter,
                ArchiveAfterUtc = _archiveAfter,
                From = _textModel.From,
                To = _textModel.To, 
                Body = _textModel.Body,
                Attachments = _textModel.Attachments,
                Properties = _textModel.Properties,
            };

            // Send the text.
            var response = await client.SendTextMessageAsync(
                message
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
                $"<ul><li>{ex.Message}</li></ul>" +
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
            // Copy the message, so we don't have to back anything out
            //   if the caller cancels the dialog, later.
            var temp = _textModel.QuickClone();

            // Show the dialog.
            var dialog = await DialogService.ShowAsync<AttachmentsDialog>(
                "Attachments", 
                new DialogParameters()
                {
                    { "Model", temp.Attachments },
                },
                new DialogOptions()
                {
                    CloseButton = true,
                    CloseOnEscapeKey = true,
                    MaxWidth = MaxWidth.Medium,
                    FullWidth = true,
                    Position = DialogPosition.TopCenter,
                });

            // Show the dialog.
            var result = await dialog.Result;

            // Did the user cancel?
            if (result.Cancelled)
            {
                return;
            }

            // Save the changes.
            _textModel.Attachments.Clear();
            _textModel.Attachments.AddRange(temp.Attachments);
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
            // Copy the message, so we don't have to back anything out
            //   if the caller cancels the dialog, later.
            var temp = _mailModel.QuickClone();

            // Show the dialog.
            var dialog = await DialogService.ShowAsync<AttachmentsDialog>(
                "Attachments",
                new DialogParameters()
                {
                    { "Model", temp.Attachments },
                },
                new DialogOptions()
                {
                    CloseButton = true,
                    CloseOnEscapeKey = true,
                    MaxWidth = MaxWidth.Medium,
                    FullWidth = true,
                    Position = DialogPosition.TopCenter,
                });

            // Show the dialog.
            var result = await dialog.Result;

            // Did the user cancel?
            if (result.Cancelled)
            {
                return;
            }

            // Save the changes.
            _mailModel.Attachments.Clear();
            _mailModel.Attachments.AddRange(temp.Attachments);
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
            // Copy the message, so we don't have to back anything out
            //   if the caller cancels the dialog, later.
            var temp = _textModel.QuickClone();

            // Show the dialog.
            var dialog = await DialogService.ShowAsync<PropertiesDialog>(
                "Properties",
                new DialogParameters()
                {
                    { "Model", temp.Properties },
                },
                new DialogOptions()
                {
                    CloseButton = true,
                    CloseOnEscapeKey = true,
                    MaxWidth = MaxWidth.Medium,
                    FullWidth = true,
                    Position = DialogPosition.TopCenter,
                });

            // Show the dialog.
            var result = await dialog.Result;

            // Did the user cancel?
            if (result.Cancelled)
            {
                return;
            }

            // Save the changes.
            _textModel.Properties.Clear();
            _textModel.Properties.AddRange(temp.Properties);
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
            // Copy the message, so we don't have to back anything out
            //   if the caller cancels the dialog, later.
            var temp = _mailModel.QuickClone();

            // Show the dialog.
            var dialog = await DialogService.ShowAsync<PropertiesDialog>(
                "Properties",
                new DialogParameters()
                {
                    { "Model", temp.Properties },
                },
                new DialogOptions()
                {
                    CloseButton = true,
                    CloseOnEscapeKey = true,
                    MaxWidth = MaxWidth.Medium,
                    FullWidth = true,
                    Position = DialogPosition.TopCenter,
                });

            // Show the dialog.
            var result = await dialog.Result;

            // Did the user cancel?
            if (result.Cancelled)
            {
                return;
            }

            // Save the changes.
            _mailModel.Properties.Clear();
            _mailModel.Properties.AddRange(temp.Properties);
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
