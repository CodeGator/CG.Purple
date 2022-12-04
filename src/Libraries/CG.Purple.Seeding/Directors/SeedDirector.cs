
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
    internal protected readonly IAttachmentManager _attachmentManager = null!;

    /// <summary>
    /// This field contains the file type manager for this director.
    /// </summary>
    internal protected readonly IFileTypeManager _fileTypeManager = null!;

    /// <summary>
    /// This field contains the mail message manager for this director.
    /// </summary>
    internal protected readonly IMailMessageManager _mailMessageManager = null!;

    /// <summary>
    /// This field contains the message property manager for this director.
    /// </summary>
    internal protected readonly IMessagePropertyManager _messagePropertyManager = null!;

    /// <summary>
    /// This field contains the mime type manager for this director.
    /// </summary>
    internal protected readonly IMimeTypeManager _mimeTypeManager = null!;

    /// <summary>
    /// This field contains the parameter type manager for this director.
    /// </summary>
    internal protected readonly IParameterTypeManager _parameterTypeManager = null!;

    /// <summary>
    /// This field contains the property type manager for this director.
    /// </summary>
    internal protected readonly IPropertyTypeManager _propertyTypeManager = null!;

    /// <summary>
    /// This field contains the provider parameter manager for this director.
    /// </summary>
    internal protected readonly IProviderParameterManager _providerParameterManager = null!;

    /// <summary>
    /// This field contains the provider log manager for this director.
    /// </summary>
    internal protected readonly IMessageLogManager _providerLogManager = null!;

    /// <summary>
    /// This field contains the provider type manager for this director.
    /// </summary>
    internal protected readonly IProviderTypeManager _providerTypeManager = null!;

    /// <summary>
    /// This field contains the text message manager for this director.
    /// </summary>
    internal protected readonly ITextMessageManager _textMessageManager = null!;

    /// <summary>
    /// This field contains the logger for this director.
    /// </summary>
    internal protected readonly ILogger<ISeedDirector> _logger = null!;

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
    /// <param name="fileTypeManager">The file type manager to use with this
    /// director.</param>
    /// <param name="mailMessageManager">The mail message manager to use with 
    /// this director.</param>
    /// <param name="messagePropertyManager">The message property manager to use 
    /// with this director.</param>
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
    /// <param name="providerLogManager">The provider log manager to use 
    /// with this director.</param>
    /// <param name="textMessageManager">The text message manager to use with 
    /// this director.</param>
    /// <param name="logger">The logger to use with this director.</param>
    public SeedDirector(
        IAttachmentManager attachmenteManager,
        IFileTypeManager fileTypeManager,
        IMailMessageManager mailMessageManager,
        IMessagePropertyManager messagePropertyManager,
        IMimeTypeManager mimeTypeManager,
        IParameterTypeManager parameterTypeManager,
        IPropertyTypeManager propertyTypeManager,
        IProviderParameterManager providerParameterManager,
        IProviderTypeManager providerTypeManager,
        IMessageLogManager providerLogManager,
        ITextMessageManager textMessageManager,
        ILogger<ISeedDirector> logger
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(attachmenteManager, nameof(attachmenteManager))
            .ThrowIfNull(fileTypeManager, nameof(fileTypeManager))
            .ThrowIfNull(mailMessageManager, nameof(mailMessageManager))
            .ThrowIfNull(messagePropertyManager, nameof(messagePropertyManager))
            .ThrowIfNull(mimeTypeManager, nameof(mimeTypeManager))
            .ThrowIfNull(parameterTypeManager, nameof(parameterTypeManager))
            .ThrowIfNull(propertyTypeManager, nameof(propertyTypeManager))
            .ThrowIfNull(providerParameterManager, nameof(providerParameterManager))
            .ThrowIfNull(providerTypeManager, nameof(providerTypeManager))
            .ThrowIfNull(providerLogManager, nameof(providerLogManager))
            .ThrowIfNull(textMessageManager, nameof(textMessageManager))
            .ThrowIfNull(logger, nameof(logger));

        // Save the reference(s).
        _attachmentManager = attachmenteManager;
        _fileTypeManager = fileTypeManager;
        _mailMessageManager = mailMessageManager;
        _messagePropertyManager = messagePropertyManager;
        _mimeTypeManager = mimeTypeManager;
        _parameterTypeManager = parameterTypeManager;
        _propertyTypeManager = propertyTypeManager;
        _providerParameterManager = providerParameterManager;
        _providerTypeManager = providerTypeManager;
        _providerLogManager = providerLogManager;
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
            // Log what happened.
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
            // Log what happened.
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
            // Log what happened.
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
            // Log what happened.
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
            // Log what happened.
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
            // Log what happened.
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
    public virtual async Task SeedProcessLogsAsync(
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
                "Binding process log options."
                );

            // Bind the options.
            var providerLogOptions = new List<ProcessLogOptions>();
            configuration.GetSection("ProcessLogs").Bind(providerLogOptions);
                        
            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding process logs from the configuration."
                );

            // Seed the provider logs from the options.
            await SeedProcessLogsAsync(
                providerLogOptions,
                userName,
                force,
                cancellationToken
                );
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to seed process logs!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed process logs!",
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
            // Log what happened.
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
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    private async Task SeedMailMessagesAsync(
        List<MailMessageOptions> mailMessageOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Verify the options.
        Guard.Instance().ThrowIfInvalidObject(mailMessageOptions, nameof(mailMessageOptions));

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
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding {count} mail messages.",
                mailMessageOptions.Count
                );

            // Loop through the options.
            foreach (var mailMessageOption in mailMessageOptions)
            {
                // Was a provider given?
                ProviderType? providerType = null;
                if (!string.IsNullOrEmpty(mailMessageOption.ProviderType))
                {
                    // Look for the provider.
                    providerType = await _providerTypeManager.FindByNameAsync(
                        mailMessageOption.ProviderType
                        ).ConfigureAwait(false);

                    // Did we fail?
                    if (providerType is null)
                    {
                        // Panic!!
                        throw new KeyNotFoundException(
                            $"The provider type: {mailMessageOption.ProviderType} is missing!"
                            );
                    }
                }

                // Log what we are about to do.
                _logger.LogTrace(
                    "Deferring to {name}",
                    nameof(IMailMessageManager.CreateAsync)
                    );

                // Create the mail message.
                var mailMessage = await _mailMessageManager.CreateAsync(
                    new MailMessage()
                    {
                        MessageKey = mailMessageOption.MessageKey,
                        From = mailMessageOption.From,
                        To = mailMessageOption.To,
                        CC = mailMessageOption.CC,
                        BCC = mailMessageOption.BCC,
                        Subject = mailMessageOption.Subject,
                        Body = mailMessageOption.Body,
                        IsHtml = mailMessageOption.IsHtml,
                        IsDisabled = mailMessageOption.IsDisabled,
                        Priority = mailMessageOption.Priority,
                        MaxErrors = mailMessageOption.MaxErrors,
                        ProcessAfterUtc = mailMessageOption.ProcessAfterUtc,
                        MessageType = MessageType.Mail,
                        ProviderType = providerType,
                        ErrorCount = 0
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
                                $"No mime type was found for extension: {ext}!"
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
                                
                // Are there any properties?
                if (mailMessageOption.Properties.Any())
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Seeding {count} properties.",
                        mailMessageOption.Properties.Count
                        );

                    // Loop through the options.
                    foreach (var property in mailMessageOption.Properties)
                    {
                        // Find the property type.
                        var propertyType = await _propertyTypeManager.FindByNameAsync(
                            property.PropertyTypeName,
                            cancellationToken
                            ).ConfigureAwait(false);

                        // Did we fail?
                        if (propertyType is null)
                        {
                            // Panic!!
                            throw new KeyNotFoundException(
                                $"No property type was found for name: {property.PropertyTypeName}!"
                                );
                        }

                        // Create the message property.
                        _ = _messagePropertyManager.CreateAsync(
                            new MessageProperty()
                            {
                                Message = mailMessage,
                                PropertyType = propertyType,
                                Value = property.Value
                            },
                            userName,
                            cancellationToken
                            ).ConfigureAwait(false);
                    }
                }

                // Record what we did, in the log.
                await _providerLogManager.CreateAsync(
                    new MessageLog()
                    {
                        Message = mailMessage,
                        AfterState = MessageState.Pending,
                        MessageEvent = MessageEvent.Stored
                    },
                    "seed",
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
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
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    private async Task SeedMimeTypesAsync(
        List<MimeTypeOptions> mimeTypeOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Verify the options.
        Guard.Instance().ThrowIfInvalidObject(mimeTypeOptions, nameof(mimeTypeOptions));

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
            // Log what happened.
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
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    private async Task SeedParameterTypesAsync(
        List<ParameterTypeOptions> parameterTypeOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Verify the options.
        Guard.Instance().ThrowIfInvalidObject(parameterTypeOptions, nameof(parameterTypeOptions));

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
            // Log what happened.
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
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    private async Task SeedPropertyTypesAsync(
        List<PropertyTypeOptions> propertyTypeOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Verify the options.
        Guard.Instance().ThrowIfInvalidObject(propertyTypeOptions, nameof(propertyTypeOptions));

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
            // Log what happened.
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
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    private async Task SeedProviderParametersAsync(
        List<ProviderParameterOptions> providerParameterOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Verify the options.
        Guard.Instance().ThrowIfInvalidObject(providerParameterOptions, nameof(providerParameterOptions));

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
                        ProviderType = providerType,
                        ParameterType = parameterType,
                        Value = providerParameterOption.Value
                    },
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
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
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    private async Task SeedProviderTypesAsync(
        List<ProviderTypeOptions> providerTypeOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Verify the options.
        Guard.Instance().ThrowIfInvalidObject(providerTypeOptions, nameof(providerTypeOptions));

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
                        Description = providerTypeOption.Description,
                        IsDisabled = providerTypeOption.IsDisabled,
                        Priority = providerTypeOption.Priority,
                        CanProcessEmails = providerTypeOption.CanProcessEmails,
                        CanProcessTexts = providerTypeOption.CanProcessTexts,
                        FactoryType = providerTypeOption.FactoryType
                    },
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
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
    /// <see cref="ProcessLogOptions"/> objects.
    /// </summary>
    /// <param name="providerLogOptions">The options to use for the operation.</param>
    /// <param name="userName">The name of the user performing the operation.</param>
    /// <param name="force"><c>true</c> to force the operation; <c>false</c>
    /// otherwise.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// for the lifetime of the method.</param>
    /// <returns>A task to perform the operation.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    private async Task SeedProcessLogsAsync(
        List<ProcessLogOptions> providerLogOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Verify the options.
        Guard.Instance().ThrowIfInvalidObject(providerLogOptions, nameof(providerLogOptions));

        try
        {
            // Should we check for existing data?
            if (!force)
            {
                // Are there existing provider logs?
                var hasExistingData = await _providerLogManager.AnyAsync(
                    cancellationToken
                    ).ConfigureAwait(false);

                // Should we stop?
                if (hasExistingData)
                {
                    // Log what we didn't do.
                    _logger.LogWarning(
                        "Skipping seeding process logs because the 'force' flag " +
                        "was not specified and there are existing process logs " +
                        "in the database.",
                        providerLogOptions.Count
                        );
                    return; // Nothing else to do!
                }
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Seeding {count} process logs.",
                providerLogOptions.Count
                );

            // Loop through the options.
            foreach (var providerLogOption in providerLogOptions)
            {
                // Parse the enumeration.
                if (!Enum.TryParse<MessageType>(
                    providerLogOption.MessageType,
                    out var messageType
                    ))
                {
                    // Panic!!
                    throw new KeyNotFoundException(
                        $"The message type: {providerLogOption.MessageType} was not found!"
                        );
                }

                // Is a provider specified?
                ProviderType? providerType = null;
                if (!string.IsNullOrEmpty(providerLogOption.ProviderTypeName))
                {
                    // Look for the provider type.
                    providerType = await _providerTypeManager.FindByNameAsync(
                        providerLogOption.ProviderTypeName,
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Did we fail?
                    if (providerType is null)
                    {
                        // Panic!!
                        throw new KeyNotFoundException(
                            $"The provider type name: {providerLogOption.ProviderTypeName} was not found!"
                            );
                    }
                }

                // Parse the optional enumeration.
                MessageState? optionalBeforeState = null;
                if (Enum.TryParse<MessageState>(
                    providerLogOption.BeforeState,
                    out var beforeState
                    ))
                {
                    optionalBeforeState = beforeState;
                }

                // Parse the optional enumeration.
                MessageState? optionalAfterState = null;
                if (Enum.TryParse<MessageState>(
                    providerLogOption.AfterState,
                    out var afterState
                    ))
                {
                    optionalAfterState = afterState;
                }

                // Is this an email message?
                if (messageType == MessageType.Mail)
                {
                    // Look for the message.
                    var mailMessage = await _mailMessageManager.FindByKeyAsync(
                        providerLogOption.MessageKey,
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Did we fail?
                    if (mailMessage is null)
                    {
                        // Panic!!
                        throw new KeyNotFoundException(
                            $"The message key: {providerLogOption.MessageKey} was not found!"
                            );
                    }

                    // Log what we are about to do.
                    _logger.LogTrace(
                        "Deferring to {name}",
                        nameof(IMessageLogManager.CreateAsync)
                        );

                    // Create the provider log.
                    _ = await _providerLogManager.CreateAsync(
                        new MessageLog()
                        {
                            Message = mailMessage,
                            ProviderType = providerType,
                            MessageEvent = Enum.Parse<MessageEvent>(providerLogOption.Event),
                            Error = providerLogOption.Error,
                            BeforeState = optionalBeforeState,
                            AfterState = optionalAfterState,
                            Data = providerLogOption.Data
                        },
                        userName,
                        cancellationToken
                        ).ConfigureAwait(false);
                }

                // Is this a text message?
                else if (messageType == MessageType.Text)
                {
                    // Look for the message.
                    var textMessage = await _textMessageManager.FindByKeyAsync(
                        providerLogOption.MessageKey,
                        cancellationToken
                        ).ConfigureAwait(false);

                    // Did we fail?
                    if (textMessage is null)
                    {
                        // Panic!!
                        throw new KeyNotFoundException(
                            $"The message key: {providerLogOption.MessageKey} was not found!"
                            );
                    }

                    // Log what we are about to do.
                    _logger.LogTrace(
                        "Deferring to {name}",
                        nameof(IMessageLogManager.CreateAsync)
                        );

                    // Create the provider log.
                    _ = await _providerLogManager.CreateAsync(
                        new MessageLog()
                        {
                            Message = textMessage,
                            ProviderType = providerType,
                            MessageEvent = Enum.Parse<MessageEvent>(providerLogOption.Event),
                            Error = providerLogOption.Error,
                            BeforeState = optionalBeforeState,
                            AfterState = optionalAfterState,
                            Data = providerLogOption.Data
                        },
                        userName,
                        cancellationToken
                        ).ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to seed process logs!"
                );

            // Provider better context.
            throw new DirectorException(
                message: $"The director failed to seed process logs!",
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
    /// <exception cref="ArgumentException">This exception is thrown whenever one
    /// or more arguments are missing, or invalid.</exception>
    /// <exception cref="DirectorException">This exception is thrown whenever
    /// the director fails to complete the operation.</exception>
    private async Task SeedTextMessagesAsync(
        List<TextMessageOptions> textMessageOptions,
        string userName,
        bool force = false,
        CancellationToken cancellationToken = default
        )
    {
        // Verify the options.
        Guard.Instance().ThrowIfInvalidObject(textMessageOptions, nameof(textMessageOptions));

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
                var textMessage = await _textMessageManager.CreateAsync(
                    new TextMessage()
                    {
                        MessageKey = textMessageOption.MessageKey,
                        From = textMessageOption.From,
                        To = textMessageOption.To,
                        Body = textMessageOption.Body,
                        IsDisabled = textMessageOption.IsDisabled,
                        MessageType = MessageType.Text,
                        Priority = textMessageOption.Priority,
                        MaxErrors = textMessageOption.MaxErrors,
                        ProcessAfterUtc = textMessageOption.ProcessAfterUtc
                    },
                    userName,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Are there any attachments?
                if (textMessageOption.Attachments.Any())
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Seeding {count} attachments.",
                        textMessageOption.Attachments.Count
                        );

                    // Loop through the options.
                    foreach (var attachment in textMessageOption.Attachments)
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
                                $"No mime type was found for extension: {ext}!"
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
                                Message = textMessage,
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

                // Are there any properties?
                if (textMessageOption.Properties.Any())
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                    "Seeding {count} properties.",
                        textMessageOption.Properties.Count
                        );

                    // Loop through the options.
                    foreach (var property in textMessageOption.Properties)
                    {
                        // Find the property type.
                        var propertyType = await _propertyTypeManager.FindByNameAsync(
                            property.PropertyTypeName,
                            cancellationToken
                            ).ConfigureAwait(false);

                        // Did we fail?
                        if (propertyType is null)
                        {
                            // Panic!!
                            throw new KeyNotFoundException(
                                $"No property type was found for name: {property.PropertyTypeName}!"
                                );
                        }

                        // Create the message property.
                        _ = _messagePropertyManager.CreateAsync(
                            new MessageProperty()
                            {
                                Message = textMessage,
                                PropertyType = propertyType,
                                Value = property.Value
                            },
                            userName,
                            cancellationToken
                            ).ConfigureAwait(false);
                    }
                }

                // Record what we did, in the log.
                await _providerLogManager.CreateAsync(
                    new MessageLog()
                    {
                        Message = textMessage,
                        AfterState = MessageState.Pending,
                        MessageEvent = MessageEvent.Stored
                    },
                    "seed",
                    cancellationToken
                    ).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
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
