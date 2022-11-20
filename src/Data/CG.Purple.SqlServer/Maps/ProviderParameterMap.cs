
namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.ProviderParameter"/>
/// entity type.
/// </summary>
internal class ProviderParameterMap : EntityMapBase<Entities.ProviderParameter>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderParameterMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public ProviderParameterMap(
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
    /// This method configures the <see cref="Entities.ProviderParameter"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.ProviderParameter> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "ProviderParameters",
            "Purple"
            );

        // Setup the primary key.
        builder.HasKey(e => new { e.ProviderTypeId, e.ParameterTypeId });

        // Setup the relationship.
        _modelBuilder.Entity<Entities.ProviderParameter>()
            .HasOne(e => e.ProviderType)
            .WithMany(e => e.Parameters)
            .HasForeignKey(e => e.ProviderTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Setup the relationship.
        _modelBuilder.Entity<Entities.ProviderParameter>()
            .HasOne(e => e.ParameterType)
            .WithMany()
            .HasForeignKey(e => e.ParameterTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Setup the column.
        builder.Property(e => e.Value)
            .IsRequired();
    }

    #endregion
}
