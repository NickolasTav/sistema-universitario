using System;
using Sistema.Universitario.Domain.Entities;
using Sistema.Universitario.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Sistema.Universitario.Infrastructure.Repositories;

public class CursoRepository : BaseRepository<Curso>, ICursoRepository
{
    public CursoRepository(SistemaUniversitarioDbContext context) : base(context){}
    
    public async Task<Curso> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.Alunos)
            .Include(c => c.Materias)
                .ThenInclude(m => m.Professor)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
