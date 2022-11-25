
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

    /// <summary>
    /// This property contains the valid property types.
    /// </summary>
    [Parameter]
    public IEnumerable<PropertyType> PropertyTypes { get; set; } = null!;

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
        try
        {
            // Remove any property types already used by the message.
            var filteredPropertyTypes = PropertyTypes.Except(
                Model.MessageProperties.Select(x => x.PropertyType),
                PropertyTypeEqualityComparer.Instance()
                ).ToList();

            // Create a new model.
            var tempMessageProperty = new MessageProperty()
            {
                Message = Model,
                CreatedBy = UserName,
                CreatedOnUtc = DateTime.UtcNow 
            };

            // Show the dialog.
            var dialog = await DialogService.ShowEx<MessagePropertyDialog>(
                "Properties",
                new DialogParameters()
                {
                     { "Model", tempMessageProperty },
                     { "PropertyTypes", filteredPropertyTypes }
                },
                new DialogOptionsEx()
                {
                    MaximizeButton = true,
                    CloseButton = true,
                    CloseOnEscapeKey = true,
                    MaxWidth = MaxWidth.Small,
                    FullWidth = true,
                    DragMode = MudDialogDragMode.Simple,
                    Animations = new[] { AnimationType.SlideIn },
                    Position = DialogPosition.Center,
                    DisableSizeMarginY = true,
                    DisablePositionMargin = true
                });

            // Show the dialog.
            var result = await dialog.Result;

            // Did the user save?
            if (!result.Cancelled)
            {
                // Add the message property to the message.
                Model.MessageProperties.Add(
                    tempMessageProperty
                    );
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
    }

    #endregion
}
