using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelaxService.Migrations
{
    /// <inheritdoc />
    public partial class baseAnalysisSheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdsClick",
                columns: table => new
                {
                    AdsClickId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: true),
                    AdsId = table.Column<int>(type: "int", nullable: false),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AdsClickTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdsClick", x => x.AdsClickId);
                });

            migrationBuilder.CreateTable(
                name: "AdsDetailM",
                columns: table => new
                {
                    AdsDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adsId = table.Column<int>(type: "int", nullable: false),
                    AnalysisId = table.Column<int>(type: "int", nullable: false),
                    ClickMemberId = table.Column<int>(type: "int", nullable: true),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AdsClick = table.Column<int>(type: "int", nullable: true),
                    LikeClick = table.Column<int>(type: "int", nullable: true),
                    DataDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdsDetailM", x => x.AdsDId);
                });

            migrationBuilder.CreateTable(
                name: "AdsDetailN",
                columns: table => new
                {
                    AdsDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adsId = table.Column<int>(type: "int", nullable: false),
                    AnalysisId = table.Column<int>(type: "int", nullable: false),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AdsClick = table.Column<int>(type: "int", nullable: true),
                    LikeClick = table.Column<int>(type: "int", nullable: true),
                    DataDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdsDetailN", x => x.AdsDId);
                });

            migrationBuilder.CreateTable(
                name: "AdsLike",
                columns: table => new
                {
                    AdsLikeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: true),
                    AdsId = table.Column<int>(type: "int", nullable: false),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AdsLikeTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdsLike", x => x.AdsLikeId);
                });

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    AdId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tripId = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clicks = table.Column<int>(type: "int", nullable: true),
                    AdLoved = table.Column<int>(type: "int", nullable: true),
                    AdUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.AdId);
                });

            migrationBuilder.CreateTable(
                name: "Analysis",
                columns: table => new
                {
                    AnalysisId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalEnter = table.Column<int>(type: "int", nullable: true),
                    EnterDesktop = table.Column<int>(type: "int", nullable: true),
                    EnterMobile = table.Column<int>(type: "int", nullable: true),
                    TotalSearch = table.Column<int>(type: "int", nullable: true),
                    SearchDesktop = table.Column<int>(type: "int", nullable: true),
                    SearchMobile = table.Column<int>(type: "int", nullable: true),
                    TotalAds = table.Column<int>(type: "int", nullable: true),
                    AdsDesktop = table.Column<int>(type: "int", nullable: true),
                    AdsMobile = table.Column<int>(type: "int", nullable: true),
                    TotalLike = table.Column<int>(type: "int", nullable: true),
                    LikeDesktop = table.Column<int>(type: "int", nullable: true),
                    LikeMobile = table.Column<int>(type: "int", nullable: true),
                    DataDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analysis", x => x.AnalysisId);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisDetailM",
                columns: table => new
                {
                    AnalysisDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    analysisId = table.Column<int>(type: "int", nullable: false),
                    memberId = table.Column<int>(type: "int", nullable: true),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SearchClicks = table.Column<int>(type: "int", nullable: true),
                    AdsClicks = table.Column<int>(type: "int", nullable: true),
                    LikeClicks = table.Column<int>(type: "int", nullable: true),
                    DataTime = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisDetailM", x => x.AnalysisDId);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisDetailN",
                columns: table => new
                {
                    AnalysisDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    analysisId = table.Column<int>(type: "int", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Device = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SearchClicks = table.Column<int>(type: "int", nullable: true),
                    AdsClicks = table.Column<int>(type: "int", nullable: true),
                    LikeClicks = table.Column<int>(type: "int", nullable: true),
                    DataTime = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisDetailN", x => x.AnalysisDId);
                });

            migrationBuilder.CreateTable(
                name: "Attractions",
                columns: table => new
                {
                    AttractionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AttractionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TimeCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EstimatedStayTime = table.Column<int>(type: "int", nullable: true),
                    XCoordinate = table.Column<int>(type: "int", nullable: true),
                    YCoordinate = table.Column<int>(type: "int", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attractions", x => x.AttractionId);
                });

            migrationBuilder.CreateTable(
                name: "EnterHome",
                columns: table => new
                {
                    EnterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: true),
                    PublicIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EnterDevice = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EnterPage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EnterTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterHome", x => x.EnterId);
                });

            migrationBuilder.CreateTable(
                name: "Search",
                columns: table => new
                {
                    SearchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: true),
                    SearchIp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SearchTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Search", x => x.SearchId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdsClick");

            migrationBuilder.DropTable(
                name: "AdsDetailM");

            migrationBuilder.DropTable(
                name: "AdsDetailN");

            migrationBuilder.DropTable(
                name: "AdsLike");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "Analysis");

            migrationBuilder.DropTable(
                name: "AnalysisDetailM");

            migrationBuilder.DropTable(
                name: "AnalysisDetailN");

            migrationBuilder.DropTable(
                name: "Attractions");

            migrationBuilder.DropTable(
                name: "EnterHome");

            migrationBuilder.DropTable(
                name: "Search");
        }
    }
}
