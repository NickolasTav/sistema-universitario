using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sistema.Universitario.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SistemaUniversitarioDbContext>
{
    public SistemaUniversitarioDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SistemaUniversitarioDbContext>();
        // default to LocalDB for development; override via environment variable if needed
        var conn = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")
                   ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SistemaUniversitarioDb;Integrated Security=True;MultipleActiveResultSets=true";
        optionsBuilder.UseSqlServer(conn);
        return new SistemaUniversitarioDbContext(optionsBuilder.Options);
    }
}
