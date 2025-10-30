using System;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Universitario.Application.ViewModels;

public class AlunoViewModel
{
    public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(20)]
        public string Matricula { get; set; }

        [Required]
        public Guid CursoId { get; set; }
}
