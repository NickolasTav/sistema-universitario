using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;
using Sistema.Universitario.Domain.Entities;
using Sistema.Universitario.Infrastructure.Repositories;

namespace Sistema.Universitario.Application.Services;

public class AlunoService : IAlunoService
{
    private readonly IAlunoRepository _repository;

    public AlunoService(IAlunoRepository repository)
    {
        _repository = repository;
    }

    public async Task<AlunoViewModel> AddAsync(AlunoViewModel aluno)
    {
        var entity = ToEntity(aluno);
        var added = await _repository.AddAsync(entity);
        return ToViewModel(added);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<AlunoViewModel>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(ToViewModel);
    }

    public async Task<AlunoViewModel> GetByIdAsync(Guid id)
    {
        var e = await _repository.GetByIdAsync(id);
        return e == null ? null : ToViewModel(e);
    }

    public async Task<AlunoViewModel> UpdateAsync(AlunoViewModel aluno)
    {
        var entity = ToEntity(aluno);
        var updated = await _repository.UpdateAsync(entity);
        return ToViewModel(updated);
    }

    // simple mappers
    private static AlunoViewModel ToViewModel(Aluno e) => new AlunoViewModel
    {
        Id = e.Id,
        Nome = e.Nome,
        Matricula = e.Matricula,
        CursoId = e.CursoId
    };

    private static Aluno ToEntity(AlunoViewModel vm) => new Aluno
    {
        Id = vm.Id == Guid.Empty ? Guid.NewGuid() : vm.Id,
        Nome = vm.Nome,
        Matricula = vm.Matricula,
        CursoId = vm.CursoId
    };
}
