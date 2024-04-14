using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MetricDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddedNavigationProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RadialSettings_Metrics_MetricEnum",
                table: "RadialSettings");

            migrationBuilder.DropIndex(
                name: "IX_RadialSettings_MetricEnum",
                table: "RadialSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Metrics",
                table: "Metrics");

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "MetricEnum",
                keyValue: 15);

            migrationBuilder.AddColumn<int>(
                name: "MetricId",
                table: "RadialSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Metrics",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Metrics",
                table: "Metrics",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Metrics",
                columns: new[] { "Id", "IsDisabled", "MetricEnum", "Settings", "System" },
                values: new object[,]
                {
                    { 1, false, 0, "", 0 },
                    { 2, false, 1, "", 0 },
                    { 3, false, 2, "", 0 },
                    { 4, false, 3, "", 0 },
                    { 5, false, 4, "", 1 },
                    { 6, false, 5, "", 1 },
                    { 7, false, 6, "", 1 },
                    { 8, false, 7, "", 1 },
                    { 9, false, 8, "", 1 },
                    { 10, false, 9, "", 1 },
                    { 11, false, 10, "", 1 },
                    { 12, false, 11, "", 1 },
                    { 13, false, 12, "", 1 },
                    { 14, false, 13, "", 1 },
                    { 15, false, 14, "", 1 },
                    { 16, false, 15, "", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RadialSettings_MetricId",
                table: "RadialSettings",
                column: "MetricId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RadialSettings_Metrics_MetricId",
                table: "RadialSettings",
                column: "MetricId",
                principalTable: "Metrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RadialSettings_Metrics_MetricId",
                table: "RadialSettings");

            migrationBuilder.DropIndex(
                name: "IX_RadialSettings_MetricId",
                table: "RadialSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Metrics",
                table: "Metrics");

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Metrics",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 16);

            migrationBuilder.DropColumn(
                name: "MetricId",
                table: "RadialSettings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Metrics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Metrics",
                table: "Metrics",
                column: "MetricEnum");

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

            migrationBuilder.CreateIndex(
                name: "IX_RadialSettings_MetricEnum",
                table: "RadialSettings",
                column: "MetricEnum",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RadialSettings_Metrics_MetricEnum",
                table: "RadialSettings",
                column: "MetricEnum",
                principalTable: "Metrics",
                principalColumn: "MetricEnum",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
