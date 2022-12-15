
using CG.Purple.Directors;
using CG.Purple.Managers;
using CG.Purple.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace CG.Purple.Seeding.Directors;


/// <summary>
/// This class is a test fixture for the <see cref="SeedDirector"/>
/// class.
/// </summary>
[TestClass]
public class SeedDirectorFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="SeedDirector.SeedDirector(IAttachmentManager, IFileTypeManager, IMailMessageManager, IMessagePropertyManager, IMimeTypeManager, IParameterTypeManager, IPropertyTypeManager, IProviderParameterManager, IProviderTypeManager, IMessageLogManager, ITextMessageManager, ILogger{ISeedDirector})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public void SeedDirector_ctor()
    {
        // Arrange ...
        var attachmentManager = new Mock<IAttachmentManager>();
        var fileTypeManager = new Mock<IFileTypeManager>();
        var mailMessageManager = new Mock<IMailMessageManager>();
        var messagePropertyManager = new Mock<IMessagePropertyManager>();   
        var mimeTypeManager = new Mock<IMimeTypeManager>();
        var parameterTypeManager = new Mock<IParameterTypeManager>();
        var propertyTypeManager = new Mock<IPropertyTypeManager>();
        var providerParameterManager = new Mock<IProviderParameterManager>();
        var providerTypeManager = new Mock<IProviderTypeManager>(); 
        var messageLogManager = new Mock<IMessageLogManager>();
        var textMessageManager = new Mock<ITextMessageManager>();
        var logger = new Mock<ILogger<ISeedDirector>>();

        // Act ...
        var result = new SeedDirector(
            attachmentManager.Object,
            fileTypeManager.Object,
            mailMessageManager.Object,
            messagePropertyManager.Object,
            mimeTypeManager.Object,
            parameterTypeManager.Object,
            propertyTypeManager.Object,
            providerParameterManager.Object,
            providerTypeManager.Object,
            messageLogManager.Object,
            textMessageManager.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
           result._attachmentManager != null,
           "The _attachmentManager field is invalid"
           );
        Assert.IsTrue(
           result._fileTypeManager != null,
           "The _fileTypeManager field is invalid"
           );
        Assert.IsTrue(
           result._mailMessageManager != null,
           "The _mailMessageManager field is invalid"
           );
        Assert.IsTrue(
           result._messagePropertyManager != null,
           "The _messagePropertyManager field is invalid"
           );
        Assert.IsTrue(
           result._mimeTypeManager != null,
           "The _mimeTypeManager field is invalid"
           );
        Assert.IsTrue(
           result._parameterTypeManager != null,
           "The _parameterTypeManager field is invalid"
           );
        Assert.IsTrue(
           result._propertyTypeManager != null,
           "The _propertyTypeManager field is invalid"
           );
        Assert.IsTrue(
           result._providerParameterManager != null,
           "The _providerParameterManager field is invalid"
           );
        Assert.IsTrue(
           result._messageLogManager != null,
           "The _messageLogManager field is invalid"
           );
        Assert.IsTrue(
           result._providerTypeManager != null,
           "The _providerTypeManager field is invalid"
           );
        Assert.IsTrue(
           result._textMessageManager != null,
           "The _textMessageManager field is invalid"
           );
        Assert.IsTrue(
           result._logger != null,
           "The _logger field is invalid"
           );

        Mock.Verify(
            attachmentManager,
            fileTypeManager,
            mailMessageManager,
            messagePropertyManager,
            mimeTypeManager,
            parameterTypeManager,
            propertyTypeManager,
            providerParameterManager,
            providerTypeManager,
            messageLogManager,
            textMessageManager,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="SeedDirector.SeedMailMessagesAsync(Microsoft.Extensions.Configuration.IConfiguration, string, bool, CancellationToken)"/>
    /// method calls the proper manager methods.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task SeedDirector_SeedMailMessagesAsync()
    {
        // Arrange ...
        var attachmentManager = new Mock<IAttachmentManager>();
        var fileTypeManager = new Mock<IFileTypeManager>();
        var mailMessageManager = new Mock<IMailMessageManager>();
        var messagePropertyManager = new Mock<IMessagePropertyManager>();
        var mimeTypeManager = new Mock<IMimeTypeManager>();
        var parameterTypeManager = new Mock<IParameterTypeManager>();
        var propertyTypeManager = new Mock<IPropertyTypeManager>();
        var providerParameterManager = new Mock<IProviderParameterManager>();
        var providerTypeManager = new Mock<IProviderTypeManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var textMessageManager = new Mock<ITextMessageManager>();
        var logger = new Mock<ILogger<ISeedDirector>>();
                
        var director = new SeedDirector(
            attachmentManager.Object,
            fileTypeManager.Object,
            mailMessageManager.Object,
            messagePropertyManager.Object,
            mimeTypeManager.Object,
            parameterTypeManager.Object,
            propertyTypeManager.Object,
            providerParameterManager.Object,
            providerTypeManager.Object,
            messageLogManager.Object,
            textMessageManager.Object,
            logger.Object
            );

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

        mailMessageManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.MailMessage>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.MailMessage() { })
            .Verifiable();

        messageLogManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.MessageLog>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.MessageLog() { })
            .Verifiable();

        providerTypeManager.Setup(x => x.FindByNameAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.ProviderType())
            .Verifiable();

        mimeTypeManager.Setup(x => x.FindByExtensionAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.MimeType())
            .Verifiable();

        attachmentManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.Attachment>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.Attachment())
            .Verifiable();

        propertyTypeManager.Setup(x => x.FindByNameAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.PropertyType())
            .Verifiable();

        propertyTypeManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.PropertyType>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.PropertyType())
            .Verifiable();

        messageLogManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.MessageLog>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.MessageLog())
            .Verifiable();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[] 
            { 
                new KeyValuePair<string, string?>("MailMessages:0:From", "test1@codegator.com"),
                new KeyValuePair<string, string?>("MailMessages:0:ProviderTypeName", "TestProvider"),
                new KeyValuePair<string, string?>("MailMessages:0:Attachments:0", "file1.txt"),
                new KeyValuePair<string, string?>("MailMessages:0:Properties:0:ProviderTypeName", "TestProvider"),
                new KeyValuePair<string, string?>("MailMessages:0:Properties:0:ParameterTypeName", "TestParameter")
            }.AsEnumerable()).Build();

        // Act ...
        await director.SeedMailMessagesAsync(
            configuration,
            "test",
            false
            );

        Mock.Verify(
            attachmentManager,
            fileTypeManager,
            mailMessageManager,
            messagePropertyManager,
            mimeTypeManager,
            parameterTypeManager,
            propertyTypeManager,
            providerParameterManager,
            providerTypeManager,
            messageLogManager,
            textMessageManager,
            logger
            );
    }

    #endregion
}
