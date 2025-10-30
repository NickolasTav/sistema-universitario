using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Universitario.Domain.Entities
{
    public class Aluno
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(20)]
        public string Matricula { get; set; }

        [Required]
        public Guid CursoId { get; set; } 
        public Curso Curso { get; set; }

        public ICollection<Materia> Materias { get; set; } = new List<Materia>();
    }
}
