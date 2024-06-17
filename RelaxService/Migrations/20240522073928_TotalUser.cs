using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelaxService.Migrations
{
    /// <inheritdoc />
    public partial class TotalUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalUser",
                table: "Analysis",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalUser",
                table: "Analysis");
        }
    }
}
