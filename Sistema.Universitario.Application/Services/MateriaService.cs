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
        var entity = materia.Adapt<Materia>();
        if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
        // ensure CursoId is set (Mapster maps nullable to Guid.Empty in config)
        if (entity.CursoId == Guid.Empty && materia.CursoId.HasValue) entity.CursoId = materia.CursoId.Value;
        var added = await _repository.AddAsync(entity);
        return added.Adapt<MateriaViewModel>();
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
        var vms = list.Select(m => m.Adapt<MateriaViewModel>()).ToList();
        foreach (var vm in vms)
        {
            if (vm.CursoId.HasValue && cursos.TryGetValue(vm.CursoId.Value, out var cn)) vm.CursoNome = cn; else vm.CursoNome = null;
            if (vm.ProfessorId.HasValue && profs.TryGetValue(vm.ProfessorId.Value, out var pn)) vm.ProfessorNome = pn; else vm.ProfessorNome = null;
        }
        return vms;
    }

    public async Task<MateriaViewModel?> GetByIdAsync(Guid id)
    {
        var m = await _repository.GetByIdAsync(id);
        if (m == null) return null;
        var curso = await _cursoRepository.GetByIdAsync(m.CursoId);
    var prof = m.ProfessorId.HasValue ? await _professorRepository.GetByIdAsync(m.ProfessorId.Value) : null;
        var vm = m.Adapt<MateriaViewModel>();
        vm.CursoNome = curso?.Nome;
        vm.ProfessorNome = prof?.Nome;
        return vm;
    }

    public async Task<MateriaViewModel> UpdateAsync(MateriaViewModel materia)
    {
        if (!materia.CursoId.HasValue)
            throw new ArgumentException("CursoId is required", nameof(materia.CursoId));
        var entity = materia.Adapt<Materia>();
        if (entity.CursoId == Guid.Empty && materia.CursoId.HasValue) entity.CursoId = materia.CursoId.Value;
        var updated = await _repository.UpdateAsync(entity);
        var curso = await _cursoRepository.GetByIdAsync(updated.CursoId);
    var prof = updated.ProfessorId.HasValue ? await _professorRepository.GetByIdAsync(updated.ProfessorId.Value) : null;
        var vm = updated.Adapt<MateriaViewModel>();
        vm.CursoNome = curso?.Nome;
        vm.ProfessorNome = prof?.Nome;
        return vm;
    }
}



