using System;
using Sistema.Universitario.Application.ViewModels;

namespace Sistema.Universitario.Application.Interfaces;

public interface IProfessorService
{
     Task<IEnumerable<ProfessorViewModel>> GetAllAsync();
        Task<ProfessorViewModel> GetByIdAsync(Guid id);
        Task<ProfessorViewModel> AddAsync(ProfessorViewModel professor);
        Task<ProfessorViewModel> UpdateAsync(ProfessorViewModel professor);
        Task<bool> DeleteAsync(Guid id);
}
