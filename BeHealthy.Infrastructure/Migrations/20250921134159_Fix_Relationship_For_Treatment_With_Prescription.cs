using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeHealthy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Relationship_For_Treatment_With_Prescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Treatments_TreatmentId1",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_TreatmentId1",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "TreatmentId1",
                table: "Prescriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TreatmentId1",
                table: "Prescriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_TreatmentId1",
                table: "Prescriptions",
                column: "TreatmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Treatments_TreatmentId1",
                table: "Prescriptions",
                column: "TreatmentId1",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
