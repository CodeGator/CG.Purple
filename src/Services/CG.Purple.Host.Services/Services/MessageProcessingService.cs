
namespace CG.Purple.Host.Services;

/// <summary>
/// This class is a message processing service.
/// </summary>
internal class MessageProcessingService : BackgroundService
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the service provider for this service.
    /// </summary>
    internal protected readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// This field contains the logger for this service.
    /// </summary>
    internal protected readonly ILogger<MessageProcessingService> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessageProcessingService"/>
    /// class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use with
    /// this service.</param>
    /// <param name="logger">The logger to use with this service.</param>
    public MessageProcessingService(
        IServiceProvider serviceProvider,
        ILogger<MessageProcessingService> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceProvider, nameof(serviceProvider))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _serviceProvider = serviceProvider;
        _logger = logger; 
    }

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method performs the work for this service.
    /// </summary>
    /// <param name="stoppingToken">A cancellation token that is signaled
    /// when the service is stopping.</param>
    /// <returns>A task to perform the operation.</returns>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken
        )
    {
        try
        {
            // Log what we are  about to do.
            _logger.LogDebug(
                "The {svc} service is running.",
                nameof(MessageProcessingService)
                );

            // Register for the stop signal.
            stoppingToken.Register(() =>
                _logger.LogDebug(
                    "{svc} task is stopping.",
                    nameof(MessageProcessingService)
                    )
                );

            // Log what we are about to do.
            _logger.LogTrace(
                "Pausing the {svc} service for {time}.",
                nameof(MessageProcessingService),
                TimeSpan.FromSeconds(30)
                );

            // Let's not work tooo soon.
            await Task.Delay(
                TimeSpan.FromSeconds(30),
                stoppingToken
                );

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating a DI scope"
                );

            // Create a DI scope.
            using var scope = _serviceProvider.CreateScope();

            // Log what we are about to do.
            _logger.LogDebug(
                "Creating an {name} instance",
                nameof(IProcessDirector)
                );

            // Get a scoped process director.
            var processDirector = scope.ServiceProvider.GetRequiredService<
                IProcessDirector
                >();

            // Log what we are about to do.
            _logger.LogDebug(
                "Entering main {svc} service loop",
                nameof(MessageProcessingService)
                );

            // While the service is running ...
            while (!stoppingToken.IsCancellationRequested)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Deferring to {name}",
                    nameof(IProcessDirector.ProcessAsync)
                    );

                // Process pending messages.
                await processDirector.ProcessAsync(
                    stoppingToken
                    ).ConfigureAwait(false);

                // Log what we are about to do.
                _logger.LogTrace(
                    "Pausing the {svc} service for {time}.",
                    nameof(MessageProcessingService),
                    TimeSpan.FromSeconds(30)
                    );

                // Let's not work tooo hard.
                await Task.Delay(
                    TimeSpan.FromSeconds(30),
                    stoppingToken
                    );
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Leaving main {svc} service loop",
                nameof(MessageProcessingService)
                );
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogCritical(
                ex,
                "The {svc} service failed while running!",
                nameof(MessageProcessingService)
                );
        }
        finally
        {
            // Log what happened.
            _logger.LogDebug(
                "{svc} task is stopped.",
                nameof(MessageProcessingService)
                );
        }
    }

    #endregion
}
