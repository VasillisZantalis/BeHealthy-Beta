using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace BeHealthy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Specialties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_SpecialtyId",
                table: "Doctors",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Specialities_SpecialtyId",
                table: "Doctors",
                column: "SpecialtyId",
                principalTable: "Specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Specialities_SpecialtyId",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_SpecialtyId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Doctors",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
