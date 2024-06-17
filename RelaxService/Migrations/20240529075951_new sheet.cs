using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelaxService.Migrations
{
    /// <inheritdoc />
    public partial class newsheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Search",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Search",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AdsLike",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AdsLike",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AdsClick",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AdsClick",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GenderAds",
                columns: table => new
                {
                    GenderAId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Age18 = table.Column<int>(type: "int", nullable: true),
                    Age25 = table.Column<int>(type: "int", nullable: true),
                    Age35 = table.Column<int>(type: "int", nullable: true),
                    Age45 = table.Column<int>(type: "int", nullable: true),
                    Age65 = table.Column<int>(type: "int", nullable: true),
                    DataDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenderAds", x => x.GenderAId);
                });

            migrationBuilder.CreateTable(
                name: "GenderAdsLike",
                columns: table => new
                {
                    GenderALId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Age18 = table.Column<int>(type: "int", nullable: true),
                    Age25 = table.Column<int>(type: "int", nullable: true),
                    Age35 = table.Column<int>(type: "int", nullable: true),
                    Age45 = table.Column<int>(type: "int", nullable: true),
                    Age65 = table.Column<int>(type: "int", nullable: true),
                    DataDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenderAdsLike", x => x.GenderALId);
                });

            migrationBuilder.CreateTable(
                name: "GenderSearch",
                columns: table => new
                {
                    GenderSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Age18 = table.Column<int>(type: "int", nullable: true),
                    Age25 = table.Column<int>(type: "int", nullable: true),
                    Age35 = table.Column<int>(type: "int", nullable: true),
                    Age45 = table.Column<int>(type: "int", nullable: true),
                    Age65 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenderSearch", x => x.GenderSId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenderAds");

            migrationBuilder.DropTable(
                name: "GenderAdsLike");

            migrationBuilder.DropTable(
                name: "GenderSearch");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Search");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Search");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AdsLike");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AdsLike");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AdsClick");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AdsClick");
        }
    }
}
