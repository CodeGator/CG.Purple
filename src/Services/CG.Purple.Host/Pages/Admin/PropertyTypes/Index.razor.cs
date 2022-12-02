
namespace CG.Purple.Host.Pages.Admin.PropertyTypes;

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
        new BreadcrumbItem("Property Types", href: "/admin/propertytypes")
    };

    /// <summary>
    /// This field indicates the page is busy.
    /// </summary>
    protected bool _isBusy;

    /// <summary>
    /// This field contains the collection of property types.
    /// </summary>
    protected IEnumerable<PropertyType>? _propertyTypes = Array.Empty<PropertyType>();

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
    /// This property contains the property type manager for this page.
    /// </summary>
    [Inject]
    protected IPropertyTypeManager PropertyTypeManager { get; set; } = null!;

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

            // Find the property types.
            _propertyTypes = await PropertyTypeManager.FindAllAsync();

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
    /// This method adapts the <see cref="FilterFunc(PropertyType, string)"/> method
    /// for use with a <see cref="MudTable{T}"/> control.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc1(PropertyType element) =>
        FilterFunc(element, _gridSearchString);

    // *******************************************************************

    /// <summary>
    /// This method performs a search of the property types.
    /// </summary>
    /// <param name="element">The element to uses for the operation.</param>
    /// <param name="searchString">The search string to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc(
        PropertyType element,
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
    /// This method add a new property type.
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
                "Creating dialog properties."
                );

            // Create the dialog properties.
            var properties = new DialogParameters()
            {
                { "Model", new PropertyType()
                {
                    CreatedBy = UserName,
                    CreatedOnUtc = DateTime.UtcNow,
                }}
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating property type dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<PropertyTypeDialog>(
                "Edit Property Type",
                properties,
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
                    "Recovering the modified property type."
                    );

                // Recover the edited property type.
                var changedPropertyType = (PropertyType)result.Data;

                // Log what we are about to do.
                Logger.LogDebug(
                    "Saving changes to property type: {id}",
                    changedPropertyType.Id
                    );

                // Defer to the manager for the update.
                _ = await PropertyTypeManager.CreateAsync(
                    changedPropertyType,
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
                _propertyTypes = await PropertyTypeManager.FindAllAsync();
            }
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to create a property type."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to create property type!</b> " +
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
    /// This method deletes the given property type.
    /// </summary>
    /// <param name="propertyType">The property type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteAsync(
        PropertyType propertyType
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
                markupMessage: new MarkupString("This will delete the property " +
                $"type <b>{propertyType.Name}</b> <br /> <br /> Are you " +
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
            await PropertyTypeManager.DeleteAsync(
                propertyType,
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
            _propertyTypes = await PropertyTypeManager.FindAllAsync();
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to delete property type: {id}",
                propertyType.Id
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to delete the property type!</b> " +
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
    /// This method edits the given property type.
    /// </summary>
    /// <param name="propertyType">The property type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnEditAsync(
        PropertyType propertyType
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
                "Cloning the property type: {id}.",
                propertyType.Id
                );

            // We clone the property type because anything we do to it, in
            //   the dialog is difficult to undo without a round trip
            //   to the database, which seems silly. This way, if the
            //   user manipulates the object, via the UI, then cancels
            //   the operation, no harm done.
            var tempPropertyType = propertyType.QuickClone();

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog properties."
                );

            // Create the dialog properties.
            var properties = new DialogParameters()
            {
                { "Model", tempPropertyType }
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating property type dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<PropertyTypeDialog>(
                "Edit Property Type",
                properties,
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
                    "Recovering the modified property type."
                    );

                // Recover the edited property type.
                var changedPropertyType = (PropertyType)result.Data;

                // Log what we are about to do.
                Logger.LogDebug(
                    "Saving changes to property type: {id}",
                    changedPropertyType.Id
                    );

                // Defer to the manager for the update.
                _ = await PropertyTypeManager.UpdateAsync(
                    changedPropertyType,
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
                _propertyTypes = await PropertyTypeManager.FindAllAsync();
            }
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to edit the property type!</b> " +
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
