using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MetricDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    MetricEnum = table.Column<int>(type: "int", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    System = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.MetricEnum);
                });

            migrationBuilder.InsertData(
                table: "Metrics",
                columns: new[] { "MetricEnum", "IsDisabled", "Settings", "System" },
                values: new object[,]
                {
                    { 0, false, "", 0 },
                    { 1, false, "", 0 },
                    { 2, false, "", 0 },
                    { 3, false, "", 0 },
                    { 4, false, "", 1 },
                    { 5, false, "", 1 },
                    { 6, false, "", 1 },
                    { 7, false, "", 1 },
                    { 8, false, "", 1 },
                    { 9, false, "", 1 },
                    { 10, false, "", 1 },
                    { 11, false, "", 1 },
                    { 12, false, "", 1 },
                    { 13, false, "", 1 },
                    { 14, false, "", 1 },
                    { 15, false, "", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metrics");
        }
    }
}
