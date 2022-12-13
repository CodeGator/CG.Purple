
using static System.Net.Mime.MediaTypeNames;

namespace CG.Purple.Samples.QuickStart.Pages.Home;

/// <summary>
/// This class is the code-behind for the <see cref="AttachmentsDialog"/> page.
/// </summary>
public partial class AttachmentsDialog
{
    // *******************************************************************
    // Constants.
    // *******************************************************************

    #region Constants

    /// <summary>
    /// This constant represents the default drag CSS style.
    /// </summary>
    private static string _defaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";

    #endregion

    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field indicates the page is busy.
    /// </summary>
    private bool _isBusy;

    /// <summary>
    /// This field indicates the user is clearing the file collection.
    /// </summary>
    private bool _clearing = false;

    /// <summary>
    /// This field contains the current drag CSS style.
    /// </summary>
    private string _dragClass = _defaultDragClass;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the MudDialog instance.
    /// </summary>
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// This property contains the model for the dialog.
    /// </summary>
    [Parameter]
    public List<AttachmentRequest> Model { get; set; } = null!;

    /// <summary>
    /// This property contains the dialog service for the page.
    /// </summary>
    [Inject]
    protected IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// This property contains the snackbar service for the page.
    /// </summary>
    [Inject]
    protected ISnackbar SnackbarService { get; set; } = null!;

    /// <summary>
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<AttachmentsDialog> Logger { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method closes the dialog.
    /// </summary>
    protected void Ok()
    {
        MudDialog.Close(DialogResult.Ok(Model));
    }

    // *******************************************************************

    /// <summary>
    /// This method closes the dialog.
    /// </summary>
    protected void Cancel()
    {
        MudDialog.Close(DialogResult.Cancel);
    }

    // *******************************************************************

    /// <summary>
    /// This method creates (imports) a new file.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnCreateAsync()
    {
        try
        {

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
    /// This method is called when a file as added to the model.
    /// </summary>
    /// <param name="e">The arguments for the event.</param>
    protected async Task OnInputFileChanged(InputFileChangeEventArgs e)
    {
        try
        {
            // We're busy.
            _isBusy = true;

            // Give the UI time to show the busy indicator.
            await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
            await Task.Delay(250);

            // Clear the drag CSS style.
            ClearDragClass();

            // Loop through the files.
            foreach (var file in e.GetMultipleFiles())
            {
                // Ensure we don't bother with empty files.
                if (file.Size == 0)
                {
                    // Tell the world what happened.
                    SnackbarService.Add(
                        $"<ul><li>The file: {file.Name} appears to be empty.</li></ul>",
                        Severity.Warning,
                        options => options.CloseAfterNavigation = true
                        );
                    continue;
                }

                // Ensure we don't upload ginormous files.
                if (file.Size > 512000)
                {
                    // Tell the world what happened.
                    SnackbarService.Add(
                        $"<ul><li>The file: {file.Name} is too large for processing.</li> " +
                        "<li>Please add files less than 512KB in size.</li></ul>",
                        Severity.Warning,
                        options => options.CloseAfterNavigation = true
                        );
                    continue;
                }

                // Read the file's content now because, if we're dropping files
                //   one a time, the first IBrowserFile becomes invalid when
                //   the second files is dropped. If we read the file here, we
                //   can sidestep that little problem.
                using var stream = file.OpenReadStream();
                var bytes = new byte[file.Size];
                await stream.ReadAsync(bytes, 0, bytes.Length);

                // Add the file to the list of pending uploads.
                Model.Add(new AttachmentRequest()
                {
                    MimeType = file.ContentType,
                    FileName = file.Name,
                    Length = bytes.Length,  
                    Data = Convert.ToBase64String(bytes)
                });
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
        finally
        {
            // We're no longer busy.
            _isBusy = false;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method is called when the list of files is cleared.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task Clear()
    {
        try
        {
            // We're clearing the pending files.
            _clearing = true;

            // Clear the list of files.
            Model.Clear();

            // Clear any old CSS styles.
            ClearDragClass();

            // Tell Blazor to update.
            await InvokeAsync(() => StateHasChanged());
        }
        finally
        {
            // We're done.
            _clearing = false;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method is called when a chip is clicked, to remove a file.
    /// </summary>
    /// <param name="attachmentRequest">The attachment request to use
    /// for the operation.</param>
    protected async Task OnChipClickAsync(
        AttachmentRequest attachmentRequest
        )
    {
        try
        {
            // Prompt the user.
            var result = await DialogService.ShowMessageBox(
                title: "Purple QuickStart",
                markupMessage: new MarkupString("This will delete the attachment " +
                $"<b>{attachmentRequest.FileName}</b> <br /> <br /> Are you " +
                "<i>sure</i> you want to do that?"),
                noText: "Cancel"
                );

            // Did the user cancel?
            if (result.HasValue && !result.Value)
            {
                return; // Nothing more to do.
            }

            // We're busy.
            _isBusy = true;

            // Give the UI time to show the busy indicator.
            await InvokeAsync(() => StateHasChanged());
            await Task.Delay(250);

            // Update the model.
            Model.Remove(attachmentRequest);
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to delete the application!</b> " +
                $"<ul><li>{ex.Message}</li></ul>",
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
    /// This method is called to set the CSS style for drag operations.
    /// </summary>
    protected void SetDragClass()
    {
        _dragClass = $"{_defaultDragClass} mud-border-primary";
    }

    // *******************************************************************

    /// <summary>
    /// This method is called to clear the CSS style for drag operations.
    /// </summary>
    protected void ClearDragClass()
    {
        _dragClass = _defaultDragClass;
    }

    #endregion
}
