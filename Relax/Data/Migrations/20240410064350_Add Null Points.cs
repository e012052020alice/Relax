using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Relax.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNullPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Points",
                table: "AspNetUsers",
                type: "int",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Points",
                table: "AspNetUsers",
                type: "int",
                maxLength: 4,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 4,
                oldNullable: true);
        }
    }
}
