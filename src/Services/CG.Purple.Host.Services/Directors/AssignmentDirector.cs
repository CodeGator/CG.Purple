
namespace CG.Purple.Host.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="IAssignmentDirector"/>
/// </summary>
internal class AssignmentDirector : IAssignmentDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<IArchiveDirector> _logger = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ArchiveDirector"/>
    /// class.
    /// </summary>
    /// <param name="logger">The logger to use with this director.</param>
    public AssignmentDirector(
        ILogger<IArchiveDirector> logger
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
    public virtual async Task<ProviderType> SelectProviderAsync(
        Message message,
        IEnumerable<ProviderType> availableProviders,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(message, nameof(message))
            .ThrowIfNull(availableProviders, nameof(availableProviders));

        try
        {
            // TODO : write the code for this.
            return null;
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to select a provider type for message: {id}!",
                message.Id
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to select a provider type for " +
                $"message: {message.Id}!",
                innerException: ex
                );
        }
    }

    #endregion
}
