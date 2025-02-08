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
                    { "0397ae9c-b983-4371-9ae4-f71fa07d94ec", 0, null, "f6bde931-fb7a-43cf-89dd-c583d10eae4a", null, "nurse1@hospital.com", false, "NurseFirstName1", null, "NurseLastName1", false, null, "NURSE1@HOSPITAL.COM", "NURSE1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEB4+GkP7XNcxeA3o8MR1T0OHo62MPrRwoeAxnlJo0lUEQUGFHctb8Mp6oQPOq5X9ng==", null, false, "1116e4ae-1322-404c-a4e7-65fa519a77d8", false, "nurse1@hospital.com" },
                    { "05435dd4-4ebf-4362-89ac-eff6dd5e1459", 0, null, "da21d2bc-d3bb-4c33-9608-22ac6dc953c8", null, "doctor5@hospital.com", false, "DoctorFirstName5", null, "DoctorLastName5", false, null, "DOCTOR5@HOSPITAL.COM", "DOCTOR5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEKsuAMq8Ia8y0Tbtfv8cM6xpBttRo39YjsbHX4YXFIB/K1bxXRyF7KTtQKFP+JOMbw==", null, false, "ae62f0ef-92fb-442f-9532-b0f649ea3988", false, "doctor5@hospital.com" },
                    { "09d2f364-2cac-44c6-8f24-f10f0bd141f4", 0, null, "27f4524f-62b8-465f-85a9-6178f7e863cc", null, "patient5@hospital.com", false, "PatientFirstName5", null, "PatientLastName5", false, null, "PATIENT5@HOSPITAL.COM", "PATIENT5@HOSPITAL.COM", "AQAAAAIAAYagAAAAELHlNVL/CI7QssJJ49PlyVy7R6vbLwQurVqyscT1Uk2A+OFSRWhNGqdAkHNO3hCNNA==", null, false, "075efb86-d14b-4a0c-a651-9d3ab48b417b", false, "patient5@hospital.com" },
                    { "238eb6ae-b314-4feb-97e5-a6796f105334", 0, null, "d20e189d-ebb5-4120-9cfc-eb02a1788996", null, "patient2@hospital.com", false, "PatientFirstName2", null, "PatientLastName2", false, null, "PATIENT2@HOSPITAL.COM", "PATIENT2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAQO7CSRY0pVsrdZHjQZADww5VFAZXhhvGos86hGlUkL87qDyj9FmpJBS9sBGZTlGA==", null, false, "a7e0c70e-896f-4c88-ae98-ec4aa1d95a28", false, "patient2@hospital.com" },
                    { "28bd2f90-e12d-47c7-9eaf-ca603d053a8a", 0, null, "e3cdb6ae-3799-4a05-8a84-54b31b747c53", null, "nurse3@hospital.com", false, "NurseFirstName3", null, "NurseLastName3", false, null, "NURSE3@HOSPITAL.COM", "NURSE3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEA5iX+O1ALDc5sqUDeUrry4mTi70AWgrdnh/ew1K+rlDdcHK6wOq83b89yENod7hsg==", null, false, "2e61996d-ca11-4162-a9e6-4ba059a19e22", false, "nurse3@hospital.com" },
                    { "3946916e-103d-4f9d-a7ae-1e95469952b8", 0, null, "6f47e241-b1cd-4054-98ce-56deeb6b8ab4", null, "patient3@hospital.com", false, "PatientFirstName3", null, "PatientLastName3", false, null, "PATIENT3@HOSPITAL.COM", "PATIENT3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEPHAJbD3DP7HgPsf5U75O9YpFw1RJSPXheVUHOZdX0rKtNMhVX1vEjd0A5Mmee52GQ==", null, false, "5f90f68d-1001-42e4-8646-dd73996f0d7a", false, "patient3@hospital.com" },
                    { "4c369bd1-8936-4b9c-8f8d-bf41d5c2d639", 0, null, "0609c1b1-cc0a-4687-8787-83591478de54", null, "doctor2@hospital.com", false, "DoctorFirstName2", null, "DoctorLastName2", false, null, "DOCTOR2@HOSPITAL.COM", "DOCTOR2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEBB9zV2kqRSgZUs5nQ/d7zHbfJfy861o6pGuRWyipt7EDHuMvow8oR5IVGndwVpOpw==", null, false, "061cfd2c-1e77-49ec-bf5e-6bc472215b64", false, "doctor2@hospital.com" },
                    { "54406172-49a9-450a-82c6-abaac31032c3", 0, null, "fb8582c4-72f8-46b2-8d0c-49bd3b9d460b", null, "patient4@hospital.com", false, "PatientFirstName4", null, "PatientLastName4", false, null, "PATIENT4@HOSPITAL.COM", "PATIENT4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAT7Zahn47Jet54l+Z4kQCNjFO685RAkOmZOQynEhyCH0WDXpkCa3UF5ei4FcXj/Gw==", null, false, "2f980558-1cf7-4346-9e51-3bdd05f47fd9", false, "patient4@hospital.com" },
                    { "7dbca746-42a3-4ef6-b534-88f264c618c0", 0, null, "b52f7640-31d0-4d6e-a0a3-248a3141c47f", null, "nurse2@hospital.com", false, "NurseFirstName2", null, "NurseLastName2", false, null, "NURSE2@HOSPITAL.COM", "NURSE2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEI6RabpfjAv6yyKhQjsbZngmh+Y9ZvGc6sFGLM0fBXo8ZKd3wkvCsXklWi+qR3lLgw==", null, false, "64e1a8cf-45e2-4c12-928e-76c3bee8b668", false, "nurse2@hospital.com" },
                    { "9cb38449-3a3c-4b6b-86c9-89575b9078af", 0, null, "50cb6bf3-7183-46c6-8487-cd806ee916b0", null, "doctor1@hospital.com", false, "DoctorFirstName1", null, "DoctorLastName1", false, null, "DOCTOR1@HOSPITAL.COM", "DOCTOR1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFseB4t3J4s7FYmuyP2QuS0OAPQ+4pOXYVUkbhFHs6Z0kR4vqkfoxTG9sj+dUCYxOg==", null, false, "234f6bc6-f013-4ac0-a678-563d9fd6c3fa", false, "doctor1@hospital.com" },
                    { "admin", 0, null, "6d578dd3-d312-4523-98be-f212b13f4082", null, "admin@gmail.com", false, "Admin", null, "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAECNqqIeqwnY3yQm2E9+UKccFVYpICyvLxjq/R8f5mNrULlfeMLyjSTSNXrbUZmpV+A==", null, false, "03b3472c-d01b-4ee4-965f-ec31d8b79f62", false, "admin@gmail.com" },
                    { "c54be023-67b8-4ea9-a7e7-fefc68510f68", 0, null, "8ba74890-d9e2-4da3-b538-a4424c81c38e", null, "doctor3@hospital.com", false, "DoctorFirstName3", null, "DoctorLastName3", false, null, "DOCTOR3@HOSPITAL.COM", "DOCTOR3@HOSPITAL.COM", "AQAAAAIAAYagAAAAENfY8tvJJ+Vwn3Rw2bQD44IORzKOYPc3m71TeQbPU72NCgcmbVSCIc9hfxFQ9B/lvg==", null, false, "6bf5b1bb-ec52-4400-8d8c-27aa43efec13", false, "doctor3@hospital.com" },
                    { "c6c11fe9-e030-4ded-bf68-c000331ae54d", 0, null, "7c3c09ac-4ac6-4b75-adc3-04e03dddffc8", null, "nurse5@hospital.com", false, "NurseFirstName5", null, "NurseLastName5", false, null, "NURSE5@HOSPITAL.COM", "NURSE5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJcDpLuonL7GIHe8GhWsJW7wb6nxdf7WWjB5b2q2Y7wJjwlqyPxIluOUoQ8uCN0FXA==", null, false, "f596da1c-49b0-451e-8ea7-ebc56c5a53dc", false, "nurse5@hospital.com" },
                    { "d9a6e9f3-6137-4388-9745-02765fb200c3", 0, null, "2cc07658-e345-4703-a872-0388d19af17d", null, "doctor4@hospital.com", false, "DoctorFirstName4", null, "DoctorLastName4", false, null, "DOCTOR4@HOSPITAL.COM", "DOCTOR4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEE48Y4AneNs5sO/SEqL4nEY7Jh/zRqPIhXQ8iiVwE1rzN5+wILpDHTmC6Cw2ONa1hw==", null, false, "40d6ba1c-3b1f-4093-9f15-4489005fd448", false, "doctor4@hospital.com" },
                    { "db3040a9-ee23-48ed-b272-a6584fed270e", 0, null, "538e173d-c6b4-465c-bc06-e9c08a5fe855", null, "nurse4@hospital.com", false, "NurseFirstName4", null, "NurseLastName4", false, null, "NURSE4@HOSPITAL.COM", "NURSE4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJHHDxSpbWxHDYqv6Daw2BsyGxOteK6jEimidijSsNcC5DUxL7wBBPMpxPWBCl9W8g==", null, false, "ecb3cc2b-2165-43d1-b2c5-3624fb695312", false, "nurse4@hospital.com" },
                    { "ffff3493-5849-4efe-ba6e-92bbc570f8ce", 0, null, "6c799115-7e98-4fbe-8719-00879195050d", null, "patient1@hospital.com", false, "PatientFirstName1", null, "PatientLastName1", false, null, "PATIENT1@HOSPITAL.COM", "PATIENT1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFSp2PE1acapOGgBL6GiNs97Rx6B3TNWmoCmGSzrZyKs4EMM/rpTD4MOFIA68tbtgQ==", null, false, "6cf83c15-cf3b-4239-945d-47520d9d6c79", false, "patient1@hospital.com" }
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
                    { "3", "0397ae9c-b983-4371-9ae4-f71fa07d94ec" },
                    { "2", "05435dd4-4ebf-4362-89ac-eff6dd5e1459" },
                    { "3", "09d2f364-2cac-44c6-8f24-f10f0bd141f4" },
                    { "3", "238eb6ae-b314-4feb-97e5-a6796f105334" },
                    { "3", "28bd2f90-e12d-47c7-9eaf-ca603d053a8a" },
                    { "3", "3946916e-103d-4f9d-a7ae-1e95469952b8" },
                    { "2", "4c369bd1-8936-4b9c-8f8d-bf41d5c2d639" },
                    { "3", "54406172-49a9-450a-82c6-abaac31032c3" },
                    { "3", "7dbca746-42a3-4ef6-b534-88f264c618c0" },
                    { "2", "9cb38449-3a3c-4b6b-86c9-89575b9078af" },
                    { "0", "admin" },
                    { "2", "c54be023-67b8-4ea9-a7e7-fefc68510f68" },
                    { "3", "c6c11fe9-e030-4ded-bf68-c000331ae54d" },
                    { "2", "d9a6e9f3-6137-4388-9745-02765fb200c3" },
                    { "3", "db3040a9-ee23-48ed-b272-a6584fed270e" },
                    { "3", "ffff3493-5849-4efe-ba6e-92bbc570f8ce" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "SpecialtyId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 8, 20, 7, 43, 486, DateTimeKind.Utc).AddTicks(1190), null, "DoctorFirstName1", null, "DoctorLastName1", null, "9cb38449-3a3c-4b6b-86c9-89575b9078af" },
                    { 2, new DateTime(2025, 2, 8, 20, 7, 43, 680, DateTimeKind.Utc).AddTicks(8851), null, "DoctorFirstName2", null, "DoctorLastName2", null, "4c369bd1-8936-4b9c-8f8d-bf41d5c2d639" },
                    { 3, new DateTime(2025, 2, 8, 20, 7, 43, 871, DateTimeKind.Utc).AddTicks(4694), null, "DoctorFirstName3", null, "DoctorLastName3", null, "c54be023-67b8-4ea9-a7e7-fefc68510f68" },
                    { 4, new DateTime(2025, 2, 8, 20, 7, 44, 60, DateTimeKind.Utc).AddTicks(4112), null, "DoctorFirstName4", null, "DoctorLastName4", null, "d9a6e9f3-6137-4388-9745-02765fb200c3" },
                    { 5, new DateTime(2025, 2, 8, 20, 7, 44, 246, DateTimeKind.Utc).AddTicks(3306), null, "DoctorFirstName5", null, "DoctorLastName5", null, "05435dd4-4ebf-4362-89ac-eff6dd5e1459" }
                });

            migrationBuilder.InsertData(
                table: "Nurses",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 8, 20, 7, 43, 550, DateTimeKind.Utc).AddTicks(6411), null, "NurseFirstName1", null, "NurseLastName1", "0397ae9c-b983-4371-9ae4-f71fa07d94ec" },
                    { 2, new DateTime(2025, 2, 8, 20, 7, 43, 743, DateTimeKind.Utc).AddTicks(7543), null, "NurseFirstName2", null, "NurseLastName2", "7dbca746-42a3-4ef6-b534-88f264c618c0" },
                    { 3, new DateTime(2025, 2, 8, 20, 7, 43, 934, DateTimeKind.Utc).AddTicks(4917), null, "NurseFirstName3", null, "NurseLastName3", "28bd2f90-e12d-47c7-9eaf-ca603d053a8a" },
                    { 4, new DateTime(2025, 2, 8, 20, 7, 44, 122, DateTimeKind.Utc).AddTicks(8034), null, "NurseFirstName4", null, "NurseLastName4", "db3040a9-ee23-48ed-b272-a6584fed270e" },
                    { 5, new DateTime(2025, 2, 8, 20, 7, 44, 307, DateTimeKind.Utc).AddTicks(8910), null, "NurseFirstName5", null, "NurseLastName5", "c6c11fe9-e030-4ded-bf68-c000331ae54d" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 8, 20, 7, 43, 615, DateTimeKind.Utc).AddTicks(8528), null, "PatientFirstName1", null, "PatientLastName1", "ffff3493-5849-4efe-ba6e-92bbc570f8ce" },
                    { 2, new DateTime(2025, 2, 8, 20, 7, 43, 807, DateTimeKind.Utc).AddTicks(5965), null, "PatientFirstName2", null, "PatientLastName2", "238eb6ae-b314-4feb-97e5-a6796f105334" },
                    { 3, new DateTime(2025, 2, 8, 20, 7, 43, 997, DateTimeKind.Utc).AddTicks(6375), null, "PatientFirstName3", null, "PatientLastName3", "3946916e-103d-4f9d-a7ae-1e95469952b8" },
                    { 4, new DateTime(2025, 2, 8, 20, 7, 44, 184, DateTimeKind.Utc).AddTicks(7178), null, "PatientFirstName4", null, "PatientLastName4", "54406172-49a9-450a-82c6-abaac31032c3" },
                    { 5, new DateTime(2025, 2, 8, 20, 7, 44, 369, DateTimeKind.Utc).AddTicks(6776), null, "PatientFirstName5", null, "PatientLastName5", "09d2f364-2cac-44c6-8f24-f10f0bd141f4" }
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
                    { 1, new DateTime(2025, 2, 9, 10, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 1, 0, null, 0 },
                    { 2, new DateTime(2025, 2, 9, 11, 0, 0, 0, DateTimeKind.Utc), 2, 60, null, null, 2, 1, null, 0 },
                    { 3, new DateTime(2025, 2, 9, 12, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 3, 2, null, 0 },
                    { 4, new DateTime(2025, 2, 9, 13, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 4, 3, null, 0 },
                    { 5, new DateTime(2025, 2, 9, 14, 0, 0, 0, DateTimeKind.Utc), 5, 60, null, null, 5, 4, null, 0 },
                    { 6, new DateTime(2025, 2, 10, 10, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 2, 0, null, 0 },
                    { 7, new DateTime(2025, 2, 10, 11, 0, 0, 0, DateTimeKind.Utc), 2, 60, null, null, 3, 1, null, 0 },
                    { 8, new DateTime(2025, 2, 10, 12, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 4, 2, null, 0 },
                    { 9, new DateTime(2025, 2, 10, 13, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 5, 3, null, 0 },
                    { 10, new DateTime(2025, 2, 10, 14, 0, 0, 0, DateTimeKind.Utc), 5, 60, null, null, 1, 4, null, 0 }
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
