
namespace CG.Purple.SqlServer.Maps;

/// <summary>
/// This class is an EFCore configuration map for the <see cref="Entities.ProviderLog"/>
/// entity type.
/// </summary>
internal class ProviderLogMap : EntityMapBase<Entities.ProviderLog>
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="ProviderLogMap"/>
    /// class.
    /// </summary>
    /// <param name="modelBuilder">The model builder to use with this map.</param>
    public ProviderLogMap(
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
    /// This method configures the <see cref="Entities.ProviderLog"/> entity.
    /// </summary>
    /// <param name="builder">The builder to use for the operation.</param>
    public override void Configure(
        EntityTypeBuilder<Entities.ProviderLog> builder
        )
    {
        // Setup the table.
        builder.ToTable(
            "ProviderLogs",
            "Purple"
            );

        // Setup the property.
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // Setup the primary key.
        builder.HasKey(e => new { e.Id });

        // Setup the column.
        builder.Property(e => e.Event)
            .HasMaxLength(30)
            .IsUnicode(false)
            .IsRequired();

        // Setup the column.
        builder.Property(e => e.BeforeState)
            .HasMaxLength(30)
            .IsUnicode(false);

        // Setup the column.
        builder.Property(e => e.AfterState)
            .HasMaxLength(30)
            .IsUnicode(false);

        // Setup the column.
        builder.Property(e => e.Data)
            .IsUnicode(false);

        // Setup the column.
        builder.Property(e => e.Error)
            .IsUnicode(false);

        // Setup the conversion.
        _modelBuilder.Entity<Entities.ProviderLog>()
            .Property(e => e.Event)
            .HasConversion(
                e => e.ToString(),
                e => Enum.Parse<Entities.ProcessEvent>(e)
                );

        // Setup the conversion.
        _modelBuilder.Entity<Entities.ProviderLog>()
            .Property(e => e.BeforeState)
            .HasConversion(
                e => e.ToString(),
                e => e == null ? null : Enum.Parse<MessageState>(e)
                );

        // Setup the conversion.
        _modelBuilder.Entity<Entities.ProviderLog>()
            .Property(e => e.AfterState)
            .HasConversion(
                e => e.ToString(),
                e => e == null ? null : Enum.Parse<MessageState>(e)
                );

        // Setup the relationship.
        _modelBuilder.Entity<Entities.ProviderLog>()
            .HasOne(e => e.Message)
            .WithMany()
            .HasForeignKey(e => e.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        // Setup the relationship.
        _modelBuilder.Entity<Entities.ProviderLog>()
            .HasOne(e => e.ProviderType)
            .WithMany()
            .HasForeignKey(e => e.ProviderTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Setup the index.
        builder.HasIndex(e => new
        {
            e.Event,
            e.BeforeState,
            e.AfterState,
            e.ProviderTypeId
        },
        "IX_ProcessLogs"
        );
    }

    #endregion
}
