namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.FileType"/>
/// entity type.
/// </summary>
internal class FileTypeMap : EntityMapBase<Entities.FileType>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="FileTypeMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public FileTypeMap(
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
    /// This method configures the <see cref="Entities.FileType"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.FileType> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "FileTypes",
            "Purple"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.Extension)
            .HasMaxLength(260)
            .IsUnicode(false)
            .IsRequired();

        // Setup the relationship.
        _modelBuilder.Entity<Entities.FileType>()
            .HasOne(e => e.MimeType)
            .WithMany(e => e.FileTypes)
            .HasForeignKey(e => e.MimeTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.Extension
        },
        "IX_FileTypes"
        ).IsUnique();
    }

    #endregion
}
