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
                    Area = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    InsDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                table: "ApplicationUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "03e09429-4681-4c4d-a305-6c26d5dda010", 0, null, "01099e83-284b-497b-8310-c9c67d15a9b0", null, "nurse5@hospital.com", false, "NurseFirstName5", null, "NurseLastName5", false, null, "NURSE5@HOSPITAL.COM", "NURSE5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEA6kIsr8SBdGwahtDzqxSpedfJRlPrh0UbbwxIH1SF3uGLwCudLcgPWkh23ijxiwNQ==", null, false, "eecfc835-4ff6-487c-bc68-789a6c06752d", false, "nurse5@hospital.com" },
                    { "2acf712e-5eec-4c43-8d91-4c342674513c", 0, null, "c5a3d85a-b90e-4974-b107-a3caf4d9127d", null, "nurse4@hospital.com", false, "NurseFirstName4", null, "NurseLastName4", false, null, "NURSE4@HOSPITAL.COM", "NURSE4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEIKYhanuFyxkDSeH3uUkQIWAy9J/GbmBQEwplTe3+Gz7WQQr53nH8qRRpnl9Wd+kqA==", null, false, "99f1eaab-dbf0-439d-9dbe-83e7b1fb8f3a", false, "nurse4@hospital.com" },
                    { "3c5e5d6a-5b13-4eb7-a8b6-a1511761a3bc", 0, null, "8be87191-d693-426e-bd77-8c28c4740167", null, "doctor2@hospital.com", false, "DoctorFirstName2", null, "DoctorLastName2", false, null, "DOCTOR2@HOSPITAL.COM", "DOCTOR2@HOSPITAL.COM", "AQAAAAIAAYagAAAAELGNy0GcNQoBPJw7rA8V5xTRND87+nlznAnJgigo+AJNQ0OiP0oR/GkPWHn19weI0Q==", null, false, "c9695906-cec6-44ee-8446-8275a66f652c", false, "doctor2@hospital.com" },
                    { "3e6ac8f0-b5a6-47cf-bb0e-4bb849fac1ca", 0, null, "98999fa9-d20c-4a47-8c31-4b229c092fba", null, "patient3@hospital.com", false, "PatientFirstName3", null, "PatientLastName3", false, null, "PATIENT3@HOSPITAL.COM", "PATIENT3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJvCzD1MJu2us3mG5A8xL4Xy2FYQ4DsxaUK6jJbZtPsl5WaNzG7dPD44GxzICjunTQ==", null, false, "2da1faa4-9a7e-4c34-b45e-893e6701d07a", false, "patient3@hospital.com" },
                    { "4cea8fbe-6fca-4b89-a627-bac997f6e253", 0, null, "e42237fb-16ed-4b21-ab41-f4d98fe75f04", null, "nurse2@hospital.com", false, "NurseFirstName2", null, "NurseLastName2", false, null, "NURSE2@HOSPITAL.COM", "NURSE2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEOV/RHx1Ry81j26b5JNh/Az2xkrDb91BhmBpcoheqU7a8MJhKFQasOve6xF+UeKNqA==", null, false, "a85cb66d-489e-4075-8dc1-1ee6cdb6ed60", false, "nurse2@hospital.com" },
                    { "4f11152c-79cf-4038-8703-46f269dd27c5", 0, null, "30f5363f-cc0b-4bc1-943f-c27e9c32ac47", null, "doctor4@hospital.com", false, "DoctorFirstName4", null, "DoctorLastName4", false, null, "DOCTOR4@HOSPITAL.COM", "DOCTOR4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEIAbCmalGspbtU+C86du8bMdAUvwRelW+/uuEc6kZvR+6DyyULr2HQBAY5wuc7f0Rw==", null, false, "cc819c43-9f6f-40ed-835d-6085b15368ff", false, "doctor4@hospital.com" },
                    { "546e5437-587d-4f62-a849-60d7697a31ad", 0, null, "bf278dd1-c0e9-4c98-8e07-29653211df03", null, "nurse3@hospital.com", false, "NurseFirstName3", null, "NurseLastName3", false, null, "NURSE3@HOSPITAL.COM", "NURSE3@HOSPITAL.COM", "AQAAAAIAAYagAAAAELK1wV5Z2NkkYiVPksNnQq6RxjeWX98rGc2RPh+DnGa+3ivkdtyTzx+q0qORDxhIlg==", null, false, "e1d8a9f3-7214-4fac-8055-c2b0054a2fd1", false, "nurse3@hospital.com" },
                    { "6788f103-af42-4ff6-9490-10a210055007", 0, null, "b2a3d347-6a76-4bec-9ff9-54a8eb644b2b", null, "patient1@hospital.com", false, "PatientFirstName1", null, "PatientLastName1", false, null, "PATIENT1@HOSPITAL.COM", "PATIENT1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAQ2r7o5fzB0S10LhRj6bywxQLOwgYdUmRG5aoPD66YnAftVP/rCI72nIsGkNF88zA==", null, false, "7452e891-5f52-419a-a128-4752c110deb2", false, "patient1@hospital.com" },
                    { "7f577734-36d5-4c81-9fec-6e03c97f4839", 0, null, "127b01cf-27f8-4a99-a7e6-3b1d52ece9b8", null, "doctor1@hospital.com", false, "DoctorFirstName1", null, "DoctorLastName1", false, null, "DOCTOR1@HOSPITAL.COM", "DOCTOR1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEBXwkcDnpUoMcD2IVm8hh5FjXbPAaFm0PaPnveQr7ieBP0gogwhJFXPB79fmlz0B2w==", null, false, "678c821b-1c01-4d7c-a8ec-2f51ee501dcf", false, "doctor1@hospital.com" },
                    { "94bae31f-8b49-4f2f-a9d5-f192241977e6", 0, null, "25c8a6de-a2bc-47ac-a5f0-c5eb79925e30", null, "doctor3@hospital.com", false, "DoctorFirstName3", null, "DoctorLastName3", false, null, "DOCTOR3@HOSPITAL.COM", "DOCTOR3@HOSPITAL.COM", "AQAAAAIAAYagAAAAEBa6X5L2tuPZ3HXdbA4mjYj7hfZXDxB6ZKrwcruz9601wVJ5XYC9b/V7JGxZyxMW3w==", null, false, "80580eb4-dbd6-4909-a110-4a9ea3380eda", false, "doctor3@hospital.com" },
                    { "admin", 0, null, "90a6690b-32cd-4f43-ae27-099b3fa2cab5", null, "admin@gmail.com", false, "Admin", null, "User", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEJ3UIDa+YZ0DtoAaoStuTBUyc2tuFNSbUuYy5EU5q5S0uKhOOvvkmwIBP4iK95d53g==", null, false, "018e6e8f-fbd7-45f2-8422-3ff2db758df2", false, "admin@gmail.com" },
                    { "be67452b-7346-4a90-a938-36b47358de67", 0, null, "5e701219-4537-4b74-a887-1a2b566af252", null, "patient2@hospital.com", false, "PatientFirstName2", null, "PatientLastName2", false, null, "PATIENT2@HOSPITAL.COM", "PATIENT2@HOSPITAL.COM", "AQAAAAIAAYagAAAAEKYIoAvqhR+OVmJxzHceJakZ1WCwRMT5c71xNgviCkyCJQhWvH64fN1cHhaDzWBWjQ==", null, false, "655498c0-7538-421e-a3a9-397b5fb7f08f", false, "patient2@hospital.com" },
                    { "d789132b-45ff-4817-b687-24773726e23c", 0, null, "6cb6e5dc-4de8-410e-b68d-dff2592ec14d", null, "patient4@hospital.com", false, "PatientFirstName4", null, "PatientLastName4", false, null, "PATIENT4@HOSPITAL.COM", "PATIENT4@HOSPITAL.COM", "AQAAAAIAAYagAAAAEJDaV9nuoFMOB5c7zDR7EUwq4rDxYdg6xxPguskTjpORQjYQ1e3IF3kYzco3Yrev4g==", null, false, "335db97f-b76e-4da5-bd05-17d599f3b3ad", false, "patient4@hospital.com" },
                    { "ddcecbac-8a65-4e07-bab1-3d3ecb266b2c", 0, null, "dae1f43a-1574-4add-919b-f6ea0f751cde", null, "nurse1@hospital.com", false, "NurseFirstName1", null, "NurseLastName1", false, null, "NURSE1@HOSPITAL.COM", "NURSE1@HOSPITAL.COM", "AQAAAAIAAYagAAAAEOFHO8SgN7PyEBIVFiVwXG9kXXXnO5RLTaT/uYcoDt4Q1+YG14m6/+AgNQEioqdgWg==", null, false, "16cc86b9-2f60-4be4-ba88-b3461c027618", false, "nurse1@hospital.com" },
                    { "eb2c864f-242c-4b37-8e6e-8e5835caf2f0", 0, null, "08b77ec3-1bcb-42a2-af38-e365e8885d22", null, "patient5@hospital.com", false, "PatientFirstName5", null, "PatientLastName5", false, null, "PATIENT5@HOSPITAL.COM", "PATIENT5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEM/CHce39NulcklyDTVXzBw2mL9/vu4DmJrDdxZAyCOCNAbTXYBmRzm0vvAoX45BQQ==", null, false, "5a002eb6-b44d-45a2-b57a-d8d1bf0a5323", false, "patient5@hospital.com" },
                    { "f66df4a0-aba9-47f8-a318-ce26a645f7e3", 0, null, "761bdc4c-4e6b-4e52-a0ca-b4c13e64f8d5", null, "doctor5@hospital.com", false, "DoctorFirstName5", null, "DoctorLastName5", false, null, "DOCTOR5@HOSPITAL.COM", "DOCTOR5@HOSPITAL.COM", "AQAAAAIAAYagAAAAEAZQGHNeOrBDsWcURRT8+5Ssl3DBK71lCYdEUK6w/pG8TDAknKPIopb2jgBtlgFXnQ==", null, false, "5e393717-c4bf-4480-a42e-5a140a4cc867", false, "doctor5@hospital.com" }
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
                    { "3", "03e09429-4681-4c4d-a305-6c26d5dda010" },
                    { "3", "2acf712e-5eec-4c43-8d91-4c342674513c" },
                    { "2", "3c5e5d6a-5b13-4eb7-a8b6-a1511761a3bc" },
                    { "3", "3e6ac8f0-b5a6-47cf-bb0e-4bb849fac1ca" },
                    { "3", "4cea8fbe-6fca-4b89-a627-bac997f6e253" },
                    { "2", "4f11152c-79cf-4038-8703-46f269dd27c5" },
                    { "3", "546e5437-587d-4f62-a849-60d7697a31ad" },
                    { "3", "6788f103-af42-4ff6-9490-10a210055007" },
                    { "2", "7f577734-36d5-4c81-9fec-6e03c97f4839" },
                    { "2", "94bae31f-8b49-4f2f-a9d5-f192241977e6" },
                    { "0", "admin" },
                    { "3", "be67452b-7346-4a90-a938-36b47358de67" },
                    { "3", "d789132b-45ff-4817-b687-24773726e23c" },
                    { "3", "ddcecbac-8a65-4e07-bab1-3d3ecb266b2c" },
                    { "3", "eb2c864f-242c-4b37-8e6e-8e5835caf2f0" },
                    { "2", "f66df4a0-aba9-47f8-a318-ce26a645f7e3" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "SpecialtyId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 8, 15, 28, 45, 563, DateTimeKind.Utc).AddTicks(4069), null, "DoctorFirstName1", null, "DoctorLastName1", null, "7f577734-36d5-4c81-9fec-6e03c97f4839" },
                    { 2, new DateTime(2025, 2, 8, 15, 28, 45, 766, DateTimeKind.Utc).AddTicks(8633), null, "DoctorFirstName2", null, "DoctorLastName2", null, "3c5e5d6a-5b13-4eb7-a8b6-a1511761a3bc" },
                    { 3, new DateTime(2025, 2, 8, 15, 28, 45, 960, DateTimeKind.Utc).AddTicks(2486), null, "DoctorFirstName3", null, "DoctorLastName3", null, "94bae31f-8b49-4f2f-a9d5-f192241977e6" },
                    { 4, new DateTime(2025, 2, 8, 15, 28, 46, 150, DateTimeKind.Utc).AddTicks(1746), null, "DoctorFirstName4", null, "DoctorLastName4", null, "4f11152c-79cf-4038-8703-46f269dd27c5" },
                    { 5, new DateTime(2025, 2, 8, 15, 28, 46, 338, DateTimeKind.Utc).AddTicks(4533), null, "DoctorFirstName5", null, "DoctorLastName5", null, "f66df4a0-aba9-47f8-a318-ce26a645f7e3" }
                });

            migrationBuilder.InsertData(
                table: "Nurses",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 8, 15, 28, 45, 628, DateTimeKind.Utc).AddTicks(1258), null, "NurseFirstName1", null, "NurseLastName1", "ddcecbac-8a65-4e07-bab1-3d3ecb266b2c" },
                    { 2, new DateTime(2025, 2, 8, 15, 28, 45, 833, DateTimeKind.Utc).AddTicks(1174), null, "NurseFirstName2", null, "NurseLastName2", "4cea8fbe-6fca-4b89-a627-bac997f6e253" },
                    { 3, new DateTime(2025, 2, 8, 15, 28, 46, 24, DateTimeKind.Utc).AddTicks(2786), null, "NurseFirstName3", null, "NurseLastName3", "546e5437-587d-4f62-a849-60d7697a31ad" },
                    { 4, new DateTime(2025, 2, 8, 15, 28, 46, 213, DateTimeKind.Utc).AddTicks(891), null, "NurseFirstName4", null, "NurseLastName4", "2acf712e-5eec-4c43-8d91-4c342674513c" },
                    { 5, new DateTime(2025, 2, 8, 15, 28, 46, 400, DateTimeKind.Utc).AddTicks(5669), null, "NurseFirstName5", null, "NurseLastName5", "03e09429-4681-4c4d-a305-6c26d5dda010" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "CreatedAt", "DepartmentId", "FirstName", "Image", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 8, 15, 28, 45, 702, DateTimeKind.Utc).AddTicks(7495), null, "PatientFirstName1", null, "PatientLastName1", "6788f103-af42-4ff6-9490-10a210055007" },
                    { 2, new DateTime(2025, 2, 8, 15, 28, 45, 897, DateTimeKind.Utc).AddTicks(4532), null, "PatientFirstName2", null, "PatientLastName2", "be67452b-7346-4a90-a938-36b47358de67" },
                    { 3, new DateTime(2025, 2, 8, 15, 28, 46, 87, DateTimeKind.Utc).AddTicks(1106), null, "PatientFirstName3", null, "PatientLastName3", "3e6ac8f0-b5a6-47cf-bb0e-4bb849fac1ca" },
                    { 4, new DateTime(2025, 2, 8, 15, 28, 46, 275, DateTimeKind.Utc).AddTicks(9130), null, "PatientFirstName4", null, "PatientLastName4", "d789132b-45ff-4817-b687-24773726e23c" },
                    { 5, new DateTime(2025, 2, 8, 15, 28, 46, 463, DateTimeKind.Utc).AddTicks(6157), null, "PatientFirstName5", null, "PatientLastName5", "eb2c864f-242c-4b37-8e6e-8e5835caf2f0" }
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
