using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetricDashboard.Migrations
{
    /// <inheritdoc />
    public partial class ChangedRadialSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ColorRanges_RadialSettings_RadialSettingsId1",
                table: "ColorRanges");

            migrationBuilder.DropForeignKey(
                name: "FK_ColorRanges_RadialSettings_RadialSettingsId2",
                table: "ColorRanges");

            migrationBuilder.DropIndex(
                name: "IX_ColorRanges_RadialSettingsId1",
                table: "ColorRanges");

            migrationBuilder.DropIndex(
                name: "IX_ColorRanges_RadialSettingsId2",
                table: "ColorRanges");

            migrationBuilder.DropColumn(
                name: "RadialSettingsId1",
                table: "ColorRanges");

            migrationBuilder.DropColumn(
                name: "RadialSettingsId2",
                table: "ColorRanges");

            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "ColorRanges",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "ColorRanges");

            migrationBuilder.AddColumn<int>(
                name: "RadialSettingsId1",
                table: "ColorRanges",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RadialSettingsId2",
                table: "ColorRanges",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ColorRanges_RadialSettingsId1",
                table: "ColorRanges",
                column: "RadialSettingsId1");

            migrationBuilder.CreateIndex(
                name: "IX_ColorRanges_RadialSettingsId2",
                table: "ColorRanges",
                column: "RadialSettingsId2");

            migrationBuilder.AddForeignKey(
                name: "FK_ColorRanges_RadialSettings_RadialSettingsId1",
                table: "ColorRanges",
                column: "RadialSettingsId1",
                principalTable: "RadialSettings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ColorRanges_RadialSettings_RadialSettingsId2",
                table: "ColorRanges",
                column: "RadialSettingsId2",
                principalTable: "RadialSettings",
                principalColumn: "Id");
        }
    }
}
