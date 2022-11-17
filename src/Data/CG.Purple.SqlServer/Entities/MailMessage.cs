namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This class represents a notification email entity.
/// </summary>
internal class MailMessage : Message
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the to address for the message.
    /// </summary>
    public string To { get; set; } = null!;

    /// <summary>
    /// This property contains the CC address for the message.
    /// </summary>
    public string? CC { get; set; }

    /// <summary>
    /// This property contains the BCC address for the message.
    /// </summary>
    public string? BCC { get; set; }

    /// <summary>
    /// This property contains the subject for the message.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// This property contains the body for the message.
    /// </summary>
    public string Body { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the <see cref="Body"/>
    /// property contains formatted HTML, or not.
    /// </summary>
    public bool IsHtml { get; set; }

    #endregion
}
