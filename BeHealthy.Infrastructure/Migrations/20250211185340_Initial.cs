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
                    { "0021c43b-b158-4070-863c-c59c2d735a54", 0, null, "cb801718-8b8c-4c7e-b0ad-41f4776c3492", null, "patient2@hospital.com", false, "PatientFirstName2", null, "PatientLastName2", false, null, "PATIENT2@HOSPITAL.COM", "PATIENT2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEBDn9wKd+vt8OT4cv/tu6A8NBYkLV2lSGn4N8l0zi40tHHd9M3kQmcPpwJ12Hkn7iQ==", null, false, "45853418-03f3-47de-a663-254e15c6e9db", false, "patient2@hospital.com" },
                    { "290a8252-f74f-4432-9b44-a10c9bcdccbd", 0, null, "2bb32c8e-0077-4970-9ced-1ea95293faf1", null, "doctor4@hospital.com", false, "DoctorFirstName4", null, "DoctorLastName4", false, null, "DOCTOR4@HOSPITAL.COM", "DOCTOR4@HOSPITAL.COM", "AQAAAAIAAYagAAAAECMEHapwSd+qvUUyeO0zPrgMZC227DIq+H+Uh7457qCS0Q1/sLjFml4OOco1IBTwsQ==", null, false, "74bf01c1-a6e3-4b4e-b94c-75bcb1cacce3", false, "doctor4@hospital.com" },
                    { "2c928c85-219d-4b81-8d83-0114828491d5", 0, null, "13e402ee-ff9c-4435-8823-c8bd7bbb25c9", null, "nurse5@hospital.com", false, "NurseFirstName5", null, "NurseLastName5", false, null, "NURSE5@HOSPITAL.COM", "NURSE5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEBGJBKW3PBh9T4grhY4KxSEIa7aDVZ/qYdkcRVstyFMBdnKiF3GzrR85glebfi+yvA==", null, false, "2d29662b-25e0-43f7-8705-658989cf8a81", false, "nurse5@hospital.com" },
                    { "44d09ecb-330a-47a4-91f4-4e6d81290572", 0, null, "0aabde88-e2b9-48cb-9f05-fedf4f83cd5c", null, "nurse2@hospital.com", false, "NurseFirstName2", null, "NurseLastName2", false, null, "NURSE2@HOSPITAL.COM", "NURSE2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEOf7QTfXPAfqpxGq0ZM5Vuk5LjsKhWxfZrWKJ/xBAV7L9TyKTj9k+aMEgOZPRUsz1Q==", null, false, "b9fa698b-963a-43fb-9a60-3d5f1834fd93", false, "nurse2@hospital.com" },
                    { "46c27294-8635-46e6-8c66-81a569d18dca", 0, null, "a7091eda-7d8e-45e0-a329-c164b116cb90", null, "patient4@hospital.com", false, "PatientFirstName4", null, "PatientLastName4", false, null, "PATIENT4@HOSPITAL.COM", "PATIENT4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEDnV45IrDS3+AOcIEJDcBlzDGZyU83fOzhH05K58OqVJQf9RZSR6H9maoY9spNnSzA==", null, false, "e9eb44b5-e8e4-4872-ac8c-139bcea0bbe1", false, "patient4@hospital.com" },
                    { "4cbd075f-5b48-48eb-8d18-3af50b6c27a3", 0, null, "df581899-b3ae-4df5-9620-ade6021e9a33", null, "doctor2@hospital.com", false, "DoctorFirstName2", null, "DoctorLastName2", false, null, "DOCTOR2@HOSPITAL.COM", "DOCTOR2@HOSPITAL.COM", "AQAAAAIAAYagAAAAELBnml5Kxo9E2AahIhDgIri24i9uUB3O8t2Xoj37mGfwK6xMq+k/6XnkHdFVRqKQiA==", null, false, "056a719f-e2cc-46e7-965f-018e8045ff4b", false, "doctor2@hospital.com" },
                    { "56d017a4-a4ad-466f-bbec-05f52fd10437", 0, null, "0dfa5155-1f36-430b-981b-efb53a1a0fd7", null, "patient3@hospital.com", false, "PatientFirstName3", null, "PatientLastName3", false, null, "PATIENT3@HOSPITAL.COM", "PATIENT3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEINsQW7vuOZn//6OHVwI4qeBjH52gYT5Wc7/7jqcytOwYRI4IWn7uVKdvFeB9nb3+Q==", null, false, "aef90371-a798-48cb-a7a3-ac160dd93edf", false, "patient3@hospital.com" },
                    { "62330df5-5a81-4384-822d-18277113df6e", 0, null, "913e2480-2703-420f-b301-ccff9f928e77", null, "patient1@hospital.com", false, "PatientFirstName1", null, "PatientLastName1", false, null, "PATIENT1@HOSPITAL.COM", "PATIENT1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFUz9qfcSXRUbpDhF/UKob+DB1L4wymdCivZePAh72nAwCvYnr0J64jr9e0jw/vjIw==", null, false, "5e6efb6a-7ce8-41b1-8bed-e94b04362dd5", false, "patient1@hospital.com" },
                    { "7bcd232d-4185-4dc6-a057-cae3ee0c6658", 0, null, "2b11e8b0-5381-42b9-8092-e4a667035672", null, "nurse4@hospital.com", false, "NurseFirstName4", null, "NurseLastName4", false, null, "NURSE4@HOSPITAL.COM", "NURSE4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEA1NWfMv1KvheaOhXRpHjRLEdmjoCFCY9BlceFGfHG5VGorwCE+rbDAM+X92niMnjA==", null, false, "656f843b-457e-4c78-86cf-4b4c2384ee22", false, "nurse4@hospital.com" },
                    { "8667a65a-b2f0-442f-8e3c-0c3b6230ae7a", 0, null, "0c6dc6c5-bc65-423f-afc9-29a57e215ecf", null, "nurse3@hospital.com", false, "NurseFirstName3", null, "NurseLastName3", false, null, "NURSE3@HOSPITAL.COM", "NURSE3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEKiOA47vlf9MKraNy+AjryVOoex9/JMVe693nNCi58Bga6ssXRXVRdbvQhWDtcN5sw==", null, false, "2dc46482-a011-4691-8199-971843af3e69", false, "nurse3@hospital.com" },
                    { "admin", 0, null, "f987241d-ff6d-4122-9763-4bfd8da0369b", null, "admin@gmail.com", false, "Admin", null, "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEKka1TKn/MvonpDnA9MLvZ5bDVp2Rxqg6dtkgUaS6fC4UZNKjhsd84G3LjDKlBHitQ==", null, false, "f5a118cf-2c54-414e-b590-81047d8f9924", false, "admin@gmail.com" },
                    { "af846fa7-cbe0-4a92-9198-b9ab763b1ad6", 0, null, "ca08072a-c15d-44d6-b2e6-d03d535c9252", null, "doctor3@hospital.com", false, "DoctorFirstName3", null, "DoctorLastName3", false, null, "DOCTOR3@HOSPITAL.COM", "DOCTOR3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEEXR9ACndZN30nARmS0uE1twu7dLGfXysm7/scLvX6rNdlq5wqWjOn6eFC/jKdd5uw==", null, false, "264c47d7-00f7-4c20-835f-5e62703bdea6", false, "doctor3@hospital.com" },
                    { "b6ace12e-e615-4c01-8bf6-8a653c1c275f", 0, null, "234bed90-4bfa-468e-943b-9ed8d27cba59", null, "nurse1@hospital.com", false, "NurseFirstName1", null, "NurseLastName1", false, null, "NURSE1@HOSPITAL.COM", "NURSE1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEOlPuzQXzECSCKsZDoZYHdoGkcvfPD8iyQR9qhGt9hEkVnaXRAJbihbU8hN/YDHrAg==", null, false, "575323df-7ab9-4070-9ffd-979c268adb03", false, "nurse1@hospital.com" },
                    { "d2ae1474-9b71-457e-af0c-897b11ec41fe", 0, null, "7efef068-d26c-4c9b-b601-00518e2d696e", null, "doctor5@hospital.com", false, "DoctorFirstName5", null, "DoctorLastName5", false, null, "DOCTOR5@HOSPITAL.COM", "DOCTOR5@HOSPITAL.COM", "AQAAAAIAAYagAAAAECHdzD5rbkChZCo0JF22W81loFBQj1z1u75oT44yJSYoo99w6tkdb4bK0uBtJq1pkw==", null, false, "aab3c503-b3da-46f4-80d2-d98e157bfb71", false, "doctor5@hospital.com" },
                    { "dc440c59-36be-425d-b310-07ccc49bac92", 0, null, "f65420d4-75fc-4ae5-8470-4b240dea5729", null, "doctor1@hospital.com", false, "DoctorFirstName1", null, "DoctorLastName1", false, null, "DOCTOR1@HOSPITAL.COM", "DOCTOR1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEDXYbl3U6II7nTvY+zDKEFV41I+69SYGmGOMFxm4Nslpfic833w23vxMwlYuQgqbTQ==", null, false, "78614305-0c5d-48b3-97be-e7025cae86fe", false, "doctor1@hospital.com" },
                    { "dca4b91e-27f6-41da-b51b-1e4197664faa", 0, null, "6e3e981d-afcb-4749-804b-de4d15fdff49", null, "patient5@hospital.com", false, "PatientFirstName5", null, "PatientLastName5", false, null, "PATIENT5@HOSPITAL.COM", "PATIENT5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEHVdptd34GtmLQzKVNgomNzZIsWXKavzZmiBf3+0wwdyDGWIk1U9NPzYIgKaP+l7wQ==", null, false, "43f2e9d3-2cb7-4316-9d5c-232539fb2e17", false, "patient5@hospital.com" }
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
                    { "4", "0021c43b-b158-4070-863c-c59c2d735a54" },
                    { "2", "290a8252-f74f-4432-9b44-a10c9bcdccbd" },
                    { "3", "2c928c85-219d-4b81-8d83-0114828491d5" },
                    { "3", "44d09ecb-330a-47a4-91f4-4e6d81290572" },
                    { "4", "46c27294-8635-46e6-8c66-81a569d18dca" },
                    { "2", "4cbd075f-5b48-48eb-8d18-3af50b6c27a3" },
                    { "4", "56d017a4-a4ad-466f-bbec-05f52fd10437" },
                    { "4", "62330df5-5a81-4384-822d-18277113df6e" },
                    { "3", "7bcd232d-4185-4dc6-a057-cae3ee0c6658" },
                    { "3", "8667a65a-b2f0-442f-8e3c-0c3b6230ae7a" },
                    { "0", "admin" },
                    { "2", "af846fa7-cbe0-4a92-9198-b9ab763b1ad6" },
                    { "3", "b6ace12e-e615-4c01-8bf6-8a653c1c275f" },
                    { "2", "d2ae1474-9b71-457e-af0c-897b11ec41fe" },
                    { "2", "dc440c59-36be-425d-b310-07ccc49bac92" },
                    { "4", "dca4b91e-27f6-41da-b51b-1e4197664faa" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "SpecialtyId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 11, 18, 53, 39, 332, DateTimeKind.Utc).AddTicks(9653), null, "DoctorFirstName1", null, "DoctorLastName1", null, "dc440c59-36be-425d-b310-07ccc49bac92" },
                    { 2, new DateTime(2025, 2, 11, 18, 53, 39, 524, DateTimeKind.Utc).AddTicks(974), null, "DoctorFirstName2", null, "DoctorLastName2", null, "4cbd075f-5b48-48eb-8d18-3af50b6c27a3" },
                    { 3, new DateTime(2025, 2, 11, 18, 53, 39, 727, DateTimeKind.Utc).AddTicks(6107), null, "DoctorFirstName3", null, "DoctorLastName3", null, "af846fa7-cbe0-4a92-9198-b9ab763b1ad6" },
                    { 4, new DateTime(2025, 2, 11, 18, 53, 39, 918, DateTimeKind.Utc).AddTicks(5228), null, "DoctorFirstName4", null, "DoctorLastName4", null, "290a8252-f74f-4432-9b44-a10c9bcdccbd" },
                    { 5, new DateTime(2025, 2, 11, 18, 53, 40, 108, DateTimeKind.Utc).AddTicks(6450), null, "DoctorFirstName5", null, "DoctorLastName5", null, "d2ae1474-9b71-457e-af0c-897b11ec41fe" }
                });

            migrationBuilder.InsertData(
                table: "Nurses",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 11, 18, 53, 39, 396, DateTimeKind.Utc).AddTicks(5867), null, "NurseFirstName1", null, "NurseLastName1", "b6ace12e-e615-4c01-8bf6-8a653c1c275f" },
                    { 2, new DateTime(2025, 2, 11, 18, 53, 39, 597, DateTimeKind.Utc).AddTicks(774), null, "NurseFirstName2", null, "NurseLastName2", "44d09ecb-330a-47a4-91f4-4e6d81290572" },
                    { 3, new DateTime(2025, 2, 11, 18, 53, 39, 791, DateTimeKind.Utc).AddTicks(4857), null, "NurseFirstName3", null, "NurseLastName3", "8667a65a-b2f0-442f-8e3c-0c3b6230ae7a" },
                    { 4, new DateTime(2025, 2, 11, 18, 53, 39, 981, DateTimeKind.Utc).AddTicks(9176), null, "NurseFirstName4", null, "NurseLastName4", "7bcd232d-4185-4dc6-a057-cae3ee0c6658" },
                    { 5, new DateTime(2025, 2, 11, 18, 53, 40, 172, DateTimeKind.Utc).AddTicks(507), null, "NurseFirstName5", null, "NurseLastName5", "2c928c85-219d-4b81-8d83-0114828491d5" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 11, 18, 53, 39, 460, DateTimeKind.Utc).AddTicks(4192), null, "PatientFirstName1", null, "PatientLastName1", "62330df5-5a81-4384-822d-18277113df6e" },
                    { 2, new DateTime(2025, 2, 11, 18, 53, 39, 663, DateTimeKind.Utc).AddTicks(5447), null, "PatientFirstName2", null, "PatientLastName2", "0021c43b-b158-4070-863c-c59c2d735a54" },
                    { 3, new DateTime(2025, 2, 11, 18, 53, 39, 855, DateTimeKind.Utc).AddTicks(559), null, "PatientFirstName3", null, "PatientLastName3", "56d017a4-a4ad-466f-bbec-05f52fd10437" },
                    { 4, new DateTime(2025, 2, 11, 18, 53, 40, 45, DateTimeKind.Utc).AddTicks(5678), null, "PatientFirstName4", null, "PatientLastName4", "46c27294-8635-46e6-8c66-81a569d18dca" },
                    { 5, new DateTime(2025, 2, 11, 18, 53, 40, 235, DateTimeKind.Utc).AddTicks(451), null, "PatientFirstName5", null, "PatientLastName5", "dca4b91e-27f6-41da-b51b-1e4197664faa" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDate", "DoctorId", "Duration", "Notes", "NurseId", "PatientId", "Reason", "RoomId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 6, 10, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 1, 0, null, 1 },
                    { 2, new DateTime(2025, 2, 8, 11, 0, 0, 0, DateTimeKind.Utc), 2, 60, null, null, 2, 1, null, 2 },
                    { 3, new DateTime(2025, 2, 9, 9, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 3, 1, null, 2 },
                    { 4, new DateTime(2025, 2, 10, 12, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 4, 1, null, 1 },
                    { 5, new DateTime(2025, 2, 11, 12, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 3, 2, null, 0 },
                    { 6, new DateTime(2025, 2, 11, 14, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 4, 3, null, 3 },
                    { 7, new DateTime(2025, 2, 11, 9, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 1, 3, null, 0 },
                    { 8, new DateTime(2025, 2, 11, 11, 0, 0, 0, DateTimeKind.Utc), 3, 60, null, null, 2, 3, null, 0 },
                    { 9, new DateTime(2025, 2, 11, 15, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 1, 3, null, 3 },
                    { 10, new DateTime(2025, 2, 12, 10, 0, 0, 0, DateTimeKind.Utc), 5, 60, null, null, 5, 4, null, 0 },
                    { 11, new DateTime(2025, 2, 13, 11, 0, 0, 0, DateTimeKind.Utc), 1, 60, null, null, 2, 0, null, 3 },
                    { 12, new DateTime(2025, 2, 14, 12, 0, 0, 0, DateTimeKind.Utc), 2, 60, null, null, 3, 1, null, 0 },
                    { 13, new DateTime(2025, 2, 21, 13, 0, 0, 0, DateTimeKind.Utc), 4, 60, null, null, 5, 3, null, 0 },
                    { 14, new DateTime(2025, 2, 26, 14, 0, 0, 0, DateTimeKind.Utc), 5, 60, null, null, 1, 4, null, 3 }
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
