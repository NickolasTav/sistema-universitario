using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;
using Sistema.Universitario.Domain.Entities;
using Sistema.Universitario.Infrastructure.Repositories;

namespace Sistema.Universitario.Application.Services;

public class MateriaService : IMateriaService
{
    private readonly IMateriaRepository _repository;

    public MateriaService(IMateriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<MateriaViewModel> AddAsync(MateriaViewModel materia)
    {
        var entity = new Materia
        {
            Id = materia.Id == Guid.Empty ? Guid.NewGuid() : materia.Id,
            Nome = materia.Nome,
            CursoId = materia.CursoId,
            ProfessorId = materia.ProfessorId
        };

        var added = await _repository.AddAsync(entity);
        return new MateriaViewModel { Id = added.Id, Nome = added.Nome, CursoId = added.CursoId, ProfessorId = added.ProfessorId };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<MateriaViewModel>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(m => new MateriaViewModel { Id = m.Id, Nome = m.Nome, CursoId = m.CursoId, ProfessorId = m.ProfessorId });
    }

    public async Task<MateriaViewModel> GetByIdAsync(Guid id)
    {
        var m = await _repository.GetByIdAsync(id);
        return m == null ? null : new MateriaViewModel { Id = m.Id, Nome = m.Nome, CursoId = m.CursoId, ProfessorId = m.ProfessorId };
    }

    public async Task<MateriaViewModel> UpdateAsync(MateriaViewModel materia)
    {
        var entity = new Materia { Id = materia.Id, Nome = materia.Nome, CursoId = materia.CursoId, ProfessorId = materia.ProfessorId };
        var updated = await _repository.UpdateAsync(entity);
        return new MateriaViewModel { Id = updated.Id, Nome = updated.Nome, CursoId = updated.CursoId, ProfessorId = updated.ProfessorId };
    }
}



