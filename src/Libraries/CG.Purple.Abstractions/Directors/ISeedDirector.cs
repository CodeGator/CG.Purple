
namespace CG.Purple.Directors;

/// <summary>
/// This interface represents an object that performs seeding operations.
/// </summary>
public interface ISeedDirector
{
    /// <summary>
    /// This method performs a seeding operation for <see cref="MailMessage"/>
    /// objects.
    /// </summary>
    /// <param name="configuration">The configuration to use for the 
    /// operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when 
    /// there are existing <see cref="MailMessage"/> objects in the underlying
    /// data-store; <c>false</c> otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task SeedMailMessagesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method performs a seeding operation for <see cref="MimeType"/>
    /// objects.
    /// </summary>
    /// <param name="configuration">The configuration to use for the 
    /// operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when 
    /// there are existing <see cref="MimeType"/> objects in the underlying
    /// data-store; <c>false</c> otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task SeedMimeTypesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,        
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method performs a seeding operation for <see cref="ParameterType"/>
    /// objects.
    /// </summary>
    /// <param name="configuration">The configuration to use for the 
    /// operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when 
    /// there are existing <see cref="ParameterType"/> objects in the underlying
    /// data-store; <c>false</c> otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task SeedParameterTypesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method performs a seeding operation for <see cref="PropertyType"/>
    /// objects.
    /// </summary>
    /// <param name="configuration">The configuration to use for the 
    /// operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when 
    /// there are existing <see cref="PropertyType"/> objects in the underlying
    /// data-store; <c>false</c> otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task SeedPropertyTypesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method performs a seeding operation for <see cref="ProviderParameter"/>
    /// objects.
    /// </summary>
    /// <param name="configuration">The configuration to use for the 
    /// operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when 
    /// there are existing <see cref="ProviderParameter"/> objects in the underlying
    /// data-store; <c>false</c> otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task SeedProviderParametersAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method performs a seeding operation for <see cref="ProviderType"/>
    /// objects.
    /// </summary>
    /// <param name="configuration">The configuration to use for the 
    /// operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when 
    /// there are existing <see cref="ProviderType"/> objects in the underlying
    /// data-store; <c>false</c> otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task SeedProviderTypesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method performs a seeding operation for <see cref="ProviderLog"/>
    /// objects.
    /// </summary>
    /// <param name="configuration">The configuration to use for the 
    /// operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when 
    /// there are existing <see cref="ProviderLog"/> objects in the underlying
    /// data-store; <c>false</c> otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task SeedProviderLogsAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// This method performs a seeding operation for <see cref="TextMessage"/>
    /// objects.
    /// </summary>
    /// <param name="configuration">The configuration to use for the 
    /// operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the seeding operation when 
    /// there are existing <see cref="TextMessage"/> objects in the underlying
    /// data-store; <c>false</c> otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever the
    /// director fails to complete the operation.</exception>
    Task SeedTextMessagesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        );
}
