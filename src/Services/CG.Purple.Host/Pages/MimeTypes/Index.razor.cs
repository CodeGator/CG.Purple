
namespace CG.Purple.Host.Pages.MimeTypes;

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
    protected readonly List<BreadcrumbItem> _crumbs = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("Admin", href: "/admin", disabled: true),
        new BreadcrumbItem("Mime Types", href: "/admin/mimetypes")
    };

    /// <summary>
    /// This field indicates the page is busy.
    /// </summary>
    protected bool _isBusy;

    /// <summary>
    /// This field contains the collection of mime types.
    /// </summary>
    protected IEnumerable<MimeType>? _mimeTypes = Array.Empty<MimeType>();

    /// <summary>
    /// This field contains the current mail search string.
    /// </summary>
    protected string _gridSearchString = "";

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the mime type manager for this page.
    /// </summary>
    [Inject]
    protected IMimeTypeManager MimeTypeManager { get; set; } = null!;

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
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<Index> Logger { get; set; } = null!;

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
                "Fetching messages for the page."
                );

            // Find the mime types.
            _mimeTypes = await MimeTypeManager.FindAllAsync();

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

    /// <summary>
    /// This method adapts the <see cref="FilterFunc(MimeType, string)"/> method
    /// for use with a <see cref="MudTable{T}"/> control.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc1(MimeType element) =>
        FilterFunc(element, _gridSearchString);

    // *******************************************************************

    /// <summary>
    /// This method performs a search of the mime types.
    /// </summary>
    /// <param name="element">The element to uses for the operation.</param>
    /// <param name="searchString">The search string to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc(
        MimeType element,
        string searchString
        )
    {
        // How should we filter?
        if (searchString.Contains(':'))
        {
            // If we get here then we might need to filter by file extensions.

            // Should be: ext:value
            var parts = searchString.Split(':');
            if (parts.Length == 2)
            {
                // If we get here then we need to filter by extensions.

                // Look for a matching file type.
                var match = element.FileTypes.FirstOrDefault(x =>
                    x.Extension == parts[0]
                    );

                // Did we find one?
                if (match is not null)
                {
                    return true;
                }
            }
        }

        // If we get here then we should filter on all properties.

        if (string.IsNullOrWhiteSpace(searchString))
        {
            return true;
        }
        if (element.Type.Contains(
            searchString,
            StringComparison.OrdinalIgnoreCase)
            )
        {
            return true;
        }
        if (element.SubType.Contains(
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
    /// This method deletes the given mime type.
    /// </summary>
    /// <param name="mimeType">The mime type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteAsync(
        MimeType mimeType
        )
    {

    }

    // *******************************************************************

    /// <summary>
    /// This method edits the given mime type.
    /// </summary>
    /// <param name="mimeType">The mime type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnEditAsync(
        MimeType mimeType
        )
    {

    }

    #endregion
}
