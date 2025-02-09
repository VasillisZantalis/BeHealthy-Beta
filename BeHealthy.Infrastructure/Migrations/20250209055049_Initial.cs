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
                    Name = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
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
                name: "RolePrivileges",
                columns: table => new
                {
                    Role = table.Column<short>(type: "smallint", nullable: false),
                    PrivilegeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePrivileges", x => new { x.Role, x.PrivilegeId });
                    table.ForeignKey(
                        name: "FK_RolePrivileges_Privileges_PrivilegeId",
                        column: x => x.PrivilegeId,
                        principalTable: "Privileges",
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
                        name: "FK_Doctors_Specialities_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialities",
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
                    { "007db76f-2b01-4538-b901-68c74fc81346", 0, null, "7c7ea8b4-462d-4e1f-89fe-c5d6289d3a45", null, "nurse2@hospital.com", false, "NurseFirstName2", null, "NurseLastName2", false, null, "NURSE2@HOSPITAL.COM", "NURSE2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJw46XHLHY/uYIv4Ns4RbbdTBqnnpJ5GvXLGJuRo2EMbd8O3l8nvfXlgf51GRfm3QA==", null, false, "0cf33421-37a8-444b-9e19-2065d8216430", false, "nurse2@hospital.com" },
                    { "0841d3b7-b31a-4b90-a94b-263235a54766", 0, null, "a5fc8af0-082d-4749-a582-2441fd2dc7de", null, "doctor3@hospital.com", false, "DoctorFirstName3", null, "DoctorLastName3", false, null, "DOCTOR3@HOSPITAL.COM", "DOCTOR3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEHS1MOngsRTHTiXrD0WNtQ652DEyKdw2dEV3jdlALdbephjjNoR2JsARrEYVn3QmEA==", null, false, "cbf4fb64-3179-47c0-8985-9a8bb86dfd52", false, "doctor3@hospital.com" },
                    { "16448dfc-91ea-49d5-887a-f2b8eb2c439f", 0, null, "e9b9f368-a6f5-4bde-9283-ec477f30ee25", null, "nurse1@hospital.com", false, "NurseFirstName1", null, "NurseLastName1", false, null, "NURSE1@HOSPITAL.COM", "NURSE1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJLMAS6W0DMEMfaqDRZ70rkXLEmF58WWr93o4WEvUZfK0Rk2/rIgLjFsHXKzgTNw3w==", null, false, "32b2e103-8e49-4582-982d-c5e4e3e7f406", false, "nurse1@hospital.com" },
                    { "1f216f95-87f6-4303-9888-1a29d1ccfa3d", 0, null, "78c1679f-4018-4fef-9d25-b07c4ff7ef61", null, "doctor1@hospital.com", false, "DoctorFirstName1", null, "DoctorLastName1", false, null, "DOCTOR1@HOSPITAL.COM", "DOCTOR1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEL3eQVUEcBeoU70Ahw9niZFLVeJdllLFOEkp0JgNo2lDvPi9WkXqshGP7v9l1slAQQ==", null, false, "57c23246-4841-42d7-95d1-150eaa4f14ba", false, "doctor1@hospital.com" },
                    { "24e3af45-92b4-4e72-803d-de91fc240340", 0, null, "f51fd366-a0d7-4564-9bae-45127c557c05", null, "doctor4@hospital.com", false, "DoctorFirstName4", null, "DoctorLastName4", false, null, "DOCTOR4@HOSPITAL.COM", "DOCTOR4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEE62cgsaCuK2OVu+rmHgxmRz/NClajHaFBOgOincWfMX9j54Jq83wKPiN1MFBJX4WA==", null, false, "45d43f81-185f-4b00-830f-4c05799d1a2a", false, "doctor4@hospital.com" },
                    { "2a4d9452-9913-46c9-ae49-f7d01771d782", 0, null, "f09fd650-f38d-4d42-98a5-fac3cb3b1860", null, "patient1@hospital.com", false, "PatientFirstName1", null, "PatientLastName1", false, null, "PATIENT1@HOSPITAL.COM", "PATIENT1@HOSPITAL.COM", "AQAAAAIAAYagAAAAENBNHeqZ/JkIv+91YNGCwVyu8NPk7Dd9CI548cSWUKM9XDXpXbPVt/qgaKQ4B4cPxg==", null, false, "08131fe8-a80d-4bac-9c8c-66ae112369b9", false, "patient1@hospital.com" },
                    { "33de060f-5ced-412e-bc2c-6d57ac6a0c39", 0, null, "ba594907-415a-4865-aec7-d8cd40a89f54", null, "patient4@hospital.com", false, "PatientFirstName4", null, "PatientLastName4", false, null, "PATIENT4@HOSPITAL.COM", "PATIENT4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEM0eNxV+nqrUq1TnsT8BSoHv77bnRKRgiizDoi8xm8bdAyap+/gZzkHXy0zPGDQzkg==", null, false, "01a93f3f-b2e8-44a7-aa82-739ef7d8d148", false, "patient4@hospital.com" },
                    { "75a6b557-8dd2-4d67-b0a3-2692d458609c", 0, null, "604c9739-7130-4419-99c3-20c3777445f7", null, "patient5@hospital.com", false, "PatientFirstName5", null, "PatientLastName5", false, null, "PATIENT5@HOSPITAL.COM", "PATIENT5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEIV0QHX1t4EKIy8G4ZNX8jzTyta7/GQsHFpOO1+Ar+InTtyy1xRLhfqgMY4HToHVDQ==", null, false, "1e2cc34f-a690-4987-903f-0a9695188390", false, "patient5@hospital.com" },
                    { "90f35c8d-b97f-41d4-9cf6-acf9eaf50a54", 0, null, "a4ff712b-f956-42cb-b8e7-8cad9d5dfe90", null, "nurse5@hospital.com", false, "NurseFirstName5", null, "NurseLastName5", false, null, "NURSE5@HOSPITAL.COM", "NURSE5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEIJ2CKQFNIKtLt9iGAav5ZvjmRJ6hGvbdkmQr+ozjIKpFUk6whguVFj4STvWYsAlPg==", null, false, "a07ab8b3-0378-4a66-a60e-1b9b5316e271", false, "nurse5@hospital.com" },
                    { "99f2b23d-6103-4bae-8b4c-d561edf2d2c2", 0, null, "d718037c-159d-40be-ab29-52b0eef6b25b", null, "nurse3@hospital.com", false, "NurseFirstName3", null, "NurseLastName3", false, null, "NURSE3@HOSPITAL.COM", "NURSE3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJ4CxCKrYvCZTfjmXmDnuopTv4loVQ3Ip3ORQLS7WTXZhYLaxdII7QkGsD5A6dQ7/Q==", null, false, "895bd43d-bce3-4b7f-8fd0-27d49496f161", false, "nurse3@hospital.com" },
                    { "9aa95f69-2b47-4ec3-aec3-ac023b0d26c9", 0, null, "7523fc52-9936-4478-bed9-2744e7523155", null, "doctor2@hospital.com", false, "DoctorFirstName2", null, "DoctorLastName2", false, null, "DOCTOR2@HOSPITAL.COM", "DOCTOR2@HOSPITAL.COM", "AQAAAAIAAYagAAAAELZi3798+zoHiIo1QDfMNQabJFZ88sJmVaCoao2Bl+dipJd3nuz3zrxGgGO375xEqQ==", null, false, "108553b1-3ca4-4dd3-bb03-7ce21969247c", false, "doctor2@hospital.com" },
                    { "a8efe163-211c-422f-b11e-789ad05b6621", 0, null, "e858992e-1491-44b7-818f-155ae2c38295", null, "patient2@hospital.com", false, "PatientFirstName2", null, "PatientLastName2", false, null, "PATIENT2@HOSPITAL.COM", "PATIENT2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJieuadtJdhDqsJ7LJGb20g2QkS9UHTVJUJLGEmk7Y0mrwOVqmrEE2TVxHQhZ60i8g==", null, false, "b6672154-5e59-4a83-b63e-b58a07832c33", false, "patient2@hospital.com" },
                    { "admin", 0, null, "3cf7d0af-61b6-4ea4-a485-de041fbaa240", null, "admin@gmail.com", false, "Admin", null, "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEJPLfytSVE2KZsC0YvMivMMp7l62ciH9UvVz9o4LG33WUDCLpAZlk3x0RrqsKK2fsA==", null, false, "4acd0f1b-faea-4b3d-9ca0-76675d5e361b", false, "admin@gmail.com" },
                    { "b3eaac7f-00e4-48d4-a5c1-159c6a4158d6", 0, null, "181b77b7-6702-4dc8-9fcf-fc956bb3c94e", null, "doctor5@hospital.com", false, "DoctorFirstName5", null, "DoctorLastName5", false, null, "DOCTOR5@HOSPITAL.COM", "DOCTOR5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEOqTH244e3uESImws8S40+H3Fvh4Kg8ObSB1N6MXXu0mL7JdQ3t2v9Oy8hjW1aaS0w==", null, false, "573bdaec-daa7-486d-9a29-0daa3eb5eff5", false, "doctor5@hospital.com" },
                    { "d9c24d72-8c57-4225-846a-9a7fc19afb0a", 0, null, "68555299-6680-4c33-b26a-f49dc5d468c5", null, "patient3@hospital.com", false, "PatientFirstName3", null, "PatientLastName3", false, null, "PATIENT3@HOSPITAL.COM", "PATIENT3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEN6mqIDXkTlJJfHfFflyx64O3kef5EQ8hRJIudjiGoXyK5uh9jy/FmvTWU8h4DldUA==", null, false, "3de711f2-6dad-44ae-9a09-17392f15682c", false, "patient3@hospital.com" },
                    { "e9fac186-5f4c-4de6-8ce5-fd17c6be365e", 0, null, "5376c650-c1fb-48a3-9985-c84ad1f949bc", null, "nurse4@hospital.com", false, "NurseFirstName4", null, "NurseLastName4", false, null, "NURSE4@HOSPITAL.COM", "NURSE4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEA4C3Wbur6TDGgzb3kG2mGYmDr0XYY44k1o49Lbd7AW/6SXzUoFHuURzPqNeAbRrWA==", null, false, "81de3a17-569c-43b5-bab6-d167d778eed5", false, "nurse4@hospital.com" }
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
                table: "Privileges",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 1, 0, false },
                    { 2, 1, false }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "3", "007db76f-2b01-4538-b901-68c74fc81346" },
                    { "2", "0841d3b7-b31a-4b90-a94b-263235a54766" },
                    { "3", "16448dfc-91ea-49d5-887a-f2b8eb2c439f" },
                    { "2", "1f216f95-87f6-4303-9888-1a29d1ccfa3d" },
                    { "2", "24e3af45-92b4-4e72-803d-de91fc240340" },
                    { "3", "2a4d9452-9913-46c9-ae49-f7d01771d782" },
                    { "3", "33de060f-5ced-412e-bc2c-6d57ac6a0c39" },
                    { "3", "75a6b557-8dd2-4d67-b0a3-2692d458609c" },
                    { "3", "90f35c8d-b97f-41d4-9cf6-acf9eaf50a54" },
                    { "3", "99f2b23d-6103-4bae-8b4c-d561edf2d2c2" },
                    { "2", "9aa95f69-2b47-4ec3-aec3-ac023b0d26c9" },
                    { "3", "a8efe163-211c-422f-b11e-789ad05b6621" },
                    { "0", "admin" },
                    { "2", "b3eaac7f-00e4-48d4-a5c1-159c6a4158d6" },
                    { "3", "d9c24d72-8c57-4225-846a-9a7fc19afb0a" },
                    { "3", "e9fac186-5f4c-4de6-8ce5-fd17c6be365e" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "SpecialtyId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 9, 5, 50, 48, 577, DateTimeKind.Utc).AddTicks(5598), null, "DoctorFirstName1", null, "DoctorLastName1", null, "1f216f95-87f6-4303-9888-1a29d1ccfa3d" },
                    { 2, new DateTime(2025, 2, 9, 5, 50, 48, 768, DateTimeKind.Utc).AddTicks(6581), null, "DoctorFirstName2", null, "DoctorLastName2", null, "9aa95f69-2b47-4ec3-aec3-ac023b0d26c9" },
                    { 3, new DateTime(2025, 2, 9, 5, 50, 48, 963, DateTimeKind.Utc).AddTicks(26), null, "DoctorFirstName3", null, "DoctorLastName3", null, "0841d3b7-b31a-4b90-a94b-263235a54766" },
                    { 4, new DateTime(2025, 2, 9, 5, 50, 49, 153, DateTimeKind.Utc).AddTicks(9264), null, "DoctorFirstName4", null, "DoctorLastName4", null, "24e3af45-92b4-4e72-803d-de91fc240340" },
                    { 5, new DateTime(2025, 2, 9, 5, 50, 49, 343, DateTimeKind.Utc).AddTicks(42), null, "DoctorFirstName5", null, "DoctorLastName5", null, "b3eaac7f-00e4-48d4-a5c1-159c6a4158d6" }
                });

            migrationBuilder.InsertData(
                table: "Nurses",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 9, 5, 50, 48, 639, DateTimeKind.Utc).AddTicks(7767), null, "NurseFirstName1", null, "NurseLastName1", "16448dfc-91ea-49d5-887a-f2b8eb2c439f" },
                    { 2, new DateTime(2025, 2, 9, 5, 50, 48, 834, DateTimeKind.Utc).AddTicks(2715), null, "NurseFirstName2", null, "NurseLastName2", "007db76f-2b01-4538-b901-68c74fc81346" },
                    { 3, new DateTime(2025, 2, 9, 5, 50, 49, 28, DateTimeKind.Utc).AddTicks(2826), null, "NurseFirstName3", null, "NurseLastName3", "99f2b23d-6103-4bae-8b4c-d561edf2d2c2" },
                    { 4, new DateTime(2025, 2, 9, 5, 50, 49, 218, DateTimeKind.Utc).AddTicks(346), null, "NurseFirstName4", null, "NurseLastName4", "e9fac186-5f4c-4de6-8ce5-fd17c6be365e" },
                    { 5, new DateTime(2025, 2, 9, 5, 50, 49, 405, DateTimeKind.Utc).AddTicks(4219), null, "NurseFirstName5", null, "NurseLastName5", "90f35c8d-b97f-41d4-9cf6-acf9eaf50a54" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 9, 5, 50, 48, 703, DateTimeKind.Utc).AddTicks(4768), null, "PatientFirstName1", null, "PatientLastName1", "2a4d9452-9913-46c9-ae49-f7d01771d782" },
                    { 2, new DateTime(2025, 2, 9, 5, 50, 48, 899, DateTimeKind.Utc).AddTicks(1544), null, "PatientFirstName2", null, "PatientLastName2", "a8efe163-211c-422f-b11e-789ad05b6621" },
                    { 3, new DateTime(2025, 2, 9, 5, 50, 49, 91, DateTimeKind.Utc).AddTicks(2208), null, "PatientFirstName3", null, "PatientLastName3", "d9c24d72-8c57-4225-846a-9a7fc19afb0a" },
                    { 4, new DateTime(2025, 2, 9, 5, 50, 49, 280, DateTimeKind.Utc).AddTicks(1272), null, "PatientFirstName4", null, "PatientLastName4", "33de060f-5ced-412e-bc2c-6d57ac6a0c39" },
                    { 5, new DateTime(2025, 2, 9, 5, 50, 49, 467, DateTimeKind.Utc).AddTicks(26), null, "PatientFirstName5", null, "PatientLastName5", "75a6b557-8dd2-4d67-b0a3-2692d458609c" }
                });

            migrationBuilder.InsertData(
                table: "RolePrivileges",
                columns: new[] { "PrivilegeId", "Role" },
                values: new object[,]
                {
                    { 1, (short)2 },
                    { 2, (short)2 },
                    { 1, (short)3 },
                    { 2, (short)3 },
                    { 1, (short)4 },
                    { 2, (short)4 }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDate", "DoctorId", "Duration", "Notes", "NurseId", "PatientId", "Reason", "RoomId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 4, 10, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 1, 0, null, 1 },
                    { 2, new DateTime(2025, 2, 6, 11, 0, 0, 0, DateTimeKind.Utc), 2, 60, null, null, 2, 1, null, 2 },
                    { 3, new DateTime(2025, 2, 7, 9, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 3, 1, null, 2 },
                    { 4, new DateTime(2025, 2, 8, 12, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 4, 1, null, 1 },
                    { 5, new DateTime(2025, 2, 9, 12, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 3, 2, null, 0 },
                    { 6, new DateTime(2025, 2, 9, 14, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 4, 3, null, 3 },
                    { 7, new DateTime(2025, 2, 9, 9, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 1, 3, null, 0 },
                    { 8, new DateTime(2025, 2, 9, 11, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 2, 3, null, 0 },
                    { 9, new DateTime(2025, 2, 9, 15, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 1, 3, null, 3 },
                    { 10, new DateTime(2025, 2, 10, 10, 0, 0, 0, DateTimeKind.Utc), 5, 60, null, null, 5, 4, null, 0 },
                    { 11, new DateTime(2025, 2, 11, 11, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 2, 0, null, 3 },
                    { 12, new DateTime(2025, 2, 12, 12, 0, 0, 0, DateTimeKind.Utc), 2, 60, null, null, 3, 1, null, 0 },
                    { 13, new DateTime(2025, 2, 19, 13, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 5, 3, null, 0 },
                    { 14, new DateTime(2025, 2, 24, 14, 0, 0, 0, DateTimeKind.Utc), 5, 60, null, null, 1, 4, null, 3 }
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
                name: "IX_RolePrivileges_PrivilegeId",
                table: "RolePrivileges",
                column: "PrivilegeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DepartmentId",
                table: "Rooms",
                column: "DepartmentId");

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
                name: "RolePrivileges");

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
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Specialities");
        }
    }
}
