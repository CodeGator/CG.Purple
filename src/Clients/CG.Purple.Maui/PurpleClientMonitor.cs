
using System.Net.Http;

namespace CG.Purple.Maui;

/// <summary>
/// This class represents an object that monitors for status notifications
/// from the <see cref="CG.Purple"/> microservice.
/// </summary>
public class PurpleClientMonitor
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the client options.
    /// </summary>
    internal protected readonly IOptions<PurpleClientOptions> _options;

    /// <summary>
    /// This property contains an optional reference to a signalR hub.
    /// </summary>
    internal protected HubConnection? _statusHub { get; set; }

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains a delegate for receiving continuous status 
    /// updates from the microservice.
    /// </summary>
    public Action<StatusNotification>? Status { get; set; }

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PurpleClientMonitor"/>
    /// class.
    /// </summary>
    /// <param name="options">The client options to use with this class.</param>
    public PurpleClientMonitor(
        IOptions<PurpleClientOptions> options
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(options, nameof(options));

        // Save the reference(s).
        _options = options;

        // Stand up the back channel.
        EnsureBackchannel();
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method stand up a back channel for continuous status updates 
    /// from the microservice.
    /// </summary>
    private void EnsureBackchannel()
    {
        // Do we need a hub?
        if (_statusHub is null)
        {
            // Get the base address.
            var url = $"{_options.Value.DefaultBaseAddress ?? 
                "https://localhost:7134"}";

            // Ensure it ends with a trailing '/'
            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            // Create a signalR hub builder.
            var builder = new HubConnectionBuilder()
                .WithUrl($"{url}_status")
                .WithAutomaticReconnect();

            // Create the signalR hub.
            _statusHub = builder.Build();

            // Wire up a back-channel handler.
            _statusHub.On(
                "Status",
                (StatusNotification statusUpdate) =>
                {
                    try
                    {
                        // Call the delegate.
                        Status?.Invoke(statusUpdate);
                    }
                    catch (Exception ex)
                    {
                        // TODO : decide what to do here.
                    }
                });

            // Start the back channel.
            _statusHub.StartAsync().Wait();
        }
    }

    #endregion

}
