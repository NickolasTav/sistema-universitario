using System;
using Sistema.Universitario.Application.ViewModels;

namespace Sistema.Universitario.Application.Interfaces;

public interface ICursoService
{
     Task<IEnumerable<CursoViewModel>> GetAllAsync();
        Task<CursoViewModel> GetByIdAsync(Guid id);
        Task<CursoDetailViewModel> GetByIdWithDetailsAsync(Guid id);
        Task<CursoViewModel> AddAsync(CursoViewModel curso);
        Task<CursoViewModel> UpdateAsync(CursoViewModel curso);
        Task<bool> DeleteAsync(Guid id);
}
