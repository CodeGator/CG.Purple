using CG.Purple.SqlServer.Entities;

namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class represents a base map for entity types derived from <see cref="EntityBase"/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
internal abstract class EntityMapBase<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the model builder for this map.
    /// </summary>
    internal protected readonly ModelBuilder _modelBuilder;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="EntityMapBase{TEntity}"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public EntityMapBase(
        ModelBuilder modelBuilder
        )
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(modelBuilder, nameof(modelBuilder));

        // Save the reference(s).
        _modelBuilder = modelBuilder;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method configures the <typeparamref name="TEntity"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public virtual void Configure(
        EntityTypeBuilder<TEntity> builder
        )
    {
        // Setup the property.
        builder.Property(e => e.CreatedOnUtc)
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();

        // Setup the property.
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(32)
            .IsRequired();

        // Setup the property.
        builder.Property(e => e.LastUpdatedBy)
            .HasMaxLength(32);

        // Setup the property.
        builder.Property(e => e.LastUpdatedOnUtc);

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.CreatedOnUtc,
            e.CreatedBy,
            e.LastUpdatedBy,
            e.LastUpdatedOnUtc
        },
        $"IX_{typeof(TEntity).Name}_Stats"
        );
    }

    #endregion
}
