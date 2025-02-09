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
                    { "04aac39b-e962-437f-944a-496b0172a908", 0, null, "4b8eb44e-54e5-4fd4-aa82-d6803eab1206", null, "doctor1@hospital.com", false, "DoctorFirstName1", null, "DoctorLastName1", false, null, "DOCTOR1@HOSPITAL.COM", "DOCTOR1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEB6lk1iDIZVpeoB7/NKRd+dowpBPqLFIMZgcy2NTUwdB214uGd2DKEfEsydPKD6t5A==", null, false, "b7310163-d40d-403e-a270-54f8a8298848", false, "doctor1@hospital.com" },
                    { "122baafd-bbdc-480c-94b2-65123904a3f7", 0, null, "87049a6a-b005-468f-864c-d9f1056fdf6b", null, "nurse1@hospital.com", false, "NurseFirstName1", null, "NurseLastName1", false, null, "NURSE1@HOSPITAL.COM", "NURSE1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFfUrA0oC5rejyVhDwSD0jdJ/eP1i1e4eoxjdyA/EwDqBznanwLiZH1WMQD1YvOkeQ==", null, false, "f5383b91-c7ae-45d6-a40a-f9e2c1f79031", false, "nurse1@hospital.com" },
                    { "18310304-cdbf-4b38-9519-fdd102f4d3ae", 0, null, "0591a4d9-1b8c-458e-96b2-77e890a7e6a0", null, "doctor2@hospital.com", false, "DoctorFirstName2", null, "DoctorLastName2", false, null, "DOCTOR2@HOSPITAL.COM", "DOCTOR2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEL+h8ekHxH0rQcNgSDpbs0nfk1yuCUphASutDH3NAnE1ARQ0ey133C02XJtoFjRQww==", null, false, "67542aef-4c73-4898-8304-8ecd245de7fe", false, "doctor2@hospital.com" },
                    { "1eb3c211-11d5-4279-910e-fd541705c607", 0, null, "59b91b82-bb6d-4e48-9e03-d4a9c06e9835", null, "patient4@hospital.com", false, "PatientFirstName4", null, "PatientLastName4", false, null, "PATIENT4@HOSPITAL.COM", "PATIENT4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEN+D+AOr5slYT6EkBIU38Ldv3isceK/TAWa7ziQNk2uWlRoBdc+jneiESHrS0RvOXw==", null, false, "36f6cd6b-466a-48b8-887e-61da8fc00feb", false, "patient4@hospital.com" },
                    { "344aaeb2-e68c-4b77-a085-10526e91cf97", 0, null, "6dbae35c-0900-453e-90ed-5c5c3be7688f", null, "patient3@hospital.com", false, "PatientFirstName3", null, "PatientLastName3", false, null, "PATIENT3@HOSPITAL.COM", "PATIENT3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEPaDiYWOE+DcHSS4KlboATN66EvhkV1Rauz2USzREK5r8YzY4WBycafMGrNxyRYnGw==", null, false, "d619f1ac-c6bf-47bd-876f-9679c5d2db0e", false, "patient3@hospital.com" },
                    { "534b8153-fb31-4d95-86b1-205a15380f70", 0, null, "147c7774-99df-4112-b350-8d5051a7cc9f", null, "nurse5@hospital.com", false, "NurseFirstName5", null, "NurseLastName5", false, null, "NURSE5@HOSPITAL.COM", "NURSE5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEDq2NvLBQv0QSMfmnH4w0WSBs5XMnfZtqxyHDl0EGWT7cUKixP9geHb8RzX+S+yTrQ==", null, false, "6374a8ce-4485-4fce-890b-f1ddeb80b816", false, "nurse5@hospital.com" },
                    { "7d8933c4-3277-45cc-9c89-130468e5d2bd", 0, null, "3f8c1407-ad79-449c-901b-b641fb5948a9", null, "nurse3@hospital.com", false, "NurseFirstName3", null, "NurseLastName3", false, null, "NURSE3@HOSPITAL.COM", "NURSE3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEC8M/X9cs0H7pI1kh9v2y/KCCaWw5Ih2oVVFO6OT3a7XBHD4zXTwzFZ665vEPok48g==", null, false, "0ec33550-34ff-4b6b-8e63-6848ccebe3f1", false, "nurse3@hospital.com" },
                    { "8192db18-055e-46da-817c-dab4f45fa5a1", 0, null, "e5d7454b-5109-47e9-87ab-b56f5e60f45b", null, "nurse4@hospital.com", false, "NurseFirstName4", null, "NurseLastName4", false, null, "NURSE4@HOSPITAL.COM", "NURSE4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEGjudCH8JX4Jz3IXKMe8xERgUk8QSuH+tVlHV/SFZgcRcsqMKyfFe9frHH3cI2CKJw==", null, false, "e5ecaf23-059b-4c0b-abfb-2cc0f4d3a233", false, "nurse4@hospital.com" },
                    { "a93d843c-bb70-4919-ae6d-712e3bc60c98", 0, null, "8b3e6253-8b96-4986-acdb-de8d8fa18373", null, "patient5@hospital.com", false, "PatientFirstName5", null, "PatientLastName5", false, null, "PATIENT5@HOSPITAL.COM", "PATIENT5@HOSPITAL.COM", "AQAAAAIAAYagAAAAECav2JvEKsjCvTSWy1ML0/ui9/2CLPmKSJdmQe0PbRgydkCrHCKwLOVl+iHbdkWF7w==", null, false, "c3a9b803-b427-4b75-b75b-5030ca9b4a96", false, "patient5@hospital.com" },
                    { "ad8c4968-852f-4cc6-bf4f-5fccead39335", 0, null, "a0a8e6ac-f299-4fd9-aa14-127a2609bb49", null, "doctor4@hospital.com", false, "DoctorFirstName4", null, "DoctorLastName4", false, null, "DOCTOR4@HOSPITAL.COM", "DOCTOR4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEEcCvMshVDXy9CNV0nXzeK1HgJGq3ELX6oBFrBIekCluwuteURW7vnzJCf9Ma1HPMA==", null, false, "6d85c987-8a3a-4c37-ae5c-6acc0431817e", false, "doctor4@hospital.com" },
                    { "admin", 0, null, "5c5ab505-a8b3-402f-9469-588d6ae763db", null, "admin@gmail.com", false, "Admin", null, "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEFZ3qpP3voUVs4nl38sxvLnLDzH3fhZcmFCSpN0Z1a1V4uL5BcPakBKG7R5PwSTu7g==", null, false, "94b87810-9ffe-42c1-b618-cd961fbc8eb0", false, "admin@gmail.com" },
                    { "b2980f2f-9fc9-4476-96e3-ded86037d63e", 0, null, "59dd5810-e6aa-4ec3-9220-2160b9d45af3", null, "doctor3@hospital.com", false, "DoctorFirstName3", null, "DoctorLastName3", false, null, "DOCTOR3@HOSPITAL.COM", "DOCTOR3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFaMRo1tpRbVFO1OedsDcmfrSTOYmT7rrrlQ0Vn1hHoQBQ6+xdPVUTA8xQhhkaELfQ==", null, false, "6f63a2e1-5a6e-4c57-b4fe-c020c2ebff7a", false, "doctor3@hospital.com" },
                    { "bef1fb4e-ca3d-48f1-8dd3-178558aff3e7", 0, null, "b915e4f7-3d61-40c1-a9fd-721b60a0f4b4", null, "patient2@hospital.com", false, "PatientFirstName2", null, "PatientLastName2", false, null, "PATIENT2@HOSPITAL.COM", "PATIENT2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEILiuJLrtbldXNsTe87o7OGu4TMtNBLyyllmgK7dCFy21f2p7VXwPWRydbSlU9TlRQ==", null, false, "44b8c997-a269-4b21-9db0-ad4a2e846c0c", false, "patient2@hospital.com" },
                    { "d329c623-b0c4-411e-8b7b-da6c6ddb01ef", 0, null, "483b1317-7254-4a2b-905f-237ad5668eb6", null, "doctor5@hospital.com", false, "DoctorFirstName5", null, "DoctorLastName5", false, null, "DOCTOR5@HOSPITAL.COM", "DOCTOR5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAT0YAcR7GWzdWjZ6PhCXYk+eAnrbb4xb1h/OQCvj2AFFZMMvKu8okpFVkKEsiCJQQ==", null, false, "6c36aba5-aad4-477f-a1d1-f8eb5a6386c3", false, "doctor5@hospital.com" },
                    { "dd971efe-f077-43be-a667-b243966d74d2", 0, null, "a6b08ed0-082a-466e-a7c1-edb540abd4c9", null, "patient1@hospital.com", false, "PatientFirstName1", null, "PatientLastName1", false, null, "PATIENT1@HOSPITAL.COM", "PATIENT1@HOSPITAL.COM", "AQAAAAIAAYagAAAAECK6jKi4HzTSXF42gYkCbUS+Cnm6wfYHBF3XCY8Jx3i3OIUjKuRjkcWMJKAtqgdniA==", null, false, "88ffa93d-ce40-42f3-99d0-fb5ae1e2b342", false, "patient1@hospital.com" },
                    { "e2f6cb31-7069-4f54-9ed7-9a0b9b822d65", 0, null, "ae2332e2-680a-4d67-bc8c-8832b054920c", null, "nurse2@hospital.com", false, "NurseFirstName2", null, "NurseLastName2", false, null, "NURSE2@HOSPITAL.COM", "NURSE2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJYEHjNtQDIewbNlY6lJE66qKr2NyWopvWI+F1xE/BwU4Yn6EFuLe8Fd7neLMGZqHg==", null, false, "88ea5c69-5f7f-48af-9582-8b325bdfb557", false, "nurse2@hospital.com" }
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
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2", "04aac39b-e962-437f-944a-496b0172a908" },
                    { "3", "122baafd-bbdc-480c-94b2-65123904a3f7" },
                    { "2", "18310304-cdbf-4b38-9519-fdd102f4d3ae" },
                    { "3", "1eb3c211-11d5-4279-910e-fd541705c607" },
                    { "3", "344aaeb2-e68c-4b77-a085-10526e91cf97" },
                    { "3", "534b8153-fb31-4d95-86b1-205a15380f70" },
                    { "3", "7d8933c4-3277-45cc-9c89-130468e5d2bd" },
                    { "3", "8192db18-055e-46da-817c-dab4f45fa5a1" },
                    { "3", "a93d843c-bb70-4919-ae6d-712e3bc60c98" },
                    { "2", "ad8c4968-852f-4cc6-bf4f-5fccead39335" },
                    { "0", "admin" },
                    { "2", "b2980f2f-9fc9-4476-96e3-ded86037d63e" },
                    { "3", "bef1fb4e-ca3d-48f1-8dd3-178558aff3e7" },
                    { "2", "d329c623-b0c4-411e-8b7b-da6c6ddb01ef" },
                    { "3", "dd971efe-f077-43be-a667-b243966d74d2" },
                    { "3", "e2f6cb31-7069-4f54-9ed7-9a0b9b822d65" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "SpecialtyId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 9, 12, 7, 40, 769, DateTimeKind.Utc).AddTicks(1976), null, "DoctorFirstName1", null, "DoctorLastName1", null, "04aac39b-e962-437f-944a-496b0172a908" },
                    { 2, new DateTime(2025, 2, 9, 12, 7, 40, 989, DateTimeKind.Utc).AddTicks(3220), null, "DoctorFirstName2", null, "DoctorLastName2", null, "18310304-cdbf-4b38-9519-fdd102f4d3ae" },
                    { 3, new DateTime(2025, 2, 9, 12, 7, 41, 182, DateTimeKind.Utc).AddTicks(6245), null, "DoctorFirstName3", null, "DoctorLastName3", null, "b2980f2f-9fc9-4476-96e3-ded86037d63e" },
                    { 4, new DateTime(2025, 2, 9, 12, 7, 41, 371, DateTimeKind.Utc).AddTicks(9953), null, "DoctorFirstName4", null, "DoctorLastName4", null, "ad8c4968-852f-4cc6-bf4f-5fccead39335" },
                    { 5, new DateTime(2025, 2, 9, 12, 7, 41, 571, DateTimeKind.Utc).AddTicks(6600), null, "DoctorFirstName5", null, "DoctorLastName5", null, "d329c623-b0c4-411e-8b7b-da6c6ddb01ef" }
                });

            migrationBuilder.InsertData(
                table: "Nurses",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 9, 12, 7, 40, 841, DateTimeKind.Utc).AddTicks(6680), null, "NurseFirstName1", null, "NurseLastName1", "122baafd-bbdc-480c-94b2-65123904a3f7" },
                    { 2, new DateTime(2025, 2, 9, 12, 7, 41, 54, DateTimeKind.Utc).AddTicks(7382), null, "NurseFirstName2", null, "NurseLastName2", "e2f6cb31-7069-4f54-9ed7-9a0b9b822d65" },
                    { 3, new DateTime(2025, 2, 9, 12, 7, 41, 246, DateTimeKind.Utc).AddTicks(2817), null, "NurseFirstName3", null, "NurseLastName3", "7d8933c4-3277-45cc-9c89-130468e5d2bd" },
                    { 4, new DateTime(2025, 2, 9, 12, 7, 41, 434, DateTimeKind.Utc).AddTicks(3223), null, "NurseFirstName4", null, "NurseLastName4", "8192db18-055e-46da-817c-dab4f45fa5a1" },
                    { 5, new DateTime(2025, 2, 9, 12, 7, 41, 633, DateTimeKind.Utc).AddTicks(9385), null, "NurseFirstName5", null, "NurseLastName5", "534b8153-fb31-4d95-86b1-205a15380f70" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 9, 12, 7, 40, 921, DateTimeKind.Utc).AddTicks(867), null, "PatientFirstName1", null, "PatientLastName1", "dd971efe-f077-43be-a667-b243966d74d2" },
                    { 2, new DateTime(2025, 2, 9, 12, 7, 41, 119, DateTimeKind.Utc).AddTicks(4118), null, "PatientFirstName2", null, "PatientLastName2", "bef1fb4e-ca3d-48f1-8dd3-178558aff3e7" },
                    { 3, new DateTime(2025, 2, 9, 12, 7, 41, 309, DateTimeKind.Utc).AddTicks(1179), null, "PatientFirstName3", null, "PatientLastName3", "344aaeb2-e68c-4b77-a085-10526e91cf97" },
                    { 4, new DateTime(2025, 2, 9, 12, 7, 41, 509, DateTimeKind.Utc).AddTicks(7627), null, "PatientFirstName4", null, "PatientLastName4", "1eb3c211-11d5-4279-910e-fd541705c607" },
                    { 5, new DateTime(2025, 2, 9, 12, 7, 41, 696, DateTimeKind.Utc).AddTicks(2566), null, "PatientFirstName5", null, "PatientLastName5", "a93d843c-bb70-4919-ae6d-712e3bc60c98" }
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
                name: "Specialities");
        }
    }
}
