using System;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Universitario.Application.ViewModels;

public class AlunoViewModel
{
    public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Sistema.Universitario.Application.Validation.FullName]
        public string Nome { get; set; }

        [Required]
        [MaxLength(20)]
        [Sistema.Universitario.Application.Validation.Matricula]
        public string Matricula { get; set; }

        [Required]
        public Guid CursoId { get; set; }
    // optional display name for the course
    public string CursoNome { get; set; }
}
