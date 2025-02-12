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
                    Value = table.Column<bool>(type: "boolean", nullable: false),
                    Role = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.Id);
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
                    { "00a6e247-38b4-4b63-9366-115d6a09c5f3", 0, null, "c324dbb5-b313-4d35-8650-04905fd9fa77", null, "nurse2@hospital.com", false, "NurseFirstName2", null, "NurseLastName2", false, null, "NURSE2@HOSPITAL.COM", "NURSE2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFdCoZvqRpnKPPftY8wFjqHuaUrdpyeAxX5bl/MgtWZ4hN8dTRFbpLssZVAMWYZVDA==", null, false, "bbd096f3-f1bf-4fe4-b600-8d4f583b3557", false, "nurse2@hospital.com" },
                    { "231c2fd9-0a6d-4e07-a68b-985995f02a1e", 0, null, "b1ab772d-a616-4e72-abcc-84e6164d52ad", null, "nurse3@hospital.com", false, "NurseFirstName3", null, "NurseLastName3", false, null, "NURSE3@HOSPITAL.COM", "NURSE3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEOD5EXvpVaOAwW6aDb9M8l9pTQj07wseNF9OKQlNrAn8d8Byl9fv58chkhjNZ9swyQ==", null, false, "3d174285-f7ee-4244-9f55-630b35344d1a", false, "nurse3@hospital.com" },
                    { "45d6985b-92e8-4b74-82d7-b41b582cb38c", 0, null, "bda223dc-606c-42d4-9342-ddf2ce94f4df", null, "nurse4@hospital.com", false, "NurseFirstName4", null, "NurseLastName4", false, null, "NURSE4@HOSPITAL.COM", "NURSE4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEObSh2sPCNKPL3aIAI61655Q3XXO57C6Qn9zymAscOc2M2+HiLn1t4YDwvkujVjydg==", null, false, "708ca8dd-8c54-4a66-9173-988e1b28b264", false, "nurse4@hospital.com" },
                    { "48a0a661-e1f5-46c9-bd07-1d0d469d81e5", 0, null, "658955dd-3a31-4adc-a504-1b11e4918454", null, "doctor4@hospital.com", false, "DoctorFirstName4", null, "DoctorLastName4", false, null, "DOCTOR4@HOSPITAL.COM", "DOCTOR4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEGEuhApiRMnn7wincAee1u8IpJNjL8RdBLf5oHPOsrR8OqzWVN1EbgmW2xeiMuyJAg==", null, false, "36ae072d-cb59-40b1-8dd8-d39798e81365", false, "doctor4@hospital.com" },
                    { "51dca3b9-a53b-442c-ace0-8b5b3a521534", 0, null, "fd7af345-070b-4a5c-970d-ca412ba01e8d", null, "doctor3@hospital.com", false, "DoctorFirstName3", null, "DoctorLastName3", false, null, "DOCTOR3@HOSPITAL.COM", "DOCTOR3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMVDnQavM2Jc5fDWHc69Cc6R7tpw4sjJ7bOjdleRF3cean+Ox4tYDv6fgvT9N+KUMA==", null, false, "fbd25bb1-37e9-47e3-be83-32dbf6bbb730", false, "doctor3@hospital.com" },
                    { "5832fbc1-a87a-4f13-baea-ea6190a72eeb", 0, null, "10c5e49c-9303-48ab-849e-b9d9b3bd7a68", null, "doctor2@hospital.com", false, "DoctorFirstName2", null, "DoctorLastName2", false, null, "DOCTOR2@HOSPITAL.COM", "DOCTOR2@HOSPITAL.COM", "AQAAAAIAAYagAAAAECoqnF3q/8HNHlQGPgM/pzp9U7mfHnDdtIKwN3TgHfnFl3SPd3sISss6FRYPXB6N4g==", null, false, "56fce081-862c-4c2c-bfd4-c1942a9c23f3", false, "doctor2@hospital.com" },
                    { "5c2ef146-eb1f-49b0-846a-356c6483371e", 0, null, "e4b9a07d-d915-40ca-bbf0-d4f74abb82b2", null, "patient3@hospital.com", false, "PatientFirstName3", null, "PatientLastName3", false, null, "PATIENT3@HOSPITAL.COM", "PATIENT3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEDWd0pCxkdbFbfTRy2j3VNlFPAwQZgFn0n+7cWQe8QEg6OUlIZBHcPqyY56XhlNs5A==", null, false, "dcf7341c-d575-4c7c-bf7b-1a9a2e16c5e5", false, "patient3@hospital.com" },
                    { "7763ead9-092a-4f77-b276-73a12ff8c004", 0, null, "3f7de4ba-d3ba-435b-b338-3338a485f22a", null, "doctor5@hospital.com", false, "DoctorFirstName5", null, "DoctorLastName5", false, null, "DOCTOR5@HOSPITAL.COM", "DOCTOR5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEEO8B938oD9clPS7A3CWfAo8qIlTjkbWS/kAbuZPBfucU3YX7Zu8T4CugwblGzkdQQ==", null, false, "bc3ff746-1db9-461f-aa6e-172428d8ec5c", false, "doctor5@hospital.com" },
                    { "97b027e1-b4db-49fe-b32c-998f4bfda382", 0, null, "680dde61-d3da-4722-96f5-1058916c67d8", null, "nurse5@hospital.com", false, "NurseFirstName5", null, "NurseLastName5", false, null, "NURSE5@HOSPITAL.COM", "NURSE5@HOSPITAL.COM", "AQAAAAIAAYagAAAAELQOGEozmTCUFZ4UFEQu07FlUW6Oaj8/TRwD2EgG4s3SGdPcop/CSBBtbmPPB3qdsA==", null, false, "4a850a3e-6b4b-4dc6-9189-8092dd87bf1b", false, "nurse5@hospital.com" },
                    { "a4500349-ba39-4744-856d-f1278a4edcfd", 0, null, "55ad2278-80ca-4415-89d5-dcfd6eb6cec1", null, "patient2@hospital.com", false, "PatientFirstName2", null, "PatientLastName2", false, null, "PATIENT2@HOSPITAL.COM", "PATIENT2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEKoNq4CergTD7csgRH/z1U3DwKK4i1fWTMHfq7OYDVI8CtokmzBeOKdGyfE/xKh45g==", null, false, "3ca122d8-c4b1-40aa-bbe6-7bbfa28e8366", false, "patient2@hospital.com" },
                    { "acda1a42-da2b-4ffc-b498-c0a581584add", 0, null, "e729bcd2-2fc3-4e35-98d9-a6cc0cd58a4a", null, "nurse1@hospital.com", false, "NurseFirstName1", null, "NurseLastName1", false, null, "NURSE1@HOSPITAL.COM", "NURSE1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEM4aPHwbS6KaP5dDonONiT+9wlgIB8F4rSNLWhP+/lXP8P9ct+p26bnb4W7lyh5brQ==", null, false, "1e12ac78-c063-47fa-bca9-868a3fafa17e", false, "nurse1@hospital.com" },
                    { "admin", 0, null, "e4b901c6-6238-468d-b518-237d4ea3333a", null, "admin@gmail.com", false, "Admin", null, "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEO7s322OtKGjoKl0P6Ia429GPP0txfu1gpdVWafDJxDBM94w+NYK5w4GLdp94jgk1g==", null, false, "8b78a353-4166-4292-afa9-9e6fd3087671", false, "admin@gmail.com" },
                    { "d1e9df37-cdde-40af-be2f-aa63cba67abc", 0, null, "1261d937-e179-4151-971f-09f76f36dfe5", null, "patient5@hospital.com", false, "PatientFirstName5", null, "PatientLastName5", false, null, "PATIENT5@HOSPITAL.COM", "PATIENT5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEIfnyqQxSNzqyDwYPE32AxOal9WphNsOgWt829f3iydc6o3SI3UJ0e60AtfyTuuPlw==", null, false, "b0f356fa-171e-4e0d-845d-21b03e87b9e7", false, "patient5@hospital.com" },
                    { "df4b2193-ea45-4e51-8510-23009e5b76da", 0, null, "b834a27e-d26f-4db8-a2c6-9ce77a97362e", null, "patient1@hospital.com", false, "PatientFirstName1", null, "PatientLastName1", false, null, "PATIENT1@HOSPITAL.COM", "PATIENT1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEIkgrBA24Fd6SoW6cDU7N7DeHGzPNrA81Wu98Sb9y5P3w/Ic52TtRtsacj5Wkell+g==", null, false, "df163052-aae6-42fd-82d3-8413727788da", false, "patient1@hospital.com" },
                    { "e513e63c-3bdb-4921-aa89-2eb0deca2b05", 0, null, "f6bac65b-99a8-4b29-9594-67ae8051e2c1", null, "doctor1@hospital.com", false, "DoctorFirstName1", null, "DoctorLastName1", false, null, "DOCTOR1@HOSPITAL.COM", "DOCTOR1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEORjib0pY1A1acijcIaK0gruB/0/lIDoNW+ITZh6BfD7KDDLQYsoILN/Ov60HbYzrQ==", null, false, "3293d6b0-7d4f-4b30-8860-9cd4814fa805", false, "doctor1@hospital.com" },
                    { "ef657508-f8bc-4086-97f4-1e9958f230e9", 0, null, "24499bbf-14c6-4042-b175-b172f1bf1a0c", null, "patient4@hospital.com", false, "PatientFirstName4", null, "PatientLastName4", false, null, "PATIENT4@HOSPITAL.COM", "PATIENT4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMiD922Z8B9QG15Suq3f1AHUUTDrGUJI8mx5HDr8bw6FkKrOW14vHpjhL38+G08Oxg==", null, false, "ce1db907-db24-4e9a-98eb-b7fdfa87670d", false, "patient4@hospital.com" }
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
                    { 1, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2415), null, "Building A - Floor 3", "Cardiology" },
                    { 2, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2418), null, "Building B - Floor 2", "Neurology" },
                    { 3, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2419), null, "Building C - Floor 1", "Orthopedics" },
                    { 4, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2421), null, "Building D - Floor 4", "Pediatrics" },
                    { 5, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2423), null, "Building E - Ground Floor", "Emergency" }
                });

            migrationBuilder.InsertData(
                table: "Privileges",
                columns: new[] { "Id", "Name", "Role", "Value" },
                values: new object[,]
                {
                    { 1, 0, (short)2, true },
                    { 2, 1, (short)2, true },
                    { 3, 4, (short)2, true },
                    { 4, 5, (short)2, false },
                    { 5, 3, (short)2, false },
                    { 6, 2, (short)2, false },
                    { 7, 6, (short)4, false },
                    { 8, 7, (short)4, false },
                    { 9, 8, (short)3, false },
                    { 10, 9, (short)3, false },
                    { 11, 10, (short)3, false }
                });

            migrationBuilder.InsertData(
                table: "Specialties",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2519), "Cardiology" },
                    { 2, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2521), "Neurology" },
                    { 3, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2522), "Orthopedics" },
                    { 4, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2523), "Pediatrics" },
                    { 5, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2524), "Emergency Medicine" },
                    { 6, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2525), "Radiology" },
                    { 7, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2526), "Oncology" },
                    { 8, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2535), "Dermatology" },
                    { 9, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2536), "General Surgery" },
                    { 10, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2538), "Anesthesiology" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "3", "00a6e247-38b4-4b63-9366-115d6a09c5f3" },
                    { "3", "231c2fd9-0a6d-4e07-a68b-985995f02a1e" },
                    { "3", "45d6985b-92e8-4b74-82d7-b41b582cb38c" },
                    { "2", "48a0a661-e1f5-46c9-bd07-1d0d469d81e5" },
                    { "2", "51dca3b9-a53b-442c-ace0-8b5b3a521534" },
                    { "2", "5832fbc1-a87a-4f13-baea-ea6190a72eeb" },
                    { "4", "5c2ef146-eb1f-49b0-846a-356c6483371e" },
                    { "2", "7763ead9-092a-4f77-b276-73a12ff8c004" },
                    { "3", "97b027e1-b4db-49fe-b32c-998f4bfda382" },
                    { "4", "a4500349-ba39-4744-856d-f1278a4edcfd" },
                    { "3", "acda1a42-da2b-4ffc-b498-c0a581584add" },
                    { "0", "admin" },
                    { "4", "d1e9df37-cdde-40af-be2f-aa63cba67abc" },
                    { "4", "df4b2193-ea45-4e51-8510-23009e5b76da" },
                    { "2", "e513e63c-3bdb-4921-aa89-2eb0deca2b05" },
                    { "4", "ef657508-f8bc-4086-97f4-1e9958f230e9" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "SpecialtyId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 12, 15, 46, 30, 628, DateTimeKind.Utc).AddTicks(1407), null, "DoctorFirstName1", null, "DoctorLastName1", null, "e513e63c-3bdb-4921-aa89-2eb0deca2b05" },
                    { 2, new DateTime(2025, 2, 12, 15, 46, 30, 818, DateTimeKind.Utc).AddTicks(416), null, "DoctorFirstName2", null, "DoctorLastName2", null, "5832fbc1-a87a-4f13-baea-ea6190a72eeb" },
                    { 3, new DateTime(2025, 2, 12, 15, 46, 31, 7, DateTimeKind.Utc).AddTicks(5767), null, "DoctorFirstName3", null, "DoctorLastName3", null, "51dca3b9-a53b-442c-ace0-8b5b3a521534" },
                    { 4, new DateTime(2025, 2, 12, 15, 46, 31, 198, DateTimeKind.Utc).AddTicks(97), null, "DoctorFirstName4", null, "DoctorLastName4", null, "48a0a661-e1f5-46c9-bd07-1d0d469d81e5" },
                    { 5, new DateTime(2025, 2, 12, 15, 46, 31, 385, DateTimeKind.Utc).AddTicks(3209), null, "DoctorFirstName5", null, "DoctorLastName5", null, "7763ead9-092a-4f77-b276-73a12ff8c004" }
                });

            migrationBuilder.InsertData(
                table: "Nurses",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 12, 15, 46, 30, 691, DateTimeKind.Utc).AddTicks(9694), null, "NurseFirstName1", null, "NurseLastName1", "acda1a42-da2b-4ffc-b498-c0a581584add" },
                    { 2, new DateTime(2025, 2, 12, 15, 46, 30, 881, DateTimeKind.Utc).AddTicks(1081), null, "NurseFirstName2", null, "NurseLastName2", "00a6e247-38b4-4b63-9366-115d6a09c5f3" },
                    { 3, new DateTime(2025, 2, 12, 15, 46, 31, 71, DateTimeKind.Utc).AddTicks(2659), null, "NurseFirstName3", null, "NurseLastName3", "231c2fd9-0a6d-4e07-a68b-985995f02a1e" },
                    { 4, new DateTime(2025, 2, 12, 15, 46, 31, 261, DateTimeKind.Utc).AddTicks(972), null, "NurseFirstName4", null, "NurseLastName4", "45d6985b-92e8-4b74-82d7-b41b582cb38c" },
                    { 5, new DateTime(2025, 2, 12, 15, 46, 31, 447, DateTimeKind.Utc).AddTicks(6234), null, "NurseFirstName5", null, "NurseLastName5", "97b027e1-b4db-49fe-b32c-998f4bfda382" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 12, 15, 46, 30, 754, DateTimeKind.Utc).AddTicks(7000), null, "PatientFirstName1", null, "PatientLastName1", "df4b2193-ea45-4e51-8510-23009e5b76da" },
                    { 2, new DateTime(2025, 2, 12, 15, 46, 30, 944, DateTimeKind.Utc).AddTicks(1326), null, "PatientFirstName2", null, "PatientLastName2", "a4500349-ba39-4744-856d-f1278a4edcfd" },
                    { 3, new DateTime(2025, 2, 12, 15, 46, 31, 134, DateTimeKind.Utc).AddTicks(6589), null, "PatientFirstName3", null, "PatientLastName3", "5c2ef146-eb1f-49b0-846a-356c6483371e" },
                    { 4, new DateTime(2025, 2, 12, 15, 46, 31, 323, DateTimeKind.Utc).AddTicks(2261), null, "PatientFirstName4", null, "PatientLastName4", "ef657508-f8bc-4086-97f4-1e9958f230e9" },
                    { 5, new DateTime(2025, 2, 12, 15, 46, 31, 509, DateTimeKind.Utc).AddTicks(7402), null, "PatientFirstName5", null, "PatientLastName5", "d1e9df37-cdde-40af-be2f-aa63cba67abc" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "Name", "Number" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2469), 1, "Room 301", 301 },
                    { 2, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2471), 1, "Room 302", 302 },
                    { 3, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2476), 2, "Room 201", 201 },
                    { 4, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2478), 2, "Room 202", 202 },
                    { 5, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2479), 3, "Room 101", 101 },
                    { 6, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2480), 3, "Room 102", 102 },
                    { 7, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2481), 4, "Room 401", 401 },
                    { 8, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2482), 4, "Room 402", 402 },
                    { 9, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2483), 5, "Emergency Room 1", 1 },
                    { 10, new DateTime(2025, 2, 12, 15, 46, 30, 502, DateTimeKind.Utc).AddTicks(2485), 5, "Emergency Room 2", 2 }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDate", "DoctorId", "Duration", "Notes", "NurseId", "PatientId", "Reason", "RoomId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 7, 10, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 1, 0, null, 1 },
                    { 2, new DateTime(2025, 2, 9, 11, 0, 0, 0, DateTimeKind.Utc), 2, 60, null, null, 2, 1, null, 2 },
                    { 3, new DateTime(2025, 2, 10, 9, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 3, 1, null, 2 },
                    { 4, new DateTime(2025, 2, 11, 12, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 4, 1, null, 1 },
                    { 5, new DateTime(2025, 2, 12, 12, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 3, 2, null, 0 },
                    { 6, new DateTime(2025, 2, 12, 14, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 4, 3, null, 3 },
                    { 7, new DateTime(2025, 2, 12, 9, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 1, 3, null, 0 },
                    { 8, new DateTime(2025, 2, 12, 11, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 2, 3, null, 0 },
                    { 9, new DateTime(2025, 2, 12, 15, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 1, 3, null, 3 },
                    { 10, new DateTime(2025, 2, 13, 10, 0, 0, 0, DateTimeKind.Utc), 5, 60, null, null, 5, 4, null, 0 },
                    { 11, new DateTime(2025, 2, 14, 11, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 2, 0, null, 3 },
                    { 12, new DateTime(2025, 2, 15, 12, 0, 0, 0, DateTimeKind.Utc), 2, 60, null, null, 3, 1, null, 0 },
                    { 13, new DateTime(2025, 2, 22, 13, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 5, 3, null, 0 },
                    { 14, new DateTime(2025, 2, 27, 14, 0, 0, 0, DateTimeKind.Utc), 5, 60, null, null, 1, 4, null, 3 }
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
                name: "Privileges");

            migrationBuilder.DropTable(
                name: "Nurses");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Patients");

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
