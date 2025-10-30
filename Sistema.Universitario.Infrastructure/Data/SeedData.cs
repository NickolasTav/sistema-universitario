using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sistema.Universitario.Domain.Entities;

namespace Sistema.Universitario.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SistemaUniversitarioDbContext>();

            if (context.Cursos.Any()) return; // already seeded

            // Professores
            var prof1 = new Professor { Id = Guid.NewGuid(), Nome = "Carlos Silva" };
            var prof2 = new Professor { Id = Guid.NewGuid(), Nome = "Mariana Souza" };
            context.Professores.Add(prof1);
            context.Professores.Add(prof2);

            // Cursos
            var curso1 = new Curso { Id = Guid.NewGuid(), Nome = "Engenharia de Software" };
            var curso2 = new Curso { Id = Guid.NewGuid(), Nome = "Sistemas de Informação" };
            context.Cursos.Add(curso1);
            context.Cursos.Add(curso2);

            // Materias
            var mat1 = new Materia { Id = Guid.NewGuid(), Nome = "Algoritmos", CursoId = curso1.Id, ProfessorId = prof1.Id };
            var mat2 = new Materia { Id = Guid.NewGuid(), Nome = "Banco de Dados", CursoId = curso1.Id, ProfessorId = prof2.Id };
            var mat3 = new Materia { Id = Guid.NewGuid(), Nome = "Redes", CursoId = curso2.Id, ProfessorId = prof2.Id };
            context.Materias.AddRange(mat1, mat2, mat3);

            // Alunos
            var a1 = new Aluno { Id = Guid.NewGuid(), Nome = "João Pereira", Matricula = "2025001", CursoId = curso1.Id };
            var a2 = new Aluno { Id = Guid.NewGuid(), Nome = "Ana Costa", Matricula = "2025002", CursoId = curso2.Id };
            context.Alunos.AddRange(a1, a2);

            await context.SaveChangesAsync();
        }
    }
}
