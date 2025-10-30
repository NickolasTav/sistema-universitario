using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Universitario.Domain.Entities
{
    public class Curso
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        public ICollection<Aluno> Alunos { get; set; } = new List<Aluno>();
        public ICollection<Materia> Materias { get; set; } = new List<Materia>();
    }
}
