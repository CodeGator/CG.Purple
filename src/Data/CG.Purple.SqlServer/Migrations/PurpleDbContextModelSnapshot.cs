﻿// <auto-generated />
using System;
using CG.Purple.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CG.Purple.SqlServer.Migrations
{
    [DbContext(typeof(PurpleDbContext))]
    partial class PurpleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.Attachment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<long>("Length")
                        .HasColumnType("bigint");

                    b.Property<long>("MessageId")
                        .HasColumnType("bigint");

                    b.Property<int>("MimeTypeId")
                        .HasColumnType("int");

                    b.Property<string>("OriginalFileName")
                        .HasMaxLength(260)
                        .IsUnicode(false)
                        .HasColumnType("varchar(260)");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.HasIndex("MimeTypeId");

                    b.ToTable("Attachments", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.FileType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(260)
                        .IsUnicode(false)
                        .HasColumnType("varchar(260)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("MimeTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MimeTypeId");

                    b.HasIndex(new[] { "Extension" }, "IX_FileTypes")
                        .IsUnique();

                    b.ToTable("FileTypes", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("MessageKey")
                        .IsRequired()
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("MessageState")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "From", "MessageType", "MessageState", "IsDisabled" }, "IX_Messages");

                    b.HasIndex(new[] { "MessageKey" }, "IX_Messages_Keys")
                        .IsUnique();

                    b.ToTable("Messages", "Purple");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.MessageProperty", b =>
                {
                    b.Property<long>("MessageId")
                        .HasColumnType("bigint");

                    b.Property<int>("PropertyTypeId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MessageId", "PropertyTypeId");

                    b.HasIndex("PropertyTypeId");

                    b.ToTable("MessageProperties", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.MimeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubType")
                        .IsRequired()
                        .HasMaxLength(127)
                        .IsUnicode(false)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(127)
                        .IsUnicode(false)
                        .HasColumnType("varchar(127)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Type", "SubType" }, "IX_MimeTypes")
                        .IsUnique();

                    b.ToTable("MimeTypes", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.ParameterType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "IX_ParameterTypes");

                    b.ToTable("ParameterTypes", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.PropertyType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "IX_PropertyTypes");

                    b.ToTable("PropertyTypes", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.ProviderLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AfterState")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("BeforeState")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Error")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<long>("MessageId")
                        .HasColumnType("bigint");

                    b.Property<int?>("ProviderTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.HasIndex("ProviderTypeId");

                    b.HasIndex(new[] { "Event", "BeforeState", "AfterState", "ProviderTypeId" }, "IX_ProcessLogs");

                    b.ToTable("ProviderLogs", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.ProviderParameter", b =>
                {
                    b.Property<int>("ProviderTypeId")
                        .HasColumnType("int");

                    b.Property<int>("ParameterTypeId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProviderTypeId", "ParameterTypeId");

                    b.HasIndex("ParameterTypeId");

                    b.ToTable("ProviderParameters", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.ProviderType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("CanProcessEmails")
                        .HasColumnType("bit");

                    b.Property<bool>("CanProcessTexts")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("FactoryType")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name", "CanProcessEmails", "CanProcessTexts", "Priority", "IsDisabled" }, "IX_ProviderTypes");

                    b.ToTable("ProviderTypes", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.MailMessage", b =>
                {
                    b.HasBaseType("CG.Purple.SqlServer.Entities.Message");

                    b.Property<string>("BCC")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CC")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.Property<bool>("IsHtml")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Subject")
                        .HasMaxLength(998)
                        .HasColumnType("nvarchar(998)");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.HasIndex(new[] { "To", "CC", "BCC", "Subject", "IsHtml" }, "IX_MailMessages");

                    b.ToTable("MailMessages", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.TextMessage", b =>
                {
                    b.HasBaseType("CG.Purple.SqlServer.Entities.Message");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasIndex(new[] { "To" }, "IX_TextMessages");

                    b.ToTable("TextMessages", "Purple");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.Attachment", b =>
                {
                    b.HasOne("CG.Purple.SqlServer.Entities.Message", "Message")
                        .WithMany("Attachments")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CG.Purple.SqlServer.Entities.MimeType", "MimeType")
                        .WithMany()
                        .HasForeignKey("MimeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("MimeType");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.FileType", b =>
                {
                    b.HasOne("CG.Purple.SqlServer.Entities.MimeType", "MimeType")
                        .WithMany("FileTypes")
                        .HasForeignKey("MimeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MimeType");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.MessageProperty", b =>
                {
                    b.HasOne("CG.Purple.SqlServer.Entities.Message", "Message")
                        .WithMany("MessageProperties")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CG.Purple.SqlServer.Entities.PropertyType", "PropertyType")
                        .WithMany()
                        .HasForeignKey("PropertyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("PropertyType");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.ProviderLog", b =>
                {
                    b.HasOne("CG.Purple.SqlServer.Entities.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CG.Purple.SqlServer.Entities.ProviderType", "ProviderType")
                        .WithMany()
                        .HasForeignKey("ProviderTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Message");

                    b.Navigation("ProviderType");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.ProviderParameter", b =>
                {
                    b.HasOne("CG.Purple.SqlServer.Entities.ParameterType", "ParameterType")
                        .WithMany()
                        .HasForeignKey("ParameterTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CG.Purple.SqlServer.Entities.ProviderType", "ProviderType")
                        .WithMany()
                        .HasForeignKey("ProviderTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParameterType");

                    b.Navigation("ProviderType");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.MailMessage", b =>
                {
                    b.HasOne("CG.Purple.SqlServer.Entities.Message", null)
                        .WithOne()
                        .HasForeignKey("CG.Purple.SqlServer.Entities.MailMessage", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.TextMessage", b =>
                {
                    b.HasOne("CG.Purple.SqlServer.Entities.Message", null)
                        .WithOne()
                        .HasForeignKey("CG.Purple.SqlServer.Entities.TextMessage", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.Message", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("MessageProperties");
                });

            modelBuilder.Entity("CG.Purple.SqlServer.Entities.MimeType", b =>
                {
                    b.Navigation("FileTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
