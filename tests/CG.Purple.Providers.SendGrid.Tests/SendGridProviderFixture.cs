
namespace CG.Purple.Providers.SendGrid;

/// <summary>
/// This class is a test fixture for the <see cref="SendGridProvider"/>
/// class.
/// </summary>
[TestClass]
public class SendGridProviderFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="SendGridProvider.SendGridProvider(ISendGridClient, StatusHub, IMailMessageManager, IMessageManager, IMessageLogManager, ILogger{SendGridProvider})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]  
    public void SendGridProvider_ctor()
    {
        // Arrange ...
        var sendGridClient = new Mock<ISendGridClient>();
        var serviceProvider = new Mock<IServiceProvider>();
        var statusHub = new Mock<StatusHub>(serviceProvider.Object);
        var mailMessageManager = new Mock<IMailMessageManager>();
        var messageManager = new Mock<IMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var logger = new Mock<ILogger<SendGridProvider>>();

        // Act ...
        var result = new SendGridProvider(
            sendGridClient.Object,
            statusHub.Object,
            mailMessageManager.Object,
            messageManager.Object,
            messageLogManager.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            result._sendGridClient != null,
            "The _sendGridClient field is invalid"
            );
        Assert.IsTrue(
            result._mailMessageManager != null,
            "The _mailMessageManager field is invalid"
            );

        Mock.Verify(
            sendGridClient,
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
    /// This method ensures the <see cref="SendGridProvider.ProcessMessagesAsync(IEnumerable{Purple.Models.Message}, Purple.Models.ProviderType, CancellationToken)"/>
    /// method properly calls the manager methods.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task SendGridProvider_ProcessMessagesAsync()
    {
        // Arrange ...
        var sendGridClient = new Mock<ISendGridClient>();
        var serviceProvider = new Mock<IServiceProvider>();
        var statusHub = new Mock<StatusHub>(serviceProvider.Object);
        var mailMessageManager = new Mock<IMailMessageManager>();
        var messageManager = new Mock<IMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var logger = new Mock<ILogger<SendGridProvider>>();

        sendGridClient.Setup(x => x.SendEmailAsync(
            It.IsAny<SendGridMessage>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Response(
                System.Net.HttpStatusCode.OK,
                new StringContent(""),
                null
                ))
            .Verifiable();

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

        var manager = new SendGridProvider(
            sendGridClient.Object,
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
                        Name = "ApiKey"
                    },
                    ProviderType = new ProviderType()
                    {
                        Name = "TestProvider"
                    },
                    Value = "test apikey"
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
        await manager.ProcessMessagesAsync(
            messages,
            providerType
            );

        // Assert ...

        Mock.Verify(
            sendGridClient,
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

