using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeHealthy.Migrations
{
    /// <inheritdoc />
    public partial class Doctors_Image_Nullable_String : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Doctors",
                type: "longtext",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsDate",
                table: "AppSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 26, 4, 43, 12, 469, DateTimeKind.Utc).AddTicks(6072),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 10, 24, 5, 39, 37, 602, DateTimeKind.Utc).AddTicks(4854));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Doctors");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "Doctors",
                type: "longblob",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsDate",
                table: "AppSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 24, 5, 39, 37, 602, DateTimeKind.Utc).AddTicks(4854),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 10, 26, 4, 43, 12, 469, DateTimeKind.Utc).AddTicks(6072));
        }
    }
}
