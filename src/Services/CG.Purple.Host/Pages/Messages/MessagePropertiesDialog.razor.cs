
using Microsoft.AspNetCore.Components.Forms;

namespace CG.Purple.Host.Pages.Messages;

/// <summary>
/// This class is the code-behind for the <see cref="MessagePropertiesDialog"/> page.
/// </summary>
public partial class MessagePropertiesDialog
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

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
    public Message Model { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is called when the form submits with valid data.
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
    /// This method edits the currently selected message property.
    /// </summary>
    /// <param name="messageProperty">The message property to use for the
    /// operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnEditMessagePropertyAsync(
        MessageProperty messageProperty
        )
    {

    }

    // *******************************************************************

    /// <summary>
    /// This method deletes the currently selected message property.
    /// </summary>
    /// <param name="messageProperty">The message property to use for the
    /// operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteMessagePropertyAsync(
        MessageProperty messageProperty
        )
    {

    }

    // *******************************************************************

    /// <summary>
    /// This method creates a new message property.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnCreateMessagePropertyAsync()
    {

    }

    #endregion
}
