using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeHealthy.Migrations
{
    /// <inheritdoc />
    public partial class Images_Convertion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Patients",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Nurses",
                type: "longtext",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsDate",
                table: "AppSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 26, 5, 42, 45, 518, DateTimeKind.Utc).AddTicks(5485),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 10, 26, 4, 43, 12, 469, DateTimeKind.Utc).AddTicks(6072));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Nurses");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsDate",
                table: "AppSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 26, 4, 43, 12, 469, DateTimeKind.Utc).AddTicks(6072),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 10, 26, 5, 42, 45, 518, DateTimeKind.Utc).AddTicks(5485));
        }
    }
}
