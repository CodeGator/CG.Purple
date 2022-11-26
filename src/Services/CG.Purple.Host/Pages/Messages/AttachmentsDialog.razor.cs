
namespace CG.Purple.Host.Pages.Messages;

/// <summary>
/// This class is the code-behind for the <see cref="AttachmentsDialog"/> page.
/// </summary>
public partial class AttachmentsDialog
{
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
    public Message Model { get; set; } = null!;

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
    /// This method displays a preview of the attachment.
    /// </summary>
    /// <param name="attachment">The attachment to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnPreviewAsync(
        Attachment attachment
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Creating the attachment preview dialog."
                );

            // Create the dialog.
            var dialog = await DialogService.ShowFileDisplayDialog(
                browserFile: new AttachmentBrowserFile(attachment)
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the attachment preview dialog."
                );

            // Show the dialog.
            await dialog.Result;
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
