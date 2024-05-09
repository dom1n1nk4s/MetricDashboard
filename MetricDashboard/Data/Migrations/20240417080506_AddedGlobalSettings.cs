using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetricDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddedGlobalSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalMetricSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Scope = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalMetricSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetricResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetricEnum = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    ObjectsAffectingScore = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetricResults", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GlobalMetricSettings",
                columns: new[] { "Id", "Scope" },
                values: new object[] { 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalMetricSettings");

            migrationBuilder.DropTable(
                name: "MetricResults");
        }
    }
}
