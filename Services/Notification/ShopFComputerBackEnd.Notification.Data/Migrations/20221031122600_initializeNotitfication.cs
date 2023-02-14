using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;

#nullable disable

namespace ShopFComputerBackEnd.Notification.Data.Migrations
{
    public partial class initializeNotitfication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:notification_status", "fail,success")
                .Annotation("Npgsql:Enum:notification_type", "sms,email,mobile");

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Devicetoken = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => new { x.UserId, x.Devicetoken, x.ProfileId });
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Context = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Variables = table.Column<ICollection<NotificationVariableValueObject>>(type: "jsonb", nullable: true),
                    Type = table.Column<NotificationType>(type: "notification_type", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyTimestamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: true),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Attachments = table.Column<IEnumerable<NotificationTemplateAttachmentValueObject>>(type: "jsonb", nullable: true),
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Action = table.Column<string>(type: "text", nullable: true),
                    GenealogyName = table.Column<string>(type: "text", nullable: true),
                    ActionTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<NotificationType>(type: "notification_type", nullable: false),
                    ConfigurationType = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<NotificationStatus>(type: "notification_status", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    SentTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Destination = table.Column<string>(type: "text", nullable: true),
                    RawData = table.Column<NotificationBuiltValueObject>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Histories_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ProfileId",
                table: "Devices",
                column: "ProfileId")
                .Annotation("Npgsql:IndexInclude", new[] { "Devicetoken", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId")
                .Annotation("Npgsql:IndexInclude", new[] { "Devicetoken", "ProfileId" });

            migrationBuilder.CreateIndex(
                name: "IX_Histories_TemplateId",
                table: "Histories",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Context",
                table: "Notifications",
                column: "Context")
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "Name", "Type", "Variables" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Context_Name",
                table: "Notifications",
                columns: new[] { "Context", "Name" })
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "Type", "Variables" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Id",
                table: "Notifications",
                column: "Id",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "Context", "Name", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_Id",
                table: "Templates",
                column: "Id")
                .Annotation("Npgsql:IndexInclude", new[] { "LanguageCode", "Subject", "Content" });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_LanguageCode",
                table: "Templates",
                column: "LanguageCode")
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "Subject", "Content" });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_NotificationId",
                table: "Templates",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
