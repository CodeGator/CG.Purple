
namespace CG.Purple.Host.Controllers;

/// <summary>
/// This class is a test fixture for the <see cref="MailController"/>
/// class.
/// </summary>
[TestClass]
public class MailControllerFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="MailController.MailController(Managers.IMailMessageManager, Managers.IMessageLogManager, Managers.IMimeTypeManager, Managers.IPropertyTypeManager, Microsoft.Extensions.Logging.ILogger{MailController})"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public void MailController_ctor()
    {
        // Arrange ...
        var mailMessageManager = new Mock<IMailMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var mimeTypeManager = new Mock<IMimeTypeManager>();
        var propertyTypeManager = new Mock<IPropertyTypeManager>();
        var logger = new Mock<ILogger<MailController>>();

        // Act ...
        var controller = new MailController(
            mailMessageManager.Object,
            messageLogManager.Object,
            mimeTypeManager.Object,
            propertyTypeManager.Object,
            logger.Object
            );

        // Assert ...
        Assert.IsTrue(
            controller._mailMessageManager != null,
            "The _mailMessageManager field wasn't initialize!"
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
    /// This method ensures the <see cref="MailController.GetByKeyAsync(string)"/>
    /// method properly calls the managers and returns the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MailController_GetByKeyAsync()
    {
        // Arrange ...
        var mailMessageManager = new Mock<IMailMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var mimeTypeManager = new Mock<IMimeTypeManager>();
        var propertyTypeManager = new Mock<IPropertyTypeManager>();
        var logger = new Mock<ILogger<MailController>>();

        mailMessageManager.Setup(x => x.FindByKeyAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new MailMessage())
            .Verifiable();

        messageLogManager.Setup(x => x.FindByMessageAsync(
            It.IsAny<Message>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new[] { new MessageLog() })
            .Verifiable();

        var controller = new MailController(
            mailMessageManager.Object,
            messageLogManager.Object,
            mimeTypeManager.Object,
            propertyTypeManager.Object,
            logger.Object
            );

        // Act ...
        var actionResult = await controller.GetByKeyAsync(
            $"{Guid.NewGuid():N}"
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
            mailMessageManager,
            messageLogManager,
            mimeTypeManager,
            propertyTypeManager,
            logger
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="MailController.PostAsync(ViewModels.MailStorageRequest)"/>
    /// method properly calls the managers and returns the result.
    /// </summary>
    [TestMethod]
    [TestCategory("UnitTest")]
    public async Task MailController_PostAsync()
    {
        // Arrange ...
        var mailMessageManager = new Mock<IMailMessageManager>();
        var messageLogManager = new Mock<IMessageLogManager>();
        var mimeTypeManager = new Mock<IMimeTypeManager>();
        var propertyTypeManager = new Mock<IPropertyTypeManager>();
        var logger = new Mock<ILogger<MailController>>();

        mailMessageManager.Setup(x => x.CreateAsync(
            It.IsAny<MailMessage>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new MailMessage())
            .Verifiable();

        var controller = new MailController(
            mailMessageManager.Object,
            messageLogManager.Object,
            mimeTypeManager.Object,
            propertyTypeManager.Object,
            logger.Object
            );

        // Act ...
        var actionResult = await controller.PostAsync(
            new MailStorageRequest()
            {
                From = "test1@codegator.com",
                To = "test1@codegator.com",
                Subject = "test email 1",
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
            mailMessageManager,
            messageLogManager,
            mimeTypeManager,
            propertyTypeManager,
            logger
            );
    }

    #endregion
}
