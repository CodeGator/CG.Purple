﻿
namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// This class contains extension methods related to the <see cref="WebApplicationBuilder"/>
/// type.
/// </summary>
public static partial class WebApplicationBuilderExtensions_Purple_SqlServer
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method adds the data-access layer for the <see cref="CG.Purple"/>
    /// project.
    /// </summary>
    /// <param name="webApplicationBuilder">The web application builder to
    /// use for the operation.</param>
    /// <param name="sectionName">The configuration section to use for the 
    /// operation. Defaults to <c>DAL</c>.</param>
    /// <param name="bootstrapLogger">A bootstrap logger to use for the
    /// operation.</param>
    /// <returns>The value of the <paramref name="webApplicationBuilder"/>
    /// parameter, for chaining calls together, Fluent style.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    public static WebApplicationBuilder AddDataAccessLayer(
        this WebApplicationBuilder webApplicationBuilder,
        string sectionName = "DAL",
        ILogger? bootstrapLogger = null
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(webApplicationBuilder, nameof(webApplicationBuilder))
            .ThrowIfNullOrEmpty(sectionName, nameof(sectionName));

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Configuring DAL options from the {section} section",
            sectionName
            );

        // Configure the DAL options.
        webApplicationBuilder.Services.ConfigureOptions<DalOptions>(
            webApplicationBuilder.Configuration.GetSection(sectionName),
            out var dalOptions
            );

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Fetching the EFCORE migration assembly name"
            );

        // We include migrations in this assembly.
        var migrationAssembly = Assembly.GetExecutingAssembly()
            .GetName().Name;

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the DAL {ctx} data-context",
            nameof(PurpleDbContext)
            );

        // Wire up the EFCORE data context factory.
        webApplicationBuilder.Services.AddDbContextFactory<PurpleDbContext>(options => 
        {
            // Use SQL-Server with our migrations and basic retry logic.
            options.UseSqlServer(
                dalOptions.ConnectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(migrationAssembly);
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null
                        );
                });

            // Are we running in a development environment?
            if (webApplicationBuilder.Environment.IsDevelopment())
            {
                // Tell the world what we are about to do.
                bootstrapLogger?.LogDebug(
                    "Enabling sensitive logging for EFCORE since we're in a development environment"
                    );

                // Enable sensitive logging.
                options.EnableDetailedErrors().EnableSensitiveDataLogging();
            }
        });

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the DAL auto-mapper"
            );

        // Wire up the auto-mapper.
        webApplicationBuilder.Services.AddAutoMapper(cfg =>
        {
            // Wire up the conversion maps.

            cfg.CreateMap<Attachment, CG.Purple.SqlServer.Entities.Attachment>().ReverseMap();
            cfg.CreateMap<FileType, CG.Purple.SqlServer.Entities.FileType>().ReverseMap();
            cfg.CreateMap<MailMessage, CG.Purple.SqlServer.Entities.MailMessage>().ReverseMap();
            cfg.CreateMap<Message, CG.Purple.SqlServer.Entities.Message>().ReverseMap();
            cfg.CreateMap<MessageProperty, CG.Purple.SqlServer.Entities.MessageProperty>().ReverseMap();
            cfg.CreateMap<MimeType, CG.Purple.SqlServer.Entities.MimeType>().ReverseMap();
            cfg.CreateMap<ParameterType, CG.Purple.SqlServer.Entities.ParameterType>().ReverseMap();
            cfg.CreateMap<PropertyType, CG.Purple.SqlServer.Entities.PropertyType>().ReverseMap();
            cfg.CreateMap<ProviderLog, CG.Purple.SqlServer.Entities.ProviderLog>().ReverseMap();
            cfg.CreateMap<ProviderType, CG.Purple.SqlServer.Entities.ProviderType>().ReverseMap();
            cfg.CreateMap<ProviderParameter, CG.Purple.SqlServer.Entities.ProviderParameter>().ReverseMap();
            cfg.CreateMap<TextMessage, CG.Purple.SqlServer.Entities.TextMessage>().ReverseMap();
        });

        // Tell the world what we are about to do.
        bootstrapLogger?.LogDebug(
            "Wiring up the DAL repositories"
            );

        // Wire up the repositories.
        webApplicationBuilder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        webApplicationBuilder.Services.AddScoped<IFileTypeRepository, FileTypeRepository>();
        webApplicationBuilder.Services.AddScoped<IMailMessageRepository, MailMessageRepository>();
        webApplicationBuilder.Services.AddScoped<IMimeTypeRepository, MimeTypeRepository>();
        webApplicationBuilder.Services.AddScoped<IMessagePropertyRepository, MessagePropertyRepository>();
        webApplicationBuilder.Services.AddScoped<IParameterTypeRepository, ParameterTypeRepository>();
        webApplicationBuilder.Services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
        webApplicationBuilder.Services.AddScoped<IProviderLogRepository, ProviderLogRepository>();
        webApplicationBuilder.Services.AddScoped<IProviderTypeRepository, ProviderTypeRepository>();
        webApplicationBuilder.Services.AddScoped<IProviderParameterRepository, ProviderParameterRepository>();
        webApplicationBuilder.Services.AddScoped<ITextMessageRepository, TextMessageRepository>();

        // Return the application builder.
        return webApplicationBuilder;
    }

    #endregion
}
