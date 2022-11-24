
namespace CG.Purple.Host.Pages.Messages;

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
    private readonly List<BreadcrumbItem> _crumbs = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("Message", href: "/message")
    };

    /// <summary>
    /// This field indicates the page is busy.
    /// </summary>
    private bool _isBusy;

    /// <summary>
    /// This field contains the collection of mail messages.
    /// </summary>
    private IEnumerable<MailMessage>? _mailMessages = Array.Empty<MailMessage>();

    /// <summary>
    /// This field contains the collection of text messages.
    /// </summary>
    private IEnumerable<TextMessage>? _textMessages = Array.Empty<TextMessage>();

    /// <summary>
    /// This field contains the current mail search string.
    /// </summary>
    private string mailGridSearchString = "";

    /// <summary>
    /// This field contains the current text search string.
    /// </summary>
    private string textGridSearchString = "";

    /// <summary>
    /// This field contains the timer for the page refresh operations.
    /// </summary>
    private Timer _timer = null!;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the mail message manager for this page.
    /// </summary>
    [Inject]
    protected IMailMessageManager MailManager { get; set; } = null!;

    /// <summary>
    /// This property contains the text message manager for this page.
    /// </summary>
    [Inject]
    protected ITextMessageManager TextManager { get; set; } = null!;

    /// <summary>
    /// This property contains the dialog service for this page.
    /// </summary>
    [Inject]
    protected IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// This property contains the snackbar service for this page.
    /// </summary>
    [Inject]
    protected ISnackbar SnackbarService { get; set; } = null!;

    /// <summary>
    /// This property contains the HTTP context accessor.
    /// </summary>
    [Inject]
    protected IHttpContextAccessor HttpContextAccessor { get; set; } = null!;

    /// <summary>
    /// This property contains the name of the current user, or the word
    /// 'anonymous' if nobody is currently authenticated.
    /// </summary>
    protected string UserName => HttpContextAccessor.HttpContext?.User?.Identity?.Name ?? "anonymous";

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
            // We're busy.
            _isBusy = true;

            // Give the UI time to show the busy indicator.
            await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
            await Task.Delay(250);

            // Fetch the messages.
            _mailMessages = await MailManager.FindAllAsync();
            _textMessages = await TextManager.FindAllAsync();

            // Start the refresh timer.
            _timer = new Timer(
                _TimerCallback,
                this,
                15000,
                15000
                );

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
            // We're no longer busy.
            _isBusy = false;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method adapts the <see cref="MailFilterFunc(MailMessage, string)"/> method
    /// for use with a <see cref="MudTable{T}"/> control.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool MailFilterFunc1(MailMessage element) => 
        MailFilterFunc(element, mailGridSearchString);

    // *******************************************************************

    /// <summary>
    /// This method adapts the <see cref="TextFilterFunc(TextMessage, string)"/> method
    /// for use with a <see cref="MudTable{T}"/> control.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool TextFilterFunc1(TextMessage element) => 
        TextFilterFunc(element, textGridSearchString);

    // *******************************************************************

    /// <summary>
    /// This method performs a search of the mail messages.
    /// </summary>
    /// <param name="element">The element to uses for the operation.</param>
    /// <param name="searchString">The search string to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool MailFilterFunc(
        MailMessage element,
        string searchString
        )
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return true;
        }
        if (element.To.Contains(
            searchString, 
            StringComparison.OrdinalIgnoreCase)
            )
        {
            return true;
        }
        if (!string.IsNullOrEmpty(element.CC))
        {
            if (element.CC.Contains(
                searchString, 
                StringComparison.OrdinalIgnoreCase)
                )
            {
                return true;
            }
        }
        if (!string.IsNullOrEmpty(element.BCC))
        {
            if (element.BCC.Contains(
                searchString, 
                StringComparison.OrdinalIgnoreCase)
                )
            {
                return true;
            }
        }
        if (!string.IsNullOrEmpty(element.Subject))
        {
            if (element.Subject.Contains(
                searchString, 
                StringComparison.OrdinalIgnoreCase)
                )
            {
                return true;
            }
        }
        if ((Enum.GetName<MessageState>(element.MessageState) ?? "")
            .Contains(searchString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        if (element.Body.Contains(
            searchString, 
            StringComparison.OrdinalIgnoreCase)
            )
        {
            return true;
        }
        return false;
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a search of the text messages.
    /// </summary>
    /// <param name="element">The element to uses for the operation.</param>
    /// <param name="searchString">The search string to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool TextFilterFunc(
        TextMessage element,
        string searchString
        )
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return true;
        }
        if (element.To.Contains(
            searchString, 
            StringComparison.OrdinalIgnoreCase)
            )
        {
            return true;
        }
        if ((Enum.GetName<MessageState>(element.MessageState) ?? "")
            .Contains(searchString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        if (element.Body.Contains(
            searchString, 
            StringComparison.OrdinalIgnoreCase)
            )
        {
            return true;
        }
        return false;
    }

    // *******************************************************************

    /// <summary>
    /// This method manually refreshes the mail messages collection.
    /// </summary>
    protected async Task OnRefreshMailMessages()
    {
        try
        {
            // We're busy.
            _isBusy = true;

            // Give the UI time to show the busy indicator.
            await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
            await Task.Delay(250);

            // Fetch the messages.
            _mailMessages = await MailManager.FindAllAsync();

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
            // We're no longer busy.
            _isBusy = false;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method manually refreshes the text messages collection.
    /// </summary>
    protected async Task OnRefreshTextMessages()
    {
        try
        {
            // We're busy.
            _isBusy = true;

            // Give the UI time to show the busy indicator.
            await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
            await Task.Delay(250);

            // Fetch the messages.
            _textMessages = await TextManager.FindAllAsync();

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
            // We're no longer busy.
            _isBusy = false;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method displays a dialog for the message properties.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnPropertiesAsync(
        Message message
        )
    {
        try
        {
            // We clone the message because anything we do to it, in
            //   the dialog, is difficult to undo without a round trip
            //   to the database, which seems silly. This way, if the
            //   user manipulates the object, via the UI, then cancels
            //   the operation, no harm done.
            var tempMessage = message.QuickClone();

            // Show the dialog.
            var dialog = await DialogService.ShowEx<MessagePropertiesDialog>(
                "Properties",
                new DialogParameters()
                {
                     { "Model", tempMessage }
                },
                new DialogOptionsEx()
                {
                    MaximizeButton = true,
                    CloseButton = true,
                    //FullHeight = true,
                    CloseOnEscapeKey = true,
                    MaxWidth = MaxWidth.Medium,
                    FullWidth = true,
                    DragMode = MudDialogDragMode.Simple,
                    Animations = new[] { AnimationType.SlideIn },
                    Position = DialogPosition.CenterRight,
                    DisableSizeMarginY = true,
                    DisablePositionMargin = true
                }).ConfigureAwait(false);

            // Show the dialog.
            var result = await dialog.Result;

            // Did the user save?
            if (!result.Cancelled)
            {
                // We're busy.
                _isBusy = true;

                // Give the UI time to show the busy indicator.
                await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
                await Task.Delay(250);

                // TODO : save the changes.

                // Tell the world what happened.
                SnackbarService.Add(
                    $"Changes were saved",
                    Severity.Success,
                    options => options.CloseAfterNavigation = true
                    );
            }

        }
        catch ( Exception ex )
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
            // We're no longer busy.
            _isBusy = false;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method is called by the timer, to periodically refresh the page.
    /// </summary>
    /// <param name="state">The optional state for the operation.</param>
    protected async void _TimerCallback(
        object? state
        )
    {
        try
        {
            // Refresh the page.
            await OnRefreshMailMessages().ConfigureAwait(false);
            await OnRefreshTextMessages().ConfigureAwait(false);
        }
        finally
        {
            // Ensure the busy indicator is hidden.
            await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
        }
    }

    #endregion
}
