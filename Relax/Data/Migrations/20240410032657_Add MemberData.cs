using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Relax.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "AspNetUsers",
                type: "int",
                maxLength: 4,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "AspNetUsers");
        }
    }
}
