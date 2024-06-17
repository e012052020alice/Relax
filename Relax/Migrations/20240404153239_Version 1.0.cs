using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Relax.Migrations
{
    /// <inheritdoc />
    public partial class Version10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    AdLoved = table.Column<int>(type: "int", nullable: true),
                    AdUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.AdId);
                });

            migrationBuilder.CreateTable(
                name: "Attractions",
                columns: table => new
                {
                    AttractionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttractionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: true),
                    Authority = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    PointId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberID = table.Column<int>(type: "int", nullable: true),
                    ChangeTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChangeReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemainingPoints = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.PointId);
                });

            migrationBuilder.CreateTable(
                name: "RandomTripHistory",
                columns: table => new
                {
                    RandomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberID = table.Column<int>(type: "int", nullable: true),
                    RandomName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleMapResultLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandomTripHistory", x => x.RandomId);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDetails",
                columns: table => new
                {
                    ScheduleDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    scheduleId = table.Column<int>(type: "int", nullable: true),
                    attractionId = table.Column<int>(type: "int", nullable: true),
                    DailyTripNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDetails", x => x.ScheduleDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tripID = table.Column<int>(type: "int", nullable: true),
                    DayNumber = table.Column<int>(type: "int", nullable: true),
                    TotalStayTime = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: true),
                    TripName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.TripId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "Attractions");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "RandomTripHistory");

            migrationBuilder.DropTable(
                name: "ScheduleDetails");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Trips");
        }
    }
}
