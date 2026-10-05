using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeHealthy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1", "a1b2c3d4-0001-0000-0000-000000000000", "Staff", "STAFF" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");
        }
    }
}
