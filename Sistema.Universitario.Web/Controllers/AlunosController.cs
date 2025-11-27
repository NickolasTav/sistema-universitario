using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;
using Microsoft.Extensions.Logging;

namespace Sistema.Universitario.Web.Controllers;

public class AlunosController : Controller
{
    private readonly IAlunoService _alunoService;
    private readonly ICursoService _cursoService;
    private readonly ILogger<AlunosController> _logger;

    public AlunosController(IAlunoService alunoService, ICursoService cursoService, ILogger<AlunosController> logger)
    {
        _alunoService = alunoService;
        _cursoService = cursoService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _alunoService.GetAllAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string q)
    {
        var list = (await _alunoService.GetAllAsync()).ToList();
        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            list = list.Where(a => (a.Nome ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase)
                || (a.Matricula ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase)
                || (a.CursoNome ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return Json(list.Take(50));
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var model = await _alunoService.GetByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        var cursos = await _cursoService.GetAllAsync();
        ViewBag.Cursos = new SelectList(cursos, "Id", "Nome");
        if (!cursos.Any())
        {
            TempData["Info"] = "Não existem cursos. Crie um curso antes de cadastrar alunos.";
            return RedirectToAction("Create", "Cursos");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AlunoViewModel vm)
    {
        _logger.LogInformation("Create Aluno attempt: {@Aluno}", vm);
        if (!ModelState.IsValid)
        {
            foreach (var entry in ModelState)
            {
                var key = entry.Key;
                var errors = entry.Value.Errors.Select(e => e.ErrorMessage).Where(m => !string.IsNullOrWhiteSpace(m));
                if (errors.Any())
                {
                    _logger.LogWarning("ModelState error for {Key}: {Errors}", key, string.Join("; ", errors));
                }
            }
            var cursos = await _cursoService.GetAllAsync();
            ViewBag.Cursos = new SelectList(cursos, "Id", "Nome");
            _logger.LogWarning("Create Aluno failed validation: {@ModelState}", ModelState);
            return View(vm);
        }
        try
        {
            var added = await _alunoService.AddAsync(vm);
            _logger.LogInformation("Aluno created: {Id}", added?.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Aluno");
            var cursos = await _cursoService.GetAllAsync();
            ViewBag.Cursos = new SelectList(cursos, "Id", "Nome");
            TempData["Error"] = "Erro ao salvar aluno. Veja os logs.";
            return View(vm);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var vm = await _alunoService.GetByIdAsync(id);
        if (vm == null) return NotFound();
        var cursos = await _cursoService.GetAllAsync();
        ViewBag.Cursos = new SelectList(cursos, "Id", "Nome", vm.CursoId);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, AlunoViewModel vm)
    {
        if (id != vm.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            var cursos = await _cursoService.GetAllAsync();
            ViewBag.Cursos = new SelectList(cursos, "Id", "Nome", vm.CursoId);
            _logger.LogWarning("Edit Aluno failed validation: {@ModelState}", ModelState);
            return View(vm);
        }
        try
        {
            await _alunoService.UpdateAsync(vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Aluno {Id}", vm.Id);
            var cursos = await _cursoService.GetAllAsync();
            ViewBag.Cursos = new SelectList(cursos, "Id", "Nome", vm.CursoId);
            TempData["Error"] = "Erro ao atualizar aluno. Veja os logs.";
            return View(vm);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var vm = await _alunoService.GetByIdAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _alunoService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
