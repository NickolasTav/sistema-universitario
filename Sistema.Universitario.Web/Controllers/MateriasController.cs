using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;
using Microsoft.Extensions.Logging;

namespace Sistema.Universitario.Web.Controllers;

public class MateriasController : Controller
{
    private readonly IMateriaService _materiaService;
    private readonly ICursoService _cursoService;
    private readonly IProfessorService _professorService;
    private readonly ILogger<MateriasController> _logger;

    public MateriasController(IMateriaService materiaService, ICursoService cursoService, IProfessorService professorService, ILogger<MateriasController> logger)
    {
    _materiaService = materiaService;
    _cursoService = cursoService;
        _professorService = professorService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _materiaService.GetAllAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string q)
    {
        var list = (await _materiaService.GetAllAsync()).ToList();
        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            list = list.Where(m => (m.Nome ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase)
                || (m.CursoNome ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase)
                || (m.ProfessorNome ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return Json(list.Take(50));
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var model = await _materiaService.GetByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    public async Task<IActionResult> Create(Guid? cursoId)
    {
        var cursos = await _cursoService.GetAllAsync();
        var profs = await _professorService.GetAllAsync();
        // If there are no courses or professors yet, redirect user to create them first
        if (!cursos.Any())
        {
            TempData["Info"] = "Não existem cursos. Cadastre um curso antes de criar uma disciplina.";
            return RedirectToAction("Create", "Cursos");
        }

        if (!profs.Any())
        {
            TempData["Info"] = "Não existem professores. Cadastre um professor antes de criar uma disciplina.";
            return RedirectToAction("Create", "Professores");
        }

        ViewBag.Cursos = new SelectList(cursos, "Id", "Nome", cursoId ?? Guid.Empty);
        ViewBag.Professores = new SelectList(profs, "Id", "Nome");
        // If cursoId provided, preselect it in the view to simplify adding the first disciplina
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MateriaViewModel vm)
    {
        _logger.LogInformation("Create Materia attempt: {@Materia}", vm);
        // ensure a curso was selected (select may post empty value)
        if (!vm.CursoId.HasValue)
        {
            ModelState.AddModelError(nameof(vm.CursoId), "Selecione um curso.");
        }

        if (!ModelState.IsValid)
        {
            var cursos = await _cursoService.GetAllAsync();
            var profs = await _professorService.GetAllAsync();
            ViewBag.Cursos = new SelectList(cursos, "Id", "Nome");
            ViewBag.Professores = new SelectList(profs, "Id", "Nome");
            var errors = ModelState.SelectMany(kvp => kvp.Value.Errors.Select(e => new { Key = kvp.Key, Error = e.ErrorMessage })).ToList();
            _logger.LogWarning("Create Materia failed validation details: {@Errors}", errors);
            return View(vm);
        }
        try
        {
            var added = await _materiaService.AddAsync(vm);
            _logger.LogInformation("Materia created: {Id}", added?.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Materia");
            TempData["Error"] = "Erro ao salvar disciplina. Veja os logs.";
            var cursos = await _cursoService.GetAllAsync();
            var profs = await _professorService.GetAllAsync();
            ViewBag.Cursos = new SelectList(cursos, "Id", "Nome");
            ViewBag.Professores = new SelectList(profs, "Id", "Nome");
            return View(vm);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var vm = await _materiaService.GetByIdAsync(id);
        if (vm == null) return NotFound();
    var cursos = await _cursoService.GetAllAsync();
        var profs = await _professorService.GetAllAsync();
        ViewBag.Cursos = new SelectList(cursos, "Id", "Nome", vm.CursoId);
        ViewBag.Professores = new SelectList(profs, "Id", "Nome", vm.ProfessorId);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MateriaViewModel vm)
    {
        if (id != vm.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            var cursos = await _cursoService.GetAllAsync();
            var profs = await _professorService.GetAllAsync();
            ViewBag.Cursos = new SelectList(cursos, "Id", "Nome", vm.CursoId);
            ViewBag.Professores = new SelectList(profs, "Id", "Nome", vm.ProfessorId);
            return View(vm);
        }
        await _materiaService.UpdateAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var vm = await _materiaService.GetByIdAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _materiaService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
