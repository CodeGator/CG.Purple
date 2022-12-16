
using CG.Purple.Managers;
using CG.Purple.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace CG.Purple.Providers.Smtp;

/// <summary>
/// This class is a test fixture for the <see cref="SmtpProvider"/>
/// </summary>
[TestClass]
public class SmtpProviderFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="SmtpProvider.SmtpProvider(Internal.ISmtpClient, StatusHub, IMailMessageManager, IMessageManager, IMessageLogManager, ILogger{SmtpProvider})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public void SmtpProvider_ctor()
    {
        // Arrange ...
        var smtpClient = new Mock<Internal.ISmtpClient>();
        var serviceProvider = new Mock<IServiceProvider>();
        var statusHub = new Mock<StatusHub>(serviceProvider.Object);
        var mailMessageManager = new Mock<IMailMessageManager>();
        var messageManager = new Mock<IMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var logger = new Mock<ILogger<SmtpProvider>>();

        // Act ...
        var result = new SmtpProvider(
            smtpClient.Object,
            statusHub.Object,
            mailMessageManager.Object,
            messageManager.Object,
            messageLogManager.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            result._smtpClient != null,
            "The _smtpClient field is invalid"
            );
        Assert.IsTrue(
            result._mailMessageManager != null,
            "The _mailMessageManager field is invalid"
            );

        Mock.Verify(
            smtpClient,
            serviceProvider,
            statusHub,
            mailMessageManager,
            messageManager,
            messageLogManager,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="SmtpProvider.ProcessMessagesAsync(IEnumerable{Purple.Models.Message}, Purple.Models.ProviderType, CancellationToken)"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task SmtpProvider_ProcessMessagesAsync()
    {
        // Arrange ...
        var smtpClient = new Mock<Internal.ISmtpClient>();
        var serviceProvider = new Mock<IServiceProvider>();
        var statusHub = new Mock<StatusHub>(serviceProvider.Object);
        var mailMessageManager = new Mock<IMailMessageManager>();
        var messageManager = new Mock<IMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var logger = new Mock<ILogger<SmtpProvider>>();

        smtpClient.Setup(x => x.Send(
            It.IsAny<System.Net.Mail.MailMessage>()
            )).Verifiable();

        mailMessageManager.Setup(x => x.FindByIdAsync(
            It.IsAny<long>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new MailMessage()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Subject = "test email",
                MessageKey = $"{Guid.NewGuid():N}",
                Body = "this is a test email",
                MessageType = MessageType.Mail
            }).Verifiable();

        logger.Setup(x => x.Log<object>(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<object>(),
            It.IsAny<Exception?>(),
            It.IsAny<Func<object, Exception?, string>>()
            )).Callback((LogLevel logLevel, EventId eventId, object state, Exception? ex, Func<object, Exception?, string> func) =>
            {
                if (logLevel == LogLevel.Error)
                {
                    Assert.Fail(
                        "The logger logged an error during the method."
                        );
                }
            });

        messageManager.Setup(x => x.UpdateAsync(
            It.IsAny<Message>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Message())
            .Verifiable();

        messageLogManager.Setup(x => x.CreateAsync(
            It.IsAny<MessageLog>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new MessageLog())
            .Verifiable();

        statusHub.Setup(x => x.OnStatusAsync(
            It.IsAny<Message>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var provider = new SmtpProvider(
            smtpClient.Object,
            statusHub.Object,
            mailMessageManager.Object,
            messageManager.Object,
            messageLogManager.Object,
            logger.Object
            );

        var providerType = new ProviderType()
        {
            Name = "Test Provider",
            Parameters = new List<ProviderParameter>()
            {
                new ProviderParameter()
                {
                    ParameterType = new ParameterType()
                    {
                        Name = "TestParameter"
                    },
                    ProviderType = new ProviderType()
                    {
                        Name = "TestProvider"
                    },
                    Value = "thisisatestvalue"
                }
            }
        };

        var messages = new[]
        {
            new Message()
            {
                MessageType = MessageType.Mail
            }
        }.AsEnumerable();

        // Act ...
        await provider.ProcessMessagesAsync(
            messages,
            providerType
            );

        // Assert ...

        Mock.Verify(
            smtpClient,
            serviceProvider,
            statusHub,
            mailMessageManager,
            messageManager,
            messageLogManager,
            logger
            );
    }

    #endregion
}
