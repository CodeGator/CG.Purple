
namespace CG.Purple.Host.Controllers;

/// <summary>
/// This class is a test fixture for the <see cref="TextController"/>
/// class.
/// </summary>
[TestClass]
public class TextControllerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="TextController.TextController(Managers.ITextMessageManager, Managers.IMessageLogManager, Managers.IMimeTypeManager, Managers.IPropertyTypeManager, Microsoft.Extensions.Logging.ILogger{TextController})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    public void TextController_ctor()
    {
        // Arrange ...
        var textMessageManager = new Mock<ITextMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var mimeTypeManager = new Mock<IMimeTypeManager>();
        var propertyTypeManager = new Mock<IPropertyTypeManager>();
        var logger = new Mock<ILogger<TextController>>();

        // Act ...
        var controller = new TextController(
            textMessageManager.Object,
            messageLogManager.Object,
            mimeTypeManager.Object,
            propertyTypeManager.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            controller._textMessageManager != null,
            "The _textMessageManager field wasn't initialize!"
            );
        Assert.IsTrue(
            controller._messageLogManager != null,
            "The _messageLogManager field wasn't initialize!"
            );
        Assert.IsTrue(
            controller._mimeTypeManager != null,
            "The _mimeTypeManager field wasn't initialize!"
            );
        Assert.IsTrue(
            controller._propertyTypeManager != null,
            "The _propertyTypeManager field wasn't initialize!"
            );
        Assert.IsTrue(
            controller._logger != null,
            "The _logger field wasn't initialize!"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="TextController.GetByKeyAsync(string)"/>
    /// method properly calls the managers and returns the result.
    /// </summary>
    [TestMethod]
    public async Task TextController_GetByKeyAsync()
    {
        // Arrange ...
        var textMessageManager = new Mock<ITextMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var mimeTypeManager = new Mock<IMimeTypeManager>();
        var propertyTypeManager = new Mock<IPropertyTypeManager>();
        var logger = new Mock<ILogger<TextController>>();

        textMessageManager.Setup(x => x.FindByKeyAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new TextMessage())
            .Verifiable();

        messageLogManager.Setup(x => x.FindByMessageAsync(
            It.IsAny<Message>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new[] { new MessageLog() })
            .Verifiable();

        var controller = new TextController(
            textMessageManager.Object,
            messageLogManager.Object,
            mimeTypeManager.Object,
            propertyTypeManager.Object,
            logger.Object
            );

        // Act ...
        var actionResult = await controller.GetByKeyAsync(
            $"{Guid.NewGuid():D}"
            ).ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            actionResult is OkObjectResult,
            "The return type is invalid!"
            );
        Assert.IsTrue(
            (actionResult as OkObjectResult)?.StatusCode == 200,
            "The status code is invalid!"
            );

        Mock.Verify(
            textMessageManager,
            messageLogManager,
            mimeTypeManager,
            propertyTypeManager,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="TextController.PostAsync(ViewModels.TextStorageRequest)"/>
    /// method properly calls the managers and returns the result.
    /// </summary>
    [TestMethod]
    public async Task TextController_PostAsync()
    {
        // Arrange ...
        var textMessageManager = new Mock<ITextMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var mimeTypeManager = new Mock<IMimeTypeManager>();
        var propertyTypeManager = new Mock<IPropertyTypeManager>();
        var logger = new Mock<ILogger<TextController>>();

        textMessageManager.Setup(x => x.CreateAsync(
            It.IsAny<TextMessage>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new TextMessage())
            .Verifiable();

        var controller = new TextController(
            textMessageManager.Object,
            messageLogManager.Object,
            mimeTypeManager.Object,
            propertyTypeManager.Object,
            logger.Object
            );

        // Act ...
        var actionResult = await controller.PostAsync(
            new TextStorageRequest()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Body = "this is test email 1"
            }).ConfigureAwait(false);

        // Assert ...
        Assert.IsTrue(
            actionResult is CreatedResult,
            "The return type is invalid!"
        );
        Assert.IsTrue(
            (actionResult as CreatedResult)?.StatusCode == 201,
            "The status code is invalid!"
            );

        Mock.Verify(
            textMessageManager,
            messageLogManager,
            mimeTypeManager,
            propertyTypeManager,
            logger
            );
    }

    #endregion
}
