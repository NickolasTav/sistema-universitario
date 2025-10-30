using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;
using Sistema.Universitario.Domain.Entities;
using Sistema.Universitario.Infrastructure.Repositories;

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
        var entity = new Professor { Id = professor.Id == Guid.Empty ? Guid.NewGuid() : professor.Id, Nome = professor.Nome };
        var added = await _repository.AddAsync(entity);
        return new ProfessorViewModel { Id = added.Id, Nome = added.Nome };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProfessorViewModel>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(p => new ProfessorViewModel { Id = p.Id, Nome = p.Nome });
    }

    public async Task<ProfessorViewModel> GetByIdAsync(Guid id)
    {
        var p = await _repository.GetByIdAsync(id);
        return p == null ? null : new ProfessorViewModel { Id = p.Id, Nome = p.Nome };
    }

    public async Task<ProfessorViewModel> UpdateAsync(ProfessorViewModel professor)
    {
        var entity = new Professor { Id = professor.Id, Nome = professor.Nome };
        var updated = await _repository.UpdateAsync(entity);
        return new ProfessorViewModel { Id = updated.Id, Nome = updated.Nome };
    }
}
