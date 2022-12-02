
namespace CG.Purple.Host.Pages.Admin.ParameterTypes;

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
        new BreadcrumbItem("Parameter Types", href: "/admin/parameterTypes")
    };

    /// <summary>
    /// This field indicates the page is busy.
    /// </summary>
    protected bool _isBusy;

    /// <summary>
    /// This field contains the collection of parameter types.
    /// </summary>
    protected IEnumerable<ParameterType>? _parameterTypes = Array.Empty<ParameterType>();

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
    /// This property contains the parameter type manager for this page.
    /// </summary>
    [Inject]
    protected IParameterTypeManager ParameterTypeManager { get; set; } = null!;

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

            // Find the parameter types.
            _parameterTypes = await ParameterTypeManager.FindAllAsync();

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
    /// This method adapts the <see cref="FilterFunc(ParameterType, string)"/> method
    /// for use with a <see cref="MudTable{T}"/> control.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc1(ParameterType element) =>
        FilterFunc(element, _gridSearchString);

    // *******************************************************************

    /// <summary>
    /// This method performs a search of the parameter types.
    /// </summary>
    /// <param name="element">The element to uses for the operation.</param>
    /// <param name="searchString">The search string to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc(
        ParameterType element,
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
    /// This method add a new parameter type.
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
            var parameters = new DialogParameters()
            {
                { "Model", new ParameterType()
                {
                    CreatedBy = UserName,
                    CreatedOnUtc = DateTime.UtcNow,
                }}
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating parameter type dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<ParameterTypeDialog>(
                "Edit Parameter Type",
                parameters,
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
                    "Recovering the modified parameter type."
                    );

                // Recover the edited parameter type.
                var changedParameterType = (ParameterType)result.Data;

                // Log what we are about to do.
                Logger.LogDebug(
                    "Saving changes to parameter type: {id}",
                    changedParameterType.Id
                    );

                // Defer to the manager for the update.
                _ = await ParameterTypeManager.CreateAsync(
                    changedParameterType,
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
                _parameterTypes = await ParameterTypeManager.FindAllAsync();
            }
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to create a parameter type."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to create parameter type!</b> " +
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
    /// This method deletes the given parameter type.
    /// </summary>
    /// <param name="parameterType">The parameter type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteAsync(
        ParameterType parameterType
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
                markupMessage: new MarkupString("This will delete the parameter " +
                $"type <b>{parameterType.Name}</b> <br /> <br /> Are you " +
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
            await ParameterTypeManager.DeleteAsync(
                parameterType,
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
            _parameterTypes = await ParameterTypeManager.FindAllAsync();
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to delete parameter type: {id}",
                parameterType.Id
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to delete the parameter type!</b> " +
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
    /// This method edits the given parameter type.
    /// </summary>
    /// <param name="parameterType">The parameter type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnEditAsync(
        ParameterType parameterType
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
                "Cloning the parameter type: {id}.",
                parameterType.Id
                );

            // We clone the parameter type because anything we do to it, in
            //   the dialog is difficult to undo without a round trip
            //   to the database, which seems silly. This way, if the
            //   user manipulates the object, via the UI, then cancels
            //   the operation, no harm done.
            var tempParameterType = parameterType.QuickClone();

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog parameters."
                );

            // Create the dialog parameters.
            var parameters = new DialogParameters()
            {
                { "Model", tempParameterType }
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating parameter type dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<ParameterTypeDialog>(
                "Edit Mime Type",
                parameters,
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
                    "Recovering the modified parameter type."
                    );

                // Recover the edited parameter type.
                var changedParameterType = (ParameterType)result.Data;

                // Log what we are about to do.
                Logger.LogDebug(
                    "Saving changes to parameter type: {id}",
                    changedParameterType.Id
                    );

                // Defer to the manager for the update.
                _ = await ParameterTypeManager.UpdateAsync(
                    changedParameterType,
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
                _parameterTypes = await ParameterTypeManager.FindAllAsync();
            }
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to edit the parameter type!</b> " +
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
