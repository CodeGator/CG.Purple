﻿
using CG.Purple.Seeding.Options;

namespace CG.Purple.Seeding.Directors;

/// <summary>
/// This class is a default implementation of the <see cref="ISeedDirector"/>
/// interface.
/// </summary>
internal class SeedDirector : ISeedDirector
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the attachment manager for this director.
    /// </summary>
    internal protected readonly IAttachmentManager _attachmentManager;

    /// <summary>
    /// This field contains the file type manager for this director.
    /// </summary>
    internal protected readonly IFileTypeManager _fileTypeManager;

    /// <summary>
    /// This field contains the mail message manager for this director.
    /// </summary>
    internal protected readonly IMailMessageManager _mailMessageManager;

    /// <summary>
    /// This field contains the mime type manager for this director.
    /// </summary>
    internal protected readonly IMimeTypeManager _mimeTypeManager;

    /// <summary>
    /// This field contains the parameter type manager for this director.
    /// </summary>
    internal protected readonly IParameterTypeManager _parameterTypeManager;

    /// <summary>
    /// This field contains the property type manager for this director.
    /// </summary>
    internal protected readonly IPropertyTypeManager _propertyTypeManager;

    /// <summary>
    /// This field contains the provider parameter manager for this director.
    /// </summary>
    internal protected readonly IProviderParameterManager _providerParameterManager;

    /// <summary>
    /// This field contains the provider type manager for this director.
    /// </summary>
    internal protected readonly IProviderTypeManager _providerTypeManager;

    /// <summary>
    /// This field contains the text message manager for this director.
    /// </summary>
    internal protected readonly ITextMessageManager _textMessageManager;

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<ISeedDirector> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="SeedDirector"/>
    /// class.
    /// </summary>
    /// <param name="attachmenteManager">The attachment manager to use with 
    /// this director.</param>
    /// <param name="mailMessageManager">The mail message manager to use with 
    /// this director.</param>
    /// <param name="fileTypeManager">The file type manager to use with this
    /// director.</param>
    /// <param name="mimeTypeManager">The mime type manager to use with this
    /// director.</param>
    /// <param name="parameterTypeManager">The parameter type manager to use 
    /// with this director.</param>
    /// <param name="propertyTypeManager">The property type manager to use 
    /// with this director.</param>
    /// <param name="providerParameterManager">The provider parameter manager
    /// to use with this director.</param>
    /// <param name="providerTypeManager">The provider type manager to use 
    /// with this director.</param>
    /// <param name="textMessageManager">The text message manager to use with 
    /// this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public SeedDirector(
        IAttachmentManager attachmenteManager,
        IFileTypeManager fileTypeManager,
        IMailMessageManager mailMessageManager,
        IMimeTypeManager mimeTypeManager,
        IParameterTypeManager parameterTypeManager,
        IPropertyTypeManager propertyTypeManager,
        IProviderParameterManager providerParameterManager,
        IProviderTypeManager providerTypeManager,
        ITextMessageManager textMessageManager,
        ILogger<ISeedDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(attachmenteManager, nameof(attachmenteManager))
            .ThrowIfNull(fileTypeManager, nameof(fileTypeManager))
            .ThrowIfNull(mailMessageManager, nameof(mailMessageManager))
            .ThrowIfNull(mimeTypeManager, nameof(mimeTypeManager))
            .ThrowIfNull(parameterTypeManager, nameof(parameterTypeManager))
            .ThrowIfNull(propertyTypeManager, nameof(propertyTypeManager))
            .ThrowIfNull(providerParameterManager, nameof(providerParameterManager))
            .ThrowIfNull(providerTypeManager, nameof(providerTypeManager))
            .ThrowIfNull(textMessageManager, nameof(textMessageManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _attachmentManager = attachmenteManager;
        _fileTypeManager = fileTypeManager;
        _mailMessageManager = mailMessageManager;
        _mimeTypeManager = mimeTypeManager;
        _parameterTypeManager = parameterTypeManager;
        _propertyTypeManager = propertyTypeManager;
        _providerParameterManager = providerParameterManager;
        _providerTypeManager = providerTypeManager;
        _textMessageManager = textMessageManager;
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <inheritdoc/>
    public virtual async Task SeedMailMessagesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configuration, nameof(configuration));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Binding mail message options."
                );

            // Bind the options.
            var mailMessageOptions = new List<MailMessageOptions>();
            configuration.GetSection("MailMessages").Bind(mailMessageOptions);

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding mail messages from the configuration."
                );

            // Seed the mail messages from the options.
            await SeedMailMessagesAsync(
                mailMessageOptions,
                userName,
                force,
                cancellationToken
                );
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed mail messages!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed mail messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task SeedMimeTypesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configuration, nameof(configuration));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Binding mime type options."
                );

            // Bind the options.
            var mimeTypeOptions = new List<MimeTypeOptions>();
            configuration.GetSection("MimeTypes").Bind(mimeTypeOptions);

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding mime types from the configuration."
                );

            // Seed the mime types from the options.
            await SeedMimeTypesAsync(
                mimeTypeOptions,
                userName,
                force,
                cancellationToken                
                );
        }
        catch (Exception ex) 
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed mime types!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed mime types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task SeedParameterTypesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configuration, nameof(configuration));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Binding parameter type options."
                );

            // Bind the options.
            var parameterTypeOptions = new List<ParameterTypeOptions>();
            configuration.GetSection("ParameterTypes").Bind(parameterTypeOptions);

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding parameter types from the configuration."
                );

            // Seed the parameter types from the options.
            await SeedParameterTypesAsync(
                parameterTypeOptions,
                userName,
                force,
                cancellationToken
                );
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed parameter types!"
                );

            // Parameter better context.
            throw new DirectorException(
                message: $"The director failed to seed parameter types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task SeedPropertyTypesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configuration, nameof(configuration));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Binding property type options."
                );

            // Bind the options.
            var propertyTypeOptions = new List<PropertyTypeOptions>();
            configuration.GetSection("PropertyTypes").Bind(propertyTypeOptions);

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding property types from the configuration."
                );

            // Seed the property types from the options.
            await SeedPropertyTypesAsync(
                propertyTypeOptions,
                userName,
                force,
                cancellationToken
                );
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed property types!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed property types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task SeedProviderParametersAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configuration, nameof(configuration));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Binding provider parameter options."
                );

            // Bind the options.
            var providerParameterOptions = new List<ProviderParameterOptions>();
            configuration.GetSection("ProviderParameters").Bind(providerParameterOptions);

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding provider parameters from the configuration."
                );

            // Seed the provider parameters from the options.
            await SeedProviderParametersAsync(
                providerParameterOptions,
                userName,
                force,
                cancellationToken
                );
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed provider parameters!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed provider parameters!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task SeedProviderTypesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configuration, nameof(configuration));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Binding provider type options."
                );

            // Bind the options.
            var providerTypeOptions = new List<ProviderTypeOptions>();
            configuration.GetSection("ProviderTypes").Bind(providerTypeOptions);

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding provider types from the configuration."
                );

            // Seed the provider types from the options.
            await SeedProviderTypesAsync(
                providerTypeOptions,
                userName,
                force,
                cancellationToken
                );
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed provider types!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed provider types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <inheritdoc/>
    public virtual async Task SeedTextMessagesAsync(
        IConfiguration configuration,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(configuration, nameof(configuration));

        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Binding text message options."
                );

            // Bind the options.
            var textMessageOptions = new List<TextMessageOptions>();
            configuration.GetSection("TextMessages").Bind(textMessageOptions);

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding text messages from the configuration."
                );

            // Seed the text messages from the options.
            await SeedTextMessagesAsync(
                textMessageOptions,
                userName,
                force,
                cancellationToken
                );
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed text messages!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed text messages!",
                innerException: ex
                );
        }
    }

    #endregion

    // *******************************************************************
    // Private methods.
    // *******************************************************************

    #region Private methods

    /// <summary>
    /// This method performs a seeding operation for the given list of
    /// <see cref="MailMessageOptions"/> objects.
    /// </summary>
    /// <param name="mailMessageOptions">The options to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the operation; <c>false</c>
    /// otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="DirectorException"></exception>
    private async Task SeedMailMessagesAsync(
        List<MailMessageOptions> mailMessageOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Should we check for existing data?
            if (!force)
            {
                // Are the existing mail messages?
                var hasExistingData = await _mailMessageManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding mail messages because the 'force' flag " +
                        "was not specified and there are existing mail messages " +
                        "in the database.",
                        mailMessageOptions.Count
                        );
                    return; // Nothing else to do!
                }

                // Are there existing file types?
                hasExistingData = await _fileTypeManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding mail messages because the 'force' flag " +
                        "was not specified and there are existing mail messages " +
                        "in the database.",
                        mailMessageOptions.Count
                        );
                    return; // Nothing else to do!
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding {count} mail messages.",
                mailMessageOptions.Count
                );

            // Loop through the options.
            foreach (var mailMessageOption in mailMessageOptions)
            {
                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(IMailMessageManager.CreateAsync)
                    );

                // Create the mail message.
                var mailMessage = await _mailMessageManager.CreateAsync(
                    new MailMessage()
                    {
                        To = mailMessageOption.To,
                        CC = mailMessageOption.CC,
                        BCC = mailMessageOption.BCC,
                        Subject = mailMessageOption.Subject,
                        Body = mailMessageOption.Body,
                        IsHtml = mailMessageOption.IsHtml,
                        IsDisabled = mailMessageOption.IsDisabled,  
                    },
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Are there any attachments?
                if (mailMessageOption.Attachments.Any())
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Seeding {count} attachments.",
                        mailMessageOption.Attachments.Count
                        );

                    // Loop through the options.
                    foreach (var attachment in mailMessageOption.Attachments)
                    {
                        // Is the file missing?
                        if (!File.Exists(attachment))
                        {
                            // Panic!!
                            throw new FileNotFoundException(
                                $"The attachment: {attachment} is missing!"
                                );
                        }

                        // Get the attachments' file extension.
                        var ext = Path.GetExtension(attachment);

                        // Look for a mime type, for that extension.
                        var mimeType = await _mimeTypeManager.FindByExtensionAsync(
                            ext,
                            cancellationToken
                            ).ConfigureAwait(false);

                        // Did we fail?
                        if (mimeType is null)
                        {
                            // Panic!!
                            throw new KeyNotFoundException(
                                $"No mime type was found for extension: {ext} is missing!"
                                );
                        }

                        // Read the bytes for the file.
                        var bytes = await File.ReadAllBytesAsync(
                            attachment,
                            cancellationToken
                            ).ConfigureAwait(false);

                        // Create the attachment.
                        _ = await _attachmentManager.CreateAsync(
                            new Attachment()
                            {
                                Message = mailMessage,
                                MimeType = mimeType,
                                OriginalFileName = Path.GetFileName(attachment),
                                Length = bytes.Length,
                                Data = bytes
                            },
                            userName,
                            cancellationToken
                            ).ConfigureAwait(false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed mail messages!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed mail messages!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a seeding operation for the given list of
    /// <see cref="MimeTypeOptions"/> objects.
    /// </summary>
    /// <param name="mimeTypeOptions">The options to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the operation; <c>false</c>
    /// otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="DirectorException"></exception>
    private async Task SeedMimeTypesAsync(
        List<MimeTypeOptions> mimeTypeOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Should we check for existing data?
            if (!force)
            {
                // Are the existing mime types?
                var hasExistingData = await _mimeTypeManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding mime types because the 'force' flag " +
                        "was not specified and there are existing mime types " +
                        "in the database.",
                        mimeTypeOptions.Count
                        );
                    return; // Nothing else to do!
                }

                // Are there existing file types?
                hasExistingData = await _fileTypeManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding mime types because the 'force' flag " +
                        "was not specified and there are existing file types " +
                        "in the database.",
                        mimeTypeOptions.Count
                        );
                    return; // Nothing else to do!
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding {count} mime types.",
                mimeTypeOptions.Count
                );

            // Loop through the options.
            foreach (var mimeTypeOption in mimeTypeOptions ) 
            {
                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(IMimeTypeManager.CreateAsync)
                    );

                // Create the mime type.
                var mimeType = await _mimeTypeManager.CreateAsync(
                    new MimeType()
                    {
                        Type = mimeTypeOption.Type,
                        SubType = mimeTypeOption.SubType
                    },
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Log what we are about to do.
                _logger.LogDebug(
                    "Seeding {count} file types for mime type: {mt}.",
                    mimeTypeOption.Extensions.Count,
                    $"{mimeType.Type}/{mimeType.SubType}"
                    );

                // Loop through any associated file extensions.
                foreach (var extension in mimeTypeOption.Extensions)
                {
                    // Log what we are about to do.
                    _logger.LogTrace(
                        "Deferring to {name}",
                        nameof(IFileTypeManager.CreateAsync)
                        );

                    // Create the file type.
                    _ = await _fileTypeManager.CreateAsync(
                        new FileType()
                        {
                            MimeType = mimeType,
                            Extension = extension,  
                        },
                        userName,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed mime types!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed mime types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a seeding operation for the given list of
    /// <see cref="ParameterTypeOptions"/> objects.
    /// </summary>
    /// <param name="parameterTypeOptions">The options to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the operation; <c>false</c>
    /// otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="DirectorException"></exception>
    private async Task SeedParameterTypesAsync(
        List<ParameterTypeOptions> parameterTypeOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Should we check for existing data?
            if (!force)
            {
                // Are the existing parameter types?
                var hasExistingData = await _parameterTypeManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding parameter types because the 'force' flag " +
                        "was not specified and there are existing parameter types " +
                        "in the database.",
                        parameterTypeOptions.Count
                        );
                    return; // Nothing else to do!
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding {count} parameter types.",
                parameterTypeOptions.Count
                );

            // Loop through the options.
            foreach (var parameterTypeOption in parameterTypeOptions)
            {
                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(IParameterTypeManager.CreateAsync)
                    );

                // Create the parameter type.
                _ = await _parameterTypeManager.CreateAsync(
                    new ParameterType()
                    {
                        Name = parameterTypeOption.Name,
                        Description = parameterTypeOption.Description
                    },
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed parameter types!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed parameter types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a seeding operation for the given list of
    /// <see cref="PropertyTypeOptions"/> objects.
    /// </summary>
    /// <param name="propertyTypeOptions">The options to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the operation; <c>false</c>
    /// otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="DirectorException"></exception>
    private async Task SeedPropertyTypesAsync(
        List<PropertyTypeOptions> propertyTypeOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Should we check for existing data?
            if (!force)
            {
                // Are the existing property types?
                var hasExistingData = await _propertyTypeManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding property types because the 'force' flag " +
                        "was not specified and there are existing property types " +
                        "in the database.",
                        propertyTypeOptions.Count
                        );
                    return; // Nothing else to do!
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding {count} property types.",
                propertyTypeOptions.Count
                );

            // Loop through the options.
            foreach (var propertyTypeOption in propertyTypeOptions)
            {
                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(IPropertyTypeManager.CreateAsync)
                    );

                // Create the property type.
                _ = await _propertyTypeManager.CreateAsync(
                    new PropertyType()
                    {
                        Name = propertyTypeOption.Name,
                        Description = propertyTypeOption.Description
                    },
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed property types!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed property types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a seeding operation for the given list of
    /// <see cref="ProviderParameterOptions"/> objects.
    /// </summary>
    /// <param name="providerParameterOptions">The options to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the operation; <c>false</c>
    /// otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="DirectorException"></exception>
    private async Task SeedProviderParametersAsync(
        List<ProviderParameterOptions> providerParameterOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Should we check for existing data?
            if (!force)
            {
                // Are the existing provider parameters?
                var hasExistingData = await _providerParameterManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding provider parameters because the 'force' flag " +
                        "was not specified and there are existing provider parameters " +
                        "in the database.",
                        providerParameterOptions.Count
                        );
                    return; // Nothing else to do!
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding {count} provider parameters.",
                providerParameterOptions.Count
                );

            // Loop through the options.
            foreach (var providerParameterOption in providerParameterOptions)
            {
                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(IProviderTypeManager.CreateAsync)
                    );

                // Look for the provider type.
                var providerType = await _providerTypeManager.FindByNameAsync(
                    providerParameterOption.ProviderTypeName,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Did we fail?
                if (providerType == null)
                {
                    // Panic!!
                    throw new KeyNotFoundException(
                        $"Provider Type Name: '{providerParameterOption.ProviderTypeName}' " +
                        "was not found!"
                        );
                }

                // Look for the parameter type.
                var parameterType = await _parameterTypeManager.FindByNameAsync(
                    providerParameterOption.ParameterTypeName,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Did we fail?
                if (parameterType == null)
                {
                    // Panic!!
                    throw new KeyNotFoundException(
                        $"Parameter Type Name: '{providerParameterOption.ParameterTypeName}' " +
                        "was not found!"
                        );
                }

                // Create the provider parameter type.
                _ = await _providerParameterManager.CreateAsync(
                    new ProviderParameter()
                    {
                        ProviderTypeId = providerType.Id,
                        ParameterTypeId = parameterType.Id,
                        Value = providerParameterOption.Value
                    },
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed provider parameters!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed provider parameters!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a seeding operation for the given list of
    /// <see cref="ProviderTypeOptions"/> objects.
    /// </summary>
    /// <param name="providerTypeOptions">The options to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the operation; <c>false</c>
    /// otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="DirectorException"></exception>
    private async Task SeedProviderTypesAsync(
        List<ProviderTypeOptions> providerTypeOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Should we check for existing data?
            if (!force)
            {
                // Are the existing provider types?
                var hasExistingData = await _providerTypeManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding provider types because the 'force' flag " +
                        "was not specified and there are existing provider types " +
                        "in the database.",
                        providerTypeOptions.Count
                        );
                    return; // Nothing else to do!
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding {count} provider types.",
                providerTypeOptions.Count
                );

            // Loop through the options.
            foreach (var providerTypeOption in providerTypeOptions)
            {
                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(IProviderTypeManager.CreateAsync)
                    );

                // Create the provider type.
                _ = await _providerTypeManager.CreateAsync(
                    new ProviderType()
                    {
                        Name = providerTypeOption.Name,
                        Description = providerTypeOption.Description
                    },
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed provider types!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed provider types!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a seeding operation for the given list of
    /// <see cref="TextMessageOptions"/> objects.
    /// </summary>
    /// <param name="textMessageOptions">The options to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the operation; <c>false</c>
    /// otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="DirectorException"></exception>
    private async Task SeedTextMessagesAsync(
        List<TextMessageOptions> textMessageOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Should we check for existing data?
            if (!force)
            {
                // Are the existing text messages?
                var hasExistingData = await _textMessageManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding text messages because the 'force' flag " +
                        "was not specified and there are existing text messages " +
                        "in the database.",
                        textMessageOptions.Count
                        );
                    return; // Nothing else to do!
                }

                // Are there existing file types?
                hasExistingData = await _fileTypeManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding text messages because the 'force' flag " +
                        "was not specified and there are existing text messages " +
                        "in the database.",
                        textMessageOptions.Count
                        );
                    return; // Nothing else to do!
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding {count} text messages.",
                textMessageOptions.Count
                );

            // Loop through the options.
            foreach (var textMessageOption in textMessageOptions)
            {
                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(ITextMessageManager.CreateAsync)
                    );

                // Create the text message.
                _ = await _textMessageManager.CreateAsync(
                    new TextMessage()
                    {
                        To = textMessageOption.To,
                        Body = textMessageOption.Body,
                        IsDisabled = textMessageOption.IsDisabled,
                    },
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Let the world know what happened.
            _logger.LogError(
                ex,
                "Failed to seed text messages!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed text messages!",
                innerException: ex
                );
        }
    }

    #endregion
}
