
namespace CG.Purple.Host.Pages.Messages;

/// <summary>
/// This class is the code-behind for the <see cref="MessagePropertyDialog"/> page.
/// </summary>
public partial class MessagePropertyDialog
{
    // *******************************************************************
    // Delegates.
    // *******************************************************************

    #region Delegates

    /// <summary>
    /// This delegate formats a property type for display in a dropdown.
    /// </summary>
    Func<PropertyType, string> Converter = p => p?.Name ?? "";

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
    public MessageProperty Model { get; set; } = null!;

    /// <summary>
    /// This property contains the valid property types.
    /// </summary>
    [Parameter]
    public IEnumerable<PropertyType> PropertyTypes { get; set; } = null!;

    /// <summary>
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<MessagePropertyDialog> Logger { get; set; } = null!;

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

    #endregion
}
