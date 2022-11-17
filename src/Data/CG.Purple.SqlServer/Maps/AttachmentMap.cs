
namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.Attachment"/>
/// entity type.
/// </summary>
internal class AttachmentMap : EntityMapBase<Entities.Attachment>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="AttachmentMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public AttachmentMap(
        ModelBuilder modelBuilder
        ) : base(modelBuilder)
    {

    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method configures the <see cref="Entities.Attachment"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.Attachment> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "Attachments",
            "Purple"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.OriginalFileName)
            .HasMaxLength(260)
            .IsUnicode(false);

        // Setup the column.
        builder.Property(e => e.Length)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.Data)
            .IsRequired();

        // Setup the relationship.
        _modelBuilder.Entity<Entities.Attachment>()
            .HasOne(e => e.Message)
            .WithMany(e => e.Attachments)
            .HasForeignKey(e => e.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        // Setup the relationship.
        _modelBuilder.Entity<Entities.Attachment>()
            .HasOne(e => e.MimeType)
            .WithMany()
            .HasForeignKey(e => e.MimeTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}
