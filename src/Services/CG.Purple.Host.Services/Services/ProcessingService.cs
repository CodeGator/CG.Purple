
namespace CG.Purple.Host.Services;

/// <summary>
/// This class is a message processing service.
/// </summary>
internal class ProcessingService : BackgroundService
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the message processing options for this service.
    /// </summary>
    internal protected readonly IOptions<HostedServiceOptions> _hostedServiceOptions = null!;

    /// <summary>
    /// This field contains the service provider for this service.
    /// </summary>
    internal protected readonly IServiceProvider _serviceProvider = null!;

    /// <summary>
    /// This field contains the logger for this service.
    /// </summary>
    internal protected readonly ILogger<ProcessingService> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProcessingService"/>
    /// class.
    /// </summary>
    /// <param name="hostedServiceOptions">The hosted service options to 
    /// use with this service.</param>
    /// <param name="serviceProvider">The service provider to use with
    /// this service.</param>
    /// <param name="logger">The logger to use with this service.</param>
    public ProcessingService(
        IOptions<HostedServiceOptions> hostedServiceOptions,
        IServiceProvider serviceProvider,
        ILogger<ProcessingService> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(hostedServiceOptions, nameof(hostedServiceOptions))
            .ThrowIfNull(serviceProvider, nameof(serviceProvider))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _hostedServiceOptions = hostedServiceOptions;
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
                nameof(ProcessingService)
                );

            // Register for the stop signal.
            stoppingToken.Register(() =>
                _logger.LogDebug(
                    "{svc} task is stopping.",
                    nameof(ProcessingService)
                    )
                );

            // Were options provided?
            if (_hostedServiceOptions.Value.MessageProcessing is not null &&
                _hostedServiceOptions.Value.MessageProcessing.StartupDelay is not null)
            {
                // Log what we are about to do.
                _logger.LogInformation(
                    "Pausing the {svc} service startup for {time}.",
                    nameof(ProcessingService),
                    TimeSpan.FromSeconds(30)
                );

                // Let's not work tooo soon.
                await Task.Delay(
                    _hostedServiceOptions.Value.MessageProcessing.StartupDelay.Value,
                    stoppingToken
                    );
            }           

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
                nameof(ProcessingService)
                );

            // While the service is running ...
            while (!stoppingToken.IsCancellationRequested)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Deferring to {name}",
                    nameof(IProcessDirector.ProcessMessagesAsync)
                    );

                // Process pending messages.
                await processDirector.ProcessMessagesAsync(
                    stoppingToken
                    ).ConfigureAwait(false);

                // Were options provided?
                if (_hostedServiceOptions.Value.MessageProcessing is not null &&
                    _hostedServiceOptions.Value.MessageProcessing.ThrottleDuration is not null)
                {
                    // Log what we are about to do.
                    _logger.LogInformation(
                        "Pausing the {svc} service iteration for {time}.",
                        nameof(ProcessingService),
                        _hostedServiceOptions.Value.MessageProcessing.ThrottleDuration.Value
                    );

                    // Let's not work tooo soon.
                    await Task.Delay(
                        _hostedServiceOptions.Value.MessageProcessing.ThrottleDuration.Value,
                        stoppingToken
                        );
                }
                else
                {
                    // If no throttle was specified, use this default.
                    
                    // Log what we are about to do.
                    _logger.LogInformation(
                        "Pausing the {svc} service iteration for {time}.",
                        nameof(ProcessingService),
                        TimeSpan.FromSeconds(10)
                    );

                    // Let's not work tooo soon.
                    await Task.Delay(
                        TimeSpan.FromSeconds(10),
                        stoppingToken
                        );
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Leaving main {svc} service loop",
                nameof(ProcessingService)
                );
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogCritical(
                ex,
                "The {svc} service failed while running!",
                nameof(ProcessingService)
                );
        }
        finally
        {
            // Log what happened.
            _logger.LogDebug(
                "{svc} task is stopped.",
                nameof(ProcessingService)
                );
        }
    }

    #endregion
}
