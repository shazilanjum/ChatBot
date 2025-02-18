using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoChatApp.Migrations
{
    /// <inheritdoc />
    public partial class secondarymigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrequencyPenalty",
                table: "ModelParameters");

            migrationBuilder.DropColumn(
                name: "PresencePenalty",
                table: "ModelParameters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FrequencyPenalty",
                table: "ModelParameters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PresencePenalty",
                table: "ModelParameters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
