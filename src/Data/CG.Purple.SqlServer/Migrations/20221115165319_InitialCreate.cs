﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Purple.SqlServer.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Purple");

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "Purple",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    MessageState = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MimeTypes",
                schema: "Purple",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(127)", unicode: false, maxLength: 127, nullable: false),
                    SubType = table.Column<string>(type: "varchar(127)", unicode: false, maxLength: 127, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MimeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterTypes",
                schema: "Purple",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyTypes",
                schema: "Purple",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProviderTypes",
                schema: "Purple",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailMessages",
                schema: "Purple",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    To = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: false),
                    CC = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: true),
                    BCC = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(998)", maxLength: 998, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHtml = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MailMessages_Messages_Id",
                        column: x => x.Id,
                        principalSchema: "Purple",
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextMessages",
                schema: "Purple",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    To = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextMessages_Messages_Id",
                        column: x => x.Id,
                        principalSchema: "Purple",
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                schema: "Purple",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    OriginalFileName = table.Column<string>(type: "varchar(260)", unicode: false, maxLength: 260, nullable: true),
                    MimeTypeId = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_Messages_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "Purple",
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attachments_MimeTypes_MimeTypeId",
                        column: x => x.MimeTypeId,
                        principalSchema: "Purple",
                        principalTable: "MimeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileTypes",
                schema: "Purple",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MimeTypeId = table.Column<int>(type: "int", nullable: false),
                    Extension = table.Column<string>(type: "varchar(260)", unicode: false, maxLength: 260, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileTypes_MimeTypes_MimeTypeId",
                        column: x => x.MimeTypeId,
                        principalSchema: "Purple",
                        principalTable: "MimeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageProperties",
                schema: "Purple",
                columns: table => new
                {
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    PropertyTypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageProperties", x => new { x.MessageId, x.PropertyTypeId });
                    table.ForeignKey(
                        name: "FK_MessageProperties_Messages_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "Purple",
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageProperties_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalSchema: "Purple",
                        principalTable: "PropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProviderLogs",
                schema: "Purple",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    ProviderTypeId = table.Column<int>(type: "int", nullable: true),
                    Event = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    BeforeState = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    AfterState = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Data = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Error = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderLogs_Messages_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "Purple",
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderLogs_ProviderTypes_ProviderTypeId",
                        column: x => x.ProviderTypeId,
                        principalSchema: "Purple",
                        principalTable: "ProviderTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProviderParameters",
                schema: "Purple",
                columns: table => new
                {
                    ProviderTypeId = table.Column<int>(type: "int", nullable: false),
                    ParameterTypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderParameters", x => new { x.ProviderTypeId, x.ParameterTypeId });
                    table.ForeignKey(
                        name: "FK_ProviderParameters_ParameterTypes_ParameterTypeId",
                        column: x => x.ParameterTypeId,
                        principalSchema: "Purple",
                        principalTable: "ParameterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderParameters_ProviderTypes_ProviderTypeId",
                        column: x => x.ProviderTypeId,
                        principalSchema: "Purple",
                        principalTable: "ProviderTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_MessageId",
                schema: "Purple",
                table: "Attachments",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_MimeTypeId",
                schema: "Purple",
                table: "Attachments",
                column: "MimeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FileTypes",
                schema: "Purple",
                table: "FileTypes",
                column: "Extension",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileTypes_MimeTypeId",
                schema: "Purple",
                table: "FileTypes",
                column: "MimeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MailMessages",
                schema: "Purple",
                table: "MailMessages",
                columns: new[] { "To", "CC", "BCC", "Subject", "IsHtml" });

            migrationBuilder.CreateIndex(
                name: "IX_MessageProperties_PropertyTypeId",
                schema: "Purple",
                table: "MessageProperties",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages",
                schema: "Purple",
                table: "Messages",
                columns: new[] { "MessageType", "MessageState", "IsDisabled" });

            migrationBuilder.CreateIndex(
                name: "IX_MimeTypes",
                schema: "Purple",
                table: "MimeTypes",
                columns: new[] { "Type", "SubType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterTypes",
                schema: "Purple",
                table: "ParameterTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTypes",
                schema: "Purple",
                table: "PropertyTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessLogs",
                schema: "Purple",
                table: "ProviderLogs",
                columns: new[] { "Event", "BeforeState", "AfterState", "ProviderTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderLogs_MessageId",
                schema: "Purple",
                table: "ProviderLogs",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderLogs_ProviderTypeId",
                schema: "Purple",
                table: "ProviderLogs",
                column: "ProviderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderParameters_ParameterTypeId",
                schema: "Purple",
                table: "ProviderParameters",
                column: "ParameterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderTypes",
                schema: "Purple",
                table: "ProviderTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TextMessages",
                schema: "Purple",
                table: "TextMessages",
                column: "To");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "FileTypes",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "MailMessages",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "MessageProperties",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "ProviderLogs",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "ProviderParameters",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "TextMessages",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "MimeTypes",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "PropertyTypes",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "ParameterTypes",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "ProviderTypes",
                schema: "Purple");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "Purple");
        }
    }
}
