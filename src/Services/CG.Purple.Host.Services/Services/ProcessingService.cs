﻿
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
                // Get the duration.
                var delayDuration = _hostedServiceOptions.Value.MessageProcessing.StartupDelay.Value;

                // Sanity check the duration.
                if (delayDuration < TimeSpan.FromSeconds(1))
                {
                    delayDuration = TimeSpan.FromSeconds(1);
                }

                // Log what we are about to do.
                _logger.LogDebug(
                    "Pausing the {svc} service startup for {ts}.",
                    nameof(ProcessingService),
                    delayDuration
                );

                // Let's not work too soon.
                await Task.Delay(
                    delayDuration,
                    stoppingToken
                    );
            }
            else
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Pausing the {svc} service startup for {ts}.",
                    nameof(ProcessingService),
                    TimeSpan.FromSeconds(1)
                );

                // Let's not work too soon.
                await Task.Delay(
                    TimeSpan.FromSeconds(1),
                    stoppingToken
                    );
            }
            
            // Log what we are about to do.
            _logger.LogDebug(
                "Entering main {svc} service loop",
                nameof(ProcessingService)
                );

            // The mail loop doesn't contain a try/catch, which means we
            //   want anything that throws an exception here to stop
            //   processing for the website. That's because the only
            //   exceptions that will get to this point are critical
            //   errors that have no recovery option(s).

            // While the service is running ...
            while (!stoppingToken.IsCancellationRequested)
            {
                // =======
                // Step 1: stand up the DI scope.
                // =======

                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating a DI scope"
                    );

                // Create a DI scope.
                using var scope = _serviceProvider.CreateScope();

                // =======
                // Step 2: process any messages that are waiting.
                // =======

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
                    "Deferring to {name}",
                    nameof(IProcessDirector.ProcessMessagesAsync)
                    );

                // Start the stopwatch.
                var sw = new Stopwatch();
                sw.Start();
                try
                {
                    // Process pending messages.
                    await processDirector.ProcessMessagesAsync(
                        stoppingToken
                        ).ConfigureAwait(false);
                }
                finally
                {
                    // Stop the stopwatch.
                    sw.Stop();
                    
                    // Log what we did.
                    _logger.LogTrace(
                        "{name} finished in {elapsed}",
                        nameof(IProcessDirector.ProcessMessagesAsync),
                        sw.Elapsed
                        );
                }

                // =======
                // Step 3: retry any messages that are eligible.
                // =======

                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating an {name} instance",
                    nameof(IRetryDirector)
                    );

                // Get a scoped process director.
                var retryDirector = scope.ServiceProvider.GetRequiredService<
                    IRetryDirector
                    >();

                // Were options provided?
                var maxErrorCount = 0;
                if (_hostedServiceOptions.Value.MessageProcessing is not null &&
                    _hostedServiceOptions.Value.MessageProcessing.MaxErrorCount.HasValue)
                {
                    // Set the max error count.
                    maxErrorCount = _hostedServiceOptions.Value.MessageProcessing.MaxErrorCount.Value;

                    // Sanity check the value.
                    if (maxErrorCount < 3)
                    {
                        maxErrorCount = 3;
                    }

                    // Log what we did.
                    _logger.LogDebug(
                        "Setting the max error count to: {count}.",
                        maxErrorCount
                    );
                }
                else
                {
                    // Set the max error count.
                    maxErrorCount = 3;

                    // Log what we did.
                    _logger.LogDebug(
                        "Setting the max error count to: {count} since the 'MaxErrorCount' " +
                        "setting was missing.",
                        maxErrorCount
                    );                    
                }

                // Log what we are about to do.
                _logger.LogDebug(
                    "Deferring to {name}",
                    nameof(IRetryDirector.RetryMessagesAsync)
                    );

                // Restart the stopwatch.
                sw.Restart();
                try
                {
                    // Retry failed messages.
                    await retryDirector.RetryMessagesAsync(
                        maxErrorCount,
                        stoppingToken
                        ).ConfigureAwait(false);
                }
                finally
                {
                    // Stop the stopwatch.
                    sw.Stop();

                    // Log what we did.
                    _logger.LogTrace(
                        "{name} finished in {elapsed}",
                        nameof(IRetryDirector.RetryMessagesAsync),
                        sw.Elapsed
                        );
                }

                // =======
                // Step 4: archive any messages that are due.
                // =======

                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating an {name} instance",
                    nameof(IArchiveDirector)
                    );

                // Get a scoped archive director.
                var archiveDirector = scope.ServiceProvider.GetRequiredService<
                    IArchiveDirector
                    >();

                // Were options provided?
                var maxDaysToLive = 0;
                if (_hostedServiceOptions.Value.MessageProcessing is not null &&
                    _hostedServiceOptions.Value.MessageProcessing.MaxDaysToLive.HasValue)
                {
                    // Set the max days to live value.
                    maxDaysToLive = _hostedServiceOptions.Value.MessageProcessing.MaxDaysToLive.Value;

                    // Sanity check the value.
                    if (maxDaysToLive < 7)
                    {
                        maxDaysToLive = 7;
                    }

                    // Log what we did.
                    _logger.LogDebug(
                        "Setting the max days to live value to: {value}.",
                        maxDaysToLive
                    );
                }
                else
                {
                    // Set the max error count.
                    maxDaysToLive = 7;

                    // Log what we did.
                    _logger.LogDebug(
                        "Setting the max days to live value to: {value} since the " +
                        "'MaxDaysToLive' setting was missing.",
                        maxDaysToLive
                    );
                }

                // Log what we are about to do.
                _logger.LogDebug(
                    "Deferring to {name}",
                    nameof(IArchiveDirector.ArchiveMessagesAsync)
                    );

                // Restart the stopwatch.
                sw.Restart();
                try
                {
                    // Archive terminal messages.
                    await archiveDirector.ArchiveMessagesAsync(
                        maxDaysToLive,
                        "host",
                        stoppingToken
                        ).ConfigureAwait(false);
                }
                finally
                {
                    // Stop the stopwatch.
                    sw.Stop();

                    // Log what we did.
                    _logger.LogTrace(
                        "{name} finished in {elapsed}",
                        nameof(IArchiveDirector.ArchiveMessagesAsync),
                        sw.Elapsed
                        );
                }

                // =======
                // Step 5: pause for a bit, so we don't hammer the CPU.
                // =======

                // Were options provided?
                if (_hostedServiceOptions.Value.MessageProcessing is not null &&
                    _hostedServiceOptions.Value.MessageProcessing.ThrottleDuration is not null)
                {
                    // Get the pause duration.
                    var durationValue = _hostedServiceOptions.Value.MessageProcessing.ThrottleDuration.Value;

                    // Sanity check the value.
                    if (durationValue < TimeSpan.FromSeconds(5))
                    {
                        durationValue = TimeSpan.FromSeconds(5);
                    }

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Pausing the {svc} service for {time}.",
                        nameof(ProcessingService),
                        durationValue
                    );

                    // Let's not work too hard.
                    await Task.Delay(
                        durationValue,
                        stoppingToken
                        );
                }
                else
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Pausing the {svc} service for {time}, since the " +
                        "'ThrottleDuration' setting was missing.",
                        nameof(ProcessingService),
                        TimeSpan.FromSeconds(5)
                    );

                    // Let's not work too hard.
                    await Task.Delay(
                        TimeSpan.FromSeconds(5),
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
            _logger.LogError(
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