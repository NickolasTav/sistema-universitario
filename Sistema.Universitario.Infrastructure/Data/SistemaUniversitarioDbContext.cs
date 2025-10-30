using System;
using Microsoft.EntityFrameworkCore;
using Sistema.Universitario.Domain.Entities;

namespace Sistema.Universitario.Infrastructure.Data;

public class SistemaUniversitarioDbContext : DbContext
{
    public SistemaUniversitarioDbContext(DbContextOptions<SistemaUniversitarioDbContext> options)
            : base(options) { }

    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Professor> Professores { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Materia> Materias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Aluno>()
                    .HasOne(a => a.Curso)
                    .WithMany(c => c.Alunos)
                    .HasForeignKey(a => a.CursoId)
                    .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Materia>()
                    .HasOne(m => m.Curso)
                    .WithMany(c => c.Materias)
                    .HasForeignKey(m => m.CursoId)
                    .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Materia>()
                    .HasOne(m => m.Professor)
                    .WithMany(p => p.Materias)
                    .HasForeignKey(m => m.ProfessorId)
                    .OnDelete(DeleteBehavior.SetNull);
    }
}
