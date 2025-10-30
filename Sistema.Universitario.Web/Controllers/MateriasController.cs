using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema.Universitario.Application.Interfaces;
using Sistema.Universitario.Application.ViewModels;

namespace Sistema.Universitario.Web.Controllers;

public class MateriasController : Controller
{
    private readonly IMateriaService _materiaService;
    private readonly ICursoService _cursoService;
    private readonly IProfessorService _professorService;

    public MateriasController(IMateriaService materiaService, ICursoService cursoService, IProfessorService professorService)
    {
        _materiaService = materiaService;
        _cursoService = cursoService;
        _professorService = professorService;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _materiaService.GetAllAsync();
        return View(list);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var model = await _materiaService.GetByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        var cursos = await _cursoService.GetAllAsync();
        var profs = await _professorService.GetAllAsync();
        ViewBag.Cursos = new SelectList(cursos, "Id", "Nome");
        ViewBag.Professores = new SelectList(profs, "Id", "Nome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MateriaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var cursos = await _cursoService.GetAllAsync();
            var profs = await _professorService.GetAllAsync();
            ViewBag.Cursos = new SelectList(cursos, "Id", "Nome");
            ViewBag.Professores = new SelectList(profs, "Id", "Nome");
            return View(vm);
        }
        await _materiaService.AddAsync(vm);
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
