using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sistema.Universitario.Domain.Entities;

namespace Sistema.Universitario.Infrastructure.Repositories;

public interface ICursoRepository
{
    Task<IEnumerable<Curso>> GetAllAsync();
    Task<Curso> GetByIdAsync(Guid id);
    Task<Curso> AddAsync(Curso entity);
    Task<Curso> UpdateAsync(Curso entity);
    Task<bool> DeleteAsync(Guid id);
    Task<Curso> GetByIdWithDetailsAsync(Guid id);
}
