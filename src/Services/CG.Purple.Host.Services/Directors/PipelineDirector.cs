
namespace CG.Purple.Host.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IPipelineDirector"/>
/// </summary>
internal class PipelineDirector : IPipelineDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<IPipelineDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PipelineDirector"/>
    /// class.
    /// </summary>
    /// <param name="logger">The logger to use with this director.</param>
    public PipelineDirector(
        ILogger<IPipelineDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task ProcessAsync(
        CancellationToken cancellationToken = default
        )
    {
        try
        {


        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to process messages!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to process messages!",
                innerException: ex
                );
        }
    }

    #endregion
}
