using Microsoft.EntityFrameworkCore;
using Sistema.Universitario.Infrastructure.Data;
using Sistema.Universitario.Infrastructure.Repositories;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.Services;
using Sistema.Universitario.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Force the web host to listen on localhost:8080 for this app (HTTP)
builder.WebHost.UseUrls("http://localhost:8080");

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SistemaUniversitarioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Mapster configuration (config lives in Application layer)
MapsterConfig.Register();


builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<IMateriaRepository, MateriaRepository>();


builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<ICursoService, CursoService>();
builder.Services.AddScoped<IMateriaService, MateriaService>();

var app = builder.Build();

// Seed database with sample data (development)
using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    try
    {
        Sistema.Universitario.Infrastructure.Data.SeedData.SeedAsync(provider).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        // swallow to avoid blocking startup in development; logged by the app on runtime
        Console.WriteLine($"Seed error: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
