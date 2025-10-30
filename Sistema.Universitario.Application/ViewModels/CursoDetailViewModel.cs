using System;
using System.Collections.Generic;

namespace Sistema.Universitario.Application.ViewModels;

public class CursoDetailViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public IEnumerable<MateriaViewModel> Materias { get; set; } = new List<MateriaViewModel>();
    public IEnumerable<AlunoViewModel> Alunos { get; set; } = new List<AlunoViewModel>();
}
