using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetricDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddedColorCalcValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GreenCalculationValue",
                table: "GlobalMetricSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RedCalculationValue",
                table: "GlobalMetricSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YellowCalculationValue",
                table: "GlobalMetricSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "GlobalMetricSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "GreenCalculationValue", "RedCalculationValue", "YellowCalculationValue" },
                values: new object[] { 10, 1, 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GreenCalculationValue",
                table: "GlobalMetricSettings");

            migrationBuilder.DropColumn(
                name: "RedCalculationValue",
                table: "GlobalMetricSettings");

            migrationBuilder.DropColumn(
                name: "YellowCalculationValue",
                table: "GlobalMetricSettings");
        }
    }
}
