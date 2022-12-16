
namespace CG.Purple.Providers.Smtp.Internal;

/// <summary>
/// This interface represents a .NET SmtpClient.
/// </summary>
/// <remarks>
/// <para>
/// The purpose of this interface is for testability. In short, the Microsoft
/// SMTP client is simply not mockable, at all. Or at least, it isn't mockable 
/// in any way that I understand. If you know a way to mock the SmtpClient
/// type, please let me know.
/// </para>
/// <para>
/// This type wraps a standard SmtpClient instance and defers to that object 
/// at runtime. During testing, we can then mock up the ISmtpClient type, 
/// and use that, instead.
/// </para>
/// </remarks>
public interface ISmtpClient
{
    /// <summary>
    /// This method sends the given mail message.
    /// </summary>
    /// <param name="mailmessage">The mail message to use for the operation.</param>
    void Send(System.Net.Mail.MailMessage mailmessage);
}
