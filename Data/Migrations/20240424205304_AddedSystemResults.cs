using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetricDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddedSystemResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "RadialSettings");

            migrationBuilder.CreateTable(
                name: "SystemResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemEnum = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemResults");

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "RadialSettings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
