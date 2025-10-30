using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sistema.Universitario.Application.ViewModels;

namespace Sistema.Universitario.Application.Interfaces;

public interface IAlunoService
{
     Task<IEnumerable<AlunoViewModel>> GetAllAsync();
        Task<AlunoViewModel> GetByIdAsync(Guid id);
        Task<AlunoViewModel> AddAsync(AlunoViewModel aluno);
        Task<AlunoViewModel> UpdateAsync(AlunoViewModel aluno);
        Task<bool> DeleteAsync(Guid id);
}
