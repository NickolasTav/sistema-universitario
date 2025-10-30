using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sistema.Universitario.Domain.Entities;

namespace Sistema.Universitario.Infrastructure.Repositories;

public interface IAlunoRepository
{
    Task<IEnumerable<Aluno>> GetAllAsync();
    Task<Aluno> GetByIdAsync(Guid id);
    Task<Aluno> AddAsync(Aluno entity);
    Task<Aluno> UpdateAsync(Aluno entity);
    Task<bool> DeleteAsync(Guid id);
}
