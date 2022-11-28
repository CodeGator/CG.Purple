
namespace CG.Purple.Host.Pages.ProviderTypes;

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
        new BreadcrumbItem("Provider Types", href: "/admin/providertypes")
    };

    /// <summary>
    /// This field indicates the page is busy.
    /// </summary>
    protected bool _isBusy;

    /// <summary>
    /// This field contains the collection of provider types.
    /// </summary>
    protected IEnumerable<ProviderType>? _providerTypes = Array.Empty<ProviderType>();

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
    /// This property contains the provider type manager for this page.
    /// </summary>
    [Inject]
    protected IProviderTypeManager ProviderTypeManager { get; set; } = null!;

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

            // Find the provider types.
            _providerTypes = await ProviderTypeManager.FindAllAsync();

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
    /// This method adapts the <see cref="FilterFunc(ProviderType, string)"/> method
    /// for use with a <see cref="MudTable{T}"/> control.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc1(ProviderType element) =>
        FilterFunc(element, _gridSearchString);

    // *******************************************************************

    /// <summary>
    /// This method performs a search of the provider types.
    /// </summary>
    /// <param name="element">The element to uses for the operation.</param>
    /// <param name="searchString">The search string to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc(
        ProviderType element,
        string searchString
        )
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return true;
        }
        if (element.Name.Contains(
            searchString,
            StringComparison.OrdinalIgnoreCase)
            )
        {
            return true;
        }
        if (element.Description is not null)
        {
            if (element.Description.Contains(
                searchString,
                StringComparison.OrdinalIgnoreCase)
                )
            {
                return true;
            }
        }
        return false;
    }

    // *******************************************************************

    /// <summary>
    /// This method add a new provider type.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnCreateAsync()
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog options."
                );

            // Create the dialog options.
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog parameters."
                );

            // Create the dialog parameters.
            var providers = new DialogParameters()
            {
                { "Model", new ProviderType()
                {
                    CreatedBy = UserName,
                    CreatedOnUtc = DateTime.UtcNow,
                }}
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating provider type dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<ProviderTypeDialog>(
                "Edit Provider Type",
                providers,
                options
                );

            // Get the results of the dialog.
            var result = await dialog.Result;

            // Did the user save?
            if (!result.Cancelled)
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
                    "Recovering the modified provider type."
                    );

                // Recover the edited provider type.
                var changedProviderType = (ProviderType)result.Data;

                // Log what we are about to do.
                Logger.LogDebug(
                    "Saving changes to provider type: {id}",
                    changedProviderType.Id
                    );

                // Defer to the manager for the update.
                _ = await ProviderTypeManager.CreateAsync(
                    changedProviderType,
                    UserName
                    );

                // Log what we are about to do.
                Logger.LogDebug(
                    "Showing the snackbar message."
                    );

                // Tell the world what happened.
                SnackbarService.Add(
                    $"Changes were saved",
                    Severity.Success,
                    options => options.CloseAfterNavigation = true
                    );

                // Log what we are about to do.
                Logger.LogDebug(
                    "Refreshing the page."
                    );

                // Defer to the manager for the query.
                _providerTypes = await ProviderTypeManager.FindAllAsync();
            }
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to create a provider type."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to create provider type!</b> " +
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
    /// This method deletes the given provider type.
    /// </summary>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteAsync(
        ProviderType providerType
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Prompting the caller."
                );

            // Prompt the user.
            var result = await DialogService.ShowMessageBox(
                title: "Purple",
                markupMessage: new MarkupString("This will delete the provider " +
                $"type <b>{providerType.Name}</b> <br /> <br /> Are you " +
                "<i>sure</i> you want to do that?"),
                noText: "Cancel"
                );

            // Did the user cancel?
            if (result.HasValue && !result.Value)
            {
                return; // Nothing more to do.
            }

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
                "Saving the change to the database."
                );

            // Defer to the manager for the delete.
            await ProviderTypeManager.DeleteAsync(
                providerType,
                UserName
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the snackbar message."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"Changes were saved",
                Severity.Success,
                options => options.CloseAfterNavigation = true
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Refreshing the page data."
                );

            // Defer to the manager for the query.
            _providerTypes = await ProviderTypeManager.FindAllAsync();
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to delete provider type: {id}",
                providerType.Id
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to delete the provider type!</b> " +
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
    /// This method edits the given provider type.
    /// </summary>
    /// <param name="providerType">The provider type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnEditAsync(
        ProviderType providerType
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog options."
                );

            // Create the dialog options.
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Cloning the provider type: {id}.",
                providerType.Id
                );

            // We clone the provider type because anything we do to it, in
            //   the dialog is difficult to undo without a round trip
            //   to the database, which seems silly. This way, if the
            //   user manipulates the object, via the UI, then cancels
            //   the operation, no harm done.
            var tempProviderType = providerType.QuickClone();

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog parameters."
                );

            // Create the dialog parameters.
            var providers = new DialogParameters()
            {
                { "Model", tempProviderType }
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating provider type dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<ProviderTypeDialog>(
                "Edit Provider Type",
                providers,
                options
                );

            // Get the results of the dialog.
            var result = await dialog.Result;

            // Did the user save?
            if (!result.Cancelled)
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
                    "Recovering the modified provider type."
                    );

                // Recover the edited provider type.
                var changedProviderType = (ProviderType)result.Data;

                // Log what we are about to do.
                Logger.LogDebug(
                    "Saving changes to provider type: {id}",
                    changedProviderType.Id
                    );

                // Defer to the manager for the update.
                _ = await ProviderTypeManager.UpdateAsync(
                    changedProviderType,
                    UserName
                    );

                // Log what we are about to do.
                Logger.LogDebug(
                    "Showing the snackbar message."
                    );

                // Tell the world what happened.
                SnackbarService.Add(
                    $"Changes were saved",
                    Severity.Success,
                    options => options.CloseAfterNavigation = true
                    );

                // Log what we are about to do.
                Logger.LogDebug(
                    "Refreshing the page."
                    );

                // Defer to the manager for the query.
                _providerTypes = await ProviderTypeManager.FindAllAsync();
            }
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to edit the provider type!</b> " +
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

    #endregion
}
