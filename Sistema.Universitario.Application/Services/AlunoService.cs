using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;
using Sistema.Universitario.Domain.Entities;
using Sistema.Universitario.Infrastructure.Repositories;

namespace Sistema.Universitario.Application.Services;

public class AlunoService : IAlunoService
{
    private readonly IAlunoRepository _repository;
    private readonly ICursoRepository _cursoRepository;

    public AlunoService(IAlunoRepository repository, ICursoRepository cursoRepository)
    {
        _repository = repository;
        _cursoRepository = cursoRepository;
    }

    public async Task<AlunoViewModel> AddAsync(AlunoViewModel aluno)
    {
        var entity = aluno.Adapt<Domain.Entities.Aluno>();
        // ensure we don't persist an empty Guid as Id (Mapster maps default Guid from VM)
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }
        var added = await _repository.AddAsync(entity);
        return added.Adapt<AlunoViewModel>();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<AlunoViewModel>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var cursos = (await _cursoRepository.GetAllAsync()).ToDictionary(c => c.Id, c => c.Nome);
        var vms = list.Select(e => e.Adapt<AlunoViewModel>());
        // fill CursoNome from lookup
        foreach (var vm in vms)
        {
            vm.CursoNome = cursos.TryGetValue(vm.CursoId, out var nome) ? nome : null;
        }
        return vms;
    }

    public async Task<AlunoViewModel> GetByIdAsync(Guid id)
    {
        var e = await _repository.GetByIdAsync(id);
        if (e == null) return null;
        var vm = e.Adapt<AlunoViewModel>();
        var curso = await _cursoRepository.GetByIdAsync(e.CursoId);
        vm.CursoNome = curso?.Nome;
        return vm;
    }

    public async Task<AlunoViewModel> UpdateAsync(AlunoViewModel aluno)
    {
        var entity = aluno.Adapt<Domain.Entities.Aluno>();
        var updated = await _repository.UpdateAsync(entity);
        return updated.Adapt<AlunoViewModel>();
    }
    
}
