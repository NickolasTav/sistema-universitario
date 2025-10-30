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
    private readonly ICursoRepository _cursoRepository;
    private readonly IProfessorRepository _professorRepository;

    public MateriaService(IMateriaRepository repository, ICursoRepository cursoRepository, IProfessorRepository professorRepository)
    {
        _repository = repository;
        _cursoRepository = cursoRepository;
        _professorRepository = professorRepository;
    }

    public async Task<MateriaViewModel> AddAsync(MateriaViewModel materia)
    {
        if (!materia.CursoId.HasValue)
            throw new ArgumentException("CursoId is required", nameof(materia.CursoId));

        var entity = new Materia
        {
            Id = materia.Id == Guid.Empty ? Guid.NewGuid() : materia.Id,
            Nome = materia.Nome,
            CursoId = materia.CursoId.Value,
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
        var cursos = (await _cursoRepository.GetAllAsync()).ToDictionary(c => c.Id, c => c.Nome);
        var profs = (await _professorRepository.GetAllAsync()).ToDictionary(p => p.Id, p => p.Nome);
        return list.Select(m => {
            string? professorNome = null;
            Guid? professorId = m.ProfessorId;
            if (professorId.HasValue)
            {
                profs.TryGetValue(professorId.Value, out var pn);
                professorNome = pn;
            }

            return new MateriaViewModel
            {
                Id = m.Id,
                Nome = m.Nome,
                CursoId = m.CursoId,
                CursoNome = cursos.TryGetValue(m.CursoId, out var cn) ? cn : null,
                ProfessorId = professorId,
                ProfessorNome = professorNome
            };
        });
    }

    public async Task<MateriaViewModel?> GetByIdAsync(Guid id)
    {
        var m = await _repository.GetByIdAsync(id);
        if (m == null) return null;
        var curso = await _cursoRepository.GetByIdAsync(m.CursoId);
    var prof = m.ProfessorId.HasValue ? await _professorRepository.GetByIdAsync(m.ProfessorId.Value) : null;
        return new MateriaViewModel
        {
            Id = m.Id,
            Nome = m.Nome,
            CursoId = m.CursoId,
            CursoNome = curso?.Nome,
            ProfessorId = m.ProfessorId,
            ProfessorNome = prof?.Nome
        };
    }

    public async Task<MateriaViewModel> UpdateAsync(MateriaViewModel materia)
    {
        if (!materia.CursoId.HasValue)
            throw new ArgumentException("CursoId is required", nameof(materia.CursoId));

        var entity = new Materia { Id = materia.Id, Nome = materia.Nome, CursoId = materia.CursoId.Value, ProfessorId = materia.ProfessorId };
        var updated = await _repository.UpdateAsync(entity);
        var curso = await _cursoRepository.GetByIdAsync(updated.CursoId);
    var prof = updated.ProfessorId.HasValue ? await _professorRepository.GetByIdAsync(updated.ProfessorId.Value) : null;
        return new MateriaViewModel
        {
            Id = updated.Id,
            Nome = updated.Nome,
            CursoId = updated.CursoId,
            CursoNome = curso?.Nome,
            ProfessorId = updated.ProfessorId,
            ProfessorNome = prof?.Nome
        };
    }
}



