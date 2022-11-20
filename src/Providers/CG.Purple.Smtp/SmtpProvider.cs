
namespace CG.Purple.Smtp;

/// <summary>
/// This class is a SMTP implementation of the <see cref="IMessageProvider"/>
/// interface.
/// </summary>
internal class SmtpProvider : IMessageProvider
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the logger for this provider.
    /// </summary>
    internal protected readonly ILogger<IMessageProvider> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SmtpProvider"/>
    /// class.
    /// </summary>
    /// <param name="logger">The logger to use with this provider.</param>
    public SmtpProvider(
        ILogger<IMessageProvider> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual Task SendMailAsync(
        MailMessage mailMessage,
        ProviderType providerType,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(mailMessage, nameof(mailMessage))
            .ThrowIfNull(providerType, nameof(providerType));

        try
        {
            // =======
            // Step 1: Find the associated provider parameters.
            // =======

            // Get the server url property from the message properties.
            var serverUrlProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "ServerUrl"
                );

            // Did we fail?
            if (serverUrlProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The message; {mailMessage.Id} didn't have a 'ServerUrl' property!"
                    );
            }

            // Get the user name property from the message properties.
            var userNameProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "UserName"
                );

            // Did we fail?
            if (userNameProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The message; {mailMessage.Id} didn't have a 'UserName' property!"
                    );
            }

            // Get the password property from the message properties.
            var passwordProperty = providerType.Parameters.FirstOrDefault(
                x => x.ParameterType.Name == "Password"
                );

            // Did we fail?
            if (passwordProperty is null)
            {
                // Panic!!
                throw new KeyNotFoundException(
                    $"The message; {mailMessage.Id} didn't have a 'Password' property!"
                    );
            }

            // =======
            // Step 2: Create the .NET mail message.
            // =======

            // Create the .NET model.
            using var msg = new System.Net.Mail.MailMessage()
            {
                From = new System.Net.Mail.MailAddress(mailMessage.From),
                Subject = mailMessage.Subject,
                Body = mailMessage.Body,
                IsBodyHtml = mailMessage.IsHtml
            };

            // Set the target address(es).
            foreach (var to in mailMessage.To.Split(';'))
            {
                if (!string.IsNullOrEmpty(to))
                {
                    msg.To.Add(to);
                }
            }

            // Was a CC supplied?
            if (!string.IsNullOrEmpty(mailMessage.CC))
            {
                // Set the CC address(es).
                foreach (var cc in mailMessage.CC.Split(';'))
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        msg.CC.Add(cc);
                    }
                }
            }

            // Was a BCC supplied?
            if (!string.IsNullOrEmpty(mailMessage.BCC))
            {
                // Set the BCC address(es).
                foreach (var bcc in mailMessage.BCC.Split(';'))
                {
                    if (!string.IsNullOrEmpty(bcc))
                    {
                        msg.Bcc.Add(bcc);
                    }
                }
            }

            // =======
            // Step 3: Create the .NET mail client.
            // =======

            // Create the SMTP client.
            using System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(
                serverUrlProperty.Value
                );

            // Set the credentials for the client.
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(
                userNameProperty.Value,
                passwordProperty.Value
                );

            // =======
            // Step 4: Send the message.
            // =======

            // Send the message.
            client.Send(msg);



            // Return the task.
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            // If we get here is means the external SMTP client didn't send the message.

            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to process an email!"
                );

            // Provider better context.
            throw new ProviderException(
                message: $"The provider failed to process an email!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual Task SendTextAsync(
        TextMessage textMessage,
        ProviderType providerType,
        CancellationToken cancellationToken = default
        )
    {
        throw new ProviderException(
            message: $"SMTP doesn't support sending text messages!"
            );
    }

    #endregion
}
