
namespace CG.Purple.ViewModels;

/// <summary>
/// This class represents a request to a 3rd party messaging provider.
/// </summary>
/// <typeparam name="TMessage">The type of associated message.</typeparam>
public class ProviderRequest<TMessage>
    where TMessage : Message
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the associated message.
    /// </summary>
    public TMessage Message { get; set; } = null!;

    /// <summary>
    /// This property contains the associated provider type.
    /// </summary>
    public ProviderType ProviderType { get; set; } = null!;

    #endregion
}
