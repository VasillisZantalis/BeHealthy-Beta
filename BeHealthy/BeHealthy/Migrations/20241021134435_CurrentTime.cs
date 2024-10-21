using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeHealthy.Migrations
{
    /// <inheritdoc />
    public partial class CurrentTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Departments",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 10, 21, 13, 38, 40, 498, DateTimeKind.Utc).AddTicks(1765));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsDate",
                table: "AppSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 21, 13, 44, 35, 329, DateTimeKind.Utc).AddTicks(8754),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 10, 21, 13, 38, 40, 497, DateTimeKind.Utc).AddTicks(8168));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Departments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 21, 13, 38, 40, 498, DateTimeKind.Utc).AddTicks(1765),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsDate",
                table: "AppSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 21, 13, 38, 40, 497, DateTimeKind.Utc).AddTicks(8168),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 10, 21, 13, 44, 35, 329, DateTimeKind.Utc).AddTicks(8754));
        }
    }
}
