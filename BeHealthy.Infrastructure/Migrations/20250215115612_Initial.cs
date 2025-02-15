using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeHealthy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Group = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Caption = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Privileges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRolePrivileges",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    PrivilegeId = table.Column<int>(type: "integer", nullable: false),
                    HasPrivilege = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolePrivileges", x => new { x.Id, x.PrivilegeId });
                    table.ForeignKey(
                        name: "FK_UserRolePrivileges_Privileges_PrivilegeId",
                        column: x => x.PrivilegeId,
                        principalTable: "Privileges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRolePrivileges_Roles_Id",
                        column: x => x.Id,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppointmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    DoctorId = table.Column<int>(type: "integer", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: true),
                    NurseId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HeadOfDepartmentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    SpecialtyId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctors_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doctors_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Doctors_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Nurses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nurses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nurses_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nurses_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Medication = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Dosage = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DatePrescribed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    DoctorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "Id", "Caption", "Description", "Group", "Key", "Type", "Value" },
                values: new object[,]
                {
                    { 1, "Requires Room for Appointment", "Indicates if a room is required for an appointment.", 0, "AppointmentRequiresRoom", 0, "false" },
                    { 2, "Do not allow doctors without a specialty", "Indicates if doctors without a specialty should be allowed.", 2, "DoNotAllowDoctorWithoutSpecialty", 0, "false" },
                    { 3, "Department requires supervisor", "Indicates if a department requires a supervisor.", 1, "DepartmentRequiresSupervisor", 0, "false" },
                    { 4, "Default Department Supervision", "The default supervisor selection for departments.", 1, "DefaultDepartmentSupervison", 1, "0" }
                });

            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1925455c-910f-4e9c-885b-88e01a3bda70", 0, null, "3cd948de-8934-410f-a6eb-6306a00ff1b1", null, "doctor9@hospital.com", false, "DoctorFirstName9", null, "DoctorLastName9", false, null, "DOCTOR9@HOSPITAL.COM", "DOCTOR9@HOSPITAL.COM", "AQAAAAIAAYagAAAAEHCTlDs7rNTXbI5WXZkZTZvB7SIzGOkEbJ4AqvUJzuobm58SgyYbcWTudo210Vx4nQ==", null, false, "79995fe4-4bba-4ad5-b1c6-e5b34a958e2a", false, "doctor9@hospital.com" },
                    { "3516e743-ccf4-4aa1-9e3e-9f1ce723cac7", 0, null, "d8b45e5d-92e5-411b-aecb-98732fb41035", null, "nurse12@hospital.com", false, "NurseFirstName12", null, "NurseLastName12", false, null, "NURSE12@HOSPITAL.COM", "NURSE12@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAX+psbPQJdcSYYAsrmAU9eBSY3VjuiMMAFSajuqVpqoJSle0gXU66kgAJaZiuERvg==", null, false, "c2588848-05db-45f9-b18c-c9aa18e06226", false, "nurse12@hospital.com" },
                    { "3b9cdb51-945a-44f0-9146-454ba58d724b", 0, null, "c7fbbb25-cc6e-48e8-a80b-fd6b56df8572", null, "doctor13@hospital.com", false, "DoctorFirstName13", null, "DoctorLastName13", false, null, "DOCTOR13@HOSPITAL.COM", "DOCTOR13@HOSPITAL.COM", "AQAAAAIAAYagAAAAEF3bJXlbLgLrB+CDOykeXwjjZy4nuMgkyZiPvXaZUiAYjSEhMlFSDeHyaTePu24rxQ==", null, false, "a26e0abe-2a18-44e9-b5b2-1f99ebd5f48a", false, "doctor13@hospital.com" },
                    { "52a16d55-4686-40b2-a0c0-d6236496c2ac", 0, null, "b4a06754-3845-4acc-98b3-7a708112560b", null, "nurse6@hospital.com", false, "NurseFirstName6", null, "NurseLastName6", false, null, "NURSE6@HOSPITAL.COM", "NURSE6@HOSPITAL.COM", "AQAAAAIAAYagAAAAEEWr7DCNjvEr5492FUc1D+q5a4RJShBE9d+mZJkXkme4nKuJhil0fwl1UKnra3lIvQ==", null, false, "d0596611-4b5f-4b11-a477-7cb9323b9628", false, "nurse6@hospital.com" },
                    { "5a32a0d7-72d3-4d70-bb4e-f309007645b4", 0, null, "53e76a45-600a-4a3e-865b-6009960e0c5c", null, "nurse2@hospital.com", false, "NurseFirstName2", null, "NurseLastName2", false, null, "NURSE2@HOSPITAL.COM", "NURSE2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEOEcp+KjeCExJT4wuz6jOPZyPYS84y/aXG3DnV8H4CzVZlChKuAIrYkETsiHWTvBtQ==", null, false, "44880e9d-1bc9-46f3-903d-8bb8899b1059", false, "nurse2@hospital.com" },
                    { "5cb441b7-160f-44cb-bafb-6b5a5b194a49", 0, null, "b7922435-21a5-41e2-b029-a3c45b0dd3df", null, "patient10@hospital.com", false, "PatientFirstName10", null, "PatientLastName10", false, null, "PATIENT10@HOSPITAL.COM", "PATIENT10@HOSPITAL.COM", "AQAAAAIAAYagAAAAECnu0o1tqNBiH1GmkLeGUeipasmZGqAI1XgXMlFlFGsG7ZHQBH+IRPVw/KgeAJf6nw==", null, false, "352f55b7-c86e-4e7c-b0ef-3f726c6ab7f9", false, "patient10@hospital.com" },
                    { "651c4b0e-c646-4877-be64-0c814bd997d4", 0, null, "f1192fcf-a0f2-40ca-876a-3b8a2f5afc75", null, "nurse8@hospital.com", false, "NurseFirstName8", null, "NurseLastName8", false, null, "NURSE8@HOSPITAL.COM", "NURSE8@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMiDs4Dg6VvOojCFGTZYHdp95VlKtvGR0qmveNv7EvxhJ9wCtD6DDVVoQOuDr1KQ3A==", null, false, "8688eac7-cf64-4fee-885b-50c3019e9636", false, "nurse8@hospital.com" },
                    { "65661122-138d-4a13-b194-4231bafc54f1", 0, null, "1ee8399b-4359-471f-8e4e-9b288b260331", null, "patient13@hospital.com", false, "PatientFirstName13", null, "PatientLastName13", false, null, "PATIENT13@HOSPITAL.COM", "PATIENT13@HOSPITAL.COM", "AQAAAAIAAYagAAAAECMzBV1LGP5f7XL79qncwyjjVF8tRCmxT/GKejXW/duy5sdroWq/M1Qj7f3vVv499A==", null, false, "376e1cde-bde7-48e5-8598-1a18cc417877", false, "patient13@hospital.com" },
                    { "68dc23b0-1df5-4697-b452-1c484246d40d", 0, null, "4147970d-b964-48ac-b414-d8823c0226b7", null, "doctor5@hospital.com", false, "DoctorFirstName5", null, "DoctorLastName5", false, null, "DOCTOR5@HOSPITAL.COM", "DOCTOR5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEOy9bmEHmk2SzTYwS4OrpQFgSetbbqLxh1AQqXZcv/lNxOUBFHkqFztBTlgDopLRtA==", null, false, "5dce384d-904d-40c4-ba3f-be92a293e1f3", false, "doctor5@hospital.com" },
                    { "6c540a9f-48db-4e0e-9c02-01607bdd093c", 0, null, "9c66d8ab-69e8-4d3a-96b7-2f2302e5112a", null, "nurse9@hospital.com", false, "NurseFirstName9", null, "NurseLastName9", false, null, "NURSE9@HOSPITAL.COM", "NURSE9@HOSPITAL.COM", "AQAAAAIAAYagAAAAEA4r1NUMnpSx2J05TjzINGswCk03bMCik0mvzgA7Gof8uOejRFk+fHnxK17urmAjaw==", null, false, "0e5c73cc-e49e-406c-843f-f588d5b2fcf7", false, "nurse9@hospital.com" },
                    { "6db56eaf-ebc3-490b-9ea3-aa16c1f7891c", 0, null, "57042196-85e3-48e5-b3e0-d605264a5052", null, "nurse14@hospital.com", false, "NurseFirstName14", null, "NurseLastName14", false, null, "NURSE14@HOSPITAL.COM", "NURSE14@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMRX/yGBLP+0oLt1WzGErRXIdS0WWDvPYjpbYFXglgcu9jJJDzdOAO7o5sKhaFduTg==", null, false, "21310fb5-fb37-4a5a-9531-0703bca1d6cd", false, "nurse14@hospital.com" },
                    { "749fbea2-55fa-4207-a18f-806983dc83b7", 0, null, "eff53bdf-c1a9-400a-8eac-d892c2dc13b6", null, "doctor2@hospital.com", false, "DoctorFirstName2", null, "DoctorLastName2", false, null, "DOCTOR2@HOSPITAL.COM", "DOCTOR2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEEeeAaBzzl7/l9tkqiH6z/fDO/JSv812s8amhErhNEDzOdsNxnu0xhBPSsVtFhF0qw==", null, false, "48477571-e6b2-4459-9558-bb94db309012", false, "doctor2@hospital.com" },
                    { "7be7d5cb-2e2b-46df-b13a-1bf027de0bce", 0, null, "296dd06e-f44e-495e-a454-c744c0a6df97", null, "doctor8@hospital.com", false, "DoctorFirstName8", null, "DoctorLastName8", false, null, "DOCTOR8@HOSPITAL.COM", "DOCTOR8@HOSPITAL.COM", "AQAAAAIAAYagAAAAEOTAA/I7839G0T5s4JDqGXcWSKMkGcFQncYsGIlyRgyy7RnbF6h8YuC4w+xBGKRspw==", null, false, "ddc14ef2-e6e9-4c56-b30d-329aa4d30b89", false, "doctor8@hospital.com" },
                    { "8148e411-d929-425c-bd53-79815deeba46", 0, null, "6b87d1d2-8db4-4f0f-96c8-e272d7f52fb0", null, "patient12@hospital.com", false, "PatientFirstName12", null, "PatientLastName12", false, null, "PATIENT12@HOSPITAL.COM", "PATIENT12@HOSPITAL.COM", "AQAAAAIAAYagAAAAEI+WSAB+5oL0/blSQDzMb2EnnfPpTfD6rVM1VJ/JDo0HsrAjDR6uybAJA1OUh8KRmA==", null, false, "5d11cf5c-a6ec-44ce-ad71-550008f7b841", false, "patient12@hospital.com" },
                    { "834e0994-cba0-46fc-9120-00bca925cd05", 0, null, "05bc807f-4816-44ff-ab24-f98b9919d9c7", null, "patient5@hospital.com", false, "PatientFirstName5", null, "PatientLastName5", false, null, "PATIENT5@HOSPITAL.COM", "PATIENT5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAYt9bi5lzecm7VSAZKOjbOhAxckv8HqVzB3OYqmChR3z/YIRY681b2dov460+mWuw==", null, false, "30b49277-48cb-49b4-8d8f-9279f1c68931", false, "patient5@hospital.com" },
                    { "8ba00520-7e7e-4e52-90ad-cd65ee83aee1", 0, null, "97d4de1b-b820-4036-8a45-a31a58292fa0", null, "doctor15@hospital.com", false, "DoctorFirstName15", null, "DoctorLastName15", false, null, "DOCTOR15@HOSPITAL.COM", "DOCTOR15@HOSPITAL.COM", "AQAAAAIAAYagAAAAELUn0S38IFlxUFxpsHQqJj3u3o3V7l1ojQBiujgXN0pw8PNqs1XBfNgGqBf8XZv28w==", null, false, "4f9c6ac6-e733-4c91-abad-eb9da9aae6e5", false, "doctor15@hospital.com" },
                    { "8e853de5-bed8-4acc-a83b-776af3131ad3", 0, null, "d7443fb8-c8a7-45cf-b3d4-8eff89ea535b", null, "doctor10@hospital.com", false, "DoctorFirstName10", null, "DoctorLastName10", false, null, "DOCTOR10@HOSPITAL.COM", "DOCTOR10@HOSPITAL.COM", "AQAAAAIAAYagAAAAEBdszKE4Wxwd3RyGJ7CbSAKCb4cMM9LPE1cYeIbc018F173xZLekqMBdsMepHqnXyA==", null, false, "c9bcc455-4a88-4663-86ec-54fd447dac82", false, "doctor10@hospital.com" },
                    { "9049e9c9-27e8-4813-b37e-68257ae6b512", 0, null, "a1acb88e-7934-4d6e-a511-18e3b4493c64", null, "patient15@hospital.com", false, "PatientFirstName15", null, "PatientLastName15", false, null, "PATIENT15@HOSPITAL.COM", "PATIENT15@HOSPITAL.COM", "AQAAAAIAAYagAAAAEN0OSPoiCZpYdx7tAsocLiB0VpNUNBjsMl1Hlp4ftEvHkZ20UbUwY7/QuXNjk3o9AQ==", null, false, "f6695909-1199-413f-90eb-92536b938fef", false, "patient15@hospital.com" },
                    { "9a9c3db9-3b9c-4e89-aaf5-fbef0c98267d", 0, null, "e8fe64aa-7577-474d-aeaa-6afd37560bdc", null, "nurse1@hospital.com", false, "NurseFirstName1", null, "NurseLastName1", false, null, "NURSE1@HOSPITAL.COM", "NURSE1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAhrJ5GdxsiXuEyG2812wGzFkg3vFTvPDpaFUjjh+7bjU1F+IG01U9A36z8tbwGWhg==", null, false, "0f162498-469d-48b5-a15c-7fb43517f196", false, "nurse1@hospital.com" },
                    { "9af1d05b-6954-489c-b9b7-5a89d015aaa3", 0, null, "e1e31ea7-d819-4546-9cba-48451958f7f8", null, "doctor7@hospital.com", false, "DoctorFirstName7", null, "DoctorLastName7", false, null, "DOCTOR7@HOSPITAL.COM", "DOCTOR7@HOSPITAL.COM", "AQAAAAIAAYagAAAAEINDKwLmwRVD/VEDduNnCboZMJtgmzBhBt4VJOAnVZztcZZY7+g07JDJPxk2l1Xm7Q==", null, false, "5102f633-598d-45c7-8c74-98cc4409c11b", false, "doctor7@hospital.com" },
                    { "9b79fe79-a885-4469-905b-8f30be8c0c37", 0, null, "4a39fe8f-54c6-4212-9097-f365760b4858", null, "patient7@hospital.com", false, "PatientFirstName7", null, "PatientLastName7", false, null, "PATIENT7@HOSPITAL.COM", "PATIENT7@HOSPITAL.COM", "AQAAAAIAAYagAAAAEPvuS3P6INHHP6Wh5EOVac1IqezKjwt0gwK9bKk6YE60STd94q84vtihFUw2NegRaA==", null, false, "6cbceed7-8208-4369-b28d-7ccde4f66e06", false, "patient7@hospital.com" },
                    { "9f4333a9-b5f6-4960-aed1-16443f499ed0", 0, null, "9b312602-8196-4f5a-b70a-40a2d15fb4c1", null, "nurse13@hospital.com", false, "NurseFirstName13", null, "NurseLastName13", false, null, "NURSE13@HOSPITAL.COM", "NURSE13@HOSPITAL.COM", "AQAAAAIAAYagAAAAEKjm9X0YS1GYvrzLYYshpG73BdkR8xwi0exnUd/YW7aQW6AL23dfJ7LApod8iHhIDg==", null, false, "23265b10-81c4-4d10-9fac-3a441e60efce", false, "nurse13@hospital.com" },
                    { "9f6ae38b-0fda-4a80-bc83-1996b9c0a552", 0, null, "a732f728-11ba-446d-aaf1-24b26ceb8c34", null, "patient14@hospital.com", false, "PatientFirstName14", null, "PatientLastName14", false, null, "PATIENT14@HOSPITAL.COM", "PATIENT14@HOSPITAL.COM", "AQAAAAIAAYagAAAAEPwonxUZUbQRwaL7ZTFgISuJ2ocreHcvtr/1pxsNIVehmeE3252bLwoq6crGpbcWAQ==", null, false, "685d38ad-e8aa-458e-895d-c79ef8001c38", false, "patient14@hospital.com" },
                    { "a2bd6e00-cbb6-4399-ae34-13ee39547d09", 0, null, "5cb68a04-c0db-4121-ac0c-cccd93977bc8", null, "patient6@hospital.com", false, "PatientFirstName6", null, "PatientLastName6", false, null, "PATIENT6@HOSPITAL.COM", "PATIENT6@HOSPITAL.COM", "AQAAAAIAAYagAAAAEKuYKK5b7apzLW0lZB5gnpTCEKbcS2ECog0yPgXoRjhd11dbUvnd7gTKfpIml3tFnw==", null, false, "ea6d48c9-3c51-4cca-9576-9d44ad71a985", false, "patient6@hospital.com" },
                    { "ad988c62-76dc-4725-9c66-c27f539a5828", 0, null, "b7bd3ecf-1433-4151-ad2d-b98b397e4105", null, "nurse7@hospital.com", false, "NurseFirstName7", null, "NurseLastName7", false, null, "NURSE7@HOSPITAL.COM", "NURSE7@HOSPITAL.COM", "AQAAAAIAAYagAAAAENPLKOhC9fV36RAk/jRCSNRMYX9RN8Wl61mtqmw0X0giD0ptAv4OubUUbdkQfUHDMA==", null, false, "222ca360-5bdf-41f9-bd13-551173f4c091", false, "nurse7@hospital.com" },
                    { "admin", 0, null, "e1745cd4-63da-44db-ac88-b894f8593f9a", null, "admin@gmail.com", false, "Admin", null, "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAECWTT4QMaW1NPU8SsOXuK/GDdCSRMQRbM+oleh9/ED9N8qCZiGCM2aYit5KciLXzHQ==", null, false, "dfbb42a7-becb-4807-99d9-27fccb328273", false, "admin@gmail.com" },
                    { "ae802aa3-17f8-4833-aa60-831f27a3bc4f", 0, null, "f512a8a5-7601-4f1e-ba53-05a0c0b66bf4", null, "doctor14@hospital.com", false, "DoctorFirstName14", null, "DoctorLastName14", false, null, "DOCTOR14@HOSPITAL.COM", "DOCTOR14@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFh9REjgPvyhqpUXlzO29ZtjpdOnZSDPXBL0SFalL20zJZHnAvoRJc2msQGFhBfd5w==", null, false, "4affbbad-d1e7-444f-81af-5941fe184300", false, "doctor14@hospital.com" },
                    { "affdfa06-855a-420a-98c7-1f4db037a518", 0, null, "964e2141-ddff-4a14-8824-c9668cec7abc", null, "patient3@hospital.com", false, "PatientFirstName3", null, "PatientLastName3", false, null, "PATIENT3@HOSPITAL.COM", "PATIENT3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAtE5L06NwAJHSJv9q9pe/V+dOFUA20YlkaCBQUZGfgcJ0Po+RjX0rgTe8EJOIUUvA==", null, false, "ad7bca6e-68fd-4ac0-a403-7e5a9b31cef2", false, "patient3@hospital.com" },
                    { "b6aaffdd-11a2-4405-af82-9dae81ade090", 0, null, "cac1a574-d80c-4e2d-8079-5f39f4af3e24", null, "patient8@hospital.com", false, "PatientFirstName8", null, "PatientLastName8", false, null, "PATIENT8@HOSPITAL.COM", "PATIENT8@HOSPITAL.COM", "AQAAAAIAAYagAAAAEKsPUn4PST2a2f2Sv+yqaBWLFMHMBneP+R4sF0p7Z95RjxVvFqIM9sPE7D/diW4vxg==", null, false, "29b5785b-af99-45a6-9256-62a6824437cf", false, "patient8@hospital.com" },
                    { "c32af251-951f-4130-97c7-37f47179b9d2", 0, null, "142a5224-77ce-4c35-8425-85a7973ee97a", null, "doctor12@hospital.com", false, "DoctorFirstName12", null, "DoctorLastName12", false, null, "DOCTOR12@HOSPITAL.COM", "DOCTOR12@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMasNxBjZfdbVpBRj7GQoQwDtY7Dp6RIllALzf2gOR9fa0zPilBl1BPpfZa6PcBxmg==", null, false, "81d10c76-85b3-4fdd-931f-b1016ffae052", false, "doctor12@hospital.com" },
                    { "c373787d-6d95-4c76-bc2d-b8360a32f1f4", 0, null, "7e3942c7-8e81-41bf-a07c-e6cc885f7735", null, "nurse11@hospital.com", false, "NurseFirstName11", null, "NurseLastName11", false, null, "NURSE11@HOSPITAL.COM", "NURSE11@HOSPITAL.COM", "AQAAAAIAAYagAAAAEIDQse18kcV1zfQzbYIJT2vnSlipN/Vts8U2DXLzwitlYUREQ04kC0F+vKkTwjgjNA==", null, false, "6e312823-88f3-4822-a497-ba8f75646d7f", false, "nurse11@hospital.com" },
                    { "c7d51e42-1ef4-4dc5-b33f-babecef913c4", 0, null, "375f1539-2adf-4283-b50f-67f6dd11014b", null, "doctor4@hospital.com", false, "DoctorFirstName4", null, "DoctorLastName4", false, null, "DOCTOR4@HOSPITAL.COM", "DOCTOR4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEEtUcWN5xi0cnnctAof+lXPDr6UYUZZcWmeve2fc1YqllWRU/xox1KOsvDA07eysQQ==", null, false, "61534f24-1ccf-4f89-9e7c-b7ecf46d42dd", false, "doctor4@hospital.com" },
                    { "c7f2a48f-c682-41cf-baf3-50f9b0484294", 0, null, "2204de45-de89-4b6e-a06c-986174714b97", null, "doctor11@hospital.com", false, "DoctorFirstName11", null, "DoctorLastName11", false, null, "DOCTOR11@HOSPITAL.COM", "DOCTOR11@HOSPITAL.COM", "AQAAAAIAAYagAAAAEPScKWTW+oOCGgbBqAEHgDq6CuEFCBPgvEPH/FD26TGpBMDq7J93WzUkftFf+NejrA==", null, false, "296711b3-a167-4a83-8e1c-ba2fcb8263f7", false, "doctor11@hospital.com" },
                    { "caf2a0d0-8236-451a-8c9e-d89ee2ff8adb", 0, null, "09f32f88-57cb-4558-a2b2-f75aded32d36", null, "nurse5@hospital.com", false, "NurseFirstName5", null, "NurseLastName5", false, null, "NURSE5@HOSPITAL.COM", "NURSE5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEHHV5xZlote2s7Kc2uncoSnv79mUvSIZWk7UmD8HZzbAD55VyrMHnrOj1Jd3hDppRw==", null, false, "f0b5e6f2-299e-49d0-bd79-c66daf3bf0ec", false, "nurse5@hospital.com" },
                    { "cb8731bf-47b1-4e1c-b8e4-6d4ccb1f6be0", 0, null, "c6de5852-8fe2-4a88-b1e5-c1bfe2f678c0", null, "patient4@hospital.com", false, "PatientFirstName4", null, "PatientLastName4", false, null, "PATIENT4@HOSPITAL.COM", "PATIENT4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJDYJ2UWrmpmq3RXqBsyjhBN2EYqR8rvoZiwOUf1Vuhq2DoPol7WdlR2cHBZAzqrvg==", null, false, "95e9040e-efa7-4ea6-8343-02438c1bbf4c", false, "patient4@hospital.com" },
                    { "cd39d25b-6f57-44ba-9890-b78aa8741676", 0, null, "168b28f0-0653-46c0-9ca6-c0f0514b67e1", null, "doctor1@hospital.com", false, "DoctorFirstName1", null, "DoctorLastName1", false, null, "DOCTOR1@HOSPITAL.COM", "DOCTOR1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEO7worhuaLpniSWq73RzmglSwSbIftG2VXkLgsfkhMwwXTXw7sZJoD0cHd4pC0iXAw==", null, false, "6f35d00a-5d47-47d8-a68d-fe41d24b919f", false, "doctor1@hospital.com" },
                    { "d1364d48-5f8a-41ff-89de-d753ffc6fc2c", 0, null, "bc0d7b93-0d8d-4188-b772-255e20d3b0b3", null, "patient9@hospital.com", false, "PatientFirstName9", null, "PatientLastName9", false, null, "PATIENT9@HOSPITAL.COM", "PATIENT9@HOSPITAL.COM", "AQAAAAIAAYagAAAAEEnWvvmfhNhY8ypBXOR8kI4g6Z3mMSTsAeg7Ih8bJpjKQXFBEqm1RHFrPuUJf37epQ==", null, false, "bfdc3f0d-dd35-4329-96cb-68e0e30028da", false, "patient9@hospital.com" },
                    { "d1e8a559-53a9-4903-961a-1487d1f4142b", 0, null, "40bf2e80-ecf7-4d05-8d1c-1ae3c32d177e", null, "nurse3@hospital.com", false, "NurseFirstName3", null, "NurseLastName3", false, null, "NURSE3@HOSPITAL.COM", "NURSE3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEGihzpkjOzBQ2AZbNe8j4/nIxgssKt8ZuzOrH9a4lqItkch4q5cNwl1ESAw91hEy1g==", null, false, "1acb986e-5585-4980-a242-b904f970fadf", false, "nurse3@hospital.com" },
                    { "d9e08611-8db3-4050-bd8d-dfe3e90ccd1c", 0, null, "b3284e20-f08d-492c-b8de-12db6e63815c", null, "doctor3@hospital.com", false, "DoctorFirstName3", null, "DoctorLastName3", false, null, "DOCTOR3@HOSPITAL.COM", "DOCTOR3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAXHKjPpIFb5jLUEsv3SKX3SZ7WEglrkB18eVWtm14yMZpzZydPpzgazCI6lr9dNQQ==", null, false, "9e134971-223d-4658-94cb-399229d6a3d1", false, "doctor3@hospital.com" },
                    { "de52d64b-e20f-40e5-8f53-065f1691ddac", 0, null, "ea19b96e-9b6c-4392-8e0d-33b7eabb3492", null, "patient11@hospital.com", false, "PatientFirstName11", null, "PatientLastName11", false, null, "PATIENT11@HOSPITAL.COM", "PATIENT11@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMMvfW2kBF0AsKTbkL9L9Y17j0eRp8vXCgIxI++mC0WMmetrQxeKrzxYp/9LbNGADw==", null, false, "6df323af-40e7-425c-a9ae-be4336c80fdb", false, "patient11@hospital.com" },
                    { "e10f334f-7d41-465f-b049-a41ef3e47d99", 0, null, "4e5cd1a0-234c-4913-928c-ce2561bd2019", null, "doctor6@hospital.com", false, "DoctorFirstName6", null, "DoctorLastName6", false, null, "DOCTOR6@HOSPITAL.COM", "DOCTOR6@HOSPITAL.COM", "AQAAAAIAAYagAAAAEKBgwJt6BkcJhbhUK/kF9Fg7sAE5Qc/wrOVKGYwz1dvqPlFO311vRVhCHa1h6B52/w==", null, false, "d5872b17-f5ea-4e81-8cd8-4999ec46f81b", false, "doctor6@hospital.com" },
                    { "e992688f-8544-446d-90c2-b984dbad7616", 0, null, "81ac5906-f4b2-414f-8061-4259c576d56f", null, "nurse4@hospital.com", false, "NurseFirstName4", null, "NurseLastName4", false, null, "NURSE4@HOSPITAL.COM", "NURSE4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEKeKaZMq2i78DEvhbpuOSwpPQZjhCiKxhEl+7JsJP7VzOmchZwrD3wNfhc0YzAiZWQ==", null, false, "ce44408f-6c12-4ebd-b496-db9e42c57aa3", false, "nurse4@hospital.com" },
                    { "ea65e73f-4bad-4e67-9f3f-b51315dbe87c", 0, null, "0a607171-2b80-4bf7-b47a-86f2931becbe", null, "patient2@hospital.com", false, "PatientFirstName2", null, "PatientLastName2", false, null, "PATIENT2@HOSPITAL.COM", "PATIENT2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEA/DYUD2GxNz0r2eaKb9+inn/mp5AGXXycmxRpS+lDME70bVqxXAzvIps0aUZ6XdSw==", null, false, "72aeeb50-93e2-4852-bae5-03048a8eb8fe", false, "patient2@hospital.com" },
                    { "ecce0bad-d7c1-44a8-9070-479aa3ffffdf", 0, null, "eda2e9a8-6c94-46ca-b63a-ce87fadf54d0", null, "nurse15@hospital.com", false, "NurseFirstName15", null, "NurseLastName15", false, null, "NURSE15@HOSPITAL.COM", "NURSE15@HOSPITAL.COM", "AQAAAAIAAYagAAAAEKIzmSVTtGNpyLxGSYutLzrXYV/edJha42yM8ueLl+Q1AF0MGU2D0nNE0mQ9g4q/cQ==", null, false, "449d806d-01b4-4a25-aef6-190dd44c11a6", false, "nurse15@hospital.com" },
                    { "f02aa43c-5d9a-4a3b-b83f-a18a58332bbd", 0, null, "5a86de9c-36eb-40b0-bc61-202394fb7e89", null, "nurse10@hospital.com", false, "NurseFirstName10", null, "NurseLastName10", false, null, "NURSE10@HOSPITAL.COM", "NURSE10@HOSPITAL.COM", "AQAAAAIAAYagAAAAEH+0SBRFixcBLNJkojKB47xdyAEA8Uz4d3LEYeja5xN/JBUEQmkK+YV7nTxS8Hd2WQ==", null, false, "21910b6e-e136-4d3b-8cf8-90f1638ca702", false, "nurse10@hospital.com" },
                    { "f37476dd-8c18-45f7-8a6f-74559dd64c9d", 0, null, "dbc974a7-29fc-4411-a926-9571d2177262", null, "patient1@hospital.com", false, "PatientFirstName1", null, "PatientLastName1", false, null, "PATIENT1@HOSPITAL.COM", "PATIENT1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEPQnCliIs4YrIE7ujEaYg1sPEwpQGyflpqxe+cIkwA1TfRxQbNgw/fShBW3sOa4PJA==", null, false, "98f50ee0-4535-492d-9afb-8cddcf770d82", false, "patient1@hospital.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0", null, "Admin", "ADMIN" },
                    { "1", null, "Staff", "STAFF" },
                    { "2", null, "Doctor", "DOCTOR" },
                    { "3", null, "Nurse", "NURSE" },
                    { "4", null, "Patient", "PATIENT" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "HeadOfDepartmentId", "Location", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(771), null, "Building A - Floor 3", "Cardiology" },
                    { 2, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(774), null, "Building B - Floor 2", "Neurology" },
                    { 3, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(776), null, "Building C - Floor 1", "Orthopedics" },
                    { 4, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(777), null, "Building D - Floor 4", "Pediatrics" },
                    { 5, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(779), null, "Building E - Ground Floor", "Emergency" }
                });

            migrationBuilder.InsertData(
                table: "Privileges",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, 0 },
                    { 2, 1 },
                    { 3, 2 },
                    { 4, 3 },
                    { 5, 4 },
                    { 6, 5 },
                    { 7, 6 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, (short)0 },
                    { (short)2, (short)1 },
                    { (short)3, (short)2 },
                    { (short)4, (short)3 },
                    { (short)5, (short)4 }
                });

            migrationBuilder.InsertData(
                table: "Specialties",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(857), "Cardiology" },
                    { 2, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(859), "Neurology" },
                    { 3, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(859), "Orthopedics" },
                    { 4, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(860), "Pediatrics" },
                    { 5, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(861), "Emergency Medicine" },
                    { 6, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(862), "Radiology" },
                    { 7, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(863), "Oncology" },
                    { 8, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(864), "Dermatology" },
                    { 9, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(864), "General Surgery" },
                    { 10, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(865), "Anesthesiology" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2", "1925455c-910f-4e9c-885b-88e01a3bda70" },
                    { "3", "3516e743-ccf4-4aa1-9e3e-9f1ce723cac7" },
                    { "2", "3b9cdb51-945a-44f0-9146-454ba58d724b" },
                    { "3", "52a16d55-4686-40b2-a0c0-d6236496c2ac" },
                    { "3", "5a32a0d7-72d3-4d70-bb4e-f309007645b4" },
                    { "4", "5cb441b7-160f-44cb-bafb-6b5a5b194a49" },
                    { "3", "651c4b0e-c646-4877-be64-0c814bd997d4" },
                    { "4", "65661122-138d-4a13-b194-4231bafc54f1" },
                    { "2", "68dc23b0-1df5-4697-b452-1c484246d40d" },
                    { "3", "6c540a9f-48db-4e0e-9c02-01607bdd093c" },
                    { "3", "6db56eaf-ebc3-490b-9ea3-aa16c1f7891c" },
                    { "2", "749fbea2-55fa-4207-a18f-806983dc83b7" },
                    { "2", "7be7d5cb-2e2b-46df-b13a-1bf027de0bce" },
                    { "4", "8148e411-d929-425c-bd53-79815deeba46" },
                    { "4", "834e0994-cba0-46fc-9120-00bca925cd05" },
                    { "2", "8ba00520-7e7e-4e52-90ad-cd65ee83aee1" },
                    { "2", "8e853de5-bed8-4acc-a83b-776af3131ad3" },
                    { "4", "9049e9c9-27e8-4813-b37e-68257ae6b512" },
                    { "3", "9a9c3db9-3b9c-4e89-aaf5-fbef0c98267d" },
                    { "2", "9af1d05b-6954-489c-b9b7-5a89d015aaa3" },
                    { "4", "9b79fe79-a885-4469-905b-8f30be8c0c37" },
                    { "3", "9f4333a9-b5f6-4960-aed1-16443f499ed0" },
                    { "4", "9f6ae38b-0fda-4a80-bc83-1996b9c0a552" },
                    { "4", "a2bd6e00-cbb6-4399-ae34-13ee39547d09" },
                    { "3", "ad988c62-76dc-4725-9c66-c27f539a5828" },
                    { "0", "admin" },
                    { "2", "ae802aa3-17f8-4833-aa60-831f27a3bc4f" },
                    { "4", "affdfa06-855a-420a-98c7-1f4db037a518" },
                    { "4", "b6aaffdd-11a2-4405-af82-9dae81ade090" },
                    { "2", "c32af251-951f-4130-97c7-37f47179b9d2" },
                    { "3", "c373787d-6d95-4c76-bc2d-b8360a32f1f4" },
                    { "2", "c7d51e42-1ef4-4dc5-b33f-babecef913c4" },
                    { "2", "c7f2a48f-c682-41cf-baf3-50f9b0484294" },
                    { "3", "caf2a0d0-8236-451a-8c9e-d89ee2ff8adb" },
                    { "4", "cb8731bf-47b1-4e1c-b8e4-6d4ccb1f6be0" },
                    { "2", "cd39d25b-6f57-44ba-9890-b78aa8741676" },
                    { "4", "d1364d48-5f8a-41ff-89de-d753ffc6fc2c" },
                    { "3", "d1e8a559-53a9-4903-961a-1487d1f4142b" },
                    { "2", "d9e08611-8db3-4050-bd8d-dfe3e90ccd1c" },
                    { "4", "de52d64b-e20f-40e5-8f53-065f1691ddac" },
                    { "2", "e10f334f-7d41-465f-b049-a41ef3e47d99" },
                    { "3", "e992688f-8544-446d-90c2-b984dbad7616" },
                    { "4", "ea65e73f-4bad-4e67-9f3f-b51315dbe87c" },
                    { "3", "ecce0bad-d7c1-44a8-9070-479aa3ffffdf" },
                    { "3", "f02aa43c-5d9a-4a3b-b83f-a18a58332bbd" },
                    { "4", "f37476dd-8c18-45f7-8a6f-74559dd64c9d" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "SpecialtyId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 15, 11, 56, 8, 962, DateTimeKind.Utc).AddTicks(4429), null, "DoctorFirstName1", null, "DoctorLastName1", null, "cd39d25b-6f57-44ba-9890-b78aa8741676" },
                    { 2, new DateTime(2025, 2, 15, 11, 56, 9, 151, DateTimeKind.Utc).AddTicks(456), null, "DoctorFirstName2", null, "DoctorLastName2", null, "749fbea2-55fa-4207-a18f-806983dc83b7" },
                    { 3, new DateTime(2025, 2, 15, 11, 56, 9, 347, DateTimeKind.Utc).AddTicks(7668), null, "DoctorFirstName3", null, "DoctorLastName3", null, "d9e08611-8db3-4050-bd8d-dfe3e90ccd1c" },
                    { 4, new DateTime(2025, 2, 15, 11, 56, 9, 536, DateTimeKind.Utc).AddTicks(4510), null, "DoctorFirstName4", null, "DoctorLastName4", null, "c7d51e42-1ef4-4dc5-b33f-babecef913c4" },
                    { 5, new DateTime(2025, 2, 15, 11, 56, 9, 727, DateTimeKind.Utc).AddTicks(9960), null, "DoctorFirstName5", null, "DoctorLastName5", null, "68dc23b0-1df5-4697-b452-1c484246d40d" },
                    { 6, new DateTime(2025, 2, 15, 11, 56, 9, 916, DateTimeKind.Utc).AddTicks(7172), null, "DoctorFirstName6", null, "DoctorLastName6", null, "e10f334f-7d41-465f-b049-a41ef3e47d99" },
                    { 7, new DateTime(2025, 2, 15, 11, 56, 10, 105, DateTimeKind.Utc).AddTicks(1898), null, "DoctorFirstName7", null, "DoctorLastName7", null, "9af1d05b-6954-489c-b9b7-5a89d015aaa3" },
                    { 8, new DateTime(2025, 2, 15, 11, 56, 10, 292, DateTimeKind.Utc).AddTicks(1669), null, "DoctorFirstName8", null, "DoctorLastName8", null, "7be7d5cb-2e2b-46df-b13a-1bf027de0bce" },
                    { 9, new DateTime(2025, 2, 15, 11, 56, 10, 480, DateTimeKind.Utc).AddTicks(2954), null, "DoctorFirstName9", null, "DoctorLastName9", null, "1925455c-910f-4e9c-885b-88e01a3bda70" },
                    { 10, new DateTime(2025, 2, 15, 11, 56, 10, 669, DateTimeKind.Utc).AddTicks(901), null, "DoctorFirstName10", null, "DoctorLastName10", null, "8e853de5-bed8-4acc-a83b-776af3131ad3" },
                    { 11, new DateTime(2025, 2, 15, 11, 56, 10, 857, DateTimeKind.Utc).AddTicks(4243), null, "DoctorFirstName11", null, "DoctorLastName11", null, "c7f2a48f-c682-41cf-baf3-50f9b0484294" },
                    { 12, new DateTime(2025, 2, 15, 11, 56, 11, 46, DateTimeKind.Utc).AddTicks(2189), null, "DoctorFirstName12", null, "DoctorLastName12", null, "c32af251-951f-4130-97c7-37f47179b9d2" },
                    { 13, new DateTime(2025, 2, 15, 11, 56, 11, 235, DateTimeKind.Utc).AddTicks(1194), null, "DoctorFirstName13", null, "DoctorLastName13", null, "3b9cdb51-945a-44f0-9146-454ba58d724b" },
                    { 14, new DateTime(2025, 2, 15, 11, 56, 11, 422, DateTimeKind.Utc).AddTicks(900), null, "DoctorFirstName14", null, "DoctorLastName14", null, "ae802aa3-17f8-4833-aa60-831f27a3bc4f" },
                    { 15, new DateTime(2025, 2, 15, 11, 56, 11, 610, DateTimeKind.Utc).AddTicks(1430), null, "DoctorFirstName15", null, "DoctorLastName15", null, "8ba00520-7e7e-4e52-90ad-cd65ee83aee1" }
                });

            migrationBuilder.InsertData(
                table: "Nurses",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 15, 11, 56, 9, 25, DateTimeKind.Utc).AddTicks(3673), null, "NurseFirstName1", null, "NurseLastName1", "9a9c3db9-3b9c-4e89-aaf5-fbef0c98267d" },
                    { 2, new DateTime(2025, 2, 15, 11, 56, 9, 221, DateTimeKind.Utc).AddTicks(5809), null, "NurseFirstName2", null, "NurseLastName2", "5a32a0d7-72d3-4d70-bb4e-f309007645b4" },
                    { 3, new DateTime(2025, 2, 15, 11, 56, 9, 410, DateTimeKind.Utc).AddTicks(7118), null, "NurseFirstName3", null, "NurseLastName3", "d1e8a559-53a9-4903-961a-1487d1f4142b" },
                    { 4, new DateTime(2025, 2, 15, 11, 56, 9, 599, DateTimeKind.Utc).AddTicks(4116), null, "NurseFirstName4", null, "NurseLastName4", "e992688f-8544-446d-90c2-b984dbad7616" },
                    { 5, new DateTime(2025, 2, 15, 11, 56, 9, 790, DateTimeKind.Utc).AddTicks(9565), null, "NurseFirstName5", null, "NurseLastName5", "caf2a0d0-8236-451a-8c9e-d89ee2ff8adb" },
                    { 6, new DateTime(2025, 2, 15, 11, 56, 9, 979, DateTimeKind.Utc).AddTicks(6591), null, "NurseFirstName6", null, "NurseLastName6", "52a16d55-4686-40b2-a0c0-d6236496c2ac" },
                    { 7, new DateTime(2025, 2, 15, 11, 56, 10, 167, DateTimeKind.Utc).AddTicks(9491), null, "NurseFirstName7", null, "NurseLastName7", "ad988c62-76dc-4725-9c66-c27f539a5828" },
                    { 8, new DateTime(2025, 2, 15, 11, 56, 10, 354, DateTimeKind.Utc).AddTicks(2638), null, "NurseFirstName8", null, "NurseLastName8", "651c4b0e-c646-4877-be64-0c814bd997d4" },
                    { 9, new DateTime(2025, 2, 15, 11, 56, 10, 543, DateTimeKind.Utc).AddTicks(3782), null, "NurseFirstName9", null, "NurseLastName9", "6c540a9f-48db-4e0e-9c02-01607bdd093c" },
                    { 10, new DateTime(2025, 2, 15, 11, 56, 10, 732, DateTimeKind.Utc).AddTicks(2096), null, "NurseFirstName10", null, "NurseLastName10", "f02aa43c-5d9a-4a3b-b83f-a18a58332bbd" },
                    { 11, new DateTime(2025, 2, 15, 11, 56, 10, 920, DateTimeKind.Utc).AddTicks(4082), null, "NurseFirstName11", null, "NurseLastName11", "c373787d-6d95-4c76-bc2d-b8360a32f1f4" },
                    { 12, new DateTime(2025, 2, 15, 11, 56, 11, 109, DateTimeKind.Utc).AddTicks(2914), null, "NurseFirstName12", null, "NurseLastName12", "3516e743-ccf4-4aa1-9e3e-9f1ce723cac7" },
                    { 13, new DateTime(2025, 2, 15, 11, 56, 11, 297, DateTimeKind.Utc).AddTicks(5866), null, "NurseFirstName13", null, "NurseLastName13", "9f4333a9-b5f6-4960-aed1-16443f499ed0" },
                    { 14, new DateTime(2025, 2, 15, 11, 56, 11, 484, DateTimeKind.Utc).AddTicks(3908), null, "NurseFirstName14", null, "NurseLastName14", "6db56eaf-ebc3-490b-9ea3-aa16c1f7891c" },
                    { 15, new DateTime(2025, 2, 15, 11, 56, 11, 672, DateTimeKind.Utc).AddTicks(8387), null, "NurseFirstName15", null, "NurseLastName15", "ecce0bad-d7c1-44a8-9070-479aa3ffffdf" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 15, 11, 56, 9, 88, DateTimeKind.Utc).AddTicks(2452), null, "PatientFirstName1", null, "PatientLastName1", "f37476dd-8c18-45f7-8a6f-74559dd64c9d" },
                    { 2, new DateTime(2025, 2, 15, 11, 56, 9, 284, DateTimeKind.Utc).AddTicks(5020), null, "PatientFirstName2", null, "PatientLastName2", "ea65e73f-4bad-4e67-9f3f-b51315dbe87c" },
                    { 3, new DateTime(2025, 2, 15, 11, 56, 9, 473, DateTimeKind.Utc).AddTicks(6329), null, "PatientFirstName3", null, "PatientLastName3", "affdfa06-855a-420a-98c7-1f4db037a518" },
                    { 4, new DateTime(2025, 2, 15, 11, 56, 9, 662, DateTimeKind.Utc).AddTicks(8868), null, "PatientFirstName4", null, "PatientLastName4", "cb8731bf-47b1-4e1c-b8e4-6d4ccb1f6be0" },
                    { 5, new DateTime(2025, 2, 15, 11, 56, 9, 853, DateTimeKind.Utc).AddTicks(9423), null, "PatientFirstName5", null, "PatientLastName5", "834e0994-cba0-46fc-9120-00bca925cd05" },
                    { 6, new DateTime(2025, 2, 15, 11, 56, 10, 42, DateTimeKind.Utc).AddTicks(4720), null, "PatientFirstName6", null, "PatientLastName6", "a2bd6e00-cbb6-4399-ae34-13ee39547d09" },
                    { 7, new DateTime(2025, 2, 15, 11, 56, 10, 230, DateTimeKind.Utc).AddTicks(1596), null, "PatientFirstName7", null, "PatientLastName7", "9b79fe79-a885-4469-905b-8f30be8c0c37" },
                    { 8, new DateTime(2025, 2, 15, 11, 56, 10, 417, DateTimeKind.Utc).AddTicks(5258), null, "PatientFirstName8", null, "PatientLastName8", "b6aaffdd-11a2-4405-af82-9dae81ade090" },
                    { 9, new DateTime(2025, 2, 15, 11, 56, 10, 606, DateTimeKind.Utc).AddTicks(3482), null, "PatientFirstName9", null, "PatientLastName9", "d1364d48-5f8a-41ff-89de-d753ffc6fc2c" },
                    { 10, new DateTime(2025, 2, 15, 11, 56, 10, 795, DateTimeKind.Utc).AddTicks(1448), null, "PatientFirstName10", null, "PatientLastName10", "5cb441b7-160f-44cb-bafb-6b5a5b194a49" },
                    { 11, new DateTime(2025, 2, 15, 11, 56, 10, 983, DateTimeKind.Utc).AddTicks(3875), null, "PatientFirstName11", null, "PatientLastName11", "de52d64b-e20f-40e5-8f53-065f1691ddac" },
                    { 12, new DateTime(2025, 2, 15, 11, 56, 11, 172, DateTimeKind.Utc).AddTicks(1580), null, "PatientFirstName12", null, "PatientLastName12", "8148e411-d929-425c-bd53-79815deeba46" },
                    { 13, new DateTime(2025, 2, 15, 11, 56, 11, 359, DateTimeKind.Utc).AddTicks(4483), null, "PatientFirstName13", null, "PatientLastName13", "65661122-138d-4a13-b194-4231bafc54f1" },
                    { 14, new DateTime(2025, 2, 15, 11, 56, 11, 547, DateTimeKind.Utc).AddTicks(2217), null, "PatientFirstName14", null, "PatientLastName14", "9f6ae38b-0fda-4a80-bc83-1996b9c0a552" },
                    { 15, new DateTime(2025, 2, 15, 11, 56, 11, 736, DateTimeKind.Utc).AddTicks(3200), null, "PatientFirstName15", null, "PatientLastName15", "9049e9c9-27e8-4813-b37e-68257ae6b512" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "Name", "Number" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(809), 1, "Room 301", 301 },
                    { 2, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(811), 1, "Room 302", 302 },
                    { 3, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(812), 2, "Room 201", 201 },
                    { 4, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(813), 2, "Room 202", 202 },
                    { 5, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(820), 3, "Room 101", 101 },
                    { 6, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(821), 3, "Room 102", 102 },
                    { 7, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(822), 4, "Room 401", 401 },
                    { 8, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(823), 4, "Room 402", 402 },
                    { 9, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(824), 5, "Emergency Room 1", 1 },
                    { 10, new DateTime(2025, 2, 15, 11, 56, 8, 836, DateTimeKind.Utc).AddTicks(825), 5, "Emergency Room 2", 2 }
                });

            migrationBuilder.InsertData(
                table: "UserRolePrivileges",
                columns: new[] { "Id", "PrivilegeId", "HasPrivilege" },
                values: new object[,]
                {
                    { (short)1, 1, false },
                    { (short)1, 2, false },
                    { (short)1, 3, false },
                    { (short)1, 4, false },
                    { (short)1, 5, false },
                    { (short)1, 6, false },
                    { (short)1, 7, false },
                    { (short)2, 1, false },
                    { (short)2, 2, false },
                    { (short)2, 3, false },
                    { (short)2, 4, false },
                    { (short)2, 5, false },
                    { (short)2, 6, false },
                    { (short)2, 7, false },
                    { (short)3, 1, false },
                    { (short)3, 2, false },
                    { (short)3, 3, false },
                    { (short)3, 4, false },
                    { (short)3, 5, false },
                    { (short)3, 6, false },
                    { (short)3, 7, false },
                    { (short)4, 1, false },
                    { (short)4, 2, false },
                    { (short)4, 3, false },
                    { (short)4, 4, false },
                    { (short)4, 5, false },
                    { (short)4, 6, false },
                    { (short)4, 7, false },
                    { (short)5, 1, false },
                    { (short)5, 2, false },
                    { (short)5, 3, false },
                    { (short)5, 4, false },
                    { (short)5, 5, false },
                    { (short)5, 6, false },
                    { (short)5, 7, false }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDate", "DoctorId", "Duration", "Notes", "NurseId", "PatientId", "Reason", "RoomId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 10, 10, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 1, 0, null, 1 },
                    { 2, new DateTime(2025, 2, 12, 11, 0, 0, 0, DateTimeKind.Utc), 2, 60, null, null, 2, 1, null, 2 },
                    { 3, new DateTime(2025, 2, 13, 9, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 3, 1, null, 2 },
                    { 4, new DateTime(2025, 2, 14, 12, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 4, 1, null, 1 },
                    { 5, new DateTime(2025, 2, 15, 12, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 3, 2, null, 0 },
                    { 6, new DateTime(2025, 2, 15, 14, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 4, 3, null, 3 },
                    { 7, new DateTime(2025, 2, 15, 9, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 1, 3, null, 0 },
                    { 8, new DateTime(2025, 2, 15, 11, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 2, 3, null, 0 },
                    { 9, new DateTime(2025, 2, 15, 15, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 1, 3, null, 3 },
                    { 10, new DateTime(2025, 2, 16, 10, 0, 0, 0, DateTimeKind.Utc), 5, 60, null, null, 5, 4, null, 0 },
                    { 11, new DateTime(2025, 2, 17, 11, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 2, 0, null, 3 },
                    { 12, new DateTime(2025, 2, 18, 12, 0, 0, 0, DateTimeKind.Utc), 2, 60, null, null, 3, 1, null, 0 },
                    { 13, new DateTime(2025, 2, 25, 13, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 5, 3, null, 0 },
                    { 14, new DateTime(2025, 3, 2, 14, 0, 0, 0, DateTimeKind.Utc), 5, 60, null, null, 1, 4, null, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "ApplicationUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "ApplicationUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_NurseId",
                table: "Appointments",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RoomId",
                table: "Appointments",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_HeadOfDepartmentId",
                table: "Departments",
                column: "HeadOfDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctors",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_SpecialtyId",
                table: "Doctors",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UserId",
                table: "Doctors",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nurses_DepartmentId",
                table: "Nurses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Nurses_UserId",
                table: "Nurses",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DepartmentId",
                table: "Patients",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserId",
                table: "Patients",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DoctorId",
                table: "Prescriptions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientId",
                table: "Prescriptions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DepartmentId",
                table: "Rooms",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePrivileges_PrivilegeId",
                table: "UserRolePrivileges",
                column: "PrivilegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Nurses_NurseId",
                table: "Appointments",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Doctors_HeadOfDepartmentId",
                table: "Departments",
                column: "HeadOfDepartmentId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Doctors_HeadOfDepartmentId",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "UserRolePrivileges");

            migrationBuilder.DropTable(
                name: "Nurses");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Privileges");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Specialties");
        }
    }
}
