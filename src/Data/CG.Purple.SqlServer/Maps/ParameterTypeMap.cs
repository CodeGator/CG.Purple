
namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.ParameterType"/>
/// entity type.
/// </summary>
internal class ParameterTypeMap : EntityMapBase<Entities.ParameterType>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ParameterTypeMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public ParameterTypeMap(
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
    /// This method configures the <see cref="Entities.ParameterType"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.ParameterType> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "ParameterTypes",
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
        "IX_ParameterTypes"
        );
    }

    #endregion
}
