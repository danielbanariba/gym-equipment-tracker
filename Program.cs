using GymEquipmentTracker.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// AddDbContext registra GymDbContext en el contenedor de inyección de
// dependencias con ciclo de vida "Scoped" (una instancia por request HTTP).
// UseNpgsql le dice a EF Core qué proveedor de base de datos usar — el
// mismo DbContext funcionaría contra SQL Server solo cambiando esta línea
// por UseSqlServer. La connection string vive en appsettings.json /
// appsettings.Development.json, nunca hardcodeada aquí.
builder.Services.AddDbContext<GymDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GymDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
