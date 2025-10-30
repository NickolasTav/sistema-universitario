using System;
using Sistema.Universitario.Domain.Entities;
using Sistema.Universitario.Infrastructure.Data;

namespace Sistema.Universitario.Infrastructure.Repositories;

public class MateriaRepository : BaseRepository<Materia>, IMateriaRepository
{
    public MateriaRepository(SistemaUniversitarioDbContext context) : base(context){}
}
