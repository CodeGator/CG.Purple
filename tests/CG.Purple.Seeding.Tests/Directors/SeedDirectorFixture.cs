
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

        mailMessageManager.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false)
            .Verifiable();

        mailMessageManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.MailMessage>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.MailMessage() { })
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
        
        messagePropertyManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.MessageProperty>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.MessageProperty())
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

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="SeedDirector.SeedMessageLogsAsync(IConfiguration, string, bool, CancellationToken)"/>
    /// method calls the proper manager methods.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task SeedDirector_SeedMessageLogsAsync()
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

        messageLogManager.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false)
            .Verifiable();

        textMessageManager.Setup(x => x.FindByKeyAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.TextMessage());

        providerTypeManager.Setup(x => x.FindByNameAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.ProviderType())
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
                new KeyValuePair<string, string?>("MessageLogs:0:Event", "Stored"),
                new KeyValuePair<string, string?>("MessageLogs:0:MessageType", "Text"),
                new KeyValuePair<string, string?>("MessageLogs:0:ProviderTypeName", "TestProvider")
            }.AsEnumerable()).Build();

        // Act ...
        await director.SeedMessageLogsAsync(
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

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="SeedDirector.SeedMimeTypesAsync(IConfiguration, string, bool, CancellationToken)"/>
    /// method calls the proper manager methods.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task SeedDirector_SeedMimeTypesAsync()
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

        mimeTypeManager.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false)
            .Verifiable();

        textMessageManager.Setup(x => x.FindByKeyAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.TextMessage());

        mimeTypeManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.MimeType>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.MimeType())
            .Verifiable();

        fileTypeManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.FileType>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.FileType())
            .Verifiable();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("MimeTypes:0:Type", "application"),
                new KeyValuePair<string, string?>("MimeTypes:0:SubType", "text"),
                new KeyValuePair<string, string?>("MimeTypes:0:Extensions:0", ".txt")
            }.AsEnumerable()).Build();

        // Act ...
        await director.SeedMimeTypesAsync(
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

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="SeedDirector.SeedParameterTypesAsync(IConfiguration, string, bool, CancellationToken)"/>
    /// method calls the proper manager methods.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task SeedDirector_SeedParameterTypesAsync()
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

        parameterTypeManager.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false)
            .Verifiable();

        parameterTypeManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.ParameterType>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.ParameterType())
            .Verifiable();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("ParameterTypes:0:Name", "TestParameter"),
            }.AsEnumerable()).Build();

        // Act ...
        await director.SeedParameterTypesAsync(
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

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="SeedDirector.SeedPropertyTypesAsync(IConfiguration, string, bool, CancellationToken)"/>
    /// method calls the proper manager methods.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task SeedDirector_SeedPropertyTypesAsync()
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

        propertyTypeManager.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false)
            .Verifiable();

        propertyTypeManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.PropertyType>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.PropertyType())
            .Verifiable();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("PropertyTypes:0:Name", "TestParameter"),
            }.AsEnumerable()).Build();

        // Act ...
        await director.SeedPropertyTypesAsync(
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

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="SeedDirector.SeedProviderTypesAsync(IConfiguration, string, bool, CancellationToken)"/>
    /// method calls the proper manager methods.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task SeedDirector_SeedProviderTypesAsync()
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

        providerTypeManager.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false)
            .Verifiable();

        providerTypeManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.ProviderType>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.ProviderType())
            .Verifiable();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("ProviderTypes:0:Name", "TestParameter"),
            }.AsEnumerable()).Build();

        // Act ...
        await director.SeedProviderTypesAsync(
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

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="SeedDirector.SeedProviderParametersAsync(IConfiguration, string, bool, CancellationToken)"/>
    /// method calls the proper manager methods.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task SeedDirector_SeedProviderParametersAsync()
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

        parameterTypeManager.Setup(x => x.FindByNameAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.ParameterType())
            .Verifiable();

        providerTypeManager.Setup(x => x.FindByNameAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.ProviderType())
            .Verifiable();

        providerParameterManager.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false)
            .Verifiable();

        providerParameterManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.ProviderParameter>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.ProviderParameter())
            .Verifiable();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("ProviderParameters:0:ProviderTypeName", "TestProvider"),
                new KeyValuePair<string, string?>("ProviderParameters:0:ParameterTypeName", "TestParameter"),
                new KeyValuePair<string, string?>("ProviderParameters:0:Value", "Test Value"),
            }.AsEnumerable()).Build();

        // Act ...
        await director.SeedProviderParametersAsync(
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

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="SeedDirector.SeedTextMessagesAsync(IConfiguration, string, bool, CancellationToken)"/>
    /// method calls the proper manager methods.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public async Task SeedDirector_SeedTextMessagesAsync()
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

        textMessageManager.Setup(x => x.AnyAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false)
            .Verifiable();

        textMessageManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.TextMessage>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.TextMessage() { })
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

        messagePropertyManager.Setup(x => x.CreateAsync(
            It.IsAny<Models.MessageProperty>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Models.MessageProperty())
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
                new KeyValuePair<string, string?>("TextMessages:0:From", "test1@codegator.com"),
                new KeyValuePair<string, string?>("TextMessages:0:ProviderTypeName", "TestProvider"),
                new KeyValuePair<string, string?>("TextMessages:0:Attachments:0", "file1.txt"),
                new KeyValuePair<string, string?>("TextMessages:0:Properties:0:ProviderTypeName", "TestProvider"),
                new KeyValuePair<string, string?>("TextMessages:0:Properties:0:ParameterTypeName", "TestParameter")
            }.AsEnumerable()).Build();

        // Act ...
        await director.SeedTextMessagesAsync(
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
