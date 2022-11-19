
namespace CG.Purple.Host.SignalR
{
    /// <summary>
    /// This class is a SignalR hub for the status back channel.
    /// </summary>
    public class StatusHub : Hub
    {
        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="StatusHub"/>
        /// class.
        /// </summary>
        public StatusHub() { }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method sends a status change notification.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task to perform the operation.</returns>
        public async Task OnStatusChangeAsync(
            CancellationToken cancellationToken = default
            )
        {
            // Do we have any clients to notify?
            if (Clients is not null)
            {
                // Send the message to the clients.
                await Clients.All.SendAsync(
                    "StatusChange",
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }

        #endregion
    }
}
