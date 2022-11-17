
namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.Message"/>
/// entity type.
/// </summary>
internal class MessageMap : EntityMapBase<Entities.Message>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessageMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public MessageMap(
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
    /// This method configures the <see cref="Entities.Message"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.Message> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "Messages",
            "Purple"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.MessageState)
            .HasMaxLength(30)
            .IsUnicode(false)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.MessageType)
            .HasMaxLength(30)
            .IsUnicode(false)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.IsDisabled)
            .IsRequired();

        // Setup the conversion.
        _modelBuilder.Entity<Entities.Message>()
            .Property(e => e.MessageType)
            .HasConversion(
                e => e.ToString(),
                e => Enum.Parse<MessageType>(e)
                );

        // Setup the conversion.
        _modelBuilder.Entity<Entities.Message>()
            .Property(e => e.MessageState)
            .HasConversion(
                e => e.ToString(),
                e => Enum.Parse<MessageState>(e)
                );

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.MessageType,
            e.MessageState,
            e.IsDisabled
        },
        "IX_Messages"
        );
    }

    #endregion
}
