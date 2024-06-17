using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Relax.Migrations
{
    /// <inheritdoc />
    public partial class Version11addClicks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Clicks",
                table: "Advertisements",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clicks",
                table: "Advertisements");
        }
    }
}
