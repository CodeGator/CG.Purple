namespace CG.Purple.Samples.QuickStart.Pages.Home;

/// <summary>
/// This class is the code-behind for the <see cref="PropertyDialog"/> page.
/// </summary>

public partial class PropertyDialog
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
    public MessagePropertyRequest Model { get; set; } = null!;

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
    /// This method closes the dialog.
    /// </summary>
    protected void Cancel()
    {
        MudDialog.Close(DialogResult.Cancel);
    }

    #endregion
}
