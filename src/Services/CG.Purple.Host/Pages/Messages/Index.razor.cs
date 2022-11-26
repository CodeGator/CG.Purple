
using CG.Purple.Managers;

namespace CG.Purple.Host.Pages.Messages;

/// <summary>
/// This class is the code-behind for the <see cref="Index"/> page.
/// </summary>
public partial class Index
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains a reference to breadcrumbs for the view.
    /// </summary>
    private readonly List<BreadcrumbItem> _crumbs = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("Message", href: "/message")
    };

    /// <summary>
    /// This field indicates the page is busy.
    /// </summary>
    private bool _isBusy;

    /// <summary>
    /// This field contains the collection of mail messages.
    /// </summary>
    private IEnumerable<MailMessage>? _mailMessages = Array.Empty<MailMessage>();

    /// <summary>
    /// This field contains the collection of text messages.
    /// </summary>
    private IEnumerable<TextMessage>? _textMessages = Array.Empty<TextMessage>();

    /// <summary>
    /// This field contains the current mail search string.
    /// </summary>
    private string mailGridSearchString = "";

    /// <summary>
    /// This field contains the current text search string.
    /// </summary>
    private string textGridSearchString = "";

    /// <summary>
    /// This field contains the time until the next page update.
    /// </summary>
    private TimeSpan _timeTillNextUpdate = TimeSpan.FromSeconds(30);

    /// <summary>
    /// This field contains the timer for the page refresh operations.
    /// </summary>
    private Timer _timer = null!;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the mail message manager for this page.
    /// </summary>
    [Inject]
    protected IMailMessageManager MailManager { get; set; } = null!;

    /// <summary>
    /// This property contains the text message manager for this page.
    /// </summary>
    [Inject]
    protected ITextMessageManager TextManager { get; set; } = null!;

    /// <summary>
    /// This property contains the message manager for this page.
    /// </summary>
    [Inject]
    protected IMessageManager MessageManager { get; set; } = null!;

    /// <summary>
    /// This property contains the property type manager for this page.
    /// </summary>
    [Inject]
    protected IPropertyTypeManager PropertyTypeManager { get; set; } = null!;

    /// <summary>
    /// This property contains the provider type manager for this page.
    /// </summary>
    [Inject]
    protected IProviderTypeManager ProviderTypeManager { get; set; } = null!;

    /// <summary>
    /// This property contains the message property manager for this page.
    /// </summary>
    [Inject]
    protected IMessagePropertyManager MessagePropertyManager { get; set; } = null!;

    /// <summary>
    /// This property contains the process log manager for this page.
    /// </summary>
    [Inject]
    protected IProcessLogManager ProcessLogManager { get; set; } = null!;

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
    protected ILogger<Index> Logger { get; set; } = null!;

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
    /// This method is called by the framework to initialize the page.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page to busy."
                );

            // We're busy.
            _isBusy = true;

            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page state to dirty."
                );

            // Give the UI time to show the busy indicator.
            await InvokeAsync(() => StateHasChanged());
            await Task.Delay(250);

            // Log what we are about to do.
            Logger.LogDebug(
                "Fetching messages for the page."
                );

            // Fetch the messages.
            _mailMessages = await MailManager.FindAllAsync();
            _textMessages = await TextManager.FindAllAsync();

            // Log what we are about to do.
            Logger.LogDebug(
                "Starting the auto-refresh timer."
                );

            // Start the refresh timer.
            _timer = new Timer(
                _TimerCallback,
                this,
                1000,
                1000
                );

            // Give the base class a chance.
            await base.OnInitializedAsync();
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
            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page to not busy."
                );

            // We're no longer busy.
            _isBusy = false;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method adapts the <see cref="MailFilterFunc(MailMessage, string)"/> method
    /// for use with a <see cref="MudTable{T}"/> control.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool MailFilterFunc1(MailMessage element) => 
        MailFilterFunc(element, mailGridSearchString);

    // *******************************************************************

    /// <summary>
    /// This method adapts the <see cref="TextFilterFunc(TextMessage, string)"/> method
    /// for use with a <see cref="MudTable{T}"/> control.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool TextFilterFunc1(TextMessage element) => 
        TextFilterFunc(element, textGridSearchString);

    // *******************************************************************

    /// <summary>
    /// This method performs a search of the mail messages.
    /// </summary>
    /// <param name="element">The element to uses for the operation.</param>
    /// <param name="searchString">The search string to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool MailFilterFunc(
        MailMessage element,
        string searchString
        )
    {
        // How should we filter?
        if (searchString.Contains(':'))
        {
            // If we get here then we might need to filter by property types.

            // Should be: property type:value
            var parts = searchString.Split(':');
            if (parts.Length == 2)
            {
                // If we get here then we need to filter by property types.

                // Look for a matching property.
                var match = element.MessageProperties.FirstOrDefault(x => 
                    x.PropertyType.Name == parts[0]
                    );

                // Did we find one?
                if (match is not null)
                {
                    return match.Value.Contains(
                        parts[1],
                        StringComparison.OrdinalIgnoreCase
                        );
                }
            }
        }

        // If we get here then we should filter on all properties.

        if (string.IsNullOrWhiteSpace(searchString))
        {
            return true;
        }
        if (element.To.Contains(
            searchString,
            StringComparison.OrdinalIgnoreCase)
            )
        {
            return true;
        }
        if (!string.IsNullOrEmpty(element.CC))
        {
            if (element.CC.Contains(
                searchString,
                StringComparison.OrdinalIgnoreCase)
                )
            {
                return true;
            }
        }
        if (!string.IsNullOrEmpty(element.BCC))
        {
            if (element.BCC.Contains(
                searchString,
                StringComparison.OrdinalIgnoreCase)
                )
            {
                return true;
            }
        }
        if (!string.IsNullOrEmpty(element.Subject))
        {
            if (element.Subject.Contains(
                searchString,
                StringComparison.OrdinalIgnoreCase)
                )
            {
                return true;
            }
        }
        if ((Enum.GetName<MessageState>(element.MessageState) ?? "")
            .Contains(searchString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        if (element.Body.Contains(
            searchString,
            StringComparison.OrdinalIgnoreCase)
            )
        {
            return true;
        }
        return false;
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a search of the text messages.
    /// </summary>
    /// <param name="element">The element to uses for the operation.</param>
    /// <param name="searchString">The search string to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool TextFilterFunc(
        TextMessage element,
        string searchString
        )
    {
        // How should we filter?
        if (searchString.Contains(':'))
        {
            // If we get here then we might need to filter by property types.

            // Should be: property type:value
            var parts = searchString.Split(':');
            if (parts.Length == 2)
            {
                // If we get here then we need to filter by property types.

                // Look for a matching property.
                var match = element.MessageProperties.FirstOrDefault(x =>
                    x.PropertyType.Name == parts[0]
                    );

                // Did we find one?
                if (match is not null)
                {
                    return match.Value.Contains(
                        parts[1],
                        StringComparison.OrdinalIgnoreCase
                        );
                }
            }
        }

        // If we get here then we should filter on all properties.

        if (string.IsNullOrWhiteSpace(searchString))
        {
            return true;
        }
        if (element.To.Contains(
            searchString, 
            StringComparison.OrdinalIgnoreCase)
            )
        {
            return true;
        }
        if ((Enum.GetName<MessageState>(element.MessageState) ?? "")
            .Contains(searchString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        if (element.Body.Contains(
            searchString, 
            StringComparison.OrdinalIgnoreCase)
            )
        {
            return true;
        }
        return false;
    }

    // *******************************************************************

    /// <summary>
    /// This method refreshes all the data sources for the page.
    /// </summary>
    protected async Task OnRefreshPageAsync()
    {
        await OnRefreshMailMessagesAsync();
        await OnRefreshTextMessagesAsync();
    }

    // *******************************************************************

    /// <summary>
    /// This method manually refreshes the mail messages collection.
    /// </summary>
    protected async Task OnRefreshMailMessagesAsync()
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page to busy."
                );

            // We're busy.
            _isBusy = true;

            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page state to dirty."
                );

            // Give the UI time to show the busy indicator.
            await InvokeAsync(() => StateHasChanged());
            await Task.Delay(250);

            // Log what we are about to do.
            Logger.LogDebug(
                "Fetching emails for the page."
                );

            // Fetch the messages.
            _mailMessages = await MailManager.FindAllAsync();

            // Log what we are about to do.
            Logger.LogDebug(
                "Updating the time until the next refresh."
                );

            // Reset the value.
            _timeTillNextUpdate = TimeSpan.FromSeconds(30);

            // Give the base class a chance.
            await base.OnInitializedAsync();
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
            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page to not busy."
                );

            // We're no longer busy.
            _isBusy = false;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method manually refreshes the text messages collection.
    /// </summary>
    protected async Task OnRefreshTextMessagesAsync()
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page to busy."
                );

            // We're busy.
            _isBusy = true;

            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page state to dirty."
                );

            // Give the UI time to show the busy indicator.
            await InvokeAsync(() => StateHasChanged());
            await Task.Delay(250);

            // Log what we are about to do.
            Logger.LogDebug(
                "Fetching texts for the page."
                );

            // Fetch the messages.
            _textMessages = await TextManager.FindAllAsync();

            // Log what we are about to do.
            Logger.LogDebug(
                "Updating the time until the next refresh."
                );

            // Reset the value.
            _timeTillNextUpdate = TimeSpan.FromSeconds(30);

            // Give the base class a chance.
            await base.OnInitializedAsync();
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
            // Log what we are about to do.
            Logger.LogDebug(
                "Setting the page to not busy."
                );

            // We're no longer busy.
            _isBusy = false;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method displays a dialog for the message log.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnLogsAsync(
        Message message
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Looking for logs associated with message: {id}",
                message.Id
                );

            // Find any associated logs.
            var logs = await ProcessLogManager.FindByMessageAsync(
                message
                );

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating the log dialog."
                );

            // Show the dialog.
            var dialog = await DialogService.ShowEx<LogDialog>(
                "Log", new DialogParameters()
                {
                    { "Model", logs },
                },
                new DialogOptionsEx()
                {
                    MaximizeButton = true,
                    CloseButton = true,
                    CloseOnEscapeKey = true,
                    MaxWidth = MaxWidth.ExtraLarge,
                    FullWidth = true,
                    DragMode = MudDialogDragMode.Simple,
                    Animations = new[] { AnimationType.SlideIn },
                    Position = DialogPosition.CenterRight,
                    DisableSizeMarginY = true,
                    DisablePositionMargin = true
                });

            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the log dialog."
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

    // *******************************************************************

    /// <summary>
    /// This method displays a dialog for the message attachments.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnAttachmentsAsync(
        Message message
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Creating the attachments dialog."
                );

            // Show the dialog.
            var dialog = await DialogService.ShowEx<AttachmentsDialog>(
                "Attachment", new DialogParameters() 
                { 
                    { "Model", message }, 
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

            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the attachments dialog."
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

    // *******************************************************************

    /// <summary>
    /// This method toggles the enabled/disabled state for the given message.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnToggleDisableAsync(
        Message message
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Toggling the disabled state for message: {id}.",
                message.Id
                );

            // Toggle the disabled state.
            if (message.IsDisabled)
            {
                message.IsDisabled = false;
            }
            else
            {
                message.IsDisabled = true;
            }

            // Log what we are about to do.
            Logger.LogDebug(
                "Saving changes to message: {id}.",
                message.Id
                );

            // Update the storage.
            await MessageManager.UpdateAsync(
                message,
                UserName
                );
        }
        finally
        {
            // Update the page.
            await OnRefreshPageAsync();
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method displays a dialog for the message properties.
    /// </summary>
    /// <param name="message">The message to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnPropertiesAsync(
        Message message
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Looking for provider types."
                );

            // Get the valid provider types.
            var providerTypes = await ProviderTypeManager.FindAllAsync();

            // Log what we are about to do.
            Logger.LogDebug(
                "Looking for property types."
                );

            // Get the valid property types.
            var propertyTypes = await PropertyTypeManager.FindAllAsync();

            // Log what we are about to do.
            Logger.LogDebug(
                "Removing used property types."
                );

            // We remove property types that are already used, on the message,
            //   because we want to avoid duplicate properties.

            // Remove any property types already used by the message.
            var filteredPropertyTypes = propertyTypes.Except(
                message.MessageProperties.Select(x => x.PropertyType),
                PropertyTypeEqualityComparer.Instance()
                ).ToList();

            // Log what we are about to do.
            Logger.LogDebug(
                "Cloning the message."
                );

            // We clone the message because anything we do to it, in
            //   the dialog, is difficult to undo without a round trip
            //   to the database, which seems silly. This way, if the
            //   user manipulates the object, via the UI, then cancels
            //   the operation, no harm done.
            var tempMessage = message.QuickClone();

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating the message properties dialog."
                );

            // Show the dialog.
            var dialog = await DialogService.ShowEx<MessagePropertiesDialog>(
                "Message Properties", new DialogParameters() 
                { 
                    { "Model", tempMessage }, 
                    { "PropertyTypes", filteredPropertyTypes } ,
                    { "ProviderTypes", providerTypes }
                }, 
                new DialogOptionsEx() 
                { 
                    MaximizeButton = true, 
                    CloseButton = true, 
                    CloseOnEscapeKey = true, 
                    MaxWidth = MaxWidth.Medium, 
                    FullWidth = true, 
                    DragMode = MudDialogDragMode.Simple, 
                    Animations = new[] { AnimationType.SlideIn }, 
                    Position = DialogPosition.CenterRight, 
                    DisableSizeMarginY = true, 
                    DisablePositionMargin = true 
                });

            // Log what we are about to do.
            Logger.LogDebug(
                "Showing the message properties dialog."
                );

            // Show the dialog.
            var result = await dialog.Result;

            // Did the user save?
            if (!result.Cancelled)
            {
                // Log what we are about to do.
                Logger.LogDebug(
                    "Setting the page to busy."
                    );

                // We're busy.
                _isBusy = true;

                // Log what we are about to do.
                Logger.LogDebug(
                    "Setting the page state to dirty."
                    );

                // Give the UI time to show the busy indicator.
                await InvokeAsync(() => StateHasChanged());
                await Task.Delay(250);

                // Log what we are about to do.
                Logger.LogDebug(
                    "Recovering the edited message."
                    );

                // Recover the edited message.
                var changedMessage = (Message)result.Data;

                // =======
                // Step 1: Find any message properties that were deleted.
                // =======

                // Log what we are about to do.
                Logger.LogDebug(
                    "Looking for deleted message properties."
                    );

                // Find any properties that were deleted.
                var deletedProperties = message.MessageProperties.Except(
                    changedMessage.MessageProperties,
                    MessagePropertyEqualityComparer.Instance()
                    );

                // Were there any?
                if (deletedProperties.Any())
                {
                    // Log what we are about to do.
                    Logger.LogDebug(
                        "Deleting {count} message properties on message: {id}.",
                        deletedProperties.Count(),
                        message.Id
                        );

                    // Loop through the message properties.
                    foreach (var property in deletedProperties)
                    {
                        // Log what we are about to do.
                        Logger.LogDebug(
                            "Deleting property type: {id1} for message: {id2}.",
                            property.PropertyType.Id,
                            property.Message.Id
                            );

                        // Delete the message property.
                        await MessagePropertyManager.DeleteAsync(
                            property,
                            UserName
                            );
                    }
                }

                // =======
                // Step 2: Find any message properties that were added.
                // =======

                // Find any properties that were added.
                var addedProperties = changedMessage.MessageProperties.Except(
                    message.MessageProperties,
                    MessagePropertyEqualityComparer.Instance()
                    );

                // Were there any?
                if (addedProperties.Any())
                {
                    // Log what we are about to do.
                    Logger.LogDebug(
                        "Adding {count} message properties to message: {id}.",
                        addedProperties.Count(),
                        message.Id
                        );

                    // Loop through the message properties.
                    foreach (var property in addedProperties)
                    {
                        try
                        {
                            // Log what we are about to do.
                            Logger.LogDebug(
                                "Adding property type: {id1} for message: {id2}.",
                                property.PropertyType.Id,
                                property.Message.Id
                                );

                            // Create the new message property.
                            var newMessageProperty = await MessagePropertyManager.CreateAsync(
                                property,
                                UserName
                                );
                        }
                        catch (Exception ex)
                        {
                            // Log what we are about to do.
                            Logger.LogDebug(
                                "Failed to update message property: {id1} for message: {id2}! Error: {err}",
                                property.PropertyType.Id,
                                property.Message.Id,
                                ex.GetBaseException().Message
                                );

                            // Ignore duplicates - since that probably means the
                            //   hosted service added a conflicting message property,
                            //   in the background.
                            if (!ex.GetBaseException().Message.Contains("duplicate"))
                            {
                                throw;
                            }
                        }
                    }
                }

                // =======
                // Step 3: Assume all message properties were changed.
                // =======

                // If a property wasn't added, or deleted, assume it was edited.
                var editedProperties = changedMessage.MessageProperties.Except(
                    addedProperties,
                    MessagePropertyEqualityComparer.Instance()
                    ).Except(
                        deletedProperties,
                        MessagePropertyEqualityComparer.Instance()
                        ).ToList();

                // Were there any?
                if (editedProperties.Any())
                {
                    // Log what we are about to do.
                    Logger.LogDebug(
                        "Editing {count} message properties for message: {id}.",
                        editedProperties.Count(),
                        message.Id
                        );

                    // Loop through the message properties.
                    foreach (var property in changedMessage.MessageProperties)
                    {
                        // Log what we are about to do.
                        Logger.LogDebug(
                            "Updating message property: {id1} for message: {id2}.",
                            property.PropertyType.Id,
                            property.Message.Id
                            );

                        // Update the message property.
                        await MessagePropertyManager.UpdateAsync(
                            property,
                            UserName
                            );
                    }
                }

                // Log what we are about to do.
                Logger.LogDebug(
                    "Showing the snackbar message."
                    );

                // Tell the world what happened.
                SnackbarService.Add(
                    $"Changes were saved",
                    Severity.Success,
                    options => options.CloseAfterNavigation = true
                    );

                // Log what we are about to do.
                Logger.LogDebug(
                    "Refreshing the page."
                    );

                // Refresh the page.
                await OnRefreshMailMessagesAsync();
                await OnRefreshTextMessagesAsync();
            }
        }
        catch ( Exception ex )
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
    /// This method is called by the timer, to periodically refresh the page.
    /// </summary>
    /// <param name="state">The optional state for the operation.</param>
    protected async void _TimerCallback(
        object? state
        )
    {
        try
        {
            // Is it time to refresh the page?
            if (_timeTillNextUpdate <= TimeSpan.Zero)
            {
                // Refresh the page.
                await OnRefreshMailMessagesAsync();
                await OnRefreshTextMessagesAsync();
            }
            else
            {
                // Decrement the value.
                _timeTillNextUpdate = _timeTillNextUpdate.Subtract(
                    TimeSpan.FromSeconds(1)
                    );
            }
        }
        finally
        {
            // Ensure the countdown part of the page refreshes.
            await InvokeAsync(() => StateHasChanged());
        }
    }

    #endregion
}
