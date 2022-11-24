
namespace CG.Purple.Host.Shared;

/// <summary>
/// This class is the code-behind for the <see cref="MainLayout"/> page.
/// </summary>
public partial class MainLayout
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the JS runtime for the page.
    /// </summary>
    [Inject]
    public IJSRuntime JsRuntime { get; set; } = null!;

    #endregion

    /// <summary>
    /// This method is called after the page renders.
    /// </summary>
    /// <param name="firstRender"><c>true</c> for the first render, 
    /// <c>false</c> otherwise.</param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // IS this the first render?
        if (firstRender)
        {
            // Wire up the Blazor extensions.
            await JsRuntime.InitializeMudBlazorExtensionsAsync();
        }
        
        // Give the base class a chance.
        await base.OnAfterRenderAsync(firstRender);
    }
}
