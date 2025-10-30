using System;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Universitario.Application.ViewModels;

public class MateriaViewModel
{
     public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        public Guid CursoId { get; set; }

        [Required]
        public Guid ProfessorId { get; set; }
}
