using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelaxService.Migrations
{
    /// <inheritdoc />
    public partial class AddEnterRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnterRoute",
                columns: table => new
                {
                    RouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Value = table.Column<int>(type: "int", nullable: true),
                    DataDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterRoute", x => x.RouteId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnterRoute");
        }
    }
}
