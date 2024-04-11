using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetricDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddedRadialSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RadialSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetricEnum = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Start = table.Column<int>(type: "int", nullable: false),
                    End = table.Column<int>(type: "int", nullable: false),
                    Step = table.Column<int>(type: "int", nullable: false),
                    PointerUnits = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadialSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RadialSettings_Metrics_MetricEnum",
                        column: x => x.MetricEnum,
                        principalTable: "Metrics",
                        principalColumn: "MetricEnum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ColorRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<int>(type: "int", nullable: false),
                    To = table.Column<int>(type: "int", nullable: false),
                    RadialSettingsId = table.Column<int>(type: "int", nullable: false),
                    RadialSettingsId1 = table.Column<int>(type: "int", nullable: true),
                    RadialSettingsId2 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColorRanges_RadialSettings_RadialSettingsId",
                        column: x => x.RadialSettingsId,
                        principalTable: "RadialSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColorRanges_RadialSettings_RadialSettingsId1",
                        column: x => x.RadialSettingsId1,
                        principalTable: "RadialSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ColorRanges_RadialSettings_RadialSettingsId2",
                        column: x => x.RadialSettingsId2,
                        principalTable: "RadialSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColorRanges_RadialSettingsId",
                table: "ColorRanges",
                column: "RadialSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ColorRanges_RadialSettingsId1",
                table: "ColorRanges",
                column: "RadialSettingsId1");

            migrationBuilder.CreateIndex(
                name: "IX_ColorRanges_RadialSettingsId2",
                table: "ColorRanges",
                column: "RadialSettingsId2");

            migrationBuilder.CreateIndex(
                name: "IX_RadialSettings_MetricEnum",
                table: "RadialSettings",
                column: "MetricEnum",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColorRanges");

            migrationBuilder.DropTable(
                name: "RadialSettings");
        }
    }
}
