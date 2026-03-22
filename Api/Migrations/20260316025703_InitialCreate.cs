using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gifteee.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "forecasts",
                columns: table => new
                {
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    temperature_c = table.Column<int>(type: "integer", nullable: false),
                    summary = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_forecasts", x => x.date);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "forecasts");
        }
    }
}
