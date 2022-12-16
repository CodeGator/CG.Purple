
namespace CG.Purple.Providers.Smtp.Internal;

/// <summary>
/// This class is a default implementation of the <see cref="ISmtpClient"/>
/// interface.
/// </summary>
internal class SmtpClientWrapper : ISmtpClient
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the inner SMTP client for this wrapper.
    /// </summary>
    internal protected readonly System.Net.Mail.SmtpClient _innerSmtpClient = null!;

    #endregion
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SmtpClientWrapper"/>
    /// class.
    /// </summary>
    /// <param name="innerSmtpClient">The SMTP client to use for this wrapper.</param>
    public SmtpClientWrapper(
        System.Net.Mail.SmtpClient innerSmtpClient
        )
    {
        _innerSmtpClient = innerSmtpClient;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    public virtual void Send(
        System.Net.Mail.MailMessage mailmessage
        )
    {
        _innerSmtpClient.Send(mailmessage);   
    }

    #endregion
}
