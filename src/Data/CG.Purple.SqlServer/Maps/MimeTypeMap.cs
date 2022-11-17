namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.MimeType"/>
/// entity type.
/// </summary>
internal class MimeTypeMap : EntityMapBase<Entities.MimeType>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MimeTypeMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public MimeTypeMap(
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
    /// This method configures the <see cref="Entities.MimeType"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.MimeType> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "MimeTypes",
            "Purple"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.Type)
            .HasMaxLength(127)
            .IsUnicode(false)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.SubType)
            .HasMaxLength(127)
            .IsUnicode(false)
            .IsRequired();

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.Type,
            e.SubType
        },
        "IX_MimeTypes"
        ).IsUnique();
    }

    #endregion
}
