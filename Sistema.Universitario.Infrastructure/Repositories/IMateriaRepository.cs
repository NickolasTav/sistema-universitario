using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sistema.Universitario.Domain.Entities;

namespace Sistema.Universitario.Infrastructure.Repositories;

public interface IMateriaRepository
{
    Task<IEnumerable<Materia>> GetAllAsync();
    Task<Materia> GetByIdAsync(Guid id);
    Task<Materia> AddAsync(Materia entity);
    Task<Materia> UpdateAsync(Materia entity);
    Task<bool> DeleteAsync(Guid id);
}
