using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using System.Text.Json;

namespace Sistema.Universitario.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SistemaUniversitarioDbContext>
{
    public SistemaUniversitarioDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SistemaUniversitarioDbContext>();
        // Try precedence: 1) appsettings.json in the startup project (if present)
        //                  2) DEFAULT_CONNECTION environment variable
        //                  3) LocalDB fallback
        string? conn = null;

        // Look for appsettings.json in the current working directory (EF sets this to the startup project)
        var basePath = Directory.GetCurrentDirectory();
        var appsettingsPath = Path.Combine(basePath, "appsettings.json");
        if (File.Exists(appsettingsPath))
        {
            try
            {
                var json = File.ReadAllText(appsettingsPath);
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("ConnectionStrings", out var csSection) &&
                    csSection.TryGetProperty("DefaultConnection", out var defaultConn))
                {
                    conn = defaultConn.GetString();
                }
            }
            catch
            {
                // ignore and continue to environment variable fallback
            }
        }

    conn ??= Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
    // Fallback to the developer's local SQL Server Express instance (SQLEXPRESS) instead of LocalDB
    conn ??= "Server=localhost\\SQLEXPRESS;Initial Catalog=SistemaUniversitarioDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        optionsBuilder.UseSqlServer(conn);
        return new SistemaUniversitarioDbContext(optionsBuilder.Options);
    }
}
