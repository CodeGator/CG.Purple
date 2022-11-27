
using CG.Collections.Generic;

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
    /// This property contains the file type manager for this page.
    /// </summary>
    [Inject]
    protected IFileTypeManager FileTypeManager { get; set; } = null!;

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
                    x.Extension.Contains(parts[1], StringComparison.OrdinalIgnoreCase)
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
    /// This method add a new mime type.
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
                { "Model", new MimeType()
                {
                    CreatedBy = UserName,
                    CreatedOnUtc = DateTime.UtcNow, 
                }}
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating mime type dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<MimeTypeDialog>(
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
                    "Recovering the modified mime type."
                    );

                // Recover the edited mime type.
                var changedMimeType = (MimeType)result.Data;

                // Log what we are about to do.
                Logger.LogDebug(
                    "Saving changes to mime type: {id}",
                    changedMimeType.Id
                    );

                // Defer to the manager for the update.
                _ = await MimeTypeManager.CreateAsync(
                    changedMimeType,
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
                _mimeTypes = await MimeTypeManager.FindAllAsync();
            }
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to create a mime type."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to create mime type!</b> " +
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
    /// This method deletes the given mime type.
    /// </summary>
    /// <param name="mimeType">The mime type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteAsync(
        MimeType mimeType
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
                markupMessage: new MarkupString("This will delete the mime " +
                $"type <b>{mimeType.Type}/{mimeType.SubType}</b> <br /> " +
                "<br /> Are you <i>sure</i> you want to do that?"),
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
            await MimeTypeManager.DeleteAsync(
                mimeType,
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
            _mimeTypes = await MimeTypeManager.FindAllAsync();
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to delete mime type: {id}",
                mimeType.Id
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to delete the mime type!</b> " +
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
    /// This method edits the given mime type.
    /// </summary>
    /// <param name="mimeType">The mime type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnEditAsync(
        MimeType mimeType
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
                "Cloning the mime type: {id}.",
                mimeType.Id
                );

            // We clone the mime type because anything we do to it, in
            //   the dialog is difficult to undo without a round trip
            //   to the database, which seems silly. This way, if the
            //   user manipulates the object, via the UI, then cancels
            //   the operation, no harm done.
            var tempMimeType = mimeType.QuickClone();

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog parameters."
                );

            // Create the dialog parameters.
            var parameters = new DialogParameters()
            {
                { "Model", tempMimeType }
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating mime type dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<MimeTypeDialog>(
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
                    "Recovering the modified mime type."
                    );

                // Recover the edited mime type.
                var changedMimeType = (MimeType)result.Data;
                
                // =======
                // Step 1: Find any file types that were deleted, which .
                // =======

                // Log what we are about to do.
                Logger.LogDebug(
                    "Looking for deleted file types."
                    );

                // Find any file types that were deleted.
                var deletedFileTypes = mimeType.FileTypes.Except(
                    changedMimeType.FileTypes,
                    FileTypeEqualityComparer.Instance()
                    );

                // Were there any?
                if (deletedFileTypes.Any())
                {
                    // Log what we are about to do.
                    Logger.LogDebug(
                        "Deleting {count} file types on mime type: {id}.",
                        deletedFileTypes.Count(),
                        mimeType.Id
                        );

                    // Loop through the file types.
                    foreach (var fileType in deletedFileTypes)
                    {
                        // Log what we are about to do.
                        Logger.LogDebug(
                            "Deleting file type: {id1} for mime type: {id2}.",
                            mimeType.Id,
                            fileType.Id
                            );

                        // Delete the file type.
                        await FileTypeManager.DeleteAsync(
                            fileType,
                            UserName
                            );
                    }
                }

                // =======
                // Step 2: Find any file types that were added.
                // =======

                // Find any file types that were added.
                var addedFileTypes = changedMimeType.FileTypes.Except(
                    mimeType.FileTypes,
                    FileTypeEqualityComparer.Instance()
                    );

                // Were there any?
                if (addedFileTypes.Any())
                {
                    // Log what we are about to do.
                    Logger.LogDebug(
                        "Adding {count} file types to mime type: {id}.",
                        addedFileTypes.Count(),
                        mimeType.Id
                        );

                    // Loop through the file types.
                    foreach (var fileType in addedFileTypes)
                    {
                        // Log what we are about to do.
                        Logger.LogDebug(
                            "Adding file type: {id1} for mime type: {id2}.",
                            fileType.Id,
                            mimeType.Id
                            );

                        // Create the new file type.
                        var newMessageProperty = await FileTypeManager.CreateAsync(
                            fileType,
                            UserName
                            );
                    }

                    // =======
                    // Step 3: Assume anything else was changed.
                    // =======
                    
                    // If a file type wasn't added, or deleted, assume it was edited.
                    var editedFileTypes = changedMimeType.FileTypes.Except(
                        addedFileTypes,
                        FileTypeEqualityComparer.Instance()
                        ).Except(
                            deletedFileTypes,
                            FileTypeEqualityComparer.Instance()
                            ).ToList();

                    // Were there any?
                    if (editedFileTypes.Any())
                    {
                        // Log what we are about to do.
                        Logger.LogDebug(
                            "Editing {count} file types for mime type: {id}.",
                            editedFileTypes.Count(),
                            mimeType.Id
                            );

                        // Loop through the file types.
                        foreach (var fileType in editedFileTypes)
                        {
                            // Log what we are about to do.
                            Logger.LogDebug(
                                "Updating file type: {id1} for mime type: {id2}.",
                                fileType.Id,
                                mimeType.Id
                                );

                            // Update the file type.
                            await FileTypeManager.UpdateAsync(
                                fileType,
                                UserName
                                );
                        }
                    }
                }

                // Log what we are about to do.
                Logger.LogDebug(
                    "Saving changes to mime type: {id}",
                    changedMimeType.Id
                    );

                // Defer to the manager for the update.
                _ = await MimeTypeManager.UpdateAsync(
                    changedMimeType,
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
                _mimeTypes = await MimeTypeManager.FindAllAsync();
            }
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to edit the mime type!</b> " +
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
