using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetricDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddedSprintLengthToGlobal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SprintLength",
                table: "GlobalMetricSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "GlobalMetricSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "SprintLength",
                value: 14);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SprintLength",
                table: "GlobalMetricSettings");
        }
    }
}
