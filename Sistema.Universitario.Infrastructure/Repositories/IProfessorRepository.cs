using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sistema.Universitario.Domain.Entities;

namespace Sistema.Universitario.Infrastructure.Repositories;

public interface IProfessorRepository
{
    Task<IEnumerable<Professor>> GetAllAsync();
    Task<Professor> GetByIdAsync(Guid id);
    Task<Professor> AddAsync(Professor entity);
    Task<Professor> UpdateAsync(Professor entity);
    Task<bool> DeleteAsync(Guid id);
}
