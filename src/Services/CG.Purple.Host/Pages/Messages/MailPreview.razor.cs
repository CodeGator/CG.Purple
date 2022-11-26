
using CG.Purple.Models;
using MudBlazor.Extensions.Components;
using Nextended.Core.COM;
using System.Text;

namespace CG.Purple.Host.Pages.Messages;

/// <summary>
/// This class is the code-behind for the <see cref="MailPreview"/> page.
/// </summary>
public partial class MailPreview
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains a reference to breadcrumbs for this page.
    /// </summary>
    protected readonly List<BreadcrumbItem> _crumbs = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("Messages", href: "/messages"),
        new BreadcrumbItem("Mail", href: $"/messages/mail", disabled: true)
    };

    /// <summary>
    /// This field indicates the page is busy.
    /// </summary>
    protected bool _isBusy;

    /// <summary>
    /// This field contains the message for this page.
    /// </summary>
    protected MailMessage? _message;

    /// <summary>
    /// This field contains the HTML encoded body of the message.
    /// </summary>
    protected string? _html;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the identifier for the message.
    /// </summary>
    [Parameter]
    public int MessageId { get; set; }

    /// <summary>
    /// This property contains the snackbar service for this page.
    /// </summary>
    [Inject]
    protected ISnackbar SnackbarService { get; set; } = null!;

    /// <summary>
    /// This property contains the mail message manager for this page.
    /// </summary>
    [Inject]
    protected IMailMessageManager MailManager { get; set; } = null!;

    /// <summary>
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<MailPreview> Logger { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is called by the framework to initialize the page.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page to busy."
                );

            // We're busy.
            _isBusy = true;

            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page state to dirty."
                );

            // Give the UI time to show the busy indicator.
            await InvokeAsync(() => StateHasChanged());
            await Task.Delay(250);

            // Log what we are about to do.
            Logger.LogDebug(
                "Fetching message for the page."
                );

            // Look for the message.
            _message = await MailManager.FindByIdAsync(
                MessageId
                );
            
            // Did we succeed?
            if (_message is not null)
            {
                // Convert the message body to bytes.
                var bytes = Encoding.UTF8.GetBytes(_message.Body);

                // Convert the bytes to embedded HTML.
                _html = $"data:text/html;charset=utf8,{Convert.ToBase64String(bytes)}";
            }
            
            // Give the base class a chance.
            await base.OnInitializedAsync();
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
        finally
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page to not busy."
                );

            // We're no longer busy.
            _isBusy = false;
        }
    }

    // *******************************************************************

    protected Task<ContentErrorResult> HandleContentError(IFileDisplayInfos arg)
    {
        if (arg.ContentType.Contains("word"))
        {
            return Task.FromResult(ContentErrorResult
                .RedirectTo("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTiZiqnBKWS8NHcKbRH04UkYjrCgxUMz6sVNw&usqp=CAU", "image/png")
                .WithMessage("No word plugin found we display a sheep"));
        }
        return Task.FromResult(ContentErrorResult.Unhandled);
    }

    #endregion
}
