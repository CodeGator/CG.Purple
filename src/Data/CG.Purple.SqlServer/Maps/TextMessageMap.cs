
namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.TextMessage"/>
/// entity type.
/// </summary>
internal class TextMessageMap : EntityMapBase<Entities.TextMessage>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="TextMessageMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public TextMessageMap(
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
    /// This method configures the <see cref="Entities.TextMessage"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.TextMessage> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "TextMessages",
            "Purple"
            );

        // Setup the column.
        builder.Property(e => e.To)
            .HasMaxLength(1024)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.Body)
            .IsRequired();

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.To
        },
        "IX_TextMessages"
        );
    }

    #endregion
}
