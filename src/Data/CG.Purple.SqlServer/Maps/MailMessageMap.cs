
namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.MailMessage"/>
/// entity type.
/// </summary>
internal class MailMessageMap : EntityMapBase<Entities.MailMessage>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MailMessageMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public MailMessageMap(
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
    /// This method configures the <see cref="Entities.MailMessage"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.MailMessage> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "MailMessages",
            "Purple"
            );

        // Setup the column.
        builder.Property(e => e.To)
            .HasMaxLength(1024)
            .IsUnicode(false)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.CC)
            .IsUnicode(false)
            .HasMaxLength(1024);

        // Setup the column.
        builder.Property(e => e.BCC)
            .IsUnicode(false)
            .HasMaxLength(1024);

        // Setup the column.
        builder.Property(e => e.Subject)
            .HasMaxLength(998);

        // Setup the column.
        builder.Property(e => e.Body)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.IsHtml)
            .HasDefaultValue(false)
            .IsRequired();

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.To,
            e.CC,
            e.BCC,
            e.Subject,
            e.IsHtml
        },
        "IX_MailMessages"
        );
    }

    #endregion
}
