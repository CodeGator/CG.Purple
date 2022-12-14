
namespace CG.Purple.Clients;

/// <summary>
/// This class is a test fixture for the <see cref="PurpleStatusMonitor"/>
/// class.
/// </summary>
[TestClass]
public class PurpleStatusMonitorFixture
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method ensures the <see cref="PurpleStatusMonitor.PurpleStatusMonitor(IHubConnection)"/>
    /// constructor properly initializes object instances.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public void PurpleStatusMonitor_ctor()
    {
        // Arrange ...
        var hubConnection = new Mock<IHubConnection>();

        // Act ...
        var result = new PurpleStatusMonitor(
            hubConnection.Object
            );

        // Assert ...
        Assert.IsTrue(
            result._hubConnection != null,
            "The _hubConnection field is invalid"
            );

        Mock.Verify(
            hubConnection
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="PurpleStatusMonitor.IsConnected"/>
    /// property returns true when connected to the microservice.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public void PurpleStatusMonitor_IsConnected_True()
    {
        // Arrange ...
        var hubConnection = new Mock<IHubConnection>();

        hubConnection.Setup(x => x.State)
            .Returns(HubConnectionState.Connected)
            .Verifiable();

        var monitor = new PurpleStatusMonitor(
            hubConnection.Object
            );

        // Act ...
        var result = monitor.IsConnected;

        // Assert ...
        Assert.IsTrue(
            result,
            "The return is invalid"
            );

        Mock.Verify(
            hubConnection
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method ensures the <see cref="PurpleStatusMonitor.IsConnected"/>
    /// property returns false when not connected to the microservice.
    /// </summary>
    [TestMethod]
    [TestCategory("Unit")]
    public void PurpleStatusMonitor_IsConnected_False()
    {
        // Arrange ...
        var hubConnection = new Mock<IHubConnection>();

        hubConnection.Setup(x => x.State)
            .Returns(HubConnectionState.Disconnected)
            .Verifiable();

        var monitor = new PurpleStatusMonitor(
            hubConnection.Object
            );

        // Act ...
        var result = monitor.IsConnected;

        // Assert ...
        Assert.IsFalse(
            result,
            "The return is invalid"
            );

        Mock.Verify(
            hubConnection
            );
    }

    #endregion
}
