
using System.Linq;

namespace CG.Purple.Host.Pages.ProviderTypes;

/// <summary>
/// This class is the code-behind for the <see cref="ProviderTypeDialog"/> page.
/// </summary>
public partial class ProviderTypeDialog
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

            // Did the user save?
            if (!result.Cancelled)
            {
                // Log what we are about to do.
                Logger.LogDebug(
                    "Recovering the modified provider parameter."
                    );

                // Recover the edited provider parameter.
                var changedProviderParameter = (ProviderParameter)result.Data;

                // Save the changes.
                Model.Parameters.Add(changedProviderParameter);                
            }
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

            // Filter out the currently used parameter type.
            var filteredParameterTypes = ParameterTypes.Where(x =>
                x.Id == parameter.ParameterType.Id
                ).ToList();

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
                        CreatedBy = UserName,
                        CreatedOnUtc = DateTime.UtcNow,
                    }
                },
                {
                    "ParameterTypes", filteredParameterTypes
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

            // Did the user save?
            if (!result.Cancelled)
            {
                // Log what we are about to do.
                Logger.LogDebug(
                    "Recovering the modified provider parameter."
                    );

                // Recover the edited provider parameter.
                var changedProviderParameter = (ProviderParameter)result.Data;
                
                // TODO : figure out what to do here.
            }
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
    }

    #endregion

}
