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

public class ProfessorService : IProfessorService
{
    private readonly IProfessorRepository _repository;

    public ProfessorService(IProfessorRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProfessorViewModel> AddAsync(ProfessorViewModel professor)
    {
        var entity = professor.Adapt<Professor>();
        if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
        var added = await _repository.AddAsync(entity);
        return added.Adapt<ProfessorViewModel>();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProfessorViewModel>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(p => p.Adapt<ProfessorViewModel>());
    }

    public async Task<ProfessorViewModel> GetByIdAsync(Guid id)
    {
        var p = await _repository.GetByIdAsync(id);
        return p == null ? null : p.Adapt<ProfessorViewModel>();
    }

    public async Task<ProfessorViewModel> UpdateAsync(ProfessorViewModel professor)
    {
        var entity = professor.Adapt<Professor>();
        var updated = await _repository.UpdateAsync(entity);
        return updated.Adapt<ProfessorViewModel>();
    }
}
