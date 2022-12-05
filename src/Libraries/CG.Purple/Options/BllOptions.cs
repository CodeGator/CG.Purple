
namespace CG.Purple.Options;

/// <summary>
/// This class contains configuration settings for the Purple business 
/// logic layer.
/// </summary>
public class BllOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the configuration options for the <see cref="IMailMessageManager"/>
    /// manager.
    /// </summary>
    public MailMessageManagerOptions? MailMessageManager { get; set; }

    /// <summary>
    /// This property contains the configuration options for the <see cref="ITextMessageManager"/>
    /// manager.
    /// </summary>
    public TextMessageManagerOptions? TextMessageManager { get; set; }

    #endregion
}
