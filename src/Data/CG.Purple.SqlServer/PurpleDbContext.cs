
namespace CG.Purple.SqlServer;

/// <summary>
/// This class is an EFCore data context for the <see cref="Purple"/> project">
/// project.
/// </summary>
internal class PurpleDbContext : DbContext
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the collection of attachments.
    /// </summary>
    public virtual DbSet<Entities.Attachment> Attachments { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of file types.
    /// </summary>
    public virtual DbSet<Entities.FileType> FileTypes { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of messages.
    /// </summary>
    public virtual DbSet<Entities.Message> Messages { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of mail messages.
    /// </summary>
    public virtual DbSet<Entities.MailMessage> MailMessages { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of message properties.
    /// </summary>
    public virtual DbSet<Entities.MessageProperty> MessageProperties { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of MIME types.
    /// </summary>
    public virtual DbSet<Entities.MimeType> MimeTypes { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of parameter types.
    /// </summary>
    public virtual DbSet<Entities.ParameterType> ParameterTypes { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of process log entries.
    /// </summary>
    public virtual DbSet<Entities.ProviderLog> ProcessLogs { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of property types.
    /// </summary>
    public virtual DbSet<Entities.PropertyType> PropertyTypes { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of provider parameters.
    /// </summary>
    public virtual DbSet<Entities.ProviderParameter> ProviderParameters { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of message provider types.
    /// </summary>
    public virtual DbSet<Entities.ProviderType> ProviderTypes { get; set; } = null!;

    /// <summary>
    /// This property contains the collection of text messages.
    /// </summary>
    public virtual DbSet<Entities.TextMessage> TextMessages { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="PurpleDbContext"/>
    /// class.
    /// </summary>
    /// <param name="options">The options to use with the data-context.</param>
    public PurpleDbContext(
        DbContextOptions<PurpleDbContext> options
        ) : base(options)
    {

    }

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is called to create the data model.
    /// </summary>
    /// <param name="modelBuilder">The builder to use for the operation.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map the entities.
        modelBuilder.ApplyConfiguration(new AttachmentMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new FileTypeMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new MessageMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new MailMessageMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new MessagePropertyMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new MimeTypeMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new ParameterTypeMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new PropertyTypeMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new ProviderLogMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new ProviderParameterMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new ProviderTypeMap(modelBuilder));
        modelBuilder.ApplyConfiguration(new TextMessageMap(modelBuilder));

        // Give the base class a chance.
        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
