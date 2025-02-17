using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoChatApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChatID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Chats_ChatID",
                        column: x => x.ChatID,
                        principalTable: "Chats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatModelSettings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SelectedModel = table.Column<int>(type: "int", nullable: false),
                    ChatID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatModelSettings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChatModelSettings_Chats_ChatID",
                        column: x => x.ChatID,
                        principalTable: "Chats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelParameters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxTokens = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    TopP = table.Column<float>(type: "real", nullable: false),
                    FrequencyPenalty = table.Column<int>(type: "int", nullable: false),
                    PresencePenalty = table.Column<int>(type: "int", nullable: false),
                    ChatModelSettingsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelParameters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ModelParameters_ChatModelSettings_ChatModelSettingsID",
                        column: x => x.ChatModelSettingsID,
                        principalTable: "ChatModelSettings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatID",
                table: "ChatMessages",
                column: "ChatID");

            migrationBuilder.CreateIndex(
                name: "IX_ChatModelSettings_ChatID",
                table: "ChatModelSettings",
                column: "ChatID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelParameters_ChatModelSettingsID",
                table: "ModelParameters",
                column: "ChatModelSettingsID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ModelParameters");

            migrationBuilder.DropTable(
                name: "ChatModelSettings");

            migrationBuilder.DropTable(
                name: "Chats");
        }
    }
}
