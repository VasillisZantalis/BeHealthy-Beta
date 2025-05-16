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
                    AppointmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AppointmentStartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    AppointmentEndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    { "03f3e3bf-6e6d-4114-89e5-ce6a9960f312", 0, null, "9acac093-05d8-41a3-bb72-ca68793f3bb0", null, "nurse2@hospital.com", false, "NurseFirstName2", null, "NurseLastName2", false, null, "NURSE2@HOSPITAL.COM", "NURSE2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEImciYujsrLA159n9c2uISKx/XS4X1Jejv73dV7+9Cmws8oXshXkMkYZCDnYMdPogA==", null, false, "aaf6811b-4c4d-4c2d-a7ae-17ffb3b24e25", false, "nurse2@hospital.com" },
                    { "1386105f-78ab-4559-8ab9-b07a35427ef1", 0, null, "16fb8374-2889-4a0a-997b-98922a40fdc1", null, "doctor2@hospital.com", false, "DoctorFirstName2", null, "DoctorLastName2", false, null, "DOCTOR2@HOSPITAL.COM", "DOCTOR2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEC6xl9rxOEGdOfkUkvWE/YOXqPjND+KshFAzJ3chzbKlAvlXRWbPvB3T7QvNLrXh1w==", null, false, "c4f01b8c-e65c-4923-83bc-e0ee5f9b4260", false, "doctor2@hospital.com" },
                    { "13a2be0f-e28b-43ce-b231-8039b2913863", 0, null, "1b5c6467-1711-45ee-8399-5ff1cb663e33", null, "nurse10@hospital.com", false, "NurseFirstName10", null, "NurseLastName10", false, null, "NURSE10@HOSPITAL.COM", "NURSE10@HOSPITAL.COM", "AQAAAAIAAYagAAAAEI46L0bcmDVIjoN1cuRwUItyhousUMwAFIX0/szSI7B+Sc1Zs+FoJZnwqVCB1Cfpog==", null, false, "edb46878-0142-417e-b3cd-3dc39e9bc302", false, "nurse10@hospital.com" },
                    { "17f1003f-8675-4cde-8e51-a831eeda64b1", 0, null, "8b897155-1f81-494f-afd5-8a437d769018", null, "nurse9@hospital.com", false, "NurseFirstName9", null, "NurseLastName9", false, null, "NURSE9@HOSPITAL.COM", "NURSE9@HOSPITAL.COM", "AQAAAAIAAYagAAAAEODXsdvKKhUOL2tuLOnIIFfld/9DAxnnFxoSsG2uzga9Ct6WY2+jgNHYZ5NHg8gY6w==", null, false, "31e71b5d-e6d0-4452-96b2-7476fffa2041", false, "nurse9@hospital.com" },
                    { "26d02839-dd58-4031-a663-1b7b03b700cc", 0, null, "dd758f37-09c0-45d9-af66-142d30f1a072", null, "nurse5@hospital.com", false, "NurseFirstName5", null, "NurseLastName5", false, null, "NURSE5@HOSPITAL.COM", "NURSE5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEGRNrpBx77f0QNacRlH6Gfdjsesbgt9tWtk+OnbZFjcM14OB0XsLlwX2WLJsIn98pA==", null, false, "9c58b0ee-4f97-4843-a30a-657f0b5427e5", false, "nurse5@hospital.com" },
                    { "294f8dd6-2218-4a3c-bbf0-063450014f97", 0, null, "cb962b45-9f7a-44d5-b08a-5c05fce8580a", null, "doctor15@hospital.com", false, "DoctorFirstName15", null, "DoctorLastName15", false, null, "DOCTOR15@HOSPITAL.COM", "DOCTOR15@HOSPITAL.COM", "AQAAAAIAAYagAAAAEHsPjffJLv/gSU7tgxmT82e2I7DPKRTKsygSEk7h3B91ygLxufSu01airiFR/x+Eiw==", null, false, "71aa039a-699e-4e3f-bd4c-b6d56d0acbe4", false, "doctor15@hospital.com" },
                    { "2cda7d29-5f10-485f-b0fe-2c39ce5acf46", 0, null, "7bfbf796-a72b-4c99-a446-6b4822b8c523", null, "patient6@hospital.com", false, "PatientFirstName6", null, "PatientLastName6", false, null, "PATIENT6@HOSPITAL.COM", "PATIENT6@HOSPITAL.COM", "AQAAAAIAAYagAAAAECH/oz+9rYYZBtz3aZweaUcavJGCqeDdLx2aNHOtRB+U+BCBYawaHANLiH0SFhvWgA==", null, false, "894eceac-da1b-4087-bb30-175746761b8d", false, "patient6@hospital.com" },
                    { "3ad709eb-1362-4706-9cc8-a47f1dbf7c5a", 0, null, "37437035-bb7e-4e16-ac18-379d3f2a0656", null, "doctor8@hospital.com", false, "DoctorFirstName8", null, "DoctorLastName8", false, null, "DOCTOR8@HOSPITAL.COM", "DOCTOR8@HOSPITAL.COM", "AQAAAAIAAYagAAAAEPRER/UssfmTQNzdvhOkqtpUky3ZYSGNT94v0VGNn3cB4ekoS5h+PIh+kEQkF1wFIg==", null, false, "a04b9499-6a6c-46b6-8813-a248ff744468", false, "doctor8@hospital.com" },
                    { "3fe443f7-64d3-4e8f-b9a9-a43ea6cb5b85", 0, null, "f8d23d53-c64b-429b-a272-e21ae2a0a0de", null, "nurse14@hospital.com", false, "NurseFirstName14", null, "NurseLastName14", false, null, "NURSE14@HOSPITAL.COM", "NURSE14@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFBRYpsQBDG2cZV3kCH8/jxGdjYCFgOx+fzSTQQ5q4+x/4JUQshaAxu6M2cvgneavw==", null, false, "f114e60a-4f0f-470e-bdad-b59376fa4f43", false, "nurse14@hospital.com" },
                    { "42904cc5-817c-4807-bfa9-b41e26d72291", 0, null, "b584f306-8f56-444f-82fa-12672ad02ca4", null, "patient15@hospital.com", false, "PatientFirstName15", null, "PatientLastName15", false, null, "PATIENT15@HOSPITAL.COM", "PATIENT15@HOSPITAL.COM", "AQAAAAIAAYagAAAAEK2NVRznsmg3dwxQR023B8/gKCoorWPrC2rZqioWpbI6RMQr1HaQgIIyuWpdOIYY/w==", null, false, "a4bb1da3-d0cc-462c-8555-db87c3939eee", false, "patient15@hospital.com" },
                    { "4439e38b-2d1d-43b2-a053-5d001363b5f6", 0, null, "9cd3f114-b086-4150-888d-d593ea161674", null, "doctor6@hospital.com", false, "DoctorFirstName6", null, "DoctorLastName6", false, null, "DOCTOR6@HOSPITAL.COM", "DOCTOR6@HOSPITAL.COM", "AQAAAAIAAYagAAAAEO8gmGQNRTgjlB7ssbBILrhRxeKucXH7qqdd2WMLHA0IIpSfu7rHoWZXzXxuHnP5Jw==", null, false, "70545173-cee1-4fd3-82e5-d0e2746d733d", false, "doctor6@hospital.com" },
                    { "4c10b1ca-d2ae-4602-ad86-5cb0d860bcdf", 0, null, "35941f5a-7a31-4e0a-bd2c-1d87c657af56", null, "nurse1@hospital.com", false, "NurseFirstName1", null, "NurseLastName1", false, null, "NURSE1@HOSPITAL.COM", "NURSE1@HOSPITAL.COM", "AQAAAAIAAYagAAAAECDQO9URrwNZwtIC7CjW5JGbgD1eAoqGYF2pvKavs//7YZdnLtq60Xmx0J0mkyPOxA==", null, false, "8056eb25-82a0-4b44-8ff2-91fa33eb9946", false, "nurse1@hospital.com" },
                    { "4c948d64-2155-4069-b5fd-38c64f5ebd06", 0, null, "709fb64a-b743-4cb4-8e71-7d8c945f2242", null, "nurse7@hospital.com", false, "NurseFirstName7", null, "NurseLastName7", false, null, "NURSE7@HOSPITAL.COM", "NURSE7@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMiSsaG937+IMx7K72DrHQrvsfv7lgyJkcKdD933ZdhBkOTUa2RNhruJGZthd4dTxw==", null, false, "6eda371b-7753-4d6d-bbc5-9d9f459e99ac", false, "nurse7@hospital.com" },
                    { "53b34dd6-9426-477a-b6d4-d0e365f084f1", 0, null, "e220c917-5ac1-4aa8-ac0d-8a314c9b3b12", null, "nurse3@hospital.com", false, "NurseFirstName3", null, "NurseLastName3", false, null, "NURSE3@HOSPITAL.COM", "NURSE3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEBBZt1F4IiJC+0Y31TW2g2vNRHW7Chqcjb3KRcr7eUQniwjLAGURSINm1AOQEe5jNg==", null, false, "eb563d55-4e7a-49e7-a445-ec2fc65853b5", false, "nurse3@hospital.com" },
                    { "56392405-5b4d-4c37-9dba-fa00903ebb44", 0, null, "17a10f72-03ca-44c7-af1f-17abfb010af4", null, "doctor4@hospital.com", false, "DoctorFirstName4", null, "DoctorLastName4", false, null, "DOCTOR4@HOSPITAL.COM", "DOCTOR4@HOSPITAL.COM", "AQAAAAIAAYagAAAAELOXYi2rnCM5I6BVwWC6jK46G7mpIA87ApGGQMNfinn+T9WzflSyDfDmA6t7fu9YYQ==", null, false, "ecbda53c-8831-4486-9f6d-507e8d3cd15f", false, "doctor4@hospital.com" },
                    { "583f98ee-69c2-4dbc-9397-f4044acf82e4", 0, null, "58f648b7-ade9-4ae5-ab52-cc98f4dc373e", null, "doctor11@hospital.com", false, "DoctorFirstName11", null, "DoctorLastName11", false, null, "DOCTOR11@HOSPITAL.COM", "DOCTOR11@HOSPITAL.COM", "AQAAAAIAAYagAAAAEBcvZcqWZ+qHMy/6/YDr3/5mJxx8K3mclDS83ZcbC5ZxYffgU4MqDRG5TsCQwqO3hQ==", null, false, "f4de4b3f-acc7-4cfb-9060-7dc50c1b47c2", false, "doctor11@hospital.com" },
                    { "5d10b313-3ba5-4772-bdfd-3cc4c10153c1", 0, null, "12821a1a-3bc6-4933-99f0-b3932519d860", null, "patient1@hospital.com", false, "PatientFirstName1", null, "PatientLastName1", false, null, "PATIENT1@HOSPITAL.COM", "PATIENT1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFKuEnBvyb1/4k7pmpXhEOrrjEjGjAX2Cwp70ksSlIbFtDxWV0RjJiHJemD30joZdA==", null, false, "463bb6e3-f0e9-4231-b4ae-c6aa87af24b3", false, "patient1@hospital.com" },
                    { "6330f491-dbbe-49fc-bdd2-02230c96d374", 0, null, "d304affc-dadb-48fe-a11e-55a4f51a85df", null, "doctor10@hospital.com", false, "DoctorFirstName10", null, "DoctorLastName10", false, null, "DOCTOR10@HOSPITAL.COM", "DOCTOR10@HOSPITAL.COM", "AQAAAAIAAYagAAAAELUiS9oGLtvR9pqTgCeTr5HdssR/Idm07U7HrjwzbkvQrc90WdPgzaTBXohgc7Fvxg==", null, false, "258b3e8e-3723-4fbf-b512-4e628c107c21", false, "doctor10@hospital.com" },
                    { "7c21534f-1bc2-4efe-844b-1fcf5d86f59f", 0, null, "1d00823c-250a-4dce-be39-4daa211bf783", null, "doctor7@hospital.com", false, "DoctorFirstName7", null, "DoctorLastName7", false, null, "DOCTOR7@HOSPITAL.COM", "DOCTOR7@HOSPITAL.COM", "AQAAAAIAAYagAAAAEK84cbwNIBmHL919QNZ8WId5iSmTIdN/HTYHto8kluPgVyEwQwfGQFbkcmUYThC0/Q==", null, false, "fe8e46a4-5c1d-441e-855d-b83ed9d6eebd", false, "doctor7@hospital.com" },
                    { "7cf5a270-6f65-4879-a4f8-1401b7400553", 0, null, "640ad42a-8464-4405-ba1f-d75a0fbf7a35", null, "patient10@hospital.com", false, "PatientFirstName10", null, "PatientLastName10", false, null, "PATIENT10@HOSPITAL.COM", "PATIENT10@HOSPITAL.COM", "AQAAAAIAAYagAAAAEM/SefTXAExEnfkCGLuHTqnCz/AkxvbRpLvFWloE2OJfnQ7i2hGoGx+WsBFrXeBnMA==", null, false, "6443d3c5-776f-438a-9013-dd5f9ad29d9f", false, "patient10@hospital.com" },
                    { "7f0c8d5b-ffec-4d67-8217-aa6e4b5b29b7", 0, null, "8d4ca965-b336-4ca6-a6a9-99b57904d871", null, "doctor1@hospital.com", false, "DoctorFirstName1", null, "DoctorLastName1", false, null, "DOCTOR1@HOSPITAL.COM", "DOCTOR1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEO3PTjJx++GIXmoqDhk0zk0q8jvBSYl0V/fctcdnuxu2iYCNxFGjoHOddk/HTMc6Sw==", null, false, "31557f27-da0e-4e01-aa6f-8d16ca1154f9", false, "doctor1@hospital.com" },
                    { "80405353-16a0-4332-8f56-8cfd94924595", 0, null, "ae785056-ff2d-47ea-aeec-539861ce7988", null, "patient11@hospital.com", false, "PatientFirstName11", null, "PatientLastName11", false, null, "PATIENT11@HOSPITAL.COM", "PATIENT11@HOSPITAL.COM", "AQAAAAIAAYagAAAAELtElDENudPytPrUG2y0J9O6/l9mRGw6JCvaUrAvNzhr1K80Sl0pB7H3+IoYkSngrA==", null, false, "368c2b2c-5b18-423a-abc1-325dc82869f1", false, "patient11@hospital.com" },
                    { "87885af0-f0b0-487a-a549-a5c3a2dd6feb", 0, null, "d397916e-4b43-47da-9785-2bad55bdff01", null, "patient9@hospital.com", false, "PatientFirstName9", null, "PatientLastName9", false, null, "PATIENT9@HOSPITAL.COM", "PATIENT9@HOSPITAL.COM", "AQAAAAIAAYagAAAAEP+dRGBiz0RhbROuoT5pqAjo5YS4swbzvqF9rCGkgsga014gH5jtqVHmlL6Dawea9w==", null, false, "b627f749-2ffb-4dbd-ab85-4b330f8fefd4", false, "patient9@hospital.com" },
                    { "8a080b0d-40bb-400b-b4c2-65e825cd4095", 0, null, "2045f162-bd7d-4a22-b5f2-c9d0906a7584", null, "doctor12@hospital.com", false, "DoctorFirstName12", null, "DoctorLastName12", false, null, "DOCTOR12@HOSPITAL.COM", "DOCTOR12@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMgrveFNI3PXzUc5+Z8fu5gaNTpW1WZAYAAlMDMpesqT9CfmFuKgieLTjBwP5++Pxw==", null, false, "5f64f8de-b384-42a5-b45b-5deefc8eee0f", false, "doctor12@hospital.com" },
                    { "94c2a6cb-dd64-4916-ae49-ef81ea7ce4e8", 0, null, "a7656bce-88e6-4905-a957-bf57bc3007f7", null, "patient14@hospital.com", false, "PatientFirstName14", null, "PatientLastName14", false, null, "PATIENT14@HOSPITAL.COM", "PATIENT14@HOSPITAL.COM", "AQAAAAIAAYagAAAAEBRYpvuxCi7ETYhE1cDmOnAnygrhwUm37C0TsHtGkE/ta6R2rcRT/SBwECWISphC9A==", null, false, "c74abe0d-1328-47aa-b7df-05bb10882ee8", false, "patient14@hospital.com" },
                    { "97ac44f4-d51c-46fc-99fa-7e4c8d19d419", 0, null, "9847d90e-09db-4236-bfa3-09da0f4d0f26", null, "patient7@hospital.com", false, "PatientFirstName7", null, "PatientLastName7", false, null, "PATIENT7@HOSPITAL.COM", "PATIENT7@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMExs+owt75petrVdrbcBrvhI2uj7g3gTV79HsKqYULAOCApFVhvZqBfCNjmfTCtpw==", null, false, "06d69711-9036-481e-8100-9c4d40dde825", false, "patient7@hospital.com" },
                    { "99d7ac02-c932-44cc-a603-5bb6ce736456", 0, null, "39817e66-bf5e-4632-b19f-54da909954dc", null, "doctor5@hospital.com", false, "DoctorFirstName5", null, "DoctorLastName5", false, null, "DOCTOR5@HOSPITAL.COM", "DOCTOR5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEK1GLT0b3pCsFYJNdG8G/KMUOnmcI/6tk/MGxLDXl1oL0+Vb/vVKqeWQ0pDoZYPJjg==", null, false, "0b215f64-cf4f-45c5-b6fe-b4cff5ce6f5f", false, "doctor5@hospital.com" },
                    { "a89bc373-528d-4edf-8a0a-4991dfe997fe", 0, null, "044c71f3-ceb5-4209-809a-f95c2ac71f1d", null, "patient3@hospital.com", false, "PatientFirstName3", null, "PatientLastName3", false, null, "PATIENT3@HOSPITAL.COM", "PATIENT3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAGTijTvXqTCApWc6WejGtz58j+aQC+/U2sxYwHfhIRPF5WlW4zuKim2OGKrKuC2Mg==", null, false, "954a1200-ced5-40c1-85b6-52c452c6f16d", false, "patient3@hospital.com" },
                    { "ac0fb48d-ebd1-41ff-8fb4-b040dcc2ba36", 0, null, "b2c02a49-b915-4aea-981d-b23295d2122d", null, "patient12@hospital.com", false, "PatientFirstName12", null, "PatientLastName12", false, null, "PATIENT12@HOSPITAL.COM", "PATIENT12@HOSPITAL.COM", "AQAAAAIAAYagAAAAEOlgCbcpPSLm93v+ze/uGJUguhI1af5ckBkNlfUXsarmTABBpjP1PaMqsqbCkZXYKA==", null, false, "7b58caa1-8899-45eb-8982-3f33ef7dac53", false, "patient12@hospital.com" },
                    { "admin", 0, null, "34c30de7-0731-4804-a5af-2f099c24596f", null, "admin@gmail.com", false, "Admin", null, "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEJsMJq+TSgAZLaduZSXkvRYintVGjdkfBtV/rgQ+X4O2NryCufpQLhs7903jYL4wNw==", null, false, "fdd9cfed-fafe-4ae6-9b12-c078ebeec02a", false, "admin@gmail.com" },
                    { "b578c94a-52b6-4c8a-8c38-711eaf851baa", 0, null, "bb2adecc-fbf9-4748-82c5-4afe8ecec78a", null, "nurse15@hospital.com", false, "NurseFirstName15", null, "NurseLastName15", false, null, "NURSE15@HOSPITAL.COM", "NURSE15@HOSPITAL.COM", "AQAAAAIAAYagAAAAEBQlzDnB1dLuWrMJ4s24IcKwnQA79nMbSElq6txdx9cUsPYkbacKR669fRc66P7zyQ==", null, false, "46d240d6-994c-4535-8c2f-7cfa6d55dd5f", false, "nurse15@hospital.com" },
                    { "b88da60e-96e5-4f7c-b0b6-d5bc552d7c3d", 0, null, "9d59944a-be2b-4300-af46-bd6471817c8d", null, "patient13@hospital.com", false, "PatientFirstName13", null, "PatientLastName13", false, null, "PATIENT13@HOSPITAL.COM", "PATIENT13@HOSPITAL.COM", "AQAAAAIAAYagAAAAEHx2JLxtgj3CaJOi5Hkei11K59Ib1/++c4RG60P25hqfmdzOfwdUTwnteHlJb/svzg==", null, false, "17ce9fbe-ad94-458f-b682-5a7aa21e382f", false, "patient13@hospital.com" },
                    { "bb8ae1d3-13a4-4d21-9014-8f4854cf1520", 0, null, "2174b8e7-56ef-4eec-8e40-404e58a00ad7", null, "doctor13@hospital.com", false, "DoctorFirstName13", null, "DoctorLastName13", false, null, "DOCTOR13@HOSPITAL.COM", "DOCTOR13@HOSPITAL.COM", "AQAAAAIAAYagAAAAED6D+1XXjAqp7p6dZbSPQCTKLWGaIyAgNU5mUPjQm8dZtyOXTbhvzlylfeGEAOaNiA==", null, false, "8d96fdb3-6ce3-4a86-8023-4f642e8c83fd", false, "doctor13@hospital.com" },
                    { "bbed496d-8510-482c-8868-551b2a5ca9b3", 0, null, "2efa5d15-7ece-464e-909c-29d3f11a8ae0", null, "doctor9@hospital.com", false, "DoctorFirstName9", null, "DoctorLastName9", false, null, "DOCTOR9@HOSPITAL.COM", "DOCTOR9@HOSPITAL.COM", "AQAAAAIAAYagAAAAEIXxRcSh4Xb2BowCG8Z6quH5LwxvbiTz/uFUMC4Mb79THf0qW394Nqz2gfixeAEdVg==", null, false, "4d4a6678-1d40-4228-afad-fb430f9db557", false, "doctor9@hospital.com" },
                    { "c2edbc39-d0b9-4188-9b92-6871762d7d81", 0, null, "5427ef8d-6e6a-4fe9-b81b-580391e6dcd5", null, "nurse4@hospital.com", false, "NurseFirstName4", null, "NurseLastName4", false, null, "NURSE4@HOSPITAL.COM", "NURSE4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEIsFHxuQHNkKHL69eAuRxMMcDVct6FwbHPHf7KziRqu/B54YB13+SGtoUfUSUXYImg==", null, false, "b381633e-caf4-4dad-b7d1-d66198d6a2dc", false, "nurse4@hospital.com" },
                    { "c3c31130-fdf5-41ab-9cf6-f6415344955c", 0, null, "b8fc0d64-5af9-41ab-b1f6-c8a4b16fb675", null, "doctor14@hospital.com", false, "DoctorFirstName14", null, "DoctorLastName14", false, null, "DOCTOR14@HOSPITAL.COM", "DOCTOR14@HOSPITAL.COM", "AQAAAAIAAYagAAAAEA9UQLi4bTpXqEckkrwr3GnFQmKnXOkfWnyqBo9ETst48E7XmEcEZpcXv9tLSrc/7w==", null, false, "64c4aaaa-d91a-43a5-8985-d16f382966b6", false, "doctor14@hospital.com" },
                    { "ca0b6bdd-8b21-4da1-81b2-6ad7ad53c081", 0, null, "104dcefb-75c8-48c5-9746-cc205f8b0784", null, "patient2@hospital.com", false, "PatientFirstName2", null, "PatientLastName2", false, null, "PATIENT2@HOSPITAL.COM", "PATIENT2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMJDuyv9rn9NWRTBVmxLSr+V4fojEpI3w9r5yKy7AiGZqW6etMHte0sZ3z3qslvDZw==", null, false, "586e69b3-e406-4b68-82ea-fa0889740006", false, "patient2@hospital.com" },
                    { "d292f68c-8242-474a-a756-0b7edae0c142", 0, null, "014e8127-b934-4f2f-9389-9eee12a45fce", null, "nurse6@hospital.com", false, "NurseFirstName6", null, "NurseLastName6", false, null, "NURSE6@HOSPITAL.COM", "NURSE6@HOSPITAL.COM", "AQAAAAIAAYagAAAAEPJxYZyUODRv2QlvHNsuwXVwIw5KU+WjivoFKL321+s+V3dhBYSw31TCjh4LEgMEEg==", null, false, "76a838ac-03ae-4cc2-a0ce-99a516afd8f7", false, "nurse6@hospital.com" },
                    { "d31d9f53-c7fd-4bc8-bc13-027617ed4310", 0, null, "9fb83851-d998-46ca-8261-9fe8b16f3ac0", null, "patient4@hospital.com", false, "PatientFirstName4", null, "PatientLastName4", false, null, "PATIENT4@HOSPITAL.COM", "PATIENT4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFijoJWsEdhKiDqkmPM3uNwzb1dfGV4oFwUoTr+Eh9qbea88eN/AiWfrbGOH1nAH+g==", null, false, "d1aee2d1-8dcb-4291-b3fd-9987c9a89551", false, "patient4@hospital.com" },
                    { "d90522ea-9bd2-4ac3-a14c-f8bddb33000f", 0, null, "f4d15e21-8af2-423a-a70f-fc5079c691b8", null, "patient8@hospital.com", false, "PatientFirstName8", null, "PatientLastName8", false, null, "PATIENT8@HOSPITAL.COM", "PATIENT8@HOSPITAL.COM", "AQAAAAIAAYagAAAAEGyYs9rKKslHR4y0Xy4TX9cNJb3RRECli4dp6PzdfM+tbMjvXrIzQz/XVSK/qN0SGw==", null, false, "e36da6c0-0136-4871-9cca-5b5d5c119601", false, "patient8@hospital.com" },
                    { "e49305c3-50f5-43a9-b330-49b41767d5b3", 0, null, "0b345e59-ea22-439f-80d7-69d3f3d5b9d9", null, "doctor3@hospital.com", false, "DoctorFirstName3", null, "DoctorLastName3", false, null, "DOCTOR3@HOSPITAL.COM", "DOCTOR3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEDnscVoi/LWoVl8IjANa2in8gEyE+l8dOzH40o3A/5rPZGYMUShSdOr8uxP/BV7SWA==", null, false, "6ab8b608-cf58-4a97-83a5-bc6309ff7332", false, "doctor3@hospital.com" },
                    { "e5281461-20ae-4c4a-b6ba-4ea9340edd29", 0, null, "8efe3808-cd3f-4b35-b9f0-3798d30b2192", null, "nurse12@hospital.com", false, "NurseFirstName12", null, "NurseLastName12", false, null, "NURSE12@HOSPITAL.COM", "NURSE12@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMMOC8IJn3zQYEpDC4ID9ZOyiXrusLh2us9+FqHXZIMn0PzntCHn/jUt7eZdgdBAzg==", null, false, "d20a4956-b0ee-4064-9e97-5865d8087c72", false, "nurse12@hospital.com" },
                    { "e83f24f1-744b-4f47-a4e7-b079dfe65934", 0, null, "1d64f658-572c-4bbb-956f-0cb0954fb5e9", null, "nurse13@hospital.com", false, "NurseFirstName13", null, "NurseLastName13", false, null, "NURSE13@HOSPITAL.COM", "NURSE13@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJuQRfzE6eB/kFYzHKFwbuAiE76xAgFFyVJDI5GkrW74bxksk8RY3eS/cXEsOUDa5A==", null, false, "bfded83f-5df0-412c-949e-e62fbe8838d0", false, "nurse13@hospital.com" },
                    { "f45c11a2-d3ce-4430-a4f8-a35471da56b3", 0, null, "2279b622-d348-40eb-b468-ebf097edc4c5", null, "nurse8@hospital.com", false, "NurseFirstName8", null, "NurseLastName8", false, null, "NURSE8@HOSPITAL.COM", "NURSE8@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJ2crzvUMV/5FxveYODgZhbtBa85NdudZHj/Lt8AF558ZW+BS/H3Kyr7DUu+b46Y4w==", null, false, "88086bf8-a4e0-403c-a30e-7a0079508014", false, "nurse8@hospital.com" },
                    { "fd5eb233-cfd6-45e7-83b0-517995f59942", 0, null, "98d4a058-aa41-41d4-b555-4b49e9c7fa50", null, "nurse11@hospital.com", false, "NurseFirstName11", null, "NurseLastName11", false, null, "NURSE11@HOSPITAL.COM", "NURSE11@HOSPITAL.COM", "AQAAAAIAAYagAAAAEHXswZdNYAmVDFdG+Ruk69EBjo52rsT8RpX3dsZdMgOYeDjd4gjSEBOfxT5RnbBBpg==", null, false, "fddcaf16-b36e-44cd-8c5b-52439caa426c", false, "nurse11@hospital.com" },
                    { "fe8e5284-4378-4cfc-b1e0-4901997766d5", 0, null, "930c7458-731d-4385-bc8f-a502d91d54ac", null, "patient5@hospital.com", false, "PatientFirstName5", null, "PatientLastName5", false, null, "PATIENT5@HOSPITAL.COM", "PATIENT5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEIpDzGu8/yINRc/Xb8u9S+MS3r/EnZZImxpmzOCE46ZeGOEUNtgdMGcJYJAbel6eJg==", null, false, "70bd87de-3f04-46fa-aa35-9ae02e5d08c6", false, "patient5@hospital.com" }
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
                    { 1, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5455), null, "Building A - Floor 3", "Cardiology" },
                    { 2, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5458), null, "Building B - Floor 2", "Neurology" },
                    { 3, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5460), null, "Building C - Floor 1", "Orthopedics" },
                    { 4, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5462), null, "Building D - Floor 4", "Pediatrics" },
                    { 5, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5463), null, "Building E - Ground Floor", "Emergency" }
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
                    { 1, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5545), "Cardiology" },
                    { 2, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5547), "Neurology" },
                    { 3, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5548), "Orthopedics" },
                    { 4, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5548), "Pediatrics" },
                    { 5, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5549), "Emergency Medicine" },
                    { 6, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5550), "Radiology" },
                    { 7, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5551), "Oncology" },
                    { 8, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5552), "Dermatology" },
                    { 9, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5552), "General Surgery" },
                    { 10, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5553), "Anesthesiology" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "3", "03f3e3bf-6e6d-4114-89e5-ce6a9960f312" },
                    { "2", "1386105f-78ab-4559-8ab9-b07a35427ef1" },
                    { "3", "13a2be0f-e28b-43ce-b231-8039b2913863" },
                    { "3", "17f1003f-8675-4cde-8e51-a831eeda64b1" },
                    { "3", "26d02839-dd58-4031-a663-1b7b03b700cc" },
                    { "2", "294f8dd6-2218-4a3c-bbf0-063450014f97" },
                    { "4", "2cda7d29-5f10-485f-b0fe-2c39ce5acf46" },
                    { "2", "3ad709eb-1362-4706-9cc8-a47f1dbf7c5a" },
                    { "3", "3fe443f7-64d3-4e8f-b9a9-a43ea6cb5b85" },
                    { "4", "42904cc5-817c-4807-bfa9-b41e26d72291" },
                    { "2", "4439e38b-2d1d-43b2-a053-5d001363b5f6" },
                    { "3", "4c10b1ca-d2ae-4602-ad86-5cb0d860bcdf" },
                    { "3", "4c948d64-2155-4069-b5fd-38c64f5ebd06" },
                    { "3", "53b34dd6-9426-477a-b6d4-d0e365f084f1" },
                    { "2", "56392405-5b4d-4c37-9dba-fa00903ebb44" },
                    { "2", "583f98ee-69c2-4dbc-9397-f4044acf82e4" },
                    { "4", "5d10b313-3ba5-4772-bdfd-3cc4c10153c1" },
                    { "2", "6330f491-dbbe-49fc-bdd2-02230c96d374" },
                    { "2", "7c21534f-1bc2-4efe-844b-1fcf5d86f59f" },
                    { "4", "7cf5a270-6f65-4879-a4f8-1401b7400553" },
                    { "2", "7f0c8d5b-ffec-4d67-8217-aa6e4b5b29b7" },
                    { "4", "80405353-16a0-4332-8f56-8cfd94924595" },
                    { "4", "87885af0-f0b0-487a-a549-a5c3a2dd6feb" },
                    { "2", "8a080b0d-40bb-400b-b4c2-65e825cd4095" },
                    { "4", "94c2a6cb-dd64-4916-ae49-ef81ea7ce4e8" },
                    { "4", "97ac44f4-d51c-46fc-99fa-7e4c8d19d419" },
                    { "2", "99d7ac02-c932-44cc-a603-5bb6ce736456" },
                    { "4", "a89bc373-528d-4edf-8a0a-4991dfe997fe" },
                    { "4", "ac0fb48d-ebd1-41ff-8fb4-b040dcc2ba36" },
                    { "0", "admin" },
                    { "3", "b578c94a-52b6-4c8a-8c38-711eaf851baa" },
                    { "4", "b88da60e-96e5-4f7c-b0b6-d5bc552d7c3d" },
                    { "2", "bb8ae1d3-13a4-4d21-9014-8f4854cf1520" },
                    { "2", "bbed496d-8510-482c-8868-551b2a5ca9b3" },
                    { "3", "c2edbc39-d0b9-4188-9b92-6871762d7d81" },
                    { "2", "c3c31130-fdf5-41ab-9cf6-f6415344955c" },
                    { "4", "ca0b6bdd-8b21-4da1-81b2-6ad7ad53c081" },
                    { "3", "d292f68c-8242-474a-a756-0b7edae0c142" },
                    { "4", "d31d9f53-c7fd-4bc8-bc13-027617ed4310" },
                    { "4", "d90522ea-9bd2-4ac3-a14c-f8bddb33000f" },
                    { "2", "e49305c3-50f5-43a9-b330-49b41767d5b3" },
                    { "3", "e5281461-20ae-4c4a-b6ba-4ea9340edd29" },
                    { "3", "e83f24f1-744b-4f47-a4e7-b079dfe65934" },
                    { "3", "f45c11a2-d3ce-4430-a4f8-a35471da56b3" },
                    { "3", "fd5eb233-cfd6-45e7-83b0-517995f59942" },
                    { "4", "fe8e5284-4378-4cfc-b1e0-4901997766d5" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "SpecialtyId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 16, 15, 42, 11, 532, DateTimeKind.Utc).AddTicks(3299), null, "DoctorFirstName1", null, "DoctorLastName1", null, "7f0c8d5b-ffec-4d67-8217-aa6e4b5b29b7" },
                    { 2, new DateTime(2025, 5, 16, 15, 42, 11, 724, DateTimeKind.Utc).AddTicks(2885), null, "DoctorFirstName2", null, "DoctorLastName2", null, "1386105f-78ab-4559-8ab9-b07a35427ef1" },
                    { 3, new DateTime(2025, 5, 16, 15, 42, 11, 916, DateTimeKind.Utc).AddTicks(4736), null, "DoctorFirstName3", null, "DoctorLastName3", null, "e49305c3-50f5-43a9-b330-49b41767d5b3" },
                    { 4, new DateTime(2025, 5, 16, 15, 42, 12, 110, DateTimeKind.Utc).AddTicks(5400), null, "DoctorFirstName4", null, "DoctorLastName4", null, "56392405-5b4d-4c37-9dba-fa00903ebb44" },
                    { 5, new DateTime(2025, 5, 16, 15, 42, 12, 324, DateTimeKind.Utc).AddTicks(5534), null, "DoctorFirstName5", null, "DoctorLastName5", null, "99d7ac02-c932-44cc-a603-5bb6ce736456" },
                    { 6, new DateTime(2025, 5, 16, 15, 42, 12, 515, DateTimeKind.Utc).AddTicks(2909), null, "DoctorFirstName6", null, "DoctorLastName6", null, "4439e38b-2d1d-43b2-a053-5d001363b5f6" },
                    { 7, new DateTime(2025, 5, 16, 15, 42, 12, 705, DateTimeKind.Utc).AddTicks(5263), null, "DoctorFirstName7", null, "DoctorLastName7", null, "7c21534f-1bc2-4efe-844b-1fcf5d86f59f" },
                    { 8, new DateTime(2025, 5, 16, 15, 42, 12, 895, DateTimeKind.Utc).AddTicks(7751), null, "DoctorFirstName8", null, "DoctorLastName8", null, "3ad709eb-1362-4706-9cc8-a47f1dbf7c5a" },
                    { 9, new DateTime(2025, 5, 16, 15, 42, 13, 86, DateTimeKind.Utc).AddTicks(3408), null, "DoctorFirstName9", null, "DoctorLastName9", null, "bbed496d-8510-482c-8868-551b2a5ca9b3" },
                    { 10, new DateTime(2025, 5, 16, 15, 42, 13, 281, DateTimeKind.Utc).AddTicks(154), null, "DoctorFirstName10", null, "DoctorLastName10", null, "6330f491-dbbe-49fc-bdd2-02230c96d374" },
                    { 11, new DateTime(2025, 5, 16, 15, 42, 13, 473, DateTimeKind.Utc).AddTicks(3317), null, "DoctorFirstName11", null, "DoctorLastName11", null, "583f98ee-69c2-4dbc-9397-f4044acf82e4" },
                    { 12, new DateTime(2025, 5, 16, 15, 42, 13, 669, DateTimeKind.Utc).AddTicks(7447), null, "DoctorFirstName12", null, "DoctorLastName12", null, "8a080b0d-40bb-400b-b4c2-65e825cd4095" },
                    { 13, new DateTime(2025, 5, 16, 15, 42, 13, 859, DateTimeKind.Utc).AddTicks(5260), null, "DoctorFirstName13", null, "DoctorLastName13", null, "bb8ae1d3-13a4-4d21-9014-8f4854cf1520" },
                    { 14, new DateTime(2025, 5, 16, 15, 42, 14, 49, DateTimeKind.Utc).AddTicks(1025), null, "DoctorFirstName14", null, "DoctorLastName14", null, "c3c31130-fdf5-41ab-9cf6-f6415344955c" },
                    { 15, new DateTime(2025, 5, 16, 15, 42, 14, 239, DateTimeKind.Utc).AddTicks(8652), null, "DoctorFirstName15", null, "DoctorLastName15", null, "294f8dd6-2218-4a3c-bbf0-063450014f97" }
                });

            migrationBuilder.InsertData(
                table: "Nurses",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 16, 15, 42, 11, 596, DateTimeKind.Utc).AddTicks(5237), null, "NurseFirstName1", null, "NurseLastName1", "4c10b1ca-d2ae-4602-ad86-5cb0d860bcdf" },
                    { 2, new DateTime(2025, 5, 16, 15, 42, 11, 788, DateTimeKind.Utc).AddTicks(3302), null, "NurseFirstName2", null, "NurseLastName2", "03f3e3bf-6e6d-4114-89e5-ce6a9960f312" },
                    { 3, new DateTime(2025, 5, 16, 15, 42, 11, 979, DateTimeKind.Utc).AddTicks(6638), null, "NurseFirstName3", null, "NurseLastName3", "53b34dd6-9426-477a-b6d4-d0e365f084f1" },
                    { 4, new DateTime(2025, 5, 16, 15, 42, 12, 196, DateTimeKind.Utc).AddTicks(2577), null, "NurseFirstName4", null, "NurseLastName4", "c2edbc39-d0b9-4188-9b92-6871762d7d81" },
                    { 5, new DateTime(2025, 5, 16, 15, 42, 12, 387, DateTimeKind.Utc).AddTicks(7233), null, "NurseFirstName5", null, "NurseLastName5", "26d02839-dd58-4031-a663-1b7b03b700cc" },
                    { 6, new DateTime(2025, 5, 16, 15, 42, 12, 579, DateTimeKind.Utc).AddTicks(2503), null, "NurseFirstName6", null, "NurseLastName6", "d292f68c-8242-474a-a756-0b7edae0c142" },
                    { 7, new DateTime(2025, 5, 16, 15, 42, 12, 768, DateTimeKind.Utc).AddTicks(6432), null, "NurseFirstName7", null, "NurseLastName7", "4c948d64-2155-4069-b5fd-38c64f5ebd06" },
                    { 8, new DateTime(2025, 5, 16, 15, 42, 12, 959, DateTimeKind.Utc).AddTicks(582), null, "NurseFirstName8", null, "NurseLastName8", "f45c11a2-d3ce-4430-a4f8-a35471da56b3" },
                    { 9, new DateTime(2025, 5, 16, 15, 42, 13, 150, DateTimeKind.Utc).AddTicks(389), null, "NurseFirstName9", null, "NurseLastName9", "17f1003f-8675-4cde-8e51-a831eeda64b1" },
                    { 10, new DateTime(2025, 5, 16, 15, 42, 13, 345, DateTimeKind.Utc).AddTicks(7405), null, "NurseFirstName10", null, "NurseLastName10", "13a2be0f-e28b-43ce-b231-8039b2913863" },
                    { 11, new DateTime(2025, 5, 16, 15, 42, 13, 536, DateTimeKind.Utc).AddTicks(6688), null, "NurseFirstName11", null, "NurseLastName11", "fd5eb233-cfd6-45e7-83b0-517995f59942" },
                    { 12, new DateTime(2025, 5, 16, 15, 42, 13, 733, DateTimeKind.Utc).AddTicks(4187), null, "NurseFirstName12", null, "NurseLastName12", "e5281461-20ae-4c4a-b6ba-4ea9340edd29" },
                    { 13, new DateTime(2025, 5, 16, 15, 42, 13, 922, DateTimeKind.Utc).AddTicks(7361), null, "NurseFirstName13", null, "NurseLastName13", "e83f24f1-744b-4f47-a4e7-b079dfe65934" },
                    { 14, new DateTime(2025, 5, 16, 15, 42, 14, 112, DateTimeKind.Utc).AddTicks(1126), null, "NurseFirstName14", null, "NurseLastName14", "3fe443f7-64d3-4e8f-b9a9-a43ea6cb5b85" },
                    { 15, new DateTime(2025, 5, 16, 15, 42, 14, 302, DateTimeKind.Utc).AddTicks(9847), null, "NurseFirstName15", null, "NurseLastName15", "b578c94a-52b6-4c8a-8c38-711eaf851baa" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 16, 15, 42, 11, 660, DateTimeKind.Utc).AddTicks(4345), null, "PatientFirstName1", null, "PatientLastName1", "5d10b313-3ba5-4772-bdfd-3cc4c10153c1" },
                    { 2, new DateTime(2025, 5, 16, 15, 42, 11, 852, DateTimeKind.Utc).AddTicks(7060), null, "PatientFirstName2", null, "PatientLastName2", "ca0b6bdd-8b21-4da1-81b2-6ad7ad53c081" },
                    { 3, new DateTime(2025, 5, 16, 15, 42, 12, 42, DateTimeKind.Utc).AddTicks(8480), null, "PatientFirstName3", null, "PatientLastName3", "a89bc373-528d-4edf-8a0a-4991dfe997fe" },
                    { 4, new DateTime(2025, 5, 16, 15, 42, 12, 260, DateTimeKind.Utc).AddTicks(6295), null, "PatientFirstName4", null, "PatientLastName4", "d31d9f53-c7fd-4bc8-bc13-027617ed4310" },
                    { 5, new DateTime(2025, 5, 16, 15, 42, 12, 450, DateTimeKind.Utc).AddTicks(7404), null, "PatientFirstName5", null, "PatientLastName5", "fe8e5284-4378-4cfc-b1e0-4901997766d5" },
                    { 6, new DateTime(2025, 5, 16, 15, 42, 12, 642, DateTimeKind.Utc).AddTicks(5618), null, "PatientFirstName6", null, "PatientLastName6", "2cda7d29-5f10-485f-b0fe-2c39ce5acf46" },
                    { 7, new DateTime(2025, 5, 16, 15, 42, 12, 832, DateTimeKind.Utc).AddTicks(3499), null, "PatientFirstName7", null, "PatientLastName7", "97ac44f4-d51c-46fc-99fa-7e4c8d19d419" },
                    { 8, new DateTime(2025, 5, 16, 15, 42, 13, 22, DateTimeKind.Utc).AddTicks(3850), null, "PatientFirstName8", null, "PatientLastName8", "d90522ea-9bd2-4ac3-a14c-f8bddb33000f" },
                    { 9, new DateTime(2025, 5, 16, 15, 42, 13, 215, DateTimeKind.Utc).AddTicks(9470), null, "PatientFirstName9", null, "PatientLastName9", "87885af0-f0b0-487a-a549-a5c3a2dd6feb" },
                    { 10, new DateTime(2025, 5, 16, 15, 42, 13, 410, DateTimeKind.Utc).AddTicks(2041), null, "PatientFirstName10", null, "PatientLastName10", "7cf5a270-6f65-4879-a4f8-1401b7400553" },
                    { 11, new DateTime(2025, 5, 16, 15, 42, 13, 603, DateTimeKind.Utc).AddTicks(5901), null, "PatientFirstName11", null, "PatientLastName11", "80405353-16a0-4332-8f56-8cfd94924595" },
                    { 12, new DateTime(2025, 5, 16, 15, 42, 13, 796, DateTimeKind.Utc).AddTicks(5526), null, "PatientFirstName12", null, "PatientLastName12", "ac0fb48d-ebd1-41ff-8fb4-b040dcc2ba36" },
                    { 13, new DateTime(2025, 5, 16, 15, 42, 13, 985, DateTimeKind.Utc).AddTicks(9497), null, "PatientFirstName13", null, "PatientLastName13", "b88da60e-96e5-4f7c-b0b6-d5bc552d7c3d" },
                    { 14, new DateTime(2025, 5, 16, 15, 42, 14, 176, DateTimeKind.Utc).AddTicks(7726), null, "PatientFirstName14", null, "PatientLastName14", "94c2a6cb-dd64-4916-ae49-ef81ea7ce4e8" },
                    { 15, new DateTime(2025, 5, 16, 15, 42, 14, 365, DateTimeKind.Utc).AddTicks(9846), null, "PatientFirstName15", null, "PatientLastName15", "42904cc5-817c-4807-bfa9-b41e26d72291" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "Name", "Number" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5495), 1, "Room 301", 301 },
                    { 2, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5497), 1, "Room 302", 302 },
                    { 3, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5499), 2, "Room 201", 201 },
                    { 4, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5508), 2, "Room 202", 202 },
                    { 5, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5509), 3, "Room 101", 101 },
                    { 6, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5510), 3, "Room 102", 102 },
                    { 7, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5511), 4, "Room 401", 401 },
                    { 8, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5512), 4, "Room 402", 402 },
                    { 9, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5513), 5, "Emergency Room 1", 1 },
                    { 10, new DateTime(2025, 5, 16, 15, 42, 11, 404, DateTimeKind.Utc).AddTicks(5514), 5, "Emergency Room 2", 2 }
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
                columns: new[] { "Id", "AppointmentDate", "AppointmentEndTime", "AppointmentStartTime", "DoctorId", "Notes", "NurseId", "PatientId", "Reason", "RoomId", "Status" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 5, 11), new TimeOnly(11, 0, 0), new TimeOnly(10, 0, 0), 1, null, null, 1, 0, null, 1 },
                    { 2, new DateOnly(2025, 5, 13), new TimeOnly(12, 0, 0), new TimeOnly(11, 0, 0), 2, null, null, 2, 1, null, 2 },
                    { 3, new DateOnly(2025, 5, 14), new TimeOnly(10, 0, 0), new TimeOnly(9, 0, 0), 3, null, null, 3, 1, null, 2 },
                    { 4, new DateOnly(2025, 5, 15), new TimeOnly(13, 0, 0), new TimeOnly(12, 0, 0), 4, null, null, 4, 1, null, 1 },
                    { 5, new DateOnly(2025, 5, 16), new TimeOnly(13, 0, 0), new TimeOnly(12, 0, 0), 3, null, null, 3, 2, null, 0 },
                    { 6, new DateOnly(2025, 5, 16), new TimeOnly(15, 0, 0), new TimeOnly(14, 0, 0), 4, null, null, 4, 3, null, 3 },
                    { 7, new DateOnly(2025, 5, 16), new TimeOnly(10, 0, 0), new TimeOnly(9, 0, 0), 4, null, null, 1, 3, null, 0 },
                    { 8, new DateOnly(2025, 5, 16), new TimeOnly(12, 0, 0), new TimeOnly(11, 0, 0), 3, null, null, 2, 3, null, 0 },
                    { 9, new DateOnly(2025, 5, 16), new TimeOnly(16, 0, 0), new TimeOnly(15, 0, 0), 1, null, null, 1, 3, null, 3 },
                    { 10, new DateOnly(2025, 5, 17), new TimeOnly(11, 0, 0), new TimeOnly(10, 0, 0), 5, null, null, 5, 4, null, 0 },
                    { 11, new DateOnly(2025, 5, 18), new TimeOnly(12, 0, 0), new TimeOnly(11, 0, 0), 1, null, null, 2, 0, null, 3 },
                    { 12, new DateOnly(2025, 5, 19), new TimeOnly(13, 0, 0), new TimeOnly(12, 0, 0), 2, null, null, 3, 1, null, 0 },
                    { 13, new DateOnly(2025, 5, 26), new TimeOnly(14, 0, 0), new TimeOnly(13, 0, 0), 4, null, null, 5, 3, null, 0 },
                    { 14, new DateOnly(2025, 5, 31), new TimeOnly(15, 0, 0), new TimeOnly(14, 0, 0), 5, null, null, 1, 4, null, 3 }
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
