using BeHealthy.Components;
using BeHealthy.Components.Account;
using BeHealthy.Data;
using BeHealthy.Endpoints.Appointments;
using BeHealthy.Endpoints.Department;
using BeHealthy.Endpoints.Doctors;
using BeHealthy.Endpoints.Nurse;
using BeHealthy.Endpoints.Patient;
using BeHealthy.Endpoints.Prescription;
using BeHealthy.Endpoints.Room;
using BeHealthy.Endpoints.User;
using BeHealthy.Repositories;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Entities;
using BeHealthy.States;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Default")!));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityCookies();

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    //.AddSignInManager()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddDefaultTokenProviders();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<INurseRepository, NurseRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IAppSettingsRepository, AppSettingsRepository>();
builder.Services.AddScoped<IPrivilegeRepository, PrivilegeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<INurseService, NurseService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppSettingsService, AppSettingsService>();
builder.Services.AddScoped<IPrivilegeService, PrivilegeService>();

// States
builder.Services.AddScoped<ModalStateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();
app.MapAppointmentsEndpoints();
app.MapDepartmentEndpoints();
app.MapDoctorsEndpoints();
app.MapNursesEndpoints();
app.MapPatientEndpoints();
app.MapPrescriptionsEndpoints();
app.MapRoomsEndpoints();
app.MapUsersEndpoints();

app.Run();
