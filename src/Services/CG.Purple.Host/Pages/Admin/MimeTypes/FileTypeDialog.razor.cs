﻿
namespace CG.Purple.Host.Pages.Admin.MimeTypes;

/// <summary>
/// This class is the code-behind for the <see cref="FileTypeDialog"/> dialog.
/// </summary>
public partial class FileTypeDialog
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
    public FileType Model { get; set; } = null!;

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

    #endregion
}
