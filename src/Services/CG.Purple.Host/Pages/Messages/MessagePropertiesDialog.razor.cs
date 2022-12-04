
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

    /// <summary>
    /// This field indicates the page is busy.
    /// </summary>
    protected bool _isBusy;

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
    /// This property contains the valid provider types.
    /// </summary>
    [Parameter]
    public IEnumerable<ProviderType> ProviderTypes { get; set; } = null!;

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
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<MessagePropertiesDialog> Logger { get; set; } = null!;

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
        try
        {
            // Are we editing the provider for this message?
            if (messageProperty.PropertyType.Name == "Provider")
            {
                // Are we editing a message that's in the pipeline?
                if (messageProperty.Message.MessageState == MessageState.Pending ||
                    messageProperty.Message.MessageState == MessageState.Processing)
                {
                    // Prompt the user.
                    var result = await DialogService.ShowMessageBox(
                        title: "Purple",
                        markupMessage: new MarkupString("<h3 style='color:red'>WAIT" +
                        $"</h3><br /><br />The 'Provider' message property is typically " +
                        "not modified once a message is in the processing pipeline. The " +
                        "consequences of doing so might be error(s) in processing. " +
                        "<br /> <br /> Are you <u><i>sure</i></u> you want to do that?"),
                        noText: "Cancel",
                        options: new DialogOptions()
                        {
                            FullScreen = true
                        });

                    // Did the user cancel?
                    if (result.HasValue && !result.Value)
                    {
                        return; // Nothing more to do.
                    }
                }
            }

            // Filter out any provider types that don't match the message type.
            var filteredProviderTypes = (messageProperty.Message.MessageType == MessageType.Mail 
                ? ProviderTypes.Where(x => x.CanProcessEmails) 
                : ProviderTypes.Where(x => x.CanProcessTexts)
                ).ToList();

            // We clone the message property because anything we do to it,
            //   in the dialog, is difficult to undo without a round trip
            //   to the database, which seems silly. This way, if the
            //   user manipulates the object, via the UI, then cancels
            //   the operation, no harm done.
            var tempMessageProperty = messageProperty.QuickClone();

            // Show the dialog.
            var dialog = await DialogService.ShowEx<MessagePropertyDialog>(
                $"Edit {messageProperty.PropertyType.Name}",
                new DialogParameters()
                {
                    { "Model", tempMessageProperty },
                    { "PropertyTypes", PropertyTypes }
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
                    Position = DialogPosition.CenterRight,
                    DisableSizeMarginY = true,
                    DisablePositionMargin = true
                });

            // Show the dialog.
            var dialogResult = await dialog.Result;

            // Did the user save?
            if (!dialogResult.Cancelled)
            {
                // We're busy.
                _isBusy = true;

                // Give the UI time to show the busy indicator.
                await InvokeAsync(() => StateHasChanged());
                await Task.Delay(250);

                // Keep the changes.
                messageProperty.Value = tempMessageProperty.Value;
                messageProperty.LastUpdatedBy = UserName;
                messageProperty.LastUpdatedOnUtc = DateTime.UtcNow;
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
    /// This method deletes the currently selected message property.
    /// </summary>
    /// <param name="messageProperty">The message property to use for the
    /// operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteMessagePropertyAsync(
        MessageProperty messageProperty
        )
    {
        try
        {
            // Prompt the user.
            var result = await DialogService.ShowMessageBox(
                title: "Purple",
                markupMessage: new MarkupString("This will delete the message " +
                $"property <b>{messageProperty.PropertyType.Name}</b> from " +
                "the message.<br /> <br /> Are you <i>sure</i> you want " +
                "to do that?"),
                noText: "Cancel"
                );

            // Did the user cancel?
            if (result.HasValue && !result.Value)
            {
                return; // Nothing more to do.
            }

            // Are we deleting the provider for this message?
            if (messageProperty.PropertyType.Name == "Provider")
            {
                // Prompt the user, again.
                result = await DialogService.ShowMessageBox(
                    title: "Purple",
                    markupMessage: new MarkupString("<h3 style='color:red'>WAIT" +
                    $"</h3><br /><br />The 'Provider' message property is typically " +
                    "not modified once a message is in the processing pipeline. The " +
                    "consequences of doing so might be error(s) in processing. " +
                    "<br /> <br /> Are you <u><i>sure</i></u> you want to do that?"),
                    noText: "Cancel",
                    options: new DialogOptions()
                    {
                        FullScreen = true
                    });

                // Did the user cancel?
                if (result.HasValue && !result.Value)
                {
                    return; // Nothing more to do.
                }
            }

            // We're busy.
            _isBusy = true;

            // Give the UI time to show the busy indicator.
            await InvokeAsync(() => StateHasChanged());
            await Task.Delay(250);

            // Keep the changes.
            Model.MessageProperties.Remove(
                messageProperty
                );
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to delete the message property!</b> " +
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

            // We remove property types that are already used, on the message,
            //   because we want to avoid duplicate properties.

            // Create a new model.
            var tempMessageProperty = new MessageProperty()
            {
                Message = Model,
                CreatedBy = UserName,
                CreatedOnUtc = DateTime.UtcNow 
            };

            // Show the dialog.
            var dialog = await DialogService.ShowEx<MessagePropertyDialog>(
                "Create Message Property",
                new DialogParameters()
                {
                     { "Model", tempMessageProperty },
                     { "PropertyTypes", filteredPropertyTypes },
                     //{ "ProviderTypes", ProviderTypes }
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
                // We're busy.
                _isBusy = true;

                // Give the UI time to show the busy indicator.
                await InvokeAsync(() => StateHasChanged());
                await Task.Delay(250);

                // Keep the changes.
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
        finally
        {
            // We're no longer busy.
            _isBusy = false;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method indicates whether the model can have any additional 
    /// message properties added to it, or not.
    /// </summary>
    /// <returns><c>true</c> if the model can have additional message 
    /// properties added to it; <c>false</c> otherwise.</returns>
    protected bool CanAddMessageProperty()
    {
        // Return true if there are any property types not currently
        // in use by the message; return false otherwise.

        return PropertyTypes.Except(
            Model.MessageProperties.Select(x => x.PropertyType),
            PropertyTypeEqualityComparer.Instance()
            ).Any();
    }

    #endregion
}
