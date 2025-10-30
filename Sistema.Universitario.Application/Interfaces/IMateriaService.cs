using System;
using Sistema.Universitario.Application.ViewModels;

namespace Sistema.Universitario.Application.Interfaces;

public interface IMateriaService
{
      Task<IEnumerable<MateriaViewModel>> GetAllAsync();
  Task<MateriaViewModel?> GetByIdAsync(Guid id);
        Task<MateriaViewModel> AddAsync(MateriaViewModel materia);
        Task<MateriaViewModel> UpdateAsync(MateriaViewModel materia);
        Task<bool> DeleteAsync(Guid id);
}
