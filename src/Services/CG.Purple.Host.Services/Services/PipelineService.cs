﻿
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
    /// This field contains the message processing options for this service.
    /// </summary>
    internal protected readonly HostedServiceOptions _hostedServiceOptions = null!;

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
        _hostedServiceOptions = hostedServiceOptions.Value;
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

            // Were options provided?
            if (_hostedServiceOptions.Pipeline is not null &&
                _hostedServiceOptions.Pipeline.StartupDelay is not null)
            {
                // Get the duration.
                var delayDuration = _hostedServiceOptions.Pipeline.StartupDelay.Value;

                // Sanity check the duration.
                if (delayDuration < TimeSpan.FromSeconds(5))
                {
                    delayDuration = TimeSpan.FromSeconds(5);
                }

                // Log what we are about to do.
                _logger.LogDebug(
                    "Pausing the {svc} startup for {ts}.",
                    nameof(PipelineService),
                    delayDuration
                );

                // Let's not work too soon.
                await Task.Delay(
                    delayDuration,
                    cancellationToken
                    );
            }
            else
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Pausing the {svc} startup for {ts}.",
                    nameof(PipelineService),
                    TimeSpan.FromSeconds(5)
                );

                // Let's not work too soon.
                await Task.Delay(
                    TimeSpan.FromSeconds(5),
                    cancellationToken
                    );
            }
            
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