using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MetricDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddedTimeScopeToResultsAndSettingSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeScope",
                table: "SystemResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeScope",
                table: "MetricResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "RadialSettings",
                columns: new[] { "Id", "End", "MetricEnum", "MetricId", "PointerUnits", "Start", "Step" },
                values: new object[,]
                {
                    { 5, 20, 10, 13, "days", 0, 5 },
                    { 6, 50, 6, 7, "bugs", 0, 5 },
                    { 7, 100, 15, 16, "%", 0, 10 },
                    { 8, 100, 3, 4, "%", 0, 10 },
                    { 9, 10, 5, 8, "", 0, 1 },
                    { 10, 36, 8, 12, "h", 0, 6 },
                    { 11, 100, 9, 10, "%", 0, 10 },
                    { 12, 180, 0, 1, "deployments", 0, 20 },
                    { 13, 36, 1, 3, "h", 0, 6 },
                    { 14, 14, 1, 2, "days", 0, 1 },
                    { 15, 10, 3, 5, "", 0, 1 },
                    { 16, 10, 14, 15, "h", 0, 1 },
                    { 17, 5000, 7, 9, "objects", 0, 500 },
                    { 18, 5, 13, 14, "handovers", 0, 1 },
                    { 19, 72, 12, 11, "h", 0, 4 },
                    { 20, 60, 4, 6, "months", 0, 6 }
                });

            migrationBuilder.InsertData(
                table: "ColorRanges",
                columns: new[] { "Id", "Color", "From", "RadialSettingsId", "To" },
                values: new object[,]
                {
                    { 15, 0, 0, 5, 5 },
                    { 16, 1, 5, 5, 10 },
                    { 17, 2, 10, 5, 20 },
                    { 18, 0, 0, 6, 12 },
                    { 19, 1, 12, 6, 30 },
                    { 20, 2, 30, 6, 50 },
                    { 21, 0, 90, 7, 100 },
                    { 22, 1, 75, 7, 90 },
                    { 23, 2, 0, 7, 75 },
                    { 24, 0, 0, 8, 2 },
                    { 25, 1, 2, 8, 10 },
                    { 26, 2, 10, 8, 100 },
                    { 27, 0, 8, 9, 10 },
                    { 28, 1, 6, 9, 8 },
                    { 29, 2, 0, 9, 6 },
                    { 30, 0, 0, 10, 3 },
                    { 31, 1, 3, 10, 8 },
                    { 32, 2, 8, 10, 36 },
                    { 33, 0, 90, 11, 100 },
                    { 34, 1, 75, 11, 90 },
                    { 35, 2, 0, 11, 75 },
                    { 36, 0, 24, 12, 180 },
                    { 37, 1, 6, 12, 24 },
                    { 38, 2, 0, 12, 6 },
                    { 39, 0, 0, 13, 1 },
                    { 40, 1, 1, 13, 4 },
                    { 41, 2, 4, 13, 36 },
                    { 42, 0, 0, 14, 1 },
                    { 43, 1, 1, 14, 3 },
                    { 44, 2, 3, 14, 14 },
                    { 45, 0, 8, 15, 10 },
                    { 46, 1, 6, 15, 8 },
                    { 47, 2, 0, 15, 6 },
                    { 48, 0, 0, 16, 1 },
                    { 49, 1, 1, 16, 3 },
                    { 50, 2, 3, 16, 10 },
                    { 51, 0, 3000, 17, 5000 },
                    { 52, 1, 1500, 17, 3000 },
                    { 53, 2, 0, 17, 1500 },
                    { 54, 0, 0, 18, 1 },
                    { 55, 1, 1, 18, 2 },
                    { 56, 2, 2, 18, 5 },
                    { 57, 0, 0, 19, 16 },
                    { 58, 1, 16, 19, 36 },
                    { 59, 2, 36, 19, 72 },
                    { 60, 0, 24, 20, 60 },
                    { 61, 1, 12, 20, 24 },
                    { 62, 2, 0, 20, 12 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "ColorRanges",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "RadialSettings",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DropColumn(
                name: "TimeScope",
                table: "SystemResults");

            migrationBuilder.DropColumn(
                name: "TimeScope",
                table: "MetricResults");
        }
    }
}
