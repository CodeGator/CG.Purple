
using Microsoft.VisualBasic;

namespace CG.Purple.Host.Directors;

/// <summary>
/// This class is a test fixture for the <see cref="PipelineDirector"/>
/// class.
/// </summary>
[TestClass]
public class PipelineDirectorFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="PipelineDirector.PipelineDirector(Managers.IAttachmentManager, Managers.IMessageManager, Managers.IMessageLogManager, Managers.IMessagePropertyManager, Managers.IProviderTypeManager, Providers.IMessageProviderFactory, ILogger{Purple.Directors.IPipelineDirector})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public void PipelineDirector_ctor()
    {
        // Arrange ...
        var attachmentManager = new Mock<IAttachmentManager>();
        var messageManager = new Mock<IMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>(); 
        var messagePropertyManager = new Mock<IMessagePropertyManager>();
        var providerTypeManager = new Mock<IProviderTypeManager>();
        var messageProviderFactory = new Mock<IMessageProviderFactory>();
        var logger = new Mock<ILogger<IPipelineDirector>>();   

        // Act ...
        var result = new PipelineDirector(
            attachmentManager.Object,
            messageManager.Object,
            messageLogManager.Object,
            messagePropertyManager.Object,
            providerTypeManager.Object,
            messageProviderFactory.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
           result._attachmentManager != null,
           "The _attachmentManager field is invalid"
           );
        Assert.IsTrue(
           result._messageManager != null,
           "The _messageManager field is invalid"
           );
        Assert.IsTrue(
           result._messageLogManager != null,
           "The _messageLogManager field is invalid"
           );
        Assert.IsTrue(
           result._messagePropertyManager != null,
           "The _messagePropertyManager field is invalid"
           );
        Assert.IsTrue(
           result._providerTypeManager != null,
           "The _providerTypeManager field is invalid"
           );
        Assert.IsTrue(
           result._messageProviderFactory != null,
           "The _messageProviderFactory field is invalid"
           );
        Assert.IsTrue(
           result._logger != null,
           "The _logger field is invalid"
           );

        Mock.Verify(
            attachmentManager,
            messageManager,
            messageLogManager, 
            messagePropertyManager,
            providerTypeManager,
            messageProviderFactory,
            logger
            );
    }

    // *******************************************************************
    
    /// <summary>
    /// This method ensures the <see cref="PipelineDirector.ProcessAsync(TimeSpan, CancellationToken)"/>
    /// method calls the proper manager methods.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task PipelineDirector_ProcessAsync()
    {
        // Arrange ...
        var attachmentManager = new Mock<IAttachmentManager>();
        var messageManager = new Mock<IMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var messagePropertyManager = new Mock<IMessagePropertyManager>();
        var providerTypeManager = new Mock<IProviderTypeManager>();
        var messageProviderFactory = new Mock<IMessageProviderFactory>();
        var logger = new Mock<ILogger<IPipelineDirector>>();
        var provider = new Mock<IMessageProvider>();

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

        var messages = new Models.Message[]
        {
            new Models.Message()
            {
                Id = 1,
                From = "test1@codegator.com",
                MessageKey = $"{Guid.NewGuid():N}",
                MessageType = Models.MessageType.Text,
                MessageState = Models.MessageState.Pending,
                ProcessAfterUtc = DateTime.UtcNow,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }
        }.AsEnumerable();

        messageManager.Setup(x => x.FindReadyToProcessAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(messages)
            .Verifiable();

        messageManager.Setup(x => x.FindReadyToRetryAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(messages)
            .Verifiable();

        messageManager.Setup(x => x.FindReadyToArchiveAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(messages)
            .Verifiable();

        messageManager.Setup(x => x.UpdateAsync(
            It.IsAny<Models.Message>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(messages.First())
            .Verifiable();

        messageManager.Setup(x => x.DeleteAsync(
            It.IsAny<Models.Message>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var providerTypes = new Models.ProviderType[]
        {
            new Models.ProviderType()
            {
                Id = 1,
                Name = "test provider",
                CanProcessEmails = true,
                CanProcessTexts = true,
                FactoryType = "DoNothing",
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }
        }.AsEnumerable();

        providerTypeManager.Setup(x => x.FindAllAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(providerTypes)
            .Verifiable();

        messageLogManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.MessageLog>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.MessageLog()
            {
                Id = 1,
                Message = messages.First(),
                ProviderType = providerTypes.First(),
                MessageEvent = Models.MessageEvent.Assigned
            }).Verifiable();

        messageProviderFactory.Setup(x => x.CreateAsync(
            It.IsAny<Models.ProviderType>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(provider.Object);

        provider.Setup(x => x.ProcessMessagesAsync(
            It.IsAny<IEnumerable<Models.Message>>(),
            It.IsAny<Models.ProviderType>(),
            It.IsAny<CancellationToken>()
            )).Verifiable();

        var director = new PipelineDirector(
            attachmentManager.Object,
            messageManager.Object,
            messageLogManager.Object,
            messagePropertyManager.Object,
            providerTypeManager.Object,
            messageProviderFactory.Object,
            logger.Object
            );

        // Act ...
        await director.ProcessAsync(TimeSpan.FromMilliseconds(1));

        // Assert ...        

        Mock.Verify(
            attachmentManager,
            messageManager,
            messageLogManager,
            messagePropertyManager,
            providerTypeManager,
            messageProviderFactory,
            logger
            );
    }
    
    #endregion
}
