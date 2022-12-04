
namespace CG.Purple.Host.Pages.Logs;

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
    protected readonly List<BreadcrumbItem> _crumbs = new()
    {
        new BreadcrumbItem("Home", href: "/"),
        new BreadcrumbItem("Logs", href: "/logs")
    };

    /// <summary>
    /// This field indicates the page is busy.
    /// </summary>
    protected bool _isBusy;

    /// <summary>
    /// This field contains the collection of log entries.
    /// </summary>
    protected IEnumerable<PipelineLog>? _logs = Array.Empty<PipelineLog>();

    /// <summary>
    /// This field contains the collection of provider types.
    /// </summary>
    protected IEnumerable<ProviderType>? _providerTypes = Array.Empty<ProviderType>();

    /// <summary>
    /// This field contains the current search string.
    /// </summary>
    protected string _gridSearchString = "";

    /// <summary>
    /// This field contains the optional state date.
    /// </summary>
    protected DateTime? _startDate = DateTime.UtcNow;

    /// <summary>
    /// This field contains the optional end date.
    /// </summary>
    protected DateTime? _endDate = DateTime.UtcNow;

    /// <summary>
    /// This field contains the optional event type.
    /// </summary>
    protected ProcessEvent? _eventType;

    /// <summary>
    /// This field contains the optional before state.
    /// </summary>
    protected MessageState? _beforeState;

    /// <summary>
    /// This field contains the optional after state.
    /// </summary>
    protected MessageState? _afterState;

    /// <summary>
    /// This field contains the optional provider type.
    /// </summary>
    protected ProviderType? _providerType;

    /// <summary>
    /// This field contains the optional message type.
    /// </summary>
    protected MessageType? _messageType;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the process log manager for this page.
    /// </summary>
    [Inject]
    protected IProcessLogManager ProcessLogManager { get; set; } = null!;

    /// <summary>
    /// This property contains the provider type manager for this page.
    /// </summary>
    [Inject]
    protected IProviderTypeManager ProviderTypeManager { get; set; } = null!;

    /// <summary>
    /// This property contains the snackbar service for this page.
    /// </summary>
    [Inject]
    protected ISnackbar SnackbarService { get; set; } = null!;

    /// <summary>
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<Index> Logger { get; set; } = null!;

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

            // Find the provider types.
            _providerTypes = await ProviderTypeManager.FindAllAsync();

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
    /// This method refreshes the data for the page.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnRefreshAsync()
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
                "Fetching log entries for the page."
                );

            // Get the data.
            _logs = await ProcessLogManager.FindAllAsync();

            // Should we filter by the start date?
            if (_startDate is not null)
            {
                _logs = _logs.Where(x => x.CreatedOnUtc.Date >= _startDate.Value.Date).ToList();
            }

            // Should we filter by the end date?
            if (_endDate is not null)
            {
                _logs = _logs.Where(x => x.CreatedOnUtc.Date <= _endDate.Value.Date).ToList();
            }

            // Should we filter by the event type?
            if (_eventType is not null)
            {
                _logs = _logs.Where(x => x.Event == _eventType).ToList();
            }

            // Should we filter by the before state?
            if (_beforeState is not null)
            {
                _logs = _logs.Where(x => x.BeforeState == _beforeState).ToList();
            }

            // Should we filter by the after state?
            if (_afterState is not null)
            {
                _logs = _logs.Where(x => x.AfterState == _afterState).ToList();
            }

            // Should we filter by provider type?
            if (_providerType is not null)
            {
                _logs = _logs.Where(x => x.ProviderType?.Id == _providerType.Id).ToList();
            }

            // Should we filter by message type?
            if (_messageType is not null)
            {
                _logs = _logs.Where(x => x.Message?.MessageType == _messageType).ToList();
            }
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to refresh the page"
                );

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
    /// This method adapts the <see cref="FilterFunc(PipelineLog, string)"/> method
    /// for use with a <see cref="MudTable{T}"/> control.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc(PipelineLog element) =>
        FilterFunc(element, _gridSearchString);

    // *******************************************************************

    /// <summary>
    /// This method performs a search of the log entries.
    /// </summary>
    /// <param name="element">The element to uses for the operation.</param>
    /// <param name="searchString">The search string to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc(
        PipelineLog element,
        string searchString
        )
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return true;
        }

        if (element.BeforeState is not null)
        {
            var state = Enum.GetName<MessageState>(
                element.BeforeState ?? MessageState.Pending
                ) ?? "";

            if (searchString.Contains(state, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        if (element.AfterState is not null)
        {
            var state = Enum.GetName<MessageState>(
                element.AfterState ?? MessageState.Pending
                ) ?? "";

            if (searchString.Contains(state, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        if (element.ProviderType is not null)
        {
            if (searchString.Contains(element.ProviderType.Name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        var ev = Enum.GetName<ProcessEvent>(element.Event);
        if (searchString.Contains(ev ?? "", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (element.Data is not null)
        {
            if (searchString.Contains(element.Data, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        if (element.Error is not null)
        {
            if (searchString.Contains(element.Error, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        
        //if (element.Message is not null)
        //{
        //    if (searchString.Contains(element.Message.From, StringComparison.OrdinalIgnoreCase))
        //    {
        //        return true;
        //    }

        //    var type = Enum.GetName<MessageType>(element.Message.MessageType);
        //    if (searchString.Contains(type, StringComparison.OrdinalIgnoreCase))
        //    {
        //        return true;
        //    }

        //    if (searchString.Contains(element.Message.MessageKey, StringComparison.OrdinalIgnoreCase))
        //    {
        //        return true;
        //    }

        //    if (searchString.Contains($"{element.Message.Priority}", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return true;
        //    }
        //}

        //if (element.ProviderType is not null)
        //{
        //    if (searchString.Contains(element.Message.From, StringComparison.OrdinalIgnoreCase))
        //    {
        //        return true;
        //    }
        //}

        return false;
    }

    #endregion
}
