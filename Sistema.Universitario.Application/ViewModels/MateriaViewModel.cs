using System;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Universitario.Application.ViewModels;

public class MateriaViewModel
{
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Selecione um curso.")]
        public Guid? CursoId { get; set; }


        public string? CursoNome { get; set; }


        public Guid? ProfessorId { get; set; }
        public string? ProfessorNome { get; set; }
}
