
namespace CG.Purple.Host.Services;

/// <summary>
/// This class is a hosted service that continuously pushes messages 
/// through a processing pipeline.
/// </summary>
internal class PipelineService : BackgroundService
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the options for this service.
    /// </summary>
    internal protected readonly PipelineServiceOptions? _pipelineServiceOptions;

    /// <summary>
    /// This field contains the service provider for this service.
    /// </summary>
    internal protected readonly IServiceProvider _serviceProvider = null!;

    /// <summary>
    /// This field contains the logger for this service.
    /// </summary>
    internal protected readonly ILogger<PipelineService> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PipelineService"/>
    /// class.
    /// </summary>
    /// <param name="hostedServiceOptions">The hosted service options to 
    /// use with this service.</param>
    /// <param name="serviceProvider">The service provider to use with
    /// this service.</param>
    /// <param name="logger">The logger to use with this service.</param>
    public PipelineService(
        IOptions<HostedServiceOptions> hostedServiceOptions,
        IServiceProvider serviceProvider,
        ILogger<PipelineService> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(hostedServiceOptions, nameof(hostedServiceOptions))
            .ThrowIfNull(serviceProvider, nameof(serviceProvider))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _pipelineServiceOptions = hostedServiceOptions.Value.PipelineService;
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
    /// <param name="cancellationToken">A cancellation token that is signaled
    /// when the service is stopping.</param>
    /// <returns>A task to perform the operation.</returns>
    protected override async Task ExecuteAsync(
        CancellationToken cancellationToken
        )
    {
        try
        {
            // Log what we are  about to do.
            _logger.LogInformation(
                "The {svc} service is running.",
                nameof(PipelineService)
                );

            // Register for the stop signal.
            cancellationToken.Register(() =>
                _logger.LogInformation(
                    "{svc} task is stopping.",
                    nameof(PipelineService)
                    )
                );

            // Get the startup delay.
            var startupDelay = _pipelineServiceOptions?.StartupDelay
                ?? TimeSpan.FromSeconds(5);

            // Sanity check the value.
            if (startupDelay < TimeSpan.FromSeconds(5))
            {
                startupDelay = TimeSpan.FromSeconds(5);
            }
            else if (startupDelay > TimeSpan.FromMinutes(2))
            {
                startupDelay = TimeSpan.FromMinutes(2);
            }

            // Get the section delay.
            var sectionDelay = _pipelineServiceOptions?.SectionDelay
                ?? TimeSpan.FromMilliseconds(500);

            // Sanity check the value.
            if (sectionDelay < TimeSpan.FromMilliseconds(500))
            {
                sectionDelay = TimeSpan.FromMilliseconds(500);
            }
            else if (sectionDelay > TimeSpan.FromSeconds(2))
            {
                sectionDelay = TimeSpan.FromSeconds(2);
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Pausing the {svc} startup for {ts}.",
                nameof(PipelineService),
                startupDelay
            );

            // Let's not work too soon.
            await Task.Delay(
                startupDelay,
                cancellationToken
                );
            
            // Log what we are about to do.
            _logger.LogDebug(
                "Entering {svc} main loop",
                nameof(PipelineService)
                );

            // While the service is running ...
            while (!cancellationToken.IsCancellationRequested)
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating a DI scope"
                    );

                // Create a DI scope.
                using var scope = _serviceProvider.CreateScope();

                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating an {name} instance",
                    nameof(IPipelineDirector)
                    );

                // Get a scoped pipeline director.
                var pipelineDirector = scope.ServiceProvider.GetRequiredService<
                    IPipelineDirector
                    >();                

                // Start the stopwatch.
                var sw = new Stopwatch();
                sw.Start();
                try
                {
                    // Log what we are about to do.
                    _logger.LogTrace(
                        "Deferring to {name}",
                        nameof(IPipelineDirector.ProcessAsync)
                        );

                    // Process pending messages.
                    await pipelineDirector.ProcessAsync(
                        sectionDelay,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
                finally
                {
                    // Stop the stopwatch.
                    sw.Stop();
                    
                    // Log what we did.
                    _logger.LogTrace(
                        "{name} finished in {elapsed}",
                        nameof(IPipelineDirector.ProcessAsync),
                        sw.Elapsed
                        );
                }
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "The {svc} service failed while running!",
                nameof(PipelineService)
                );
        }
        finally
        {
            // Log what happened.
            _logger.LogInformation(
                "{svc} task is stopped.",
                nameof(PipelineService)
                );
        }
    }

    #endregion
}
