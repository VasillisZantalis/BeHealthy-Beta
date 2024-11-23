using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeHealthy.Migrations
{
    /// <inheritdoc />
    public partial class Room_AppointmentId_Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Appointments_AppointmentId",
                table: "Rooms");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "Rooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsDate",
                table: "AppSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 23, 7, 27, 56, 898, DateTimeKind.Utc).AddTicks(5355),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 9, 6, 50, 12, 309, DateTimeKind.Utc).AddTicks(3581));

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Appointments_AppointmentId",
                table: "Rooms",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Appointments_AppointmentId",
                table: "Rooms");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsDate",
                table: "AppSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 9, 6, 50, 12, 309, DateTimeKind.Utc).AddTicks(3581),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 23, 7, 27, 56, 898, DateTimeKind.Utc).AddTicks(5355));

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Appointments_AppointmentId",
                table: "Rooms",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
