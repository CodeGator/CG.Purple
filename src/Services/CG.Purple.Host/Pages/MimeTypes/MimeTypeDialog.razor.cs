
namespace CG.Purple.Host.Pages.MimeTypes;

/// <summary>
/// This class is the code-behind for the <see cref="MimeTypeDialog"/> dialog.
/// </summary>
public partial class MimeTypeDialog
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the dialog reference.
    /// </summary>
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// This property contains the edit form's model.
    /// </summary>
    [Parameter]
    public MimeType Model { get; set; } = null!;

    /// <summary>
    /// This property contains the snackbar service for this page.
    /// </summary>
    [Inject]
    protected ISnackbar SnackbarService { get; set; } = null!;

    /// <summary>
    /// This property contains the dialog service for the dialog.
    /// </summary>
    [Inject]
    protected IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// This property contains the HTTP context accessor.
    /// </summary>
    [Inject]
    protected IHttpContextAccessor HttpContextAccessor { get; set; } = null!;

    /// <summary>
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<MimeTypeDialog> Logger { get; set; } = null!;

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
    /// This method submits the dialog.
    /// </summary>
    protected void OnValidSubmit()
    {
        MudDialog.Close(DialogResult.Ok(Model));
    }

    // *******************************************************************

    /// <summary>
    /// This method cancels the dialog.
    /// </summary>
    protected void Cancel() => MudDialog.Cancel();

    // *******************************************************************

    /// <summary>
    /// This method creates a file type.
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
                { "Model", new FileType()
                {
                    MimeType = Model,
                    CreatedBy = UserName,
                    CreatedOnUtc = DateTime.UtcNow,
                } }
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating the file type dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<FileTypeDialog>(
                "Edit File Type",
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
                    "Recovering the modified file type."
                    );

                // Recover the edited file type.
                var changedFileType = (FileType)result.Data;

                // Log what we are about to do.
                Logger.LogDebug(
                    "Saving the changes."
                    );

                // Save the changes.
                Model.FileTypes.Add(
                    changedFileType
                    );
            }
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to create a file type for mime type: {id}",
                Model.Id
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to create the file type!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method deletes the given file type.
    /// </summary>
    /// <param name="fileType">The file type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteAsync(
        FileType fileType
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
                markupMessage: new MarkupString("This will delete the file " +
                $"type with extension <b>{fileType.Extension}</b> <br /> " +
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
                "Saving the changes."
                );

            // Save the changes.
            Model.FileTypes.Remove(fileType);

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
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to delete a file type for mime type: {id}",
                Model.Id
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to delete the file type!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method edits the given file type.
    /// </summary>
    /// <param name="fileType">The file type to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnEditAsync(
        FileType fileType
        )
    {
        try
        {
            // Create the dialog options.
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true
            };

            // We clone the file type because anything we do to it, in
            //   the dialog is difficult to undo without a round trip
            //   to the database, which seems silly. This way, if the
            //   user manipulates the object, via the UI, then cancels
            //   the operation, no harm done.
            var tempFileType = fileType.QuickClone();

            // Create the dialog parameters.
            var parameters = new DialogParameters()
            {
                { "Model", tempFileType }
            };

            // Create the dialog.
            var dialog = DialogService.Show<FileTypeDialog>(
                "Edit File Type",
                parameters,
                options
                );

            // Get the results of the dialog.
            var results = await dialog.Result;

            // Did the user save?
            if (!results.Cancelled)
            {
                // Save the changes
                if (!tempFileType.Extension.Trim().StartsWith('.'))
                {
                    fileType.Extension = $".{tempFileType.Extension.Trim()}";
                }
                else
                {
                    fileType.Extension = tempFileType.Extension.Trim();
                }
            }
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to edit a file type for mime type: {id}",
                Model.Id
                );
                        
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to edit the file type!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    #endregion
}
