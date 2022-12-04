
namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.MessageProperty"/>
/// entity type.
/// </summary>
internal class MessagePropertyMap : EntityMapBase<Entities.MessageProperty>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="MessagePropertyMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public MessagePropertyMap(
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
    /// This method configures the <see cref="Entities.MessageProperty"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.MessageProperty> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "MessageProperties",
            "Purple"
            );

        // Setup the primary key.
        builder.HasKey(e => new { e.MessageId, e.PropertyTypeId });

        // Setup the relationship.
        _modelBuilder.Entity<Entities.MessageProperty>()
            .HasOne(e => e.Message)
            .WithMany(e => e.MessageProperties)
            .HasForeignKey(e => e.MessageId)
            .OnDelete(DeleteBehavior.Restrict);

        // Setup the relationship.
        _modelBuilder.Entity<Entities.MessageProperty>()
            .HasOne(e => e.PropertyType)
            .WithMany()
            .HasForeignKey(e => e.PropertyTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Setup the column.
        builder.Property(e => e.Value)
            .IsRequired();
    }

    #endregion
}
