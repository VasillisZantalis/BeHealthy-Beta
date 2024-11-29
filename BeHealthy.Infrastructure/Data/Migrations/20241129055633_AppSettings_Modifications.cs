using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeHealthy.Migrations
{
    /// <inheritdoc />
    public partial class AppSettings_Modifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoolValue",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "IntValue",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "StringValue",
                table: "AppSettings");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppSettings",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsDate",
                table: "AppSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 29, 5, 56, 32, 727, DateTimeKind.Utc).AddTicks(9825),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 23, 7, 27, 56, 898, DateTimeKind.Utc).AddTicks(5355));

            migrationBuilder.AlterColumn<string>(
                name: "Area",
                table: "AppSettings",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppSettings",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "AppSettings",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "AppSettings",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "AppSettings");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppSettings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsDate",
                table: "AppSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 23, 7, 27, 56, 898, DateTimeKind.Utc).AddTicks(5355),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 29, 5, 56, 32, 727, DateTimeKind.Utc).AddTicks(9825));

            migrationBuilder.AlterColumn<string>(
                name: "Area",
                table: "AppSettings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "BoolValue",
                table: "AppSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IntValue",
                table: "AppSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringValue",
                table: "AppSettings",
                type: "longtext",
                nullable: true);
        }
    }
}
