using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeHealthy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_CreatedBy_To_MedicalRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MedicalRecords",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MedicalRecords");
        }
    }
}
