using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;

namespace Sistema.Universitario.Web.Controllers;

public class ProfessoresController : Controller
{
    private readonly IProfessorService _professorService;

    public ProfessoresController(IProfessorService professorService)
    {
        _professorService = professorService;
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
        if (!ModelState.IsValid) return View(vm);
        await _professorService.AddAsync(vm);
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
