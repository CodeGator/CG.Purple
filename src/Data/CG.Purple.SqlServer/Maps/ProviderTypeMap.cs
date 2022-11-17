
namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.ProviderType"/>
/// entity type.
/// </summary>
internal class ProviderTypeMap : EntityMapBase<Entities.ProviderType>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderTypeMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public ProviderTypeMap(
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
    /// This method configures the <see cref="Entities.ProviderType"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.ProviderType> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "ProviderTypes",
            "Purple"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.Name)
            .HasMaxLength(64)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.Description)
            .HasMaxLength(128);

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.Name
        },
        "IX_ProviderTypes"
        );
    }

    #endregion
}
