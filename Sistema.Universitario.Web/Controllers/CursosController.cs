using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;
using Microsoft.Extensions.Logging;

namespace Sistema.Universitario.Web.Controllers;

public class CursosController : Controller
{
    private readonly ICursoService _cursoService;
    private readonly ILogger<CursosController> _logger;

    public CursosController(ICursoService cursoService, ILogger<CursosController> logger)
    {
        _cursoService = cursoService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _cursoService.GetAllAsync();
        return View(list);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var model = await _cursoService.GetByIdWithDetailsAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CursoViewModel vm)
    {
        _logger.LogInformation("Create Curso attempt: {@Curso}", vm);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create Curso failed validation: {@ModelState}", ModelState);
            return View(vm);
        }
        try
        {
            var created = await _cursoService.AddAsync(vm);
            _logger.LogInformation("Curso created: {Id}", created?.Id);
            // After creating a course, redirect to create a Materia for this course so a course cannot exist without at least one disciplina
            if (created != null)
            {
                return RedirectToAction("Create", "Materias", new { cursoId = created.Id });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Curso");
            TempData["Error"] = "Erro ao salvar curso. Veja os logs.";
            return View(vm);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var c = await _cursoService.GetByIdAsync(id);
        if (c == null) return NotFound();
        return View(c);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CursoViewModel vm)
    {
        if (id != vm.Id) return BadRequest();
        if (!ModelState.IsValid) return View(vm);
        await _cursoService.UpdateAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var c = await _cursoService.GetByIdAsync(id);
        if (c == null) return NotFound();
        return View(c);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _cursoService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
