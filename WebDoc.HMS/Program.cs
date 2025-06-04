using WebDoc.HMS.Components;
using WebDoc.HMS.Services;
using WebDoc.HMS.StateContainers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazorBootstrap();

builder.Services.AddSingleton<SignupService>();
builder.Services.AddSingleton<LoginService>();

builder.Services.AddSingleton<DoctorService>();
builder.Services.AddSingleton<PatientService>();
builder.Services.AddSingleton<AppointmentService>();
builder.Services.AddSingleton<PrescriptionService>();
builder.Services.AddSingleton<AmbulanceBookingService>();

builder.Services.AddSingleton<UserStateContainer>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.Run();
