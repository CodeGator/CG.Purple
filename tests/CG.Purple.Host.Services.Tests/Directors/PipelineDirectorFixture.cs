
using CG.Purple.Directors;
using CG.Purple.Managers;
using CG.Purple.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Net.Http;

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
    /*
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

        var readyToProcess = new Models.Message[]
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
            )).ReturnsAsync(readyToProcess)
            .Verifiable();

        messageManager.Setup(x => x.UpdateAsync(
            It.IsAny<Models.Message>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(readyToProcess.First())
            .Verifiable();

        var providerTypes = new Models.ProviderType[]
        {
            new Models.ProviderType()
            {
                Id = 1,
                Name = "test provider",
                CanProcessEmails = true,
                CanProcessTexts = true,
                CreatedBy = "test",
                CreatedOnUtc = DateTime.UtcNow,
            }
        }.AsEnumerable();

        providerTypeManager.Setup(x => x.FindAllAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(providerTypes)
            .Verifiable();

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
    */
    #endregion
}
