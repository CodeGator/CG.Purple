namespace CG.Purple.Host.Pages.Admin.ProviderTypes;

/// <summary>
/// This class is the code-behind for the <see cref="ProviderParameterDialog"/> page.
/// </summary>
public partial class ProviderParameterDialog
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field indicates the type of the password control.
    /// </summary>
    private InputType _passwordInput = InputType.Password;

    /// <summary>
    /// This field contains the icon for the password control.
    /// </summary>
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    #endregion

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
    public ProviderParameter Model { get; set; } = null!;

    /// <summary>
    /// This property contains the list of valid parameter types.
    /// </summary>
    [Parameter]
    public IEnumerable<ParameterType> ParameterTypes { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Delegates.
    // *******************************************************************

    #region Delegates

    /// <summary>
    /// This delegate formats a parameter type for display in a dropdown.
    /// </summary>
    readonly Func<ParameterType, string> ParameterTypeConverter = p => p?.Name ?? "";
        
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
    /// This method toggles the password control between secure and not secure.
    /// </summary>
    void TogglePasswordVisibility()
    {
        if (_passwordInput != InputType.Password)
        {
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }

    #endregion
}
