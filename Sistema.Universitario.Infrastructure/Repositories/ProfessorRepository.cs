using System;
using Sistema.Universitario.Domain.Entities;
using Sistema.Universitario.Infrastructure.Data;

namespace Sistema.Universitario.Infrastructure.Repositories;

public class ProfessorRepository : BaseRepository<Professor>, IProfessorRepository
{
    public ProfessorRepository(SistemaUniversitarioDbContext context) : base(context){}
}
