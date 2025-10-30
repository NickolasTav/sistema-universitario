using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sistema.Universitario.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Sistema.Universitario.Application.ViewModels;

namespace Sistema.Universitario.Web.Controllers;

public class ProfessoresController : Controller
{
    private readonly IProfessorService _professorService;
    private readonly ILogger<ProfessoresController> _logger;

    public ProfessoresController(IProfessorService professorService, ILogger<ProfessoresController> logger)
    {
        _professorService = professorService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _professorService.GetAllAsync();
        return View(list);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var model = await _professorService.GetByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProfessorViewModel vm)
    {
        _logger.LogInformation("Create Professor attempt: {@Professor}", vm);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create Professor failed validation: {@ModelState}", ModelState);
            return View(vm);
        }
        try
        {
            var added = await _professorService.AddAsync(vm);
            _logger.LogInformation("Professor created: {Id}", added?.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Professor");
            TempData["Error"] = "Erro ao salvar professor. Veja os logs.";
            return View(vm);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var vm = await _professorService.GetByIdAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProfessorViewModel vm)
    {
        if (id != vm.Id) return BadRequest();
        if (!ModelState.IsValid) return View(vm);
        await _professorService.UpdateAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var vm = await _professorService.GetByIdAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _professorService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
