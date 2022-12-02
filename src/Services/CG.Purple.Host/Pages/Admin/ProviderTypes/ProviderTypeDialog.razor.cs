
using CG.Purple.Managers;
using CG.Purple.Models;

namespace CG.Purple.Host.Pages.Admin.ProviderTypes;

/// <summary>
/// This class is the code-behind for the <see cref="ProviderTypeDialog"/> page.
/// </summary>
public partial class ProviderTypeDialog
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
    /// This property contains the dialog reference.
    /// </summary>
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// This property contains the edit form's model.
    /// </summary>
    [Parameter]
    public ProviderType Model { get; set; } = null!;

    /// <summary>
    /// This property contains the list of valid parameter types.
    /// </summary>
    [Parameter]
    public IEnumerable<ParameterType> ParameterTypes { get; set; } = null!;

    /// <summary>
    /// This property contains the valid factory types.
    /// </summary>
    [Parameter]
    public IEnumerable<string> FactoryTypes { get; set; } = null!;

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
    /// This property contains the provider parameter manager for this page.
    /// </summary>
    [Inject]
    protected IProviderParameterManager ProviderParameterManager { get; set; } = null!;

    /// <summary>
    /// This property contains the HTTP context accessor.
    /// </summary>
    [Inject]
    protected IHttpContextAccessor HttpContextAccessor { get; set; } = null!;

    /// <summary>
    /// This property contains the logger for this page.
    /// </summary>
    [Inject]
    protected ILogger<ProviderTypeDialog> Logger { get; set; } = null!;

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
    /// This method add a new provider parameter.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnCreateAsync()
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog options."
                );

            // Create the dialog options.
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog properties."
                );

            // Create the dialog properties.
            var properties = new DialogParameters()
            {
                { 
                    "Model", new ProviderParameter()
                    {
                        ProviderType = Model,
                        CreatedBy = UserName,
                        CreatedOnUtc = DateTime.UtcNow,
                    }
                },
                {
                    "ParameterTypes", ParameterTypes
                }
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating provider parameter dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<ProviderParameterDialog>(
                "Provider Parameter",
                properties,
                options
                );

            // Get the results of the dialog.
            var result = await dialog.Result;

            // Did the user cancel?
            if (result.Cancelled)
            {
                return; // Nothing more to do.
            }

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
                "Recovering the modified provider parameter."
                );

            // Recover the edited provider parameter.
            var changedProviderParameter = (ProviderParameter)result.Data;

            // Log what we are about to do.
            Logger.LogDebug(
                "Saving the changes to the database."
            );

            // Save the changes.
            changedProviderParameter = await ProviderParameterManager.CreateAsync(
                changedProviderParameter,
                UserName
                ).ConfigureAwait(false);

            // Update the local copy.
            Model.Parameters.Add(changedProviderParameter);

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
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to create a provider parameter."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to create provider parameter!</b> " +
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
    /// This method deletes the given provider parameter.
    /// </summary>
    /// <param name="parameter">The provider parameter to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteAsync(
        ProviderParameter parameter
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Prompting the caller."
                );

            // Prompt the user.
            var result = await DialogService.ShowMessageBox(
                title: "Purple",
                markupMessage: new MarkupString("This will delete the parameter " +
                $"type <b>{parameter.ParameterType.Name}</b> from the provider. " +
                "<br /> <br /> Are you <i>sure</i> you want to do that?"),
                noText: "Cancel"
                );

            // Did the user cancel?
            if (result.HasValue && !result.Value)
            {
                return; // Nothing more to do.
            }

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
                "Saving the changes."
                );

            // Defer to the manager for the delete.
            await ProviderParameterManager.DeleteAsync(
                parameter,
                UserName
                );

            // Update the local copy.
            Model.Parameters.Remove(parameter);

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
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to delete a parameter for provider type: {id}",
                Model.Id
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to delete the provider parameter!</b> " +
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
    /// This method edits the given provider parameter.
    /// </summary>
    /// <param name="parameter">The provider parameter to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnEditAsync(
        ProviderParameter parameter
        )
    {
        try
        {
            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog options."
                );

            // Create the dialog options.
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Filtering the currently used parameter type."
                );

            // Add the currently selected parameter type to the
            //  available options.
            var filteredParameterTypes = ParameterTypes.ToList();
            filteredParameterTypes.Add(parameter.ParameterType);

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating dialog properties."
                );

            // Create the dialog properties.
            var properties = new DialogParameters()
            {
                { "Model", parameter },
                { "ParameterTypes", filteredParameterTypes }
            };

            // Log what we are about to do.
            Logger.LogDebug(
                "Creating provider parameter dialog."
                );

            // Create the dialog.
            var dialog = DialogService.Show<ProviderParameterDialog>(
                "Edit Parameter",
                properties,
                options
                );

            // Get the results of the dialog.
            var result = await dialog.Result;

            // Did the user cancel?
            if (result.Cancelled)
            {
                return;
            }

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
                "Recovering the modified provider parameter."
                );

            // Recover the edited provider parameter.
            var changedProviderParameter = (ProviderParameter)result.Data;

            // Save the changes.
            changedProviderParameter  = await ProviderParameterManager.UpdateAsync(
                changedProviderParameter,
                UserName
                ).ConfigureAwait(false);

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
        }
        catch (Exception ex)
        {
            // Log what we are about to do.
            Logger.LogError(
                ex,
                "Failed to edit a provider parameter."
                );

            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to edit provider parameter!</b> " +
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

    #endregion

}
