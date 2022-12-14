
using CG.Purple.Clients.ViewModels;
using Microsoft.Extensions.Logging;

namespace CG.Purple.Samples.QuickStart.Pages.Home;

/// <summary>
/// This class is the code-behind for the <see cref="PropertiesDialog"/> page.
/// </summary>
public partial class PropertiesDialog
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the current search string.
    /// </summary>
    protected string _gridSearchString = "";

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
    public List<MessagePropertyRequest> Model { get; set; } = null!;

    /// <summary>
    /// This property contains the dialog service for the page.
    /// </summary>
    [Inject]
    protected IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// This property contains the snackbar service for the page.
    /// </summary>
    [Inject]
    protected ISnackbar SnackbarService { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method closes the dialog.
    /// </summary>
    protected void Ok()
    {
        MudDialog.Close(DialogResult.Ok(Model));
    }

    // *******************************************************************

    /// <summary>
    /// This method closes the dialog.
    /// </summary>
    protected void Cancel()
    {
        MudDialog.Close(DialogResult.Cancel);
    }

    // *******************************************************************

    /// <summary>
    /// This method creates a new property.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnCreateAsync()
    {
        try
        {
            // Create the dialog options.
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true
            };

            // Create the dialog parameters.
            var parameters = new DialogParameters()
            {
                { "Model", new MessagePropertyRequest() }
            };

            // Create the dialog.
            var dialog = DialogService.Show<PropertyDialog>(
                "Create Property",
                parameters,
                options
                );

            // Get the results of the dialog.
            var result = await dialog.Result;

            // Did the user cancel?
            if (result.Cancelled)
            {
                return;
            }

            // Recover the property.
            var newProperty = (MessagePropertyRequest)result.Data;

            // Add the property to the model.
            Model.Add(newProperty);

            // Tell the world what happened.
            SnackbarService.Add(
                $"Changes were saved",
                Severity.Success,
                options => options.CloseAfterNavigation = true
                );
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to create property!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method edits a property.
    /// </summary>
    /// <param name="request">The message property to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnEditAsync(MessagePropertyRequest request)
    {
        try
        {
            // Create the dialog options.
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                FullWidth = true
            };

            // Create the dialog parameters.
            var parameters = new DialogParameters()
            {
                { "Model", request.QuickClone() }
            };

            // Create the dialog.
            var dialog = DialogService.Show<PropertyDialog>(
                "Edit Property",
                parameters,
                options
                );

            // Get the results of the dialog.
            var result = await dialog.Result;

            // Did the user cancel?
            if (result.Cancelled)
            {
                return;
            }

            // Recover the edited property.
            var editedProperty = (MessagePropertyRequest)result.Data;

            // Replace the property.
            Model.Remove(request);
            Model.Add(editedProperty);  

            // Tell the world what happened.
            SnackbarService.Add(
                $"Changes were saved",
                Severity.Success,
                options => options.CloseAfterNavigation = true
                );
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to edit property!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method deletes a property.
    /// </summary>
    /// <param name="request">The message property to use for the operation.</param>
    /// <returns>A task to perform the operation.</returns>
    protected async Task OnDeleteAsync(MessagePropertyRequest request)
    {
        try
        {
            // Prompt the user.
            var result = await DialogService.ShowMessageBox(
                title: "Purple QuickStart",
                markupMessage: new MarkupString("This will delete the property " +
                $"<b>{request.PropertyName}</b> <br /> <br /> Are you <i>sure" +
                "</i> you want to do that?"),
                noText: "Cancel"
                );

            // Did the user cancel?
            if (result.HasValue && !result.Value)
            {
                return; // Nothing more to do.
            }

            // Remove the property.
            Model.Remove(request);

            // Tell the world what happened.
            SnackbarService.Add(
                $"Changes were saved",
                Severity.Success,
                options => options.CloseAfterNavigation = true
                );
        }
        catch (Exception ex)
        {
            // Tell the world what happened.
            SnackbarService.Add(
                $"<b>Failed to delete property!</b> " +
                $"<ul><li>{ex.GetBaseException().Message}</li></ul>",
                Severity.Error,
                options => options.CloseAfterNavigation = true
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method adapts the <see cref="FilterFunc(MessagePropertyRequest, string)"/> method
    /// for use with a <see cref="MudTable{T}"/> control.
    /// </summary>
    /// <param name="element">The element to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc(MessagePropertyRequest element) =>
        FilterFunc(element, _gridSearchString);

    // *******************************************************************

    /// <summary>
    /// This method performs a search of the grid.
    /// </summary>
    /// <param name="element">The element to uses for the operation.</param>
    /// <param name="searchString">The search string to use for the operation.</param>
    /// <returns><c>true</c> if a match was found; <c>false</c> otherwise.</returns>
    protected bool FilterFunc(
        MessagePropertyRequest element,
        string searchString
        )
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return true;
        }
        if (searchString.Contains(element.PropertyName, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        if (searchString.Contains(element.Value, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    #endregion
}
