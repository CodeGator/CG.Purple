namespace CG.Purple.Host.Pages.Messages;

/// <summary>
/// This class is the code-behind for the <see cref="MailPreviewDialog"/> page.
/// </summary>

public partial class MailPreviewDialog
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
    public MailMessage Model { get; set; } = null!;

    /// <summary>
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<MailPreviewDialog> Logger { get; set; } = null!;

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

    #endregion

}
