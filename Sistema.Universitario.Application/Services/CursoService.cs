using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;
using Sistema.Universitario.Domain.Entities;
using Sistema.Universitario.Infrastructure.Repositories;

namespace Sistema.Universitario.Application.Services;

public class CursoService : ICursoService
{
    private readonly ICursoRepository _repository;

    public CursoService(ICursoRepository repository)
    {
        _repository = repository;
    }

    public async Task<CursoViewModel> AddAsync(CursoViewModel curso)
    {
        var entity = new Curso { Id = curso.Id == Guid.Empty ? Guid.NewGuid() : curso.Id, Nome = curso.Nome };
        var added = await _repository.AddAsync(entity);
        return new CursoViewModel { Id = added.Id, Nome = added.Nome };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CursoViewModel>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(c => new CursoViewModel { Id = c.Id, Nome = c.Nome });
    }

    public async Task<CursoViewModel> GetByIdAsync(Guid id)
    {
        var c = await _repository.GetByIdAsync(id);
        return c == null ? null : new CursoViewModel { Id = c.Id, Nome = c.Nome };
    }

    public async Task<CursoViewModel> UpdateAsync(CursoViewModel curso)
    {
        var entity = new Curso { Id = curso.Id, Nome = curso.Nome };
        var updated = await _repository.UpdateAsync(entity);
        return new CursoViewModel { Id = updated.Id, Nome = updated.Nome };
    }

    public async Task<CursoDetailViewModel> GetByIdWithDetailsAsync(Guid id)
    {
        var c = await _repository.GetByIdWithDetailsAsync(id);
        if (c == null) return null;

        var detail = new CursoDetailViewModel
        {
            Id = c.Id,
            Nome = c.Nome,
            Materias = c.Materias?.Select(m => new MateriaViewModel { Id = m.Id, Nome = m.Nome, CursoId = m.CursoId, ProfessorId = m.ProfessorId }) ?? Enumerable.Empty<MateriaViewModel>(),
            Alunos = c.Alunos?.Select(a => new AlunoViewModel { Id = a.Id, Nome = a.Nome, Matricula = a.Matricula, CursoId = a.CursoId }) ?? Enumerable.Empty<AlunoViewModel>()
        };

        return detail;
    }
}
