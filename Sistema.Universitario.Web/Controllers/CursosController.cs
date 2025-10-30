using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;

namespace Sistema.Universitario.Web.Controllers;

public class CursosController : Controller
{
    private readonly ICursoService _cursoService;

    public CursosController(ICursoService cursoService)
    {
        _cursoService = cursoService;
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
        if (!ModelState.IsValid) return View(vm);
        await _cursoService.AddAsync(vm);
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
