﻿
namespace CG.Purple.Directors;

/// <summary>
/// This interface represents an object that performs message processing operations.
/// </summary>
public interface IPipelineDirector
{
    /// <summary>
    /// This method processes messages.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task ProcessAsync(
        CancellationToken cancellationToken = default
        );
}