using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;
using Sistema.Universitario.Domain.Entities;
using Sistema.Universitario.Infrastructure.Repositories;
using Mapster;

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
        var entity = curso.Adapt<Curso>();
        if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
        var added = await _repository.AddAsync(entity);
        return added.Adapt<CursoViewModel>();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CursoViewModel>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(c => c.Adapt<CursoViewModel>());
    }

    public async Task<CursoViewModel> GetByIdAsync(Guid id)
    {
        var c = await _repository.GetByIdAsync(id);
        return c == null ? null : c.Adapt<CursoViewModel>();
    }

    public async Task<CursoViewModel> UpdateAsync(CursoViewModel curso)
    {
        var entity = curso.Adapt<Curso>();
        var updated = await _repository.UpdateAsync(entity);
        return updated.Adapt<CursoViewModel>();
    }

    public async Task<CursoDetailViewModel> GetByIdWithDetailsAsync(Guid id)
    {
        var c = await _repository.GetByIdWithDetailsAsync(id);
        if (c == null) return null;
        var detail = new CursoDetailViewModel
        {
            Id = c.Id,
            Nome = c.Nome,
            Materias = c.Materias?.Select(m => m.Adapt<MateriaViewModel>()).ToList() ?? Enumerable.Empty<MateriaViewModel>(),
            Alunos = c.Alunos?.Select(a => a.Adapt<AlunoViewModel>()).ToList() ?? Enumerable.Empty<AlunoViewModel>()
        };

        // fill related display names
        var cursos = new Dictionary<Guid, string> { { c.Id, c.Nome } };
        foreach (var m in detail.Materias)
        {
            if (m.CursoId.HasValue && cursos.TryGetValue(m.CursoId.Value, out var cn)) m.CursoNome = cn; else m.CursoNome = null;
        }
        foreach (var a in detail.Alunos)
        {
            a.CursoNome = c.Nome;
        }

        return detail;
    }
}
