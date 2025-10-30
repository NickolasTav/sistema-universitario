using System;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Universitario.Domain.Entities
{
    public class Materia
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        public Guid CursoId { get; set; }
        public Curso Curso { get; set; }

        [Required]
        public Guid ProfessorId { get; set; }
        public Professor Professor { get; set; }
    }
}
